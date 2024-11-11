using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    float playerHp;
    float movementSpeed;
    PlayerBaseStats playerBaseStats;
    private Vector2 movementInput;
    private Vector2 lastPlayerMoveDirection = new Vector2(0, -1);
    private bool isPlayerMoving;
    private Rigidbody2D rb;
    private Animator animator;
    private PlayerControls playerInput;
    private float knockBackTime = 0.1f;
    private bool isKnockedBack;
    private bool isStunned = false;
    private Coroutine knockback;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        playerBaseStats = GetComponent<PlayerBaseStats>();
        playerInput = new PlayerControls();
    }

    private void Start()
    {
        movementSpeed = playerBaseStats.movementSpeed;
    }

    void OnEnable()
    {
        playerInput.Enable();
        playerInput.Player.Attack.started += _ => BaseAttack();
    }

    private void Update()
    {
        PlayerInput();
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero && !isKnockedBack && !isStunned)
        {
            isPlayerMoving = true;
            Move();

            animator.SetFloat("MoveX", movementInput.x);
            animator.SetFloat("MoveY", movementInput.y);
            animator.SetBool("isMoving", isPlayerMoving);

            lastPlayerMoveDirection = movementInput.normalized;
        }
        else
        {
            isPlayerMoving = false;
            animator.SetFloat("MoveX", lastPlayerMoveDirection.x);
            animator.SetFloat("MoveY", lastPlayerMoveDirection.y);
            animator.SetBool("isMoving", isPlayerMoving);
        }

    }

    private void BaseAttack()
    {
        animator.SetTrigger("DoBaseAttack");
    }

    public void ReceiveDamageFrom(EnemyAI enemy)
    {
        float damage = enemy.GetEnemyStats().AttackDamage;
        playerBaseStats.TakeDamage(damage);
        if (isKnockedBack) return;
        KnockbackPlayer(enemy.transform.position, enemy.stunTime, enemy.knockbackForce);
    }

    public void KnockbackPlayer(Vector2 enemyPosition, float stunTime, float knockbackForce)
    {
        Vector2 knockbackDirection = (transform.position - (Vector3)enemyPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        isKnockedBack = true;

        if (knockback != null)
            {
                StopCoroutine(knockback);
            }

        knockback = StartCoroutine(KnockingbackCorutine(stunTime));
    }

    private IEnumerator KnockingbackCorutine(float stunTime)
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;
        isStunned = true;
        yield return new WaitForSeconds(stunTime);
        isStunned = false;
    }

    void PlayerInput()
    {
        movementInput = playerInput.Player.Move.ReadValue<Vector2>();
    }

    void Move()
    {
        Vector2 playerMove = (rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
        rb.MovePosition(playerMove);
    }
}