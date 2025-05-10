using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateMachine : StateMachine
{
    [field : SerializeField]public InputManager InputManager { get; private set; }
    [field: SerializeField] public CharacterController Controller { get; private set; }
    [field: SerializeField] public Animator Animator { get; private set; }
    [field: SerializeField] public ForceReceiver ForceReceiver { get; private set; }
    [field: SerializeField] public Attack[] Attacks{ get; private set; }
    [field: SerializeField] public ArrowFireHandler ArrowFireHandler{ get; private set; }
    [field: SerializeField] public PlayerEffects Effects{ get; private set; }
    [field: SerializeField] public float MovementSpeed{ get; private set; }
    [field: SerializeField] public Dodge Dodge{ get; private set; }
    [field: SerializeField] public float RotationSpeed{ get; private set; }

    private void Start()
    {
        SwitchStateTo(new PlayerMovmentState(this));
    }

}
