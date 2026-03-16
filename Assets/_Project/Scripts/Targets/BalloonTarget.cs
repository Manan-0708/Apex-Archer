using UnityEngine;

public class BalloonTarget : MonoBehaviour, IHittable
{
    [Header("Scoring")]
    [SerializeField] private int points = 20;

    [Header("Pop Effect")]
    [SerializeField] private GameObject popEffectPrefab;
    [SerializeField] private AudioSource popSound;

    [Header("Floating Motion")]
    [SerializeField] private float floatHeight = 0.3f;
    [SerializeField] private float floatSpeed = 1.5f;
    [SerializeField] private float swayAmount = 0.2f;
    [SerializeField] private float swaySpeed = 0.8f;

    private bool popped = false;
    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (popped) return;

        float verticalOffset =
            Mathf.Sin(Time.time * floatSpeed) * floatHeight;

        float horizontalOffset =
            Mathf.Sin(Time.time * swaySpeed) * swayAmount;

        transform.position = new Vector3(
            startPosition.x + horizontalOffset,
            startPosition.y + verticalOffset,
            startPosition.z
        );
    }

    public void GetHit()
    {
        if (popped) return;

        popped = true;

        SessionTracker.Instance?.OnHit(points);

        if (popEffectPrefab != null)
            Instantiate(popEffectPrefab, transform.position, Quaternion.identity);

        if (popSound != null && popSound.clip != null)
        {
            AudioSource.PlayClipAtPoint(popSound.clip, transform.position, popSound.volume);
        }

        FindFirstObjectByType<TargetSpawner>()?.OnTargetDestroyed();

        Destroy(gameObject);
    }
}