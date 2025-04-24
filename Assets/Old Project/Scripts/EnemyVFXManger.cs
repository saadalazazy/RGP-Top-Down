using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
public class EnemyVFXManger : MonoBehaviour
{
    [SerializeField] VisualEffect foodStep;
    public void BrustFoodStep()
    {
        foodStep.SendEvent("OnPlay");
    }
    public void BrustFoodStepStop()
    {
        foodStep.Stop();
    }
}
