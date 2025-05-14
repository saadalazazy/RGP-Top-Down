using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBottle : RotateAndFloat
{
    public int healthGain;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<PlayerStateMachine>(out PlayerStateMachine player))
        {
            player.GetComponent<PlayerEffects>().PlayPlayerCollectVfx();
        }
    }
}
