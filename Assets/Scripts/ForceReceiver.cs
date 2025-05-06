using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForceReceiver : MonoBehaviour
{
    private Vector3 impact;
    private float verticalVelocity;
    private Vector3 dumpingVelocity;
    private float drag = 0.1f;
    [SerializeField] CharacterController characterController;

    public Vector3 Movment => impact + Vector3.up * verticalVelocity;
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
        impact = Vector3.SmoothDamp(impact, Vector3.zero, ref dumpingVelocity, drag);
    }
    public void AddForce(Vector3 force)
    {
        impact += force;
    }
}
