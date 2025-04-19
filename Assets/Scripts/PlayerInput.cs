using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool Xbutton;
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(!Xbutton && Time.timeScale != 0)
        {
            Xbutton = Input.GetKeyDown(KeyCode.X);
        }
    }
    public void OnDisable()
    {
        Xbutton = false;
        horizontal = 0;
        vertical = 0;
    }
    public float GetHorizontal() { return horizontal; }
    public float GetVertical() { return vertical; }

    public bool GetXButton() { return Xbutton; }
    public void SetXButton(bool x) { Xbutton = x; }

}