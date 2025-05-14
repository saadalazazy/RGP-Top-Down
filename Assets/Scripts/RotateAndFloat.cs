using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndFloat : MonoBehaviour
{
    public Vector3 rotationSpeed = new Vector3(0f, 100f, 0f);
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;

    private Vector3 startPosition;

    protected virtual void Start()
    {
        startPosition = transform.position;
    }

    protected virtual void Update()
    {
        transform.Rotate(rotationSpeed * Time.deltaTime);

        float yOffset = Mathf.Sin(Time.time * floatFrequency) * floatAmplitude;
        transform.position = new Vector3(startPosition.x, startPosition.y + yOffset, startPosition.z);
    }
}
