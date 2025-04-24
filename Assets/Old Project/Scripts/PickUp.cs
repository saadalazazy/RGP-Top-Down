using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] public float value;
    public enum Type
    {
        coin,heal
    }
    public Type type;

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            other.gameObject.GetComponent<Player>().PickUpItem(this);
            Destroy(gameObject);
        }
    }
}
