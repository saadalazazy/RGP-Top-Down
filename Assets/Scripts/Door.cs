using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public int numKeyRequirment;

    private void Start()
    {
        if(numKeyRequirment == 0)
            OpenDoor();
        else
            CloseDoor();
    }
    public void OpenDoor()
    {
        animator.SetTrigger("Open");
    }
    public void CloseDoor()
    {
        animator.SetTrigger("Close");
    }
}
