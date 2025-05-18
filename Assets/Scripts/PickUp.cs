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
        if (coinScore == null || keyScore == null || arrowBundleCountText == null || healthScore == null) 
        {
            return;
        }
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
            if (coinScore == null)
                return;
        }
        else if(other.TryGetComponent<ArrowBundel>(out ArrowBundel arrowBundel))
        {
            Destroy(other.gameObject);
            arrowFireHandler.IncreaseArrowCount(arrowBundel.ArrowCountInBundel);
            playerEffects.PlayPlayerCollectVfx();
            arrowBundleCountText.text = arrowFireHandler.ArrowCount.ToString();
            if (arrowBundleCountText == null)
                return;
        }
        else if(other.GetComponent<Key>())
        {
            Destroy(other.gameObject);
            KeyCount++;
            playerEffects.PlayPlayerCollectVfx();
            keyScore.text = KeyCount.ToString();
            if (keyScore == null)
                return;
        }
        else if(other.TryGetComponent<HealthBottle>(out HealthBottle health))
        {
            Destroy(other.gameObject);
            playerEffects.PlayPlayerHealVfx();
            healthCount++;
            if (healthScore == null)
                return;
            healthScore.text = healthCount.ToString();        
        }
    }

    public void DecreaseKeyCount(int numKey)
    {
        if(KeyCount > 0)
            KeyCount-= numKey;
        keyScore.text = KeyCount.ToString();


    }

}
