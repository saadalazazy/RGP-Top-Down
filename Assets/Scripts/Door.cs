using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] public int numKeyRequirment;
    public void OpenDoor()
    {
        animator.SetTrigger("Open");
    }
}
