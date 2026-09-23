using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RishiRoomProgress : MonoBehaviour
{
    public GameObject runeKey, strengthKey, hiddenKey, strengthSeal;
    public GameObject stoneBlock, secretPanel, hiddenScroll, exitBarrier;
    public TextMesh status;
    public string nextScene = "yunfan_room";
    public int RuneCount { get; private set; }
    public int KeyCount { get; private set; }
    public bool HasStrength { get; private set; }
    public bool PanelOpened { get; private set; }
    public RishiSmashPuzzle SmashPuzzle { get; private set; }
    public bool CanCollectScroll => KeyCount == 3;
    public string LastMessage { get; private set; } = "";
    public bool ScrollCollected => CollectionManager.Instance.IsCollected("Scroll_Rishi");
    public string Summary => "RUNES " + RuneCount + "/3   KEYS " + KeyCount + "/3   SCROLL " +
        (ScrollCollected ? "1/1" : "0/1") + "\nSTRENGTH / JUMP: " + (HasStrength ? "UNLOCKED" : "LOCKED") +
        "   WALL SEALS: " + (SmashPuzzle != null ? SmashPuzzle.BrokenSeals : 0) + "/3";
    private readonly int[] solution = { 2, 0, 1 };
    private readonly bool[] collectedKeys = new bool[3];
    private bool scrollWasCollected;

    private void Awake()
    {
        SetVisible(runeKey, false); SetVisible(strengthKey, false); SetVisible(hiddenKey, false);
        SetVisible(strengthSeal, false);
        SetVisible(secretPanel, true); SetVisible(exitBarrier, true);
        if (hiddenScroll != null)
        {
            hiddenScroll.SetActive(!ScrollCollected);
            var grab = hiddenScroll.GetComponent<XRGrabInteractable>();
            if (grab != null) grab.enabled = false;
            var body = hiddenScroll.GetComponent<Rigidbody>();
            if (body != null) { body.useGravity = false; body.isKinematic = true; }
        }
        scrollWasCollected = ScrollCollected;
        SmashPuzzle = gameObject.AddComponent<RishiSmashPuzzle>();
        SmashPuzzle.Initialize(this);
        foreach (var root in gameObject.scene.GetRootGameObjects())
        foreach (var label in root.GetComponentsInChildren<TextMesh>(true))
        {
            if (label.name == "Entrance clue - read first")
                label.text = "PHYSICAL ENHANCEMENT EXAM\nClouds break. A spark wakes.\nFlames fade. Water remains.";
            else if (label.name == "Scroll display label") label.text = "ELIXIR RECIPE";
        }
        Notify("");
    }

    private static void SetVisible(GameObject obj, bool value) { if (obj != null) obj.SetActive(value); }

    public void PressRune(int rune)
    {
        string name = rune == 0 ? "EMBER" : rune == 1 ? "TIDE" : "STORM";
        if (RuneCount == 3) { Notify(name + " is already lit."); return; }
        if (rune == solution[RuneCount])
        {
            RuneCount++;
            if (RuneCount == 3)
            {
                SetVisible(runeKey, true); SetVisible(strengthSeal, true);
                Notify(name + ": the runes resonate.");
            }
            else Notify(name + ": correct (" + RuneCount + "/3).");
        }
        else { RuneCount = 0; Notify(name + ": the sequence fades."); }
    }

    public void Enhance()
    {
        if (RuneCount != 3) { Notify("The seal is dormant."); return; }
        if (HasStrength) { Notify("Strength is active."); return; }
        HasStrength = true;
        Notify("Strength awakened.");
    }

    public void OpenPanel()
    {
        if (!HasStrength) { Notify("It will not budge."); return; }
        if (SmashPuzzle == null || !SmashPuzzle.WallUnlocked)
        { Notify("The wall remains sealed."); return; }
        if (PanelOpened) return;
        PanelOpened = true;
        if (secretPanel != null)
            RishiPuzzleEffects.Burst(secretPanel.transform.position, secretPanel.GetComponent<Renderer>().sharedMaterial, true);
        SetVisible(secretPanel, false); SetVisible(hiddenKey, true);
        Notify("The wall crumbles.");
    }

    public void GetKey(int id, GameObject key)
    {
        if (id < 0 || id > 2 || collectedKeys[id]) return;
        if ((id == 0 && RuneCount != 3) || (id == 1 && (SmashPuzzle == null || !SmashPuzzle.StoneMoved)) || (id == 2 && !PanelOpened)) return;
        collectedKeys[id] = true; KeyCount++;
        StartCoroutine(RemoveAfterSelect(key));
        if (KeyCount == 3)
        {
            SetVisible(exitBarrier, false);
            if (hiddenScroll != null)
            {
                var grab = hiddenScroll.GetComponent<XRGrabInteractable>();
                if (grab != null) grab.enabled = true;
            }
            Notify("The final lock releases.");
        }
        else Notify("Key collected: " + KeyCount + "/3.");
    }

    private IEnumerator RemoveAfterSelect(GameObject obj) { yield return null; if (obj != null) Destroy(obj); }

    public void CollectRecipe(CollectScroll scroll)
    {
        if (!CanCollectScroll) { Notify("The scroll is sealed."); return; }
        if (scroll != null) scroll.OnGrabbed(); // Keep the team's collection script unchanged.
    }

    public void Exit()
    {
        if (KeyCount != 3) { Notify("The gate is locked."); return; }
        if (!Application.CanStreamedLevelBeLoaded(nextScene))
        { Debug.LogError("Destination missing from Build Profiles: " + nextScene); Notify("The gate is unavailable."); return; }
        SceneManager.LoadScene(nextScene);
    }

    public void Notify(string message)
    {
        LastMessage = message;
        if (status != null) status.text = Summary + "\n" + message;
    }

    private void Update()
    {
        if (!scrollWasCollected && ScrollCollected)
        { scrollWasCollected = true; Notify("Recipe fragment collected."); }
    }
}
