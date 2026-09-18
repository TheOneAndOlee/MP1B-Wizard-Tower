using UnityEngine;

public class BrickHighlight : MonoBehaviour
{
    private Renderer targetRenderer;
    public Material highlightMaterial;

    public void Show()
    {
        targetRenderer = GetComponent<Renderer>();
        targetRenderer.material = highlightMaterial;
    }
}