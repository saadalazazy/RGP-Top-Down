using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrigger : MonoBehaviour
{
    [SerializeField] private List<Enemy> Enemies;
    Collider collider;
    private void Start()
    {
        collider = GetComponent<Collider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
            return;
        collider.gameObject.SetActive(false);
        foreach (Enemy enemy in Enemies) 
        {
            enemy.SwitchToAwakeState();
        }
    }
}
