using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [field:SerializeField] public int CoinCount { get; private set; }
    [field: SerializeField] public int KeyCount { get; private set; }
    [SerializeField] ArrowFireHandler arrowFireHandler;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerEffects playerEffects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Coin>())
        {
            Destroy(other.gameObject);
            CoinCount++;
            playerEffects.PlayPlayerCollectVfx();
        }
        else if(other.TryGetComponent<ArrowBundel>(out ArrowBundel arrowBundel))
        {
            Destroy(other.gameObject);
            arrowFireHandler.IncreaseArrowCount(arrowBundel.ArrowCountInBundel);
        }
        else if(other.GetComponent<Key>())
        {
            Destroy(other.gameObject);
            KeyCount++;
        }
        else if(other.TryGetComponent<HealthBottle>(out HealthBottle health))
        {
            Destroy(other.gameObject);
            playerHealth.IncreasHeart(health.healthGain);
            playerEffects.PlayPlayerHealVfx();
        }
    }

    public void DecreaseKeyCount()
    {
        if(KeyCount > 0)
            KeyCount--;
    }

}
