using System.Collections;
using UnityEngine;

public class TargetController : MonoBehaviour, IHittable
{
    private Rigidbody rb;
    private bool destroyed = false;

    [SerializeField] private int health = 1;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void GetHit()
    {
        if (destroyed) return;

        health--;

        if (health <= 0)
        {
            destroyed = true;

            rb.isKinematic = false;

            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        FindObjectOfType<TargetSpawner>()?.OnTargetDestroyed();

        Destroy(gameObject);
    }
}
