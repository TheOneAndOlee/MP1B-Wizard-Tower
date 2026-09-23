using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

[RequireComponent(typeof(XRSimpleInteractable))]
public class RishiSeal : MonoBehaviour
{
    public enum Action { Rune, Enhance, Panel, Exit, Stone, Vessel }
    public RishiRoomProgress room;
    public Action action;
    public int runeIndex;
    private Coroutine flash;
    private void OnEnable() { GetComponent<XRSimpleInteractable>().selectEntered.AddListener(OnSelected); }
    private void OnDisable()
    {
        GetComponent<XRSimpleInteractable>().selectEntered.RemoveListener(OnSelected);
        var renderer = GetComponent<Renderer>();
        if (renderer != null) renderer.SetPropertyBlock(null);
        flash = null;
    }
    private void OnSelected(SelectEnterEventArgs args) { Interact(); }
    public void Interact()
    {
        if (room == null) { Debug.LogError("Room reference missing on " + name, this); return; }
        // Every accepted input gives visible feedback, even a wrong rune.
        if (flash != null) StopCoroutine(flash);
        flash = StartCoroutine(Flash());
        switch (action)
        {
            case Action.Rune:
                RishiPuzzleEffects.Burst(transform.position + Vector3.forward * .22f, GetComponent<Renderer>().sharedMaterial);
                room.PressRune(runeIndex); break;
            case Action.Enhance: room.Enhance(); break;
            case Action.Panel: room.OpenPanel(); break;
            case Action.Exit: room.Exit(); break;
            case Action.Stone: room.SmashPuzzle.LaunchStone(); break;
            case Action.Vessel: room.SmashPuzzle.SmashVessel(runeIndex); break;
        }
    }
    private IEnumerator Flash()
    {
        var r = GetComponent<Renderer>();
        if (r == null) yield break;
        var block = new MaterialPropertyBlock();
        block.SetColor("_BaseColor", Color.white); block.SetColor("_Color", Color.white);
        r.SetPropertyBlock(block);
        yield return new WaitForSeconds(.18f);
        if (r != null) r.SetPropertyBlock(null);
        flash = null;
    }
}
