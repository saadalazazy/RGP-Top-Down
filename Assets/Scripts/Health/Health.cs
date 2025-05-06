using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    private float health;
    [SerializeField] float maxHealth = 100;
    private void Start()
    {
        health = maxHealth;
    }
    public void DealDamage(float damage)
    {
        if (health <= 0) return;

        health = Mathf.Max(health - damage , 0);
        Debug.Log(health);
        if (health <= 0)
        {
            Debug.Log(gameObject.transform + " dead");
        }

    }
}
