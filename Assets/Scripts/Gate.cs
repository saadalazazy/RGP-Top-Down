using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    Animator anim;
    public bool isOpen1 = false;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }
    public void Open3()
    {
        anim.CrossFadeInFixedTime("Open3" , 0.1f);
    }
    public void OpenHalph()
    {
        StartCoroutine(open1());
    }
    public void Open2Halph()
    {
        StartCoroutine(open2());
    }
    IEnumerator open1()
    {
        yield return new WaitForSeconds(3);
        anim.CrossFadeInFixedTime("Open1", 0.1f);
        isOpen1 = true;

    }
    IEnumerator open2()
    {
        yield return new WaitForSeconds(3);
        anim.CrossFadeInFixedTime("Open2", 0.1f);
    }
}
