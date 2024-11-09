using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    Rigidbody2D rb;
    CharacterStateControl characterStateControl;
    bool isSlowingDown = false;
    Vector2 lastTargetDirection = Vector2.zero;
    float currentSpeed;
    Transform playerTransform;
    Vector2 playerPosition;
    [SerializeField] float moveSpeed = 0.5f;
    [SerializeField] float searchAttentionSpan = 0.1f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
        }
    }

    public void MoveTowardsPlayer(Vector2 playerPosition)
    {
        if (characterStateControl.CanMove())
        {
            Vector2 direction = (playerPosition - rb.position).normalized;
            Vector2 enemyMove = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(enemyMove);

            lastTargetDirection = direction;
            isSlowingDown = false;
        }
    }
    public void SlowDownAfterLosingSight()
    {
        if (lastTargetDirection != Vector2.zero && !isSlowingDown)
        {
            currentSpeed = moveSpeed;
            isSlowingDown = true;
        }

        if (currentSpeed > 0)
        {
            currentSpeed -= Time.deltaTime * searchAttentionSpan;
            if (currentSpeed < 0)
            {
                currentSpeed = 0;
            }

            Vector2 enemyMove = rb.position + lastTargetDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(enemyMove);
        }

        if (currentSpeed == 0)
        {
            lastTargetDirection = Vector2.zero;
            isSlowingDown = false;
        }
    }
}
