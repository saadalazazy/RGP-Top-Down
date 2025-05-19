using System.Collections;
using System.Collections.Generic;
using TMPro;
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

    public Gate gate;

    Coroutine questCoroutine;

    [SerializeField] TextMeshProUGUI QuestText;
    private void Start()
    {
        if(QuestText != null)
            QuestText.text = "";
    }

    private void Update()
    {
        Vector3 direction = useLocalDirection ? transform.TransformDirection(rayDirection.normalized) : rayDirection.normalized;
        Ray ray = new Ray(transform.position, direction);
        Debug.DrawRay(ray.origin, direction * interactDistance, Color.green);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            // Only show "Interact With E" if no quest message is showing
            if (questCoroutine == null)
            {
                if (hit.collider.GetComponent<Door>() || hit.collider.GetComponent<Lever>())
                {
                    QuestText.text = "Interact With E";
                }
                else
                {
                    QuestText.text = "";
                }
            }

            if (inputManager.IsInterAct)
            {
                Debug.Log("Hit: " + hit.collider.name);

                if (hit.collider.TryGetComponent(out Door door))
                {
                    if (pickup.KeyCount >= door.numKeyRequirment)
                    {
                        Debug.Log("Opening door");
                        door.OpenDoor();
                        pickup.DecreaseKeyCount(pickup.KeyCount);
                    }
                    else
                    {
                        if (questCoroutine == null)
                        {
                            questCoroutine = StartCoroutine(DelayToRemoveText("You need " + door.numKeyRequirment + " key(s) to open"));
                        }
                    }
                }
                else if (hit.collider.TryGetComponent(out Lever lever))
                {
                    lever.OpenLever();

                    Collider leverCollider = lever.GetComponent<Collider>();
                    if (leverCollider != null)
                        leverCollider.enabled = false;

                    animator.CrossFadeInFixedTime("Look", 0.1f);

                    if (!gate.isOpen1)
                    {
                        gate.OpenHalph();
                    }
                    else
                    {
                        gate.Open2Halph();
                    }
                }
            }
        }
        else
        {
            if (questCoroutine == null )
                if(QuestText != null)
                    QuestText.text = "";
        }
    }

    IEnumerator DelayToRemoveText(string text)
    {
        QuestText.text = text;
        yield return new WaitForSeconds(3);
        QuestText.text = "";
        questCoroutine = null;
    }
}
