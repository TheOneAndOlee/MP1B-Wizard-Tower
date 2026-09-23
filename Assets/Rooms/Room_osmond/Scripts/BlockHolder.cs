using UnityEngine;

public class BlockHolder : MonoBehaviour
{
    public Element element;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetElement(ElementalCube cube)
    {
        element = cube.element;
    }

    public void RemoveElement()
    {
        element = Element.None;
    }
}
