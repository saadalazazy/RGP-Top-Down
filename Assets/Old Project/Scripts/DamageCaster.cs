using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageCaster : MonoBehaviour
{
    [SerializeField] float damage;
    Collider damageCasterCollider;
    [SerializeField] string TargetTag;
    private List<Collider> targets;

    private void Start()
    {
        damageCasterCollider = GetComponent<Collider>();
        damageCasterCollider.enabled = false;
        targets = new List<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == TargetTag && !targets.Contains(other)) 
        {
            Player player =  other.GetComponent<Player>();
            if (player != null) 
            {
                player.ApllyDamege(damage ,transform.parent.position);
            }
            targets.Add(other);
        }
    }
    public void EnableDamaageCollider()
    {
        targets.Clear();
        damageCasterCollider.enabled = true;
    }
    public void DisableDamaageCollider()
    {
        targets.Clear();
        damageCasterCollider.enabled = false;
    }
}
