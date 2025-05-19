using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap : MonoBehaviour
{
    Animator animator;
    [SerializeField] float closeTrapTime;
    [SerializeField] float openTrapTime;
    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(OpenTrap());
    }
    IEnumerator OpenTrap()
    {
        yield return new WaitForSeconds(openTrapTime);
        animator.CrossFadeInFixedTime("Trap", 0.1f);
        StartCoroutine(CloseTrap());
    }
    IEnumerator CloseTrap()
    {
        yield return new WaitForSeconds(closeTrapTime);
        animator.CrossFadeInFixedTime("Close Trap", 0.1f);
        StartCoroutine(OpenTrap());
    }




}
