using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    private float horizontal;
    private float vertical;
    private bool Xbutton;
    private bool shiftbutton;
    private void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");
        if(!Xbutton && Time.timeScale != 0)
        {
            Xbutton = Input.GetKeyDown(KeyCode.X);
        }
        if (!shiftbutton && Time.timeScale != 0)
        {
            shiftbutton = Input.GetKeyDown(KeyCode.LeftShift);
        }
    }
    public void OnDisable()
    {
        ClearCash();
    }
    public void ClearCash()
    {
        Xbutton = false;
        horizontal = 0;
        vertical = 0;
        shiftbutton = false;
    }
    public float GetHorizontal() { return horizontal; }
    public float GetVertical() { return vertical; }

    public bool GetXButton() { return Xbutton; }

    public bool GetShiftButton() 
    {
        return shiftbutton;
    }
    public void SetXButton(bool x) { Xbutton = x; }

    public void SetShiftButton(bool shift) { shiftbutton  = shift; }


}