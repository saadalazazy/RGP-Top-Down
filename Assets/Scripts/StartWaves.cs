using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartWaves : MonoBehaviour
{
    [SerializeField] GameObject[] Spawner;
    Collider capsuleCollider;
    [SerializeField] Door door;


    private void Start()
    {
        capsuleCollider = GetComponent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
            return;
        capsuleCollider.gameObject.SetActive(false);
        door.CloseDoor();
        foreach (GameObject Object in Spawner)
        {
            Object.gameObject.SetActive(true);
        }
    }
}
