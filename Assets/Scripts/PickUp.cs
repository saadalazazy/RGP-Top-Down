using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [field: SerializeField] public int CoinCount { get; private set; } = 0;
    [field: SerializeField] public int KeyCount { get; private set; } = 0;
    [field: SerializeField] public int healthCount { get; private set; } = 0;
    [SerializeField] ArrowFireHandler arrowFireHandler;
    [SerializeField] PlayerHealth playerHealth;
    [SerializeField] PlayerEffects playerEffects;
    [SerializeField] TextMeshProUGUI coinScore;
    [SerializeField] TextMeshProUGUI keyScore;
    [SerializeField] TextMeshProUGUI healthScore;
    [SerializeField] TextMeshProUGUI arrowBundleCountText;
    private void Start()
    {
        coinScore.text = CoinCount.ToString();
        keyScore.text = KeyCount.ToString();
        arrowBundleCountText.text = arrowFireHandler.ArrowCount.ToString();
        healthScore.text = healthCount.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<Coin>())
        {
            Destroy(other.gameObject);
            CoinCount++;
            playerEffects.PlayPlayerCollectVfx();
            coinScore.text = CoinCount.ToString();
        }
        else if(other.TryGetComponent<ArrowBundel>(out ArrowBundel arrowBundel))
        {
            Destroy(other.gameObject);
            arrowFireHandler.IncreaseArrowCount(arrowBundel.ArrowCountInBundel);
            arrowBundleCountText.text = arrowFireHandler.ArrowCount.ToString();
        }
        else if(other.GetComponent<Key>())
        {
            Destroy(other.gameObject);
            KeyCount++;
            keyScore.text = KeyCount.ToString();
        }
        else if(other.TryGetComponent<HealthBottle>(out HealthBottle health))
        {
            Destroy(other.gameObject);
            playerHealth.IncreasHeart(health.healthGain);
            playerEffects.PlayPlayerHealVfx();
            healthCount++;
            healthScore.text = healthCount.ToString();        
        }
    }

    public void DecreaseKeyCount(int numKey)
    {
        if(KeyCount > 0)
            KeyCount-= numKey;
    }

}
