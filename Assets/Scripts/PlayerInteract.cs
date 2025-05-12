using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private PickUp pickup;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private Animator animator;

    [Header("Raycast Settings")]
    [SerializeField] private float interactDistance = 3f;
    [SerializeField] private Vector3 rayDirection = Vector3.forward;
    [SerializeField] private bool useLocalDirection = true;

    void Update()
    {
        Vector3 direction = useLocalDirection ? transform.TransformDirection(rayDirection.normalized) : rayDirection.normalized;
        Ray ray = new Ray(transform.position, direction);
        RaycastHit hit;

        Debug.DrawRay(ray.origin, direction * interactDistance, Color.green);

        if (inputManager.IsInterAct)
        {
            if (Physics.Raycast(ray, out hit, interactDistance))
            {
                Debug.Log("Hit: " + hit.collider.name);
                if (hit.collider.TryGetComponent<Door>(out Door door))
                {
                    if (pickup.KeyCount > 0)
                    {
                        Debug.Log("Opening door");
                        door.OpenDoor();
                        pickup.DecreaseKeyCount();
                    }
                    else
                    {
                        Debug.Log("You need a key");
                    }
                }
                else if(hit.collider.TryGetComponent<Lever>(out Lever lever))
                {
                    lever.OpenLever();
                    animator.CrossFadeInFixedTime("Look", 0.1f);
                }
            }
        }
    }
}
