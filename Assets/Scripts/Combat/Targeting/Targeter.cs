using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Targeter : MonoBehaviour
{
    [SerializeField] List<Target> targets;
    public Target CurrentTarget { get; private set; } 
    private void OnTriggerEnter(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        target.OnDestroyed += RemoveTarget;
        targets.Add(target);
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.TryGetComponent<Target>(out Target target)) return;
        RemoveTarget(target);
    }
    private void RemoveTarget(Target target)
    {
        if(CurrentTarget == target)
        {
            CurrentTarget = null;
        }
        target.OnDestroyed -= RemoveTarget;
        targets.Remove(target);
    }

    public bool SelectTarget()
    {
        if( targets.Count == 0) { return false; }

        CurrentTarget = targets[0];
        return true;
    }

    public void ClearCurrentTarget()
    {
        CurrentTarget = null;
    }

}
