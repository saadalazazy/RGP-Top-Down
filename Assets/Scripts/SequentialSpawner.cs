using System.Collections;
using UnityEngine;

public class SequentialSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] enemyPrefab;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnDelay = 2f;

    private void OnEnable()
    {
        StartCoroutine(SpawnRepeatedly());
    }

    private IEnumerator SpawnRepeatedly()
    {
       foreach (var enemy in enemyPrefab) 
       {
            Instantiate(enemy, spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(spawnDelay);
        }
    }
}