using UnityEngine;

public class FrostBook : MonoBehaviour
{
    [SerializeField] private GameObject barrier;    
    
    [SerializeField] private GameObject frostBoltPrefab;
    
    private MagicBarrier _magicBarrier;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        GetComponent<Light>().enabled = true;
        GetComponent<Rigidbody>().isKinematic = false;
        _magicBarrier = barrier.GetComponent<MagicBarrier>();
        _magicBarrier.enableBook.AddListener(EnableFunctionality);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void EnableFunctionality()
    {
        GetComponent<Light>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
    }

    void FireBolt()
    {
        GameObject frostBolt = Instantiate(frostBoltPrefab, transform.position, transform.rotation);
    }
}
