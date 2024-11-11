using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class EnemyAI : MonoBehaviour
{
    public float searchAttentionSpan = 0.1f;
    public float knockBackTime = 1f;
    public float knockbackForce = 2f;
    public float stunTime = 1f;
    private Rigidbody2D rb;
    private PlayerController player;
    private Coroutine damaging;
    private bool canMove = true;
    private bool isSlowingDown = false;
    private float lastAttackTime;
    private float currentSpeed;
    private Vector2 lastTargetDirection = Vector2.zero;
    EnemyBaseStats enemyBaseStats;
    EnemyPlayerDetection playerDetection;

    bool playerOnSight;
    //DODAÆ KOLIZJE ENEMY Z ENEMY (Hitbox jako child)

    //private bool isRoaming = true;
    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBaseStats = GetComponent<EnemyBaseStats>();
        playerDetection = GetComponentInChildren<EnemyPlayerDetection>();
    }

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerController>();
        }
        else
        {
            Debug.Log("Nie znaleziono Obiektu Gracz.");
        }
    }

    void Update()
    {
        playerOnSight = playerDetection;
    }

    private void FixedUpdate()
    {
        //Roaming do animacji i statusu Braku Agro, useless na razie
        //bool isRoaming = !playerOnSight && !isSlowingDown;
        //if (isRoaming)
        //{
        //    Debug.Log("Sobie chodze");
        //}

        if (playerOnSight && canMove)
        {
            Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
            Vector2 enemyMove = rb.position + direction * enemyBaseStats.movementSpeed * Time.fixedDeltaTime;
            rb.MovePosition(enemyMove);

            lastTargetDirection = direction;
            isSlowingDown = false;
        }
        if (!playerOnSight && lastTargetDirection != Vector2.zero)
        {
            if (!isSlowingDown)
            {
                currentSpeed = enemyBaseStats.movementSpeed;
                isSlowingDown = true;
            }

            if (currentSpeed > 0)
            {
                currentSpeed -= Time.deltaTime * (searchAttentionSpan);
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
    public EnemyBaseStats GetEnemyStats()
    {
        return enemyBaseStats;
    }

    private IEnumerator DealDamage(GameObject playerObject)
    {
        canMove = false;
        player.ReceiveDamageFrom(this);
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(enemyBaseStats.attackCD);

        canMove = true;
    }
}
