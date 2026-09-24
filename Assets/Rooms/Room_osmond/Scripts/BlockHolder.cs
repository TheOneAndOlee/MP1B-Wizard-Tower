using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

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

    public void SetElement(SelectEnterEventArgs args)
    {
        GameObject cube = args.interactableObject.transform.gameObject;
        
        if (cube.GetComponent<ElementalCube>() != null)
        {
            Debug.Log("Setting element to " + cube.GetComponent<ElementalCube>().element);
            element = cube.GetComponent<ElementalCube>().element;
        }
        else
        {
            Debug.LogWarning("Inputted object isn't an elemental cube");
        }
        // element = cube.element
        
    }

    public void RemoveElement(SelectExitEventArgs args)
    {
        element = Element.None;
    }
}
