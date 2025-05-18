using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadCount : MonoBehaviour
{
    [SerializeField] private int deadEnemyCount = 0;
    [SerializeField] Door door;
    [SerializeField] Door door2;

    [SerializeField] Gate gate;
    [SerializeField] Gate gate2;
    [SerializeField] int EnemyDead;
    private void OnTriggerStay(Collider other)
    {
        if (!other.TryGetComponent<Enemy>(out Enemy enemy))
            return;

        if (enemy.currentEnemyState == Enemy.EnemyState.Dead)
        {
            deadEnemyCount++;
            enemy.GetComponent<CapsuleCollider>().enabled = false;
            other.enabled = false;
            Debug.Log($"Dead enemy counted! Total: {deadEnemyCount}");
            if(deadEnemyCount == EnemyDead)
            {
                gate.Open3();
                door.OpenDoor();
                door2.OpenDoor();
                if(gate2 != null)
                    gate2.Open3();
            }
        }
    }

    public int GetDeadEnemyCount()
    {
        return deadEnemyCount;
    }

    public void ResetCounter()
    {
        deadEnemyCount = 0;
    }
}