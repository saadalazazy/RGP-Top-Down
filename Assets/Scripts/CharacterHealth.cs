using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    float currentHealth;
    Player player;

    private void Start()
    {
        currentHealth = maxHealth;
        player = GetComponent<Player>();
    }

    public void TackDamege(float damage)
    {
        currentHealth -= damage;

        Debug.Log(gameObject.name + "took damage" + damage);
        Debug.Log(gameObject.name + "current health" + currentHealth);
        CheckHealth();
    }
    public void CheckHealth()
    {
        if (currentHealth <= 0) 
        {
            player.SwitchStateTo(Player.PlayerState.Dead);
        }
    }
}
