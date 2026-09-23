using UnityEngine;

public class MagicAbsorbingCrystal : MonoBehaviour
{
    [SerializeField] private Material activatedMaterial;
    private bool _isActivated = false;
    
    // private bool _isGrabbable = false;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<Rigidbody>().useGravity = false;
        GetComponent<MeshCollider>().enabled = false;
    }

    public void EnableGrabbing()
    {
        Debug.Log("Enabling Grabbing");
        // GetComponent<Rigidbody>().isKinematic = false;
        GetComponent<MeshCollider>().enabled = true;
        GetComponent<Rigidbody>().useGravity = true;    
    }

    public void Activate()
    {
        _isActivated = true;
        GetComponent<MeshRenderer>().material = activatedMaterial;
    }
}
