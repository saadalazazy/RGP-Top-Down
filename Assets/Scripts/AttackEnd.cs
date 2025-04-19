using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEnd : MonoBehaviour
{
    [SerializeField] Player player;
    [SerializeField] DamageCaster damage;
    public void AttackAnimationEnd()
    {
        player.AttackanimationEnd();
    }
    public void BeingHitAnimationEnd()
    {
        player.BeingHitanimationEnd();
    }
    public void EnableDamaageCollider()
    {
        damage.EnableDamaageCollider();
    }
    public void DisableDamaageCollider() 
    {
        damage.DisableDamaageCollider();
    }
}
