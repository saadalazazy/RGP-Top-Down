using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using static Enemy;

public class Boss : MonoBehaviour
{
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Animator animator;
    CapsuleCollider capsulCollider;

    private NavMeshAgent agent;
    private Vector3 spawnPosition;

    [SerializeField] private float moveSpeed;
    [SerializeField] private float spinningMoveSpeed;
    [SerializeField] private float rotationSpeed;
    [SerializeField] private float stoppingDistance1;
    [SerializeField] private float stoppingDistance2;
    [SerializeField] private float stoppingDistance3;
    [SerializeField] private float stoppingDistance4;
    [SerializeField] private float spawnRange;



    private readonly int IdleCombatAnimationHash = Animator.StringToHash("Idle");
    private readonly int Attack1AnimationHash = Animator.StringToHash("Attacking_1");
    private readonly int Attack2AnimationHash = Animator.StringToHash("Attacking_2");
    private readonly int Attack3AnimationHash = Animator.StringToHash("Attacking_3");
    private readonly int Attack4AnimationHash = Animator.StringToHash("Spellcast_Summon");
    private readonly int RunningAnimationHash = Animator.StringToHash("Running");


    public enum BossState
    {
        Idel,Movement,Attacking
    }
    public enum AttackState
    {
        Attacking_1, Attacking_2, Attacking_3, Attacking_4,
        Intro,Awake,
        Dead
    }
    [SerializeField] public AttackState currentAttackState;
    [SerializeField] private BossState currentBossState;
    [SerializeField] public BossState lastBossState;

    private Coroutine attackCoroutine;

    [SerializeField] int numberOfEnemies;
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform spawnCenterPoint;
    [SerializeField] int spawnRadius;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        capsulCollider = GetComponent<CapsuleCollider>();
        spawnPosition = transform.position;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance1;
        SwitchStateTo(currentBossState);
    }
    private void Update()
    {
        switch(currentAttackState)
        {
            case AttackState.Intro:
                break;
            case AttackState.Awake:
                break;
            case AttackState.Attacking_1:
                switch (currentBossState)
                {
                    case BossState.Idel:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement(stoppingDistance1);
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_2:
                switch (currentBossState)
                {
                    case BossState.Idel:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement(stoppingDistance2);
                        break;
                    case BossState.Attacking:
                        BossFollowPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_3:
                switch (currentBossState)
                {
                    case BossState.Idel:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement(stoppingDistance3);
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
            case AttackState.Attacking_4:
                switch (currentBossState)
                {
                    case BossState.Idel:
                        break;
                    case BossState.Movement:
                        CalculateBossMovement(stoppingDistance4);
                        break;
                    case BossState.Attacking:
                        RotateTowardsPlayer();
                        break;
                }
                break;
            case AttackState.Dead:
                break;
        }
        if (PlayerHealth.isDead)
        {
            SwitchAttackStateTo(AttackState.Dead);
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
    public void SpawnEnemies()
    {
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Vector3 randomPosition = GetRandomPosition();
            GameObject randomEnemy = GetRandomEnemy();
            GameObject enemy = Instantiate(randomEnemy, randomPosition, Quaternion.identity);
            enemy.GetComponent<Enemy>().SwitchToAwakeState();
        }
    }

    Vector3 GetRandomPosition()
    {
        Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
        return spawnCenterPoint.position + new Vector3(randomCircle.x, 0f, randomCircle.y);
    }

    GameObject GetRandomEnemy()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        return enemyPrefabs[index];
    }
    void CalculateBossMovement(float attackDistance)
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        float distanceRange = Vector3.Distance(spawnPosition, transform.position);

        if (PlayerHealth.isDead)
        {
            agent.SetDestination(spawnPosition);
        }
        else
        {
            agent.SetDestination(targetPlayer.position);
            if(distance < attackDistance)
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
            case BossState.Idel:
                break;
        }
        switch (newState)
        {
            case BossState.Movement:
                animator.CrossFadeInFixedTime(RunningAnimationHash, 0.1f);
                int indexRange = Random.RandomRange(0, 4);
                currentAttackState = (AttackState)indexRange;
                break;
            case BossState.Attacking:
                if (currentAttackState == AttackState.Attacking_1)
                {
                    agent.speed = moveSpeed;
                    animator.CrossFadeInFixedTime(Attack1AnimationHash, 0.1f);
                }
                else if (currentAttackState == AttackState.Attacking_2)
                {
                    agent.speed = spinningMoveSpeed;
                    animator.CrossFadeInFixedTime(Attack2AnimationHash, 0.1f);
                }
                else if (currentAttackState == AttackState.Attacking_3)
                {
                    agent.speed = moveSpeed;
                    animator.CrossFadeInFixedTime(Attack3AnimationHash, 0.1f);
                }
                else if(currentAttackState == AttackState.Attacking_4)
                {
                    agent.speed = moveSpeed;
                    animator.CrossFadeInFixedTime(Attack4AnimationHash, 0.1f);
                }
                break;
            case BossState.Idel:
                animator.CrossFadeInFixedTime(IdleCombatAnimationHash, 0.1f);
                break;
        }
        currentBossState = newState;
    }
    public void SwitchAttackStateTo(AttackState newState)
    {
        switch (currentAttackState)
        {
            case AttackState.Attacking_1:
                break;
            case AttackState.Attacking_2:
                break;
            case AttackState.Attacking_3:
                break;
            case AttackState.Attacking_4:
                break;
            case AttackState.Intro:
                break;
            case AttackState.Awake:
                break;
            case AttackState.Dead:
                break;

        }
        switch (newState)
        {
            case AttackState.Attacking_1:
                break;
            case AttackState.Attacking_2:
                break;
            case AttackState.Attacking_3:
                break;
            case AttackState.Attacking_4:
                break;
            case AttackState.Intro:
                break;
            case AttackState.Awake:
                break;
            case AttackState.Dead:
                break;
        }
        currentAttackState = newState;
    }
}
