using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class RishiStoneFlight : MonoBehaviour
{
    public RishiSmashPuzzle puzzle;
    private Vector3 initialPosition;
    private bool launched, cleared, hitWall;
    public void Launch()
    {
        if (launched) return;
        launched = true;
        initialPosition = transform.position;
        var body = GetComponent<Rigidbody>();
        body.isKinematic = false; body.useGravity = true;
        body.collisionDetectionMode = CollisionDetectionMode.ContinuousDynamic;
        body.AddForce(new Vector3(9f, 3f, 0), ForceMode.VelocityChange);
        body.AddTorque(new Vector3(1.8f, 2.5f, 1.2f), ForceMode.VelocityChange);
        RishiPuzzleEffects.Burst(transform.position, GetComponent<Renderer>().sharedMaterial, true);
    }
    private void FixedUpdate()
    {
        if (launched && !cleared && Vector3.Distance(initialPosition, transform.position) > 1.4f)
        { cleared = true; puzzle.BlockCleared(); }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (!launched) return;
        if (collision.relativeVelocity.magnitude > 1.5f)
            RishiPuzzleEffects.Burst(collision.GetContact(0).point, GetComponent<Renderer>().sharedMaterial, true);
        if (!hitWall && collision.gameObject.name.ToLowerInvariant().Contains("wall"))
        { hitWall = true; puzzle.WallImpact(); }
    }
}
