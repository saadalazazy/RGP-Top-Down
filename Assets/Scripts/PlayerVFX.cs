using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class PlayerVFX : MonoBehaviour
{
    [SerializeField] VisualEffect foodSteps;

    public void UpdateVisualEffect(bool state)
    {
        if(state)
        {
            foodSteps.Stop();
            foodSteps.Reinit();
            foodSteps.Play();
        }
        else
        {
            foodSteps.Stop(); 
        }
    }
    
}
