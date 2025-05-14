using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBundel : RotateAndFloat
{
    public int ArrowCountInBundel;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent <PlayerStateMachine>(out PlayerStateMachine player))
        {
            player.GetComponent<PlayerEffects>().PlayPlayerCollectVfx();
        }
    }
}
