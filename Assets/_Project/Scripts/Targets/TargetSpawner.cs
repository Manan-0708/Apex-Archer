using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    [SerializeField] private GameObject targetPrefab;
    [SerializeField] private List<Transform> spawnPoints = new List<Transform>();

    [SerializeField] private float spawnDelay = 2f;
    [SerializeField] private int maxActiveTargets = 4;

    private int activeTargets = 0;

    private void Start()
    {
        SpawnInitialTargets();
    }

    private void SpawnInitialTargets()
    {
        foreach (Transform point in spawnPoints)
        {
            SpawnTarget(point);
        }
    }

    private void SpawnTarget(Transform point)
    {
        if (activeTargets >= maxActiveTargets) return;

        Instantiate(targetPrefab, point.position, point.rotation);
        activeTargets++;
    }

    public void OnTargetDestroyed()
    {
        activeTargets--;
        StartCoroutine(SpawnAfterDelay());
    }

    private IEnumerator SpawnAfterDelay()
    {
        yield return new WaitForSeconds(spawnDelay);

        if (spawnPoints.Count == 0) yield break;

        Transform randomPoint =
            spawnPoints[Random.Range(0, spawnPoints.Count)];

        SpawnTarget(randomPoint);
    }
}
