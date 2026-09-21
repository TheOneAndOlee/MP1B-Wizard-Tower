using System;
using UnityEngine;

public class LavaFloor : MonoBehaviour
{
    [SerializeField] private GameObject lavaFloor;
    [SerializeField] private Material obsidian;
    
    private BoxCollider _floorCollider;
    private MeshRenderer _meshRenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _floorCollider = lavaFloor.GetComponent<BoxCollider>();
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
        }
    }
}
