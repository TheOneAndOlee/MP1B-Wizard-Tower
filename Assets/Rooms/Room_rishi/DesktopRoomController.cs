using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

[RequireComponent(typeof(CharacterController))]
public class DesktopRoomController : MonoBehaviour
{
    public GameObject xrRig;
    public Camera playerCamera;
    public RishiRoomProgress room;
    public float speed = 3.5f;
    public float jumpHeight = 1.1f;
    public bool preferDesktopInEditor = true;
    private CharacterController motor;
    private float verticalSpeed, pitch;
    private bool desktopMode, actionable;
    private string prompt = "";
    private RishiSeal targetSeal;
    private RishiKey targetKey;
    private CollectScroll targetScroll;
    private RishiKeyhole targetKeyhole;
    private RishiKey heldKey;

    private void Awake()
    {
        desktopMode = (Application.isEditor && preferDesktopInEditor) || !XRSettings.isDeviceActive;
        if (playerCamera != null) playerCamera.gameObject.SetActive(desktopMode);
        if (xrRig != null) xrRig.SetActive(!desktopMode);
        motor = GetComponent<CharacterController>();
        motor.enabled = desktopMode;
        if (!desktopMode) { enabled = false; return; }
        if (room == null) room = FindFirstObjectByType<RishiRoomProgress>();
        if (playerCamera == null) { Debug.LogError("Desktop player camera reference is missing."); enabled = false; return; }
        playerCamera.stereoTargetEye = StereoTargetEyeMask.None;
        playerCamera.nearClipPlane = .05f;
    }

    private void Update()
    {
        var keyboard = Keyboard.current;
        var mouse = Mouse.current;
        if (keyboard == null || mouse == null) return;
        bool clicked = mouse.leftButton.wasPressedThisFrame;
        bool use = keyboard.eKey.wasPressedThisFrame;
        if (keyboard.escapeKey.wasPressedThisFrame)
        { Cursor.lockState = CursorLockMode.None; Cursor.visible = true; }
        // E also works while the cursor is released. The first click focuses mouse look.
        if (Cursor.lockState != CursorLockMode.Locked && clicked)
        { Cursor.lockState = CursorLockMode.Locked; Cursor.visible = false; }
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            Vector2 delta = mouse.delta.ReadValue() * .09f;
            transform.Rotate(Vector3.up, delta.x);
            pitch = Mathf.Clamp(pitch - delta.y, -75, 75);
            playerCamera.transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        }
        Vector3 move = Vector3.zero;
        if (keyboard.wKey.isPressed) move += transform.forward;
        if (keyboard.sKey.isPressed) move -= transform.forward;
        if (keyboard.dKey.isPressed) move += transform.right;
        if (keyboard.aKey.isPressed) move -= transform.right;
        if (motor.isGrounded)
        {
            if (verticalSpeed < 0) verticalSpeed = -2;
            if (keyboard.spaceKey.wasPressedThisFrame)
            {
                if (room != null && room.HasStrength) verticalSpeed = Mathf.Sqrt(jumpHeight * 2f * 9.81f);
                else if (room != null) room.Notify("Your legs feel heavy.");
            }
        }
        verticalSpeed -= 9.81f * Time.deltaTime;
        motor.Move((Vector3.ClampMagnitude(move, 1) * speed + Vector3.up * verticalSpeed) * Time.deltaTime);
        if (heldKey != null)
        {
            heldKey.transform.position = playerCamera.transform.position
                + playerCamera.transform.forward * .8f
                + playerCamera.transform.right * .22f;
            heldKey.transform.rotation = playerCamera.transform.rotation;
        }
        UpdateTarget();
        if (clicked || use)
        {
            if (heldKey != null)
            {
                if (targetKeyhole != null && targetKeyhole.TryInsert(heldKey))
                {
                    heldKey = null;
                }
                else if (targetKeyhole == null)
                {
                    var body = heldKey.GetComponent<Rigidbody>();
                    if (body != null)
                    {
                        body.isKinematic = false;
                        body.useGravity = true;
                    }
                    heldKey = null;
                }
            }
            else if (targetKey != null)
            {
                heldKey = targetKey;
                var body = heldKey.GetComponent<Rigidbody>();
                if (body != null)
                {
                    body.linearVelocity = Vector3.zero;
                    body.angularVelocity = Vector3.zero;
                    body.useGravity = false;
                    body.isKinematic = true;
                }
            }
            else if (targetSeal != null) targetSeal.Interact();
            else if (targetScroll != null && room != null) room.CollectRecipe(targetScroll);
        }
    }

    private void UpdateTarget()
    {
        targetSeal = null;
        targetKey = null;
        targetKeyhole = null;
        targetScroll = null;
        actionable = false;
        prompt = "";

        Ray ray = playerCamera.ViewportPointToRay(new Vector3(.5f, .5f, 0));
        var hits = Physics.RaycastAll(ray, 6f, ~0, QueryTriggerInteraction.Collide);
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        foreach (var hit in hits)
        {
            if (hit.transform.IsChildOf(transform)) continue;
            if (xrRig != null && hit.transform.IsChildOf(xrRig.transform)) continue;
            if (heldKey != null && hit.transform.IsChildOf(heldKey.transform)) continue;

            targetKeyhole = hit.collider.GetComponentInParent<RishiKeyhole>();
            targetSeal = hit.collider.GetComponentInParent<RishiSeal>();
            targetKey = hit.collider.GetComponentInParent<RishiKey>();
            targetScroll = hit.collider.GetComponentInParent<CollectScroll>();

            actionable = targetKeyhole != null || targetSeal != null ||
                        targetKey != null || targetScroll != null;

            if (targetKeyhole != null)
                prompt = heldKey != null ? "E / click: insert key" : "";
            else if (targetKey != null && heldKey == null)
                prompt = "E / click: pick up key";
            else if (targetSeal != null)
                prompt = "E / click: interact";
            else if (targetScroll != null)
                prompt = room != null && room.CanCollectScroll
                    ? "E / click: collect scroll" : "Sealed";

            break;
        }
    }

    private void OnGUI()
    {
        if (!desktopMode) return;
        GUI.Box(new Rect(12, 12, Mathf.Min(840, Screen.width - 24), 132), "");
        var text = new GUIStyle(GUI.skin.label) { fontSize = 18, wordWrap = true };
        text.normal.textColor = Color.white;
        string summary = room == null ? "ERROR: room reference missing" : room.Summary + "\n" + room.LastMessage;
        GUI.Label(new Rect(24, 22, Mathf.Min(810, Screen.width - 48), 112), summary, text);
        var center = new GUIStyle(text) { alignment = TextAnchor.MiddleCenter, fontSize = 22 };
        center.normal.textColor = actionable ? Color.green : Color.white;
        GUI.Label(new Rect(Screen.width / 2f - 15, Screen.height / 2f - 15, 30, 30), "+", center);
        GUI.Box(new Rect(12, Screen.height - 92, Screen.width - 24, 80), "");
        GUI.Label(new Rect(24, Screen.height - 87, Screen.width - 48, 40), prompt, text);
        GUI.Label(new Rect(24, Screen.height - 49, Screen.width - 48, 30),
            "WASD walk | Mouse look | E / click interact | Space jump | Esc cursor", text);
    }

    private void OnDisable()
    {
        if (!desktopMode) return;
        Cursor.lockState = CursorLockMode.None; Cursor.visible = true;
    }
}
