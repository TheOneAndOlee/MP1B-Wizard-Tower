using UnityEngine;

public class HiddenTextVisibility : MonoBehaviour
{
    public Transform viewer; 
    public float viewAngle = 25f;
    public CanvasGroup canvasGroup;

    private void Update()
    {
        Vector3 viewerDirection = viewer.position - transform.position;
        viewerDirection.y = 0f;
        Vector3 forwardDirection = -transform.forward;
        forwardDirection.y = 0f;
        float angle = Vector3.Angle(forwardDirection.normalized, viewerDirection.normalized);

        canvasGroup.alpha = (angle <= viewAngle) ? 1f : 0f;
    }
}
