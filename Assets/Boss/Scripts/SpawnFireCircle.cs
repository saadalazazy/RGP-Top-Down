using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnFireCircle : MonoBehaviour
{
    public Transform TargetPlayer;
    public GameObject fireCircle;
    public GameObject fireAttack;
    public float coolDownSpawn;
    public float timeBetweenCircleAndFire;

    private Coroutine fireCoroutine;

    private void Start()
    {
        TargetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
    }
    public void StartFireAttack()
    {
        fireCoroutine = StartCoroutine(SpawnFireAttack());
    }
    public void StopFireAttack()
    {
        StopAllCoroutines();
    }
        
    IEnumerator SpawnFireAttack()
    {
        while (true) 
        {
            Vector3 spawnPosition = TargetPlayer.position;
            Instantiate(fireCircle, spawnPosition, Quaternion.identity);
            yield return new WaitForSeconds(timeBetweenCircleAndFire);
            Instantiate(fireAttack, spawnPosition, Quaternion.identity); 
        }
    }

}
