using UnityEngine;

public class FrostBolt : MonoBehaviour
{
    [SerializeField] private float travelSpeed = 5.0f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // gameObject.tag = "FrostBolt";
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * travelSpeed * Time.deltaTime);
    }
    
    
}
