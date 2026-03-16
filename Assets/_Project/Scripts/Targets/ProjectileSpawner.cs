using System.Collections;
using UnityEngine;

public class ProjectileSpawner : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] private GameObject targetPrefab;

    [Header("Launch Points")]
    [SerializeField] private Transform leftLauncher;
    [SerializeField] private Transform rightLauncher;

    [Header("Launch Settings")]
    [SerializeField] private float minForwardForce = 14f;
    [SerializeField] private float maxForwardForce = 18f;

    [SerializeField] private float minUpwardForce = 6f;
    [SerializeField] private float maxUpwardForce = 9f;

    [SerializeField] private float spawnInterval = 2f;

    [Header("Spin")]
    [SerializeField] private float ySpinSpeed = 5f;

    private void Start()
    {
        StartCoroutine(SpawnRoutine());
    }

    private IEnumerator SpawnRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);
            SpawnTarget();
        }
    }

    private void SpawnTarget()
    {
        bool fromLeft = Random.value > 0.5f;
        Transform launcher = fromLeft ? leftLauncher : rightLauncher;

        GameObject target = Instantiate(
            targetPrefab,
            launcher.position,
            Quaternion.identity
        );

        Rigidbody rb = target.GetComponent<Rigidbody>();

        float forwardForce = Random.Range(minForwardForce, maxForwardForce);
        float upwardForce = Random.Range(minUpwardForce, maxUpwardForce);

        Vector3 launchVelocity =
            launcher.forward * forwardForce +
            Vector3.up * upwardForce;

        rb.linearVelocity = launchVelocity;

        // Make target face movement direction
        target.transform.forward = launchVelocity.normalized;

        // Rolling spin relative to travel direction
        rb.angularVelocity = target.transform.right * ySpinSpeed;
    }
}