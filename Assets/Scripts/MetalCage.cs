using UnityEngine;
using UnityEngine.Events;

public class MetalCage : MonoBehaviour
{
    public UnityEvent onOpened;

    [SerializeField] private GameObject door;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        Vector3 currentDoorPosition = door.GetComponent<Transform>().position;
        door.GetComponent<Transform>().position = Vector3.Lerp(currentDoorPosition, new Vector3(currentDoorPosition.x, currentDoorPosition.y + 1f, currentDoorPosition.z), 2f);
        onOpened.Invoke();
    }
}
