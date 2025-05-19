using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnEnemy : MonoBehaviour
{

    [SerializeField] int numberOfEnemies;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform spawnCenterPoint;
    [SerializeField] int spawnRadius;
    public void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            StartCoroutine(SpawnAndWakeEnemy());
        }
    }
    IEnumerator SpawnAndWakeEnemy()
    {
        Vector3 randomPosition = GetRandomPosition();
        GameObject randomEnemy = GetRandomEnemy();
        GameObject newEnemy = Instantiate(randomEnemy, randomPosition, Quaternion.identity);
        Enemy enemyScript = newEnemy.GetComponent<Enemy>();

        yield return null; // Wait 1 frame to ensure Start() has run

        enemyScript.SwitchToAwakeState();
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return spawnCenterPoint.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    GameObject GetRandomEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[index];
    }

    Quaternion GetRandomRotatoin()
    {
        int rotation = Random.Range(0 , 360);
        return Quaternion.Euler(0, rotation ,0);
    }
}
