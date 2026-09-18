using System.Collections;
using UnityEngine;

public class BottleWobble : MonoBehaviour
{
    public float wobbleAngle = 15f;
    public float duration = 1f;
    public int count = 4; 

    private void Start()
    {
        StartCoroutine(Wobble());
    }

    private IEnumerator Wobble()
    {
        Quaternion startRotation = transform.localRotation;
        float time = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float decay = 1f - t;
            float angle = Mathf.Sin(t * count * Mathf.PI * 2f) * wobbleAngle * decay;
            transform.localRotation = startRotation * Quaternion.Euler(0f, 0f, angle);
            yield return null;
        }
        transform.localRotation = startRotation;
    }
}
