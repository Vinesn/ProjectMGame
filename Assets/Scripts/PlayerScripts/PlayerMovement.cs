using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1.0f;
    Vector2 LastPlayerMoveDirection = new Vector2(0, -1);
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;

    private CharacterKnockback charKnockback;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        charKnockback = GetComponent<CharacterKnockback>();
    }

    void Update()
    {
        bool isMoving = charKnockback.CanMove() && movementInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        if (charKnockback.CanMove() && movementInput != Vector2.zero)
        {
            Vector2 playerMove = (rb.position + movementInput * MovementSpeed * Time.fixedDeltaTime);
            rb.MovePosition(playerMove);

            animator.SetFloat("MoveX", movementInput.x);
            animator.SetFloat("MoveY", movementInput.y);

            LastPlayerMoveDirection = movementInput.normalized;
        }
        else
        {
            animator.SetFloat("MoveX", LastPlayerMoveDirection.x);
            animator.SetFloat("MoveY", LastPlayerMoveDirection.y);
        }
    }

    public void SetCanMove(bool value)
    {
        charKnockback.SetCanMove(value);
    }

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
