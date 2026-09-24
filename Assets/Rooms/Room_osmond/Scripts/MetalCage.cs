using System.Collections;
using UnityEngine;
using UnityEngine.Events;


public class MetalCage : MonoBehaviour
{
    public UnityEvent onOpened;

    [SerializeField] private float openDuration = 3f;
    [SerializeField] private GameObject door;
    [SerializeField] private float travelDist = 1f;

    private bool _isOpening = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    void Awake()
    {
        // OpenDoor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenDoor()
    {
        Debug.Log("Opening Door");
        // Vector3 currentDoorPosition = door.GetComponent<Transform>().position;
        // door.GetComponent<Transform>().position = Vector3.Lerp(currentDoorPosition, new Vector3(currentDoorPosition.x, currentDoorPosition.y + 1f, currentDoorPosition.z), 5f);
        StartCoroutine(OpenDoorCoroutine());
    }

    private IEnumerator OpenDoorCoroutine()
    {
       _isOpening = true;
       Debug.Log("Opening Door");
       
       Vector3 startPosition = door.GetComponent<Transform>().position;
       Vector3 endPosition = new Vector3(startPosition.x, startPosition.y + travelDist, startPosition.z);
       
       float elapsedTime = 0f;

       while (elapsedTime < openDuration)
       {
           float t = elapsedTime / openDuration;
           
           t = Mathf.SmoothStep(0f, 1f, t);
           
           door.transform.position = Vector3.Lerp(startPosition, endPosition, t);
           
           elapsedTime +=  Time.deltaTime;
           yield return null;
       }
       
       door.transform.position = endPosition;
       onOpened.Invoke();
    }
}
