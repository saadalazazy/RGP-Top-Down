using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Animator animator;

    public void OpenLever()
    {
        animator.SetTrigger("Right");
    }
}
