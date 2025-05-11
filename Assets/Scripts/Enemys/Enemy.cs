using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class Enemy : MonoBehaviour
{
    #region Player
    [SerializeField] private Transform targetPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private float moveSpeed;
    #endregion

    #region Enemy
    private NavMeshAgent agent;
    private Vector3 spawnPosition;
    [SerializeField] private float rotationSmoothSpeedEnemy;
    [SerializeField] private float stoppingDistance;
    [SerializeField] private float range;
    [SerializeField] private float coolDownAttack;
    [SerializeField] private float delayBeforeAttack;
    #endregion

    #region States
    public enum EnemyState
    {
        Normal, Attacking, Dead, BeingHit
    }

    [SerializeField] private EnemyState currentEnemyState;
    #endregion
    private Vector3 impact = Vector3.zero;
    [SerializeField] private float impactDamping = 5f;
    [SerializeField] private float hitImpactForce = 5f;
    private Coroutine attackCoroutine;

    private readonly int MomvmentSpeed = Animator.StringToHash("speed");
    private readonly int MovmentBlendtree = Animator.StringToHash("MovmentBlendTree");
    private readonly int Attack = Animator.StringToHash("Attack");
    private readonly int Death = Animator.StringToHash("Death");
    private readonly int Hit = Animator.StringToHash("Hit");
    private readonly int IdleCombat = Animator.StringToHash("IdleCombat");

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        spawnPosition = transform.position;
        agent.speed = moveSpeed;
        agent.stoppingDistance = stoppingDistance;
        SwitchStateTo(EnemyState.Normal);
    }

    private void Update()
    {
        if (agent.enabled) agent.Move(impact * Time.deltaTime);
        impact = Vector3.Lerp(impact, Vector3.zero, impactDamping * Time.deltaTime);
        switch (currentEnemyState)
        {
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
    }

    void CalculateEnemyMovement()
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);
        float distanceRange = Vector3.Distance(spawnPosition, transform.position);

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

        float currentSpeed = agent.velocity.magnitude;
        animator.SetFloat(MomvmentSpeed, currentSpeed, 0.1f, Time.deltaTime);
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
            case EnemyState.Normal:
                animator.CrossFadeInFixedTime(MovmentBlendtree, 0.1f);
                break;
            case EnemyState.Attacking:
                attackCoroutine = StartCoroutine(DelayBforeAttacking());
                break;
            case EnemyState.Dead:
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
    }
    public void AddImpact(Vector3 direction, float force)
    {
        direction.Normalize();
        direction.y = 0;
        impact += direction * force;
    }
}