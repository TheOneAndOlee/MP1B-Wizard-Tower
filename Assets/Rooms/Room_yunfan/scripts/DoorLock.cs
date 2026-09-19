using UnityEngine;
using System.Collections;

public class DoorLock : MonoBehaviour
{
    public Transform doorPanel;
    public float openAngle = -90f;
    public float duration = 1.2f;
    private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (triggered) return;
        if (!other.CompareTag("DoorKey")) return;

        triggered = true;
        StartCoroutine(Open());
    }

    private IEnumerator Open()
    {
        Quaternion endRotation = doorPanel.rotation * Quaternion.Euler(0f, openAngle, 0f);
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            doorPanel.rotation = Quaternion.Slerp(doorPanel.rotation, endRotation, t);
            yield return null;
        }

        doorPanel.rotation = endRotation;
    }
}
