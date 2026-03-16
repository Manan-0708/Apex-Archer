using System.Collections;
using UnityEngine;

public class MovingTarget : MonoBehaviour, IHittable
{
    private Rigidbody rb;
    private bool stopped = false;

    private Vector3 nextPosition;
    private Vector3 originPosition;
    private float baseHeight;

    [SerializeField] private int health = 1;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private float arriveThreshold = 0.2f;
    [SerializeField] private float movementRadius = 2f;
    [SerializeField] private float speed = 1f;

    [Header("Vertical Movement")]
    [SerializeField] private float verticalHeight = 0.05f;
    [SerializeField] private float verticalSpeed = 1f;

    [SerializeField] private float destroyDelay = 1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();

        originPosition = transform.position;
        baseHeight = transform.position.y;

        nextPosition = GetNewMovementPosition();
    }

    private Vector3 GetNewMovementPosition()
    {
        return originPosition + (Vector3)Random.insideUnitCircle * movementRadius;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            audioSource?.Play();
        }
    }

    public void GetHit()
    {
        if (stopped) return;

        health--;

        if (health <= 0)
        {
            stopped = true;
            rb.isKinematic = false;
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private void FixedUpdate()
    {
        if (stopped) return;

        if (Vector3.Distance(transform.position, nextPosition) < arriveThreshold)
        {
            nextPosition = GetNewMovementPosition();
        }

        Vector3 direction = nextPosition - transform.position;
        Vector3 move = direction.normalized * Time.fixedDeltaTime * speed;

        rb.MovePosition(new Vector3(
            transform.position.x + move.x,
            transform.position.y,
            transform.position.z + move.z
        ));

        float verticalOffset =
            Mathf.Sin(Time.time * verticalSpeed) * verticalHeight;

        transform.position = new Vector3(
            transform.position.x,
            baseHeight + verticalOffset,
            transform.position.z
        );
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(destroyDelay);

        FindFirstObjectByType<TargetSpawner>()?.OnTargetDestroyed();

        Destroy(gameObject);
    }
}
