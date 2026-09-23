using UnityEngine;

public class FrostBook : MonoBehaviour
{
    // [SerializeField] private GameObject barrier;    
    
    [SerializeField] private GameObject frostBoltPrefab;
    
    private MeshCollider  _meshCollider;
    private Rigidbody _rigidbody;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Light>().enabled = true;
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.useGravity = false;
        
        _meshCollider = GetComponent<MeshCollider>();
        _meshCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableFunctionality()
    {
        
        GetComponent<Light>().enabled = false;
        // _rigidbody.isKinematic = true;
        _rigidbody.useGravity = true;
        _meshCollider.enabled = true;
    }

    void FireBolt()
    {
        GameObject frostBolt = Instantiate(frostBoltPrefab, transform.position, transform.rotation);
    }
}
