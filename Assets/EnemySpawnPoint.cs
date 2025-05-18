using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpawnPoint
{
    public Transform point;
    public GameObject enemyPrefab;
    public float spawnDelay;
    [Range(0, 30)] public int spawnCount = 1;
    [Tooltip("Time between each spawn when count > 1")]
    public float intervalBetweenSpawns = 0.5f;
}

public class EnemySpawnPoint : MonoBehaviour
{
    [SerializeField] private SpawnPoint[] spawnPoints;
    [SerializeField] private bool spawnOnStart = false;
    [SerializeField] private float initialDelay = 0f;

    private void Start()
    {
        if (spawnOnStart)
        {
            StartCoroutine(StartSpawning());
        }
    }

    public void StartSpawningProcess()
    {
        StartCoroutine(StartSpawning());
    }

    private IEnumerator StartSpawning()
    {
        yield return new WaitForSeconds(initialDelay);

        List<Coroutine> spawnCoroutines = new List<Coroutine>();

        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.point != null && spawnPoint.enemyPrefab != null)
            {
                var coroutine = StartCoroutine(SpawnAtPoint(spawnPoint));
                spawnCoroutines.Add(coroutine);
            }
        }

        // Wait until all spawn points finish spawning
        foreach (var coroutine in spawnCoroutines)
        {
            yield return coroutine;
        }
    }

    private IEnumerator SpawnAtPoint(SpawnPoint spawnPoint)
    {
        yield return new WaitForSeconds(spawnPoint.spawnDelay);

        for (int i = 0; i < spawnPoint.spawnCount; i++)
        {
            if (spawnPoint.point == null || spawnPoint.enemyPrefab == null)
                yield break;

            var enemy = Instantiate(
                spawnPoint.enemyPrefab,
                spawnPoint.point.position,
                spawnPoint.point.rotation
            );

            var enemyComponent = enemy.GetComponent<Enemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SwitchToAwakeState();
            }

            if (i < spawnPoint.spawnCount - 1) // Don't wait after last spawn
            {
                yield return new WaitForSeconds(spawnPoint.intervalBetweenSpawns);
            }
        }
    }
}