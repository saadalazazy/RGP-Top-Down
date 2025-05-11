using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayVFXOnAwake : MonoBehaviour
{
    [SerializeField] ParticleSystem particle;

    private void Awake()
    {
        particle.Play();
    }
}
