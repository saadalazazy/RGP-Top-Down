using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Boss : MonoBehaviour
{
    [SerializeField] BossAttackData[] attackData;


    private Transform targetPlayer;
    private Animator animator;
    private NavMeshAgent agent;
    private CapsuleCollider capsuleCollider;

    private float moveSpeed;
    private float rotationSpeed;
    private float stoppingDistance;
    private int AttackAnimationHash;
    private float delayBeforeAttck;
    private float delayAfterAttck;
    private Vector3 spawnPosition;


    private readonly int IdleCombatAnimationHash = Animator.StringToHash("Idle");
    private readonly int RunningAnimationHash = Animator.StringToHash("Running");


    public enum BossState
    {
        Idle,Movement,Attacking
    }
    public enum AttackState
    {
        Attacking_1, Attacking_2, Attacking_3, Attacking_4
    }
    public enum lifeState
    {
        Intro, Awake,
        Dead
    }
    [SerializeField] public AttackState currentAttackState;

    [SerializeField] private BossState currentBossState;
    [SerializeField] private BossState lastBossState;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPosition = transform.position;
        SwitchAttackStateTo(currentAttackState);
        SwitchStateTo(currentBossState);
    }
    private void Update()
    {
        switch(currentAttackState)
        {
            case AttackState.Attacking_1:
                switch (currentBossState)
                {
                    case BossState.Idle:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement();
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_2:
                switch (currentBossState)
                {
                    case BossState.Idle:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement();
                        break;
                    case BossState.Attacking:
                        BossFollowPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_3:
                switch (currentBossState)
                {
                    case BossState.Idle:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement();
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_4:
                switch (currentBossState)
                {
                    case BossState.Idle:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement();
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
        }
    }
    void RotateTowardsPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
    void BossFollowPlayer()
    {
        agent.SetDestination(targetPlayer.position);
    }
    
    void CalculateBossMovement()
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        if (PlayerHealth.isDead)
        {
            agent.SetDestination(spawnPosition);
        }
        else
        {
            agent.SetDestination(targetPlayer.position);
            if(distance < stoppingDistance)
            {
                agent.ResetPath();
                SwitchStateTo(BossState.Attacking);
            }
        }
    }

    public void SwitchStateTo(BossState newState)
    {
        switch (currentBossState)
        {
            case BossState.Movement:
                break;
            case BossState.Attacking:
                break;
            case BossState.Idle:
                break;
        }
        lastBossState = currentBossState;
        switch (newState)
        {
            case BossState.Movement:
                animator.CrossFadeInFixedTime(RunningAnimationHash, 0.1f);
                int indexRange = Random.RandomRange(0, attackData.Length);
                SwitchAttackStateTo((AttackState)indexRange);
                break;
            case BossState.Attacking:
                animator.CrossFadeInFixedTime(AttackAnimationHash, 0.1f);
                break;
            case BossState.Idle:
                animator.CrossFadeInFixedTime(IdleCombatAnimationHash, 0.1f);
                break;
        }
        currentBossState = newState;
    }
    public void SwitchAttackStateTo(AttackState newState)
    {
        int indexBossAttack = (int)newState;
        BossAttackData newAttackData = attackData[indexBossAttack];
        moveSpeed = newAttackData.moveSpeed;
        rotationSpeed = newAttackData.rotationSpeed;
        stoppingDistance = newAttackData.stoppingDistance;
        AttackAnimationHash = newAttackData.AttackAnimationHash;
        delayAfterAttck = newAttackData.delayAfterAttack;
        delayBeforeAttck = newAttackData.delayBeforeAttack;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        currentAttackState = newState;
    }
    
}
