using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    private float verticalVelocity;
    [SerializeField] CharacterController characterController;

    public Vector3 Movment => Vector3.up * verticalVelocity;
    private void Update()
    {
        if(characterController.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }
}
