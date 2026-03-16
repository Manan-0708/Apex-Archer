using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float rotationSpeed = 15f;

    private void FixedUpdate()
    {
        if (rb.linearVelocity.sqrMagnitude > 0.01f) // <-- KEY LINE
        {
            Vector3 dir = rb.linearVelocity.normalized;
            transform.forward = Vector3.Slerp(transform.forward, dir, Time.fixedDeltaTime * rotationSpeed);
        }
    }
}
