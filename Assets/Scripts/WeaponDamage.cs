using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [SerializeField] float Damage;
    [SerializeField] float hitImpact;
    [SerializeField] Collider myCollider;
    [SerializeField] bool isEnmey= false;
    private List<Collider> alreadyCollidedWith = new List<Collider>();


    private void OnEnable()
    {
        alreadyCollidedWith.Clear();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == myCollider) return;
        if (alreadyCollidedWith.Contains(other)) return;
        if (other.TryGetComponent<Enemy>(out Enemy enmey) && isEnmey) return;
        if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player) && !isEnmey) return;
        alreadyCollidedWith.Add(other);

        Vector3 hitDirection = (other.transform.position - transform.position).normalized;

        if (other.TryGetComponent<Health>(out Health health))
        {
            health.DealDamage(Damage);
        }
        else if(other.TryGetComponent<PlayerHealth>(out PlayerHealth playerHealth))
        {
            playerHealth.DecreaseHeart(Damage ,hitDirection, hitImpact);
        }
    }
}
