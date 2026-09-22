using System;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    [SerializeField] private GameObject lavaFloor;
    [SerializeField] private Material obsidian;
    [SerializeField] private GameObject invisWall;
    
    private BoxCollider _floorCollider;
    private MeshRenderer _meshRenderer;
    private Light _light;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _floorCollider = lavaFloor.GetComponent<BoxCollider>();
        _meshRenderer = lavaFloor.GetComponent<MeshRenderer>();
        _light = lavaFloor.GetComponent<Light>();
        _floorCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("FrostBolt")) 
        {
            _floorCollider.enabled = true;
            _meshRenderer.material = obsidian;
            _light.intensity = 7.5f;
            Destroy(invisWall);
            Destroy(other);
        }
    }
}
