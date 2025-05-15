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
    private bool isAttackingCycleRunning = false;


    public enum BossState
    {
        Idle,Movement,Attacking
    }
    private int currentAttackIndex;
    private BossAttackData CurrentAttackData => attackData[currentAttackIndex];
    public enum LifeState
    {
        Intro, Awake,Normal,
        Dead
    }

    [SerializeField] private BossState currentBossState;
    [SerializeField] private BossState lastBossState;
    [SerializeField] private LifeState currentLifeState;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        capsuleCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        targetPlayer = GameObject.FindGameObjectWithTag("Player").transform;
        spawnPosition = transform.position;
        currentAttackIndex = 0;
        SwitchAttackStateTo(currentAttackIndex);
        SwitchStateTo(currentBossState);
        currentLifeState = LifeState.Normal;

    }
    private void Update()
    {
        switch (currentLifeState)
        {
            case LifeState.Intro:
                break;
            case LifeState.Awake:
                break;
            case LifeState.Normal:
                if (currentBossState == BossState.Movement)
                {
                    if (CurrentAttackData.returnToSpawn)
                        BossGoToSpawnPoint();
                    else
                        CalculateBossMovement();
                }
                else if (currentBossState == BossState.Attacking)
                {
                    if (CurrentAttackData.followPlayer)
                        BossFollowPlayer();
                    else if (CurrentAttackData.rotateOnly)
                        RotateTowardsPlayer();
                }
                break;
            case LifeState.Dead:
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
    void BossGoToSpawnPoint()
    {
        float distance = Vector3.Distance(spawnPosition, transform.position);
        agent.SetDestination(spawnPosition);
        Debug.Log(distance + "go to spawn point");
        if (distance < 1f && !isAttackingCycleRunning)
        {
            agent.ResetPath();
            SwitchStateTo(BossState.Attacking);
            Debug.Log(distance + "innter to to air");
        }
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
            if (distance < stoppingDistance && !isAttackingCycleRunning)
            {
                agent.ResetPath();
                StartCoroutine(DelayBeforeAttacking());
            }
        }
    }

    public void SwitchStateTo(BossState newState)
    {
        if (currentBossState == newState) return;

        lastBossState = currentBossState;
        currentBossState = newState;

        switch (newState)
        {
            case BossState.Movement:
                animator.CrossFadeInFixedTime(RunningAnimationHash, 0.1f);
                int randomIndex = Random.Range(0, attackData.Length);
                SwitchAttackStateTo(randomIndex);
                break;

            case BossState.Attacking:
                animator.CrossFadeInFixedTime(AttackAnimationHash, 0.1f);
                break;

            case BossState.Idle:
                agent.ResetPath();
                animator.CrossFadeInFixedTime(IdleCombatAnimationHash, 0.1f);
                break;
        }
    }

    public void SwitchAttackStateTo(int newIndex)
    {
        if (newIndex < 0 || newIndex >= attackData.Length) return;

        BossAttackData newAttackData = attackData[newIndex];
        currentAttackIndex = newIndex;

        moveSpeed = newAttackData.moveSpeed;
        rotationSpeed = newAttackData.rotationSpeed;
        stoppingDistance = newAttackData.stoppingDistance;
        AttackAnimationHash = newAttackData.AttackAnimationHash;
        delayAfterAttck = newAttackData.delayAfterAttack;
        delayBeforeAttck = newAttackData.delayBeforeAttack;

        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
    }

    IEnumerator DelayBeforeAttacking()
    {
        isAttackingCycleRunning = true;

        SwitchStateTo(BossState.Idle);
        yield return new WaitForSeconds(delayBeforeAttck);

        SwitchStateTo(BossState.Attacking);
    }
    public void TriggerDelayAfterAttacking()
    {
        StartCoroutine(DelayAfterAttacking());
    }

    IEnumerator DelayAfterAttacking()
    {
        SwitchStateTo(BossState.Idle);
        yield return new WaitForSeconds(delayAfterAttck);

        SwitchStateTo(BossState.Movement);
        isAttackingCycleRunning = false;
    }
    public void SwitchLifeStateToDead()
    {
        currentLifeState = LifeState.Dead;
        animator.Play("Death");
        agent.ResetPath();
        agent.speed = 0;
        currentBossState = BossState.Idle;
        StopAllCoroutines();
    }    
}
