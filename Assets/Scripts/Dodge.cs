using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Dodge 
{
    [field: SerializeField] public float Speed { get; private set; }
    [field: SerializeField] public float Time{ get; private set; }
    [field: SerializeField] public float CoolDown{ get; private set; }

}
