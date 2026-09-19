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
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, t);
            yield return null;
        }

        transform.localScale = Vector3.zero;
        Destroy(gameObject);
    }
}
