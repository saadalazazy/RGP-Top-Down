using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq.Expressions;
using System.Net.Http.Headers;
using Unity.Mathematics;
using UnityEngine;

public class Player : MonoBehaviour
{
    #region player
    private Vector3 movmentVelocity;
    private CharacterController characterController;
    private PlayerInput input;
    [SerializeField] float moveSpeed;
    [SerializeField] float gravity;
    [SerializeField] Animator animator;
    [SerializeField] float rotationSmoothSpeed = 10f;
    private float verticalVelocity;
    #endregion

    #region enemy
    [SerializeField] bool isPlayer = true;
    UnityEngine.AI.NavMeshAgent agent;
    Transform targetPlayer;
    [SerializeField] float rotationSmoothSpeedEnmey;
    #endregion

    #region states
    [SerializeField] float dashSpeed;
    public enum PlayerState
    {
        Normal, Attacking, Dead, BeingHit,dash
    }
    [SerializeField] public PlayerState playerState;
    #endregion

    #region attackslide
    float attackStartTime;
    [SerializeField] float attackSlideTimeDuration;
    [SerializeField] float attackSlideSpeed;
    [SerializeField] float attackSlideDelay = 0.2f;
    private bool hasStartedSlide = false;
    CharacterHealth health;
    [SerializeField] DamageCaster damageCaster;

    // Attack cooldown variables
    private float lastAttackTime;
    [SerializeField] float attackCooldown = 0.5f; // Player attack cooldown
    [SerializeField] float enemyAttackCooldown = 1.5f; // Enemy attack cooldown
    #endregion

    Vector3 impactOnCharcter;
    [SerializeField] float invinsibiltyDuratoin;
    public bool isInvinsable = false;

    [SerializeField] float delaySweitchToNormal;
    [SerializeField] float delayBeforeEnemyAttacking;

    [SerializeField] PlayerVFX playerVfx;

    [SerializeField] float attackAnimationDuration;
    private void Start()
    {
        characterController = GetComponent<CharacterController>();
        health = GetComponent<CharacterHealth>();
        if (!isPlayer)
        {
            agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
            targetPlayer = GameObject.FindWithTag("Player").transform;
            agent.speed = moveSpeed;
        }
        else
        {
            input = GetComponent<PlayerInput>();
        }

        lastAttackTime = -attackCooldown; // Allows immediate first attack
    }

    void CalculateMovmentVlocity()
    {
        if (input.GetXButton() && characterController.isGrounded && Time.time >= lastAttackTime + attackCooldown)
        {
            SwitchStateTo(PlayerState.Attacking);
            return;
        }
        else if(input.GetShiftButton() && characterController.isGrounded)
        {
            SwitchStateTo(PlayerState.dash);
            return;
        }

        animator.SetFloat("speed", new Vector3(input.GetHorizontal(), 0, input.GetVertical()).magnitude);

        movmentVelocity.Set(input.GetHorizontal(), 0, input.GetVertical());
        movmentVelocity.Normalize();
        movmentVelocity = Quaternion.Euler(0, -45f, 0) * movmentVelocity;
        movmentVelocity *= moveSpeed * Time.deltaTime;

        if (movmentVelocity != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(movmentVelocity);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeed * Time.deltaTime);
        }

