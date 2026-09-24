using System.Collections;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RishiSmashPuzzle : MonoBehaviour
{
    private RishiRoomProgress room;
    private readonly GameObject[] vessels = new GameObject[4];
    private readonly string[] symbols = { "MOON", "DAWN", "SUN", "STAR" };
    private readonly bool[] broken = new bool[4];
    private RishiStoneFlight stone;
    private TextMesh clue;
    private bool launched, resetting;
    public bool StoneMoved { get; private set; }
    public int BrokenSeals { get; private set; }
    public bool WallUnlocked => BrokenSeals == 3;

    public void Initialize(RishiRoomProgress owner)
    {
        room = owner;
        var block = room.stoneBlock;
        var body = block.GetComponent<Rigidbody>();
        if (body == null) body = block.AddComponent<Rigidbody>();
        body.isKinematic = true; body.useGravity = false; body.mass = 8f;
        stone = block.GetComponent<RishiStoneFlight>();
        if (stone == null) stone = block.AddComponent<RishiStoneFlight>();
        stone.puzzle = this;
        AddSeal(block, RishiSeal.Action.Stone, 0);
        var positions = new[] { new Vector3(-4, .95f, .5f), new Vector3(2.7f, .95f, 1f),
            new Vector3(-1.6f, .95f, -3.2f), new Vector3(3.3f, .95f, 3.5f) };
        var shapes = new[] { PrimitiveType.Cylinder, PrimitiveType.Cube, PrimitiveType.Sphere, PrimitiveType.Capsule };
        var sizes = new[] { new Vector3(.65f, .8f, .65f), new Vector3(.85f, 1.3f, .35f),
            new Vector3(.95f, .95f, .95f), new Vector3(.55f, .8f, .55f) };
        var descriptions = new[] { "pillar", "tablet", "orb", "idol" };
        var sourceRenderer = block.GetComponent<Renderer>();
        for (int i = 0; i < vessels.Length; i++)
        {
            var vessel = GameObject.CreatePrimitive(shapes[i]);
            vessel.name = symbols[i] + " breakable " + descriptions[i];
            float halfHeight = sizes[i].y * (shapes[i] == PrimitiveType.Cylinder || shapes[i] == PrimitiveType.Capsule ? 1f : .5f);
            positions[i].y = halfHeight + .02f;
            vessel.transform.position = positions[i]; vessel.transform.localScale = sizes[i];
            vessel.GetComponent<Renderer>().sharedMaterial = sourceRenderer.sharedMaterial;
            AddSeal(vessel, RishiSeal.Action.Vessel, i);
            vessels[i] = vessel;
            Label(symbols[i], positions[i] + Vector3.up * (halfHeight + .3f));
        }
        clue = Label("SMASH THE NIGHT, THEN THE FIRST LIGHT,\nTHEN THE BRIGHTEST SKY.\nFALSE STARS RESTORE EVERY SEAL.", new Vector3(0, 1.55f, -2.4f));
        clue.gameObject.SetActive(false);
    }

    private TextMesh Label(string text, Vector3 position)
    {
        var go = new GameObject("Smash puzzle clue: " + text.Split('\n')[0]);
        var label = go.AddComponent<TextMesh>();
        label.text = text; label.fontSize = 32; label.anchor = TextAnchor.MiddleCenter;
        label.alignment = TextAlignment.Center; label.color = new Color(1f, .86f, .48f);
        go.transform.SetPositionAndRotation(position, Quaternion.Euler(0, 180, 0));
        go.transform.localScale = Vector3.one * .085f;
        return label;
    }

    private void AddSeal(GameObject obj, RishiSeal.Action action, int index)
    {
        if (obj.GetComponent<XRSimpleInteractable>() == null) obj.AddComponent<XRSimpleInteractable>();
        var seal = obj.GetComponent<RishiSeal>();
        if (seal == null) seal = obj.AddComponent<RishiSeal>();
        seal.room = room; seal.action = action; seal.runeIndex = index;
    }

    public void LaunchStone()
    {
        if (!room.HasStrength) { room.Notify("Too heavy."); return; }
        if (launched) { room.Notify(""); return; }
        launched = true; stone.Launch();
        room.Notify("");
    }
    public void BlockCleared()
    {
        StoneMoved = true;
        room.RevealStrengthKey(); clue.gameObject.SetActive(true);
        room.Notify("");
    }
    public void WallImpact() { room.Notify(""); }

    public void SmashVessel(int index)
    {
        if (!room.HasStrength) { room.Notify("The stone resists."); return; }
        if (!StoneMoved) { room.Notify("The relic remains sealed."); return; }
        if (resetting || WallUnlocked || index < 0 || index >= 4 || broken[index]) return;
        var obj = vessels[index];
        RishiPuzzleEffects.Burst(obj.transform.position, obj.GetComponent<Renderer>().sharedMaterial, true);
        obj.SetActive(false);
        if (index != BrokenSeals)
        {
            room.Notify(symbols[index] + " shatters. The seals reform.");
            StartCoroutine(ResetVessels()); return;
        }
        broken[index] = true; BrokenSeals++;
        room.Notify(WallUnlocked ? "The last seal fractures." :
            symbols[index] + " shattered. Wall seals broken: " + BrokenSeals + "/3.");
    }
    private IEnumerator ResetVessels()
    {
        resetting = true;
        yield return new WaitForSeconds(.8f);
        BrokenSeals = 0;
        for (int i = 0; i < vessels.Length; i++) { broken[i] = false; vessels[i].SetActive(true); }
        resetting = false;
        room.Notify("The relics reform.");
    }
}
