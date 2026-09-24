using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using System.Collections;
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

    public string Summary => "RUNES " + RuneCount + "/3   KEYS " + KeyCount +
        "/3   SCROLL " + (ScrollCollected ? "1/1" : "0/1") +
        "\nSTRENGTH / JUMP: " + (HasStrength ? "UNLOCKED" : "LOCKED") +
        "   WALL SEALS: " +
        (SmashPuzzle != null ? SmashPuzzle.BrokenSeals : 0) + "/3";

    private readonly int[] solution = { 2, 0, 1 };
    private readonly bool[] insertedKeys = new bool[3];
    private bool scrollWasCollected;

    private void Awake()
    {
        // The existing scene has "yunfan_room" serialized in its Inspector.
        // Correct that old value without requiring an edit to the scene asset.
        nextScene = "yunfan_room";

        // The three old yellow objects are spawn markers, not the keys.
        SetVisible(runeKey, false);
        SetVisible(strengthKey, false);
        SetVisible(hiddenKey, false);
        SetVisible(strengthSeal, false);
        SetVisible(secretPanel, true);
        SetVisible(exitBarrier, true);

        CreateKeyholes();

        if (hiddenScroll != null)
        {
            hiddenScroll.SetActive(!ScrollCollected);
            var grab = hiddenScroll.GetComponent<XRGrabInteractable>();
            if (grab != null) grab.enabled = false;

            var body = hiddenScroll.GetComponent<Rigidbody>();
            if (body != null)
            {
                body.useGravity = false;
                body.isKinematic = true;
            }
        }

        scrollWasCollected = ScrollCollected;

        SmashPuzzle = gameObject.AddComponent<RishiSmashPuzzle>();
        SmashPuzzle.Initialize(this);

        foreach (var root in gameObject.scene.GetRootGameObjects())
        foreach (var label in root.GetComponentsInChildren<TextMesh>(true))
        {
            if (label.name == "Entrance clue - read first")
                label.text = "PHYSICAL ENHANCEMENT EXAM\nClouds break. A spark wakes.\nFlames fade. Water remains.";
            else if (label.name == "Scroll display label")
                label.text = "ELIXIR RECIPE";
        }

        Notify("");
    }

    private static void SetVisible(GameObject obj, bool value)
    {
        if (obj != null) obj.SetActive(value);
    }

    private void CreateKeyholes()
    {
        if (exitBarrier == null) return;

        Transform plate = exitBarrier.transform;

        // Give three separate plaques enough room across the cover.
        var coverRenderer = plate.GetComponent<Renderer>();
        if (coverRenderer != null)
        {
            Vector3 size = plate.localScale;

            size.x *= Mathf.Max(1f, 3.6f /
                Mathf.Max(.001f, coverRenderer.bounds.size.x));

            size.y *= Mathf.Max(1f, 3.1f /
                Mathf.Max(.001f, coverRenderer.bounds.size.y));

            plate.localScale = size;
        }

        string[] inscriptions =
        {
            "SKY\nASH\nTIDE",
            "BENEATH\nTHE\nWEIGHT",
            "THREE\nBREAK\nTHE\nLIE"
        };

        Color[] colors =
        {
            new Color(.72f, .50f, 1f),  // Runes: violet
            new Color(.82f, .88f, 1f),  // Stone: silver
            new Color(.35f, .88f, .75f) // Hidden wall: teal
        };

        for (int i = 0; i < 3; i++)
        {
            var socket = new GameObject("Keyhole " + (i + 1));

            Vector3 front = plate.TransformPoint(
                new Vector3((i - 1) * .29f, 0, .5f));

            socket.transform.SetPositionAndRotation(
                front + plate.forward * .025f, plate.rotation);
            socket.transform.SetParent(plate, true);

            var hole = socket.AddComponent<RishiKeyhole>();
            hole.room = this;
            hole.keyId = i;
            hole.BuildVisual();

            AddLockSignifier(socket.transform, i, inscriptions[i], colors[i]);
        }
    }

    private void AddLockSignifier(
        Transform socket, int index, string inscription, Color color)
    {
        var sign = new GameObject("Signifier " + (index + 1));
        sign.transform.SetParent(socket, false);
        sign.transform.localPosition = new Vector3(0, .51f, .025f);

        // A separate plaque attached directly to this lock.
        AddSignPiece(sign.transform, "Plaque",
            Vector3.zero, new Vector3(.76f, .38f, .025f),
            new Color(.09f, .08f, .12f));

        if (index == 0)
        {
            // Three runes.
            for (int n = -1; n <= 1; n++)
                AddSignPiece(sign.transform, "Rune " + (n + 2),
                    new Vector3(n * .105f, .085f, .025f),
                    new Vector3(.065f, .065f, .015f), color, 45f);
        }
        else if (index == 1)
        {
            // A heavy stone over a smaller mark.
            AddSignPiece(sign.transform, "Weight",
                new Vector3(0, .10f, .025f),
                new Vector3(.20f, .09f, .015f), color);
            AddSignPiece(sign.transform, "Buried mark",
                new Vector3(0, .035f, .025f),
                new Vector3(.075f, .025f, .015f), color);
        }
        else
        {
            // Two halves of a broken wall.
            AddSignPiece(sign.transform, "Left stone",
                new Vector3(-.065f, .085f, .025f),
                new Vector3(.095f, .12f, .015f), color, -12f);
            AddSignPiece(sign.transform, "Right stone",
                new Vector3(.065f, .085f, .025f),
                new Vector3(.095f, .12f, .015f), color, 12f);
        }

        var words = new GameObject("Inscription");
        words.transform.SetParent(sign.transform, false);
        words.transform.localPosition = new Vector3(0, -.075f, .055f);
        words.transform.localRotation = Quaternion.Euler(0, 180f, 0);
        words.transform.localScale = Vector3.one * .03f;

        var text = words.AddComponent<TextMesh>();
        text.text = inscription;
        text.anchor = TextAnchor.MiddleCenter;
        text.alignment = TextAlignment.Center;
        text.fontSize = 64;
        text.fontStyle = FontStyle.Bold;
        text.color = color;
    }

    private void AddSignPiece(
        Transform parent, string pieceName, Vector3 position,
        Vector3 size, Color color, float zRotation = 0f)
    {
        var piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.name = pieceName;
        piece.transform.SetParent(parent, false);
        piece.transform.localPosition = position;
        piece.transform.localRotation = Quaternion.Euler(0, 0, zRotation);
        piece.transform.localScale = size;

        piece.GetComponent<Collider>().enabled = false;
        piece.GetComponent<Renderer>().material.color = color;
    }

    private void RevealKey(GameObject marker, int id)
    {
        if (marker == null) return;

        GameObject prefab = Resources.Load<GameObject>("Key_Door");
        if (prefab == null)
        {
            Debug.LogError("Missing Assets/Resources/Key_Door.prefab");
            return;
        }

        Vector3 position = marker.transform.position + Vector3.up * .12f;
        marker.SetActive(false);

        GameObject key = Instantiate(prefab, position, prefab.transform.rotation);
        key.name = "Rishi key " + (id + 1);

        var rishiKey = key.GetComponent<RishiKey>();
        if (rishiKey == null)
            rishiKey = key.AddComponent<RishiKey>();

        rishiKey.room = this;
        rishiKey.keyId = id;
    }

    private IEnumerator OpenCoverAfterClick()
    {
        yield return new WaitForSeconds(.6f);
        SetVisible(exitBarrier, false);
    }

    public void PressRune(int rune)
    {
        string name = rune == 0 ? "EMBER" : rune == 1 ? "TIDE" : "STORM";

        if (RuneCount == 3)
        {
            Notify(name + " is already lit.");
            return;
        }

        if (rune == solution[RuneCount])
        {
            RuneCount++;
            if (RuneCount == 3)
            {
                RevealKey(runeKey, 0);
                SetVisible(strengthSeal, true);
                Notify(name + ": the runes resonate.");
            }
            else Notify(name + ": correct (" + RuneCount + "/3).");
        }
        else
        {
            RuneCount = 0;
            Notify(name + ": the sequence fades.");
        }
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
        {
            Notify("The wall remains sealed.");
            return;
        }
        if (PanelOpened) return;

        PanelOpened = true;
        if (secretPanel != null)
            RishiPuzzleEffects.Burst(
                secretPanel.transform.position,
                secretPanel.GetComponent<Renderer>().sharedMaterial, true);

        SetVisible(secretPanel, false);
        RevealKey(hiddenKey, 2);
        Notify("The wall crumbles.");
    }

    // Called by the existing stone puzzle once the heavy block has moved.
    public void RevealStrengthKey()
    {
        RevealKey(strengthKey, 1);
    }

    // This now counts inserted keys, not keys that have merely been picked up.
    public bool InsertKey(int id)
    {
        if (id < 0 || id >= insertedKeys.Length || insertedKeys[id])
            return false;

        insertedKeys[id] = true;
        KeyCount++;

        if (KeyCount == 3)
        {
            // Hide the cover after all three keys have physically entered
            // their openings. Its keyholes are children, so they go with it.
            StartCoroutine(OpenCoverAfterClick());

            if (hiddenScroll != null)
            {
                var grab = hiddenScroll.GetComponent<XRGrabInteractable>();
                if (grab != null) grab.enabled = true;
            }

            Notify("The final lock releases.");
        }
        else Notify("A lock clicks into place: " + KeyCount + "/3.");

        return true;
    }

    public void CollectRecipe(CollectScroll scroll)
    {
        if (!CanCollectScroll) { Notify("The scroll is sealed."); return; }
        if (scroll != null) scroll.OnGrabbed();
    }

    public void Exit()
    {
        if (KeyCount != 3) { Notify("The gate is locked."); return; }

        if (!Application.CanStreamedLevelBeLoaded(nextScene))
        {
            Debug.LogError("Destination missing from Build Profiles: " + nextScene);
            Notify("The gate is unavailable.");
            return;
        }

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
        {
            scrollWasCollected = true;
            Notify("Recipe fragment collected.");
        }
    }
}