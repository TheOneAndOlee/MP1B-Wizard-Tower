using UnityEngine;

public class BrickLock : MonoBehaviour
{
    public float duration = 0.6f;
    private bool triggered = false;

    private void OnTriggerEnter(Collider hammer)
    {
        if (triggered) return;
        if (!hammer.CompareTag("Hammer")) return;

        triggered = true;
        StartCoroutine(Break());
    }

    private System.Collections.IEnumerator Break()
    {
        float time = 0f;
        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            float eased = t * t * (3f - 2f * t);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, eased);
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
