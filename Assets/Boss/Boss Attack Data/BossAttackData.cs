using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "BossAttack", menuName = "ScriptableObjects/Boss Attack", order = 1)]
public class BossAttackData : ScriptableObject
{
    public float moveSpeed;
    public float rotationSpeed;
    public float stoppingDistance;

    [SerializeField] private string attackAnimationName;

    public float delayBeforeAttack;
    public float delayAfterAttack;
    public bool followPlayer;

    public bool rotateOnly;
    public bool returnToSpawn;
    [HideInInspector] public int AttackAnimationHash => Animator.StringToHash(attackAnimationName);
}
