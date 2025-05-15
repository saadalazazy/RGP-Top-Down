using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireDamage : MonoBehaviour
{
    [SerializeField] float Damage;
    [SerializeField] float hitImpact;
    CapsuleCollider capsuleCollider;

    private void Start()
    {
        capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.enabled = false;
        StartCoroutine(DelayCollider());
    }

    IEnumerator DelayCollider()
    {
        yield return new WaitForSeconds(0.2f);
        capsuleCollider.enabled =true;
        yield return new WaitForSeconds(2f);
        capsuleCollider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Vector3 hitDirection = (other.transform.position - transform.position).normalized;

        if (other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.DecreaseHeart(Damage, hitDirection, hitImpact);
        }
    }
}
