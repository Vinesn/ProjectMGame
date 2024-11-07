using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float MovementSpeed = 1.0f;
    Vector2 LastPlayerMoveDirection = new Vector2(0, -1);
    Vector2 movementInput;
    Rigidbody2D rb;
    Animator animator;
    CharacterStateControl charStateControl;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        charStateControl = GetComponent<CharacterStateControl>();
    }

    void Update()
    {
        bool isMoving = charStateControl.CanMove() && movementInput != Vector2.zero;
        animator.SetBool("isMoving", isMoving);
    }

    void FixedUpdate()
    {
        if (charStateControl.CanMove() && movementInput != Vector2.zero)
        {
            Vector2 playerMove = rb.position + movementInput * MovementSpeed * Time.fixedDeltaTime;
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

    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }
}