        animator.SetBool("full", !characterController.isGrounded);
    }

    void CalculateEnemyMovment()
    {
        float distance = Vector3.Distance(targetPlayer.position, transform.position);

        if (distance >= agent.stoppingDistance)
        {
            if (agent.isStopped)
                agent.isStopped = false;

            agent.SetDestination(targetPlayer.position);
            animator.SetFloat("speed", 0.2f);
        }
        else
        {
            agent.isStopped = true;
            animator.SetFloat("speed", 0f);

            if (Time.time >= lastAttackTime + enemyAttackCooldown)
            {
                SwitchStateTo(PlayerState.Attacking);
            }
        }
    }

    private void FixedUpdate()
    {
        switch (playerState)
        {
            case PlayerState.Normal:
                if (isPlayer)
                    CalculateMovmentVlocity();
                else
                    CalculateEnemyMovment();
                break;
            case PlayerState.Attacking:
                if (isPlayer)
                {
                    float timePassed = Time.time - attackStartTime;

                    if (timePassed >= attackSlideDelay && timePassed < attackSlideDelay + attackSlideTimeDuration)
                    {
                        if (!hasStartedSlide)
                        {
                            hasStartedSlide = true;
                        }

                        float lerpTime = (timePassed - attackSlideDelay) / attackSlideTimeDuration;
                        movmentVelocity = Vector3.Lerp(transform.forward * attackSlideSpeed, Vector3.zero, lerpTime);
                    }
                    if(input.GetXButton() && characterController.isGrounded)
                    {
                        string currentClip = animator.GetCurrentAnimatorClipInfo(0)[0].clip.name;
                        attackAnimationDuration = animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
                        if(currentClip != "Dualwield_Melee_Attack_Slice" && attackAnimationDuration > 0.5 && attackAnimationDuration < 0.7)
                        {
                            input.SetXButton(false);
                            SwitchStateTo(PlayerState.Attacking);
                            CalculateMovmentVlocity();
                        }
                    }
                }
                if (!isPlayer)
                {
                    Quaternion targetRotation = Quaternion.LookRotation(targetPlayer.position - transform.position);
                    transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSmoothSpeedEnmey * Time.deltaTime);
                }
                break;
            case PlayerState.Dead:
                return;
            case PlayerState.BeingHit:
                if (impactOnCharcter.magnitude > 0.2)
                {
                    movmentVelocity = impactOnCharcter * Time.deltaTime;
                }
                impactOnCharcter = Vector3.Lerp(impactOnCharcter, Vector3.zero, Time.deltaTime * 5);
                break;
            case PlayerState.dash:
                movmentVelocity = transform.forward * dashSpeed * Time.deltaTime;
                break;
        }

        if (isPlayer)
        {
            if (characterController.isGrounded)
                verticalVelocity = gravity * 0.3f;
            else
                verticalVelocity = gravity;

            movmentVelocity += verticalVelocity * Vector3.up * Time.deltaTime;

            characterController.Move(movmentVelocity);
            movmentVelocity = Vector3.zero;
        }
    }

    public void SwitchStateTo(PlayerState newState)
    {
        if (isPlayer)
        {
            input.ClearCash();
        }

        switch (playerState)
        {
            case PlayerState.Normal:
                break;
            case PlayerState.Attacking:
                if (damageCaster != null)
                    damageCaster.DisableDamaageCollider();
                break;
            case PlayerState.Dead:
                return;
            case PlayerState.BeingHit:
                break;
            case PlayerState.dash:
                break;
        }

        switch (newState)
        {
            case PlayerState.Normal:
                break;
            case PlayerState.Attacking:
                if(isPlayer)
                {
                    animator.SetTrigger("attack");
                    lastAttackTime = Time.time;
                    if (isPlayer)
                    {
                        attackStartTime = Time.time;
                        hasStartedSlide = false;
                    }
                }
                else
                {
                    StartCoroutine(DeleySwitchToAttacking());
                }
                
                break;
            case PlayerState.Dead:
                characterController.enabled = false;
                animator.SetTrigger("dead");
                break;
            case PlayerState.BeingHit:
                animator.SetTrigger("bienghit");
                if(isPlayer)
                {
                    isInvinsable = true;
                    StartCoroutine(DeleyCancelInvencable());
                }
                break;
            case PlayerState.dash:
                animator.SetTrigger("dash");
                break;
        }

        playerState = newState;
        Debug.Log("player state : " + playerState);
    }
    public void DashAnimationEnd()
    {
        SwitchStateTo(PlayerState.Normal);
    }

    public void AttackanimationEnd()
    {
        if(isPlayer)
        {
            SwitchStateTo(PlayerState.Normal);
        }
        else
        {
            StartCoroutine(DeleySwitchToNormalState());
        }
    }

    public void BeingHitanimationEnd()
    {
        SwitchStateTo(PlayerState.Normal);
    }

    public void ApllyDamege(float damage, Vector3 attackerPos = new Vector3())
    {
        if(isInvinsable)
        {
            return;
        }    
        if (health != null)
        {
            health.TackDamege(damage);
        }
        if (isPlayer)
        {
            SwitchStateTo(PlayerState.BeingHit);
        }
        AddImpact(attackerPos, 2f);
    }
    IEnumerator DeleyCancelInvencable()
    {
        yield return new WaitForSeconds(invinsibiltyDuratoin);
        isInvinsable = false;
    }
    IEnumerator DeleySwitchToNormalState()
    {
        yield return new WaitForSeconds(delaySweitchToNormal);
        SwitchStateTo(PlayerState.Normal);
    }
    IEnumerator DeleySwitchToAttacking()
    {
        yield return new WaitForSeconds(delayBeforeEnemyAttacking);
        animator.SetTrigger("attack");
        lastAttackTime = Time.time;
    }
    public void AddImpact(Vector3 attacerPos , float force)
    {
        Vector3 impactDir = transform.position - attacerPos;
        impactDir.Normalize();
        impactDir.y = 0;
        impactOnCharcter = impactDir * force;
    }

    public void PickUpItem(PickUp pickUp)
    {
        switch (pickUp.type) 
        {
            case PickUp.Type.heal:
                AddHealthValue(pickUp.value);
                break;
            case PickUp.Type.coin:
                break;
        }
    }
    private void AddHealthValue(float value)
    {
        health.TakeHealth(value);
        playerVfx.HealVfx();
    }
}