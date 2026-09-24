using UnityEngine;

public class MagicAbsorbingCrystal : MonoBehaviour
{
    [SerializeField] private Material activatedMaterial;
    private bool _isActivated = false;
    
    private Rigidbody _rigidbody;
    private MeshCollider _meshCollider;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // GetComponent<Rigidbody>().isKinematic = true;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        
        _meshCollider = GetComponent<MeshCollider>();
        _meshCollider.enabled = false;
    }

    public void EnableGrabbing()
    {
        Debug.Log("Enabling Grabbing");
        // GetComponent<Rigidbody>().isKinematic = false;
        _rigidbody.useGravity = true;
        _meshCollider.enabled = true;
    }

    public void Activate()
    {
        _isActivated = true;
        GetComponent<MeshRenderer>().material = activatedMaterial;
    }
}
