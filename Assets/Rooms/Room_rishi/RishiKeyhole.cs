using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class RishiKeyhole : MonoBehaviour
{
    public RishiRoomProgress room;
    public int keyId;

    private bool filled;

    public void BuildVisual()
    {
        // Dark opening and four gold edges, all created at runtime.
        MakePiece("Opening", new Vector3(0, 0, 0),
            new Vector3(.19f, .24f, .012f), new Color(.035f, .025f, .02f));

        Color gold = new Color(.7f, .48f, .13f);
        MakePiece("Left edge", new Vector3(-.11f, 0, .008f),
            new Vector3(.025f, .29f, .025f), gold);
        MakePiece("Right edge", new Vector3(.11f, 0, .008f),
            new Vector3(.025f, .29f, .025f), gold);
        MakePiece("Top edge", new Vector3(0, .135f, .008f),
            new Vector3(.245f, .025f, .025f), gold);
        MakePiece("Bottom edge", new Vector3(0, -.135f, .008f),
            new Vector3(.245f, .025f, .025f), gold);

        // The opening itself detects a key held up to the plate.
        var trigger = gameObject.AddComponent<BoxCollider>();
        trigger.isTrigger = true;
        trigger.center = new Vector3(0, 0, .12f);
        trigger.size = new Vector3(.23f, .30f, .34f);
    }

    private void MakePiece(string pieceName, Vector3 position, Vector3 size, Color color)
    {
        var piece = GameObject.CreatePrimitive(PrimitiveType.Cube);
        piece.name = pieceName;
        piece.transform.SetParent(transform, false);
        piece.transform.localPosition = position;
        piece.transform.localScale = size;
        piece.GetComponent<Collider>().enabled = false;
        piece.GetComponent<Renderer>().material.color = color;
    }

    private void OnTriggerStay(Collider other)
    {
        var key = other.GetComponentInParent<RishiKey>();
        if (key != null)
            TryInsert(key);
    }

    public bool TryInsert(RishiKey key)
    {
        if (filled || key == null || key.IsInserted || key.keyId != keyId)
            return false;

        if (Vector3.Distance(key.transform.position, transform.position) > .45f)
            return false;

        if (room == null || !room.InsertKey(keyId))
            return false;

        filled = true;
        key.IsInserted = true;

        // Show which of the three locks has been filled.
        foreach (Transform part in transform)
        {
            var renderer = part.GetComponent<Renderer>();
            if (renderer != null)
                renderer.material.color = new Color(.15f, .9f, .45f);
        }
        var grab = key.GetComponent<XRGrabInteractable>();
        if (grab != null)
        {
            // Explicitly release it from any XR hand holding it.
            if (grab.interactionManager != null)
            {
                for (int i = grab.interactorsSelecting.Count - 1; i >= 0; i--)
                    grab.interactionManager.SelectExit(
                        grab.interactorsSelecting[i], grab);
            }

            grab.enabled = false;
        }

        // SetActive hides it immediately; Destroy cleans it up afterward.
        key.gameObject.SetActive(false);
        Destroy(key.gameObject);
        return true;
    }
}