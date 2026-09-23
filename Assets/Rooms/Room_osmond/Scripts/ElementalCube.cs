using UnityEngine;

public enum Element
{
    None,
    Fire,
    Water,
    Wind,
    Earth
}

public class ElementalCube : MonoBehaviour
{
    public Element element;

    public Material[] elementMaterials;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (element == Element.Fire)
        {
            GetComponent<MeshRenderer>().material = elementMaterials[0];
        }
        else if (element == Element.Water)
        {
            GetComponent<MeshRenderer>().material = elementMaterials[1];
        }
        else if (element == Element.Wind)
        {
            GetComponent<MeshRenderer>().material = elementMaterials[2];
        }
        else if (element == Element.Earth)
        {
            GetComponent<MeshRenderer>().material = elementMaterials[3];
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
