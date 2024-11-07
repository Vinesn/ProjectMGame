using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public float movementSpeed = 1f;
    public Vector2 playerMapLocation;
    public float playerHP = 100f;

    private Vector2 movementInput;
    private Vector2 lastPlayerMoveDirection = new Vector2(0, -1);
    private bool isPlayerMoving;
    private Rigidbody2D rb;
    private Animator animator;
    private float knockBackTime = 0.1f;
    private bool isKnockedBack;
    private bool isStunned = false;
    private Coroutine knockback;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        playerMapLocation = new Vector2(transform.position.x, transform.position.y);
    }

    private void FixedUpdate()
    {
        if (movementInput != Vector2.zero && !isKnockedBack && !isStunned)
        {
            isPlayerMoving = true;
            Vector2 playerMove = (rb.position + movementInput * movementSpeed * Time.fixedDeltaTime);
            rb.MovePosition(playerMove);

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

    public void TakeDamage(EnemyScript enemy)
    {
        playerHP -= enemy.enemyDamage;
        Debug.Log("Player HP changed to: " + playerHP);
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

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
