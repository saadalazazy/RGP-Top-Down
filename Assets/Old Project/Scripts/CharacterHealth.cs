using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterHealth : MonoBehaviour
{
    [SerializeField] float maxHealth;
    public float currentHealth;
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
    public void TakeHealth(float value) 
    {
        currentHealth += value;
        if(currentHealth >= maxHealth)
            currentHealth = maxHealth;
    }
    public void CheckHealth()
    {
        if (currentHealth <= 0) 
        {
            player.SwitchStateTo(Player.PlayerState.Dead);
        }
    }
}
