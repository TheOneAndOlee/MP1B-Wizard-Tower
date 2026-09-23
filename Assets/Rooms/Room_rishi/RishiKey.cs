using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRGrabInteractable))]
public class RishiKey : MonoBehaviour
{
    public RishiRoomProgress room;
    public int keyId;

    private void OnEnable() { GetComponent<XRGrabInteractable>().selectEntered.AddListener(OnSelected); }
    private void OnDisable() { GetComponent<XRGrabInteractable>().selectEntered.RemoveListener(OnSelected); }
    private void OnSelected(SelectEnterEventArgs args) { if (room != null) room.GetKey(keyId, gameObject); }
    public void DesktopCollect() { if (room != null) room.GetKey(keyId, gameObject); }
}
