using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    #region Player
    private Transform targetPlayer;
    private Animator animator;
    [SerializeField] private float moveSpeed;
    #endregion

    #region Enemy
    private NavMeshAgent agent;
    private Vector3 spawnPosition;
    [SerializeField] private float rotationSmoothSpeedEnemy;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float range;
    [SerializeField] private float spawnRange;
    [SerializeField] private float coolDownAttack;
    [SerializeField] private float delayBeforeAttack;
    #endregion

    #region States
    public enum EnemyState
    {
        Intro,Awake,Normal, Attacking, Dead, BeingHit
    }

    [SerializeField] private EnemyState currentEnemyState;
    [SerializeField] public EnemyState lastEnemyState;  
    #endregion
    private Vector3 impact = Vector3.zero;
    [SerializeField] private float impactDamping = 5f;
    [SerializeField] private float hitImpactForce = 5f;
    private Coroutine attackCoroutine;
    private bool canHit = true;

    private readonly int MomvmentSpeed = Animator.StringToHash("speed");
    private readonly int MovmentBlendtree = Animator.StringToHash("MovmentBlendTree");
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int Death = Animator.StringToHash("Death");
    private readonly int Hit = Animator.StringToHash("Hit");
    private readonly int IdleCombat = Animator.StringToHash("IdleCombat");
    private readonly int Intro = Animator.StringToHash("Intro");

    CapsuleCollider capsulCollider;

    [SerializeField] GameObject dropItem;
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        capsulCollider = GetComponent<CapsuleCollider>();
        animator = GetComponent<Animator>();
        targetPlayer = GameObject.FindWithTag("Player").transform;
        spawnPosition = transform.position;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        SwitchStateTo(currentEnemyState);
    }

    private void Update()
    {
        Debug.Log(canHit);
        if (agent.enabled) agent.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, impactDamping * Time.deltaTime);
        switch (currentEnemyState)
        {
            case EnemyState.Intro:
                break;
            case EnemyState.Awake: 
                break;   
            case EnemyState.Normal:
                CalculateEnemyMovement();
                break;

            case EnemyState.Attacking:
                RotateTowardsPlayer();
                break;

            case EnemyState.Dead:
                return;

            case EnemyState.BeingHit:
                break;
        }
        if (PlayerHealth.isDead)
            SwitchStateTo(EnemyState.Normal);
    }

    void CalculateEnemyMovement()
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        float distanceRange = Vector3.Distance(spawnPosition, transform.position);
        float playerDistance = Vector3.Distance(targetPlayer.position, spawnPosition);
        if(playerDistance > spawnRange)
        {
            agent.SetDestination(spawnPosition);
            return;
        }

        if (distanceRange > spawnRange || PlayerHealth.isDead)
        {
            agent.SetDestination(spawnPosition);
            return;
        }
        else
        {
            if (distance > range)
            {
                agent.SetDestination(spawnPosition);
            }
            else if (distance >= stoppingDistance)
            {
                agent.SetDestination(targetPlayer.position);
            }
            else
            {
                agent.ResetPath();
                SwitchStateTo(EnemyState.Attacking);
            }
        }

        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat(MomvmentSpeed, currentSpeed, 0.1f, Time.deltaTime);
    }

    void CalculateDistance()
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        if (distance <= stoppingDistance)
        {
            SwitchStateTo(EnemyState.Attacking);
        }
    }

    void RotateTowardsPlayer()
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeedEnemy * Time.deltaTime);
    }
    public void SwitchStateTo(EnemyState newState)
    {
        switch (currentEnemyState)
        {
            case EnemyState.Intro:
                break;
            case EnemyState.Awake:
                break;
            case EnemyState.Normal:
                break;
            case EnemyState.Attacking:
                break;
            case EnemyState.Dead:
                return;
            case EnemyState.BeingHit:
                break;
        }
        switch (newState)
        {
            case EnemyState.Intro:
                break;
            case EnemyState.Awake:
                animator.CrossFadeInFixedTime(Intro, 0.1f);
                break;
            case EnemyState.Normal:
                capsulCollider.enabled = true;
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                canHit = true;
                animator.CrossFadeInFixedTime(MovmentBlendtree, 0.1f);
                break;
            case EnemyState.Attacking:
                if (attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(DelayBforeAttacking());
                break;
            case EnemyState.Dead:
                if (dropItem != null)
                    Instantiate(dropItem, transform.position + new Vector3(0, 0.5f, 1), Quaternion.identity);
                if(attackCoroutine != null)
                    StopCoroutine(attackCoroutine);
                animator.CrossFadeInFixedTime(Death, 0.1f);
                break;
            case EnemyState.BeingHit:
                animator.CrossFadeInFixedTime(Hit, 0.1f);
                AddImpact(-transform.forward, hitImpactForce);
                break;
        }

        currentEnemyState = newState;
    }
    public void SwitchToAwakeState()
    {
        SwitchStateTo(EnemyState.Awake);
    }
    public void SwitchToLastCurrentState()
    {
        if(lastEnemyState == EnemyState.Normal || lastEnemyState == EnemyState.Attacking)
            SwitchStateTo(lastEnemyState);
        else
            SwitchStateTo(EnemyState.Normal);
    }
    public void DelaySwitchStateNormal()
    {
        StartCoroutine(DelaySwitchToState(coolDownAttack, EnemyState.Normal));
    }

    public void SwitchToDeadState()
    {
        SwitchStateTo(EnemyState.Dead);
    }
    public void SwitchToHitState()
    {
        if(!canHit) return;
        agent.ResetPath();
        lastEnemyState = currentEnemyState;
        SwitchStateTo(EnemyState.BeingHit);
    }
    private IEnumerator DelaySwitchToState(float delay, EnemyState enemyState)
    {
        yield return new WaitForSeconds(delay);
        SwitchStateTo(enemyState);
    }
    private IEnumerator DelayBforeAttacking()
    {
        animator.SetFloat(MomvmentSpeed, 0f);
        animator.CrossFadeInFixedTime(IdleCombat, 0.1f);
        yield return new WaitForSeconds(delayBeforeAttack);
        animator.CrossFadeInFixedTime(Attack, 0.1f);
        canHit = false;
    }
    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();
        direction.y = 0;
        impact += direction * force;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(spawnPosition != Vector3.zero ? spawnPosition : transform.position, spawnRange); // Spawn patrol range
    }
}