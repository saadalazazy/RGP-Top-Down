using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    [SerializeField] Animator animator;
    public Collider collider;

    private void Start()
    {
        collider = GetComponent<Collider>();
    }

    public void OpenLever()
    {
        animator.SetTrigger("Right");
    }
}
