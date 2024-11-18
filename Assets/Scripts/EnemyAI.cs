using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class EnemyAI : MonoBehaviour
{
    public float searchAttentionSpan = 0.1f;
    public float knockBackTime = 1f;
    public float knockbackForce = 2f;
    public float stunTime = 1f;
    [SerializeField] private float patrolRadius = 2f;
    [SerializeField] private float patrolFrequency = 5f;
    private Rigidbody2D rb;
    private PlayerController player;
    private Coroutine damaging;
    private float lastAttackTime;
    private float currentSpeed;
    private Vector2 lastTargetDirection = Vector2.zero;
    private Vector2 patrolTarget;
    private EnemyBaseStats enemyBaseStats;
    private EnemyPlayerDetection playerDetection;
    private EnemyStateMachine enemyStateMachine;
    bool playerOnSight;

    private Vector2 currentVelocity = Vector2.zero;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyBaseStats = GetComponent<EnemyBaseStats>();
        enemyStateMachine = GetComponent<EnemyStateMachine>();
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

        patrolTarget = GetNewPatrolPoint();
    }

    void Update()
    {
        if (playerDetection != null)
        {
            playerOnSight = playerDetection.PlayerOnSight;
        }

        if (playerOnSight)
        {
            enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Walk);
        }
        else
        {
            if (enemyStateMachine.GetEnemyBaseState() != EnemyStateMachine.BaseState.Patrol)
            {
                ChangeToPatrolState();
            }
        }
        if (enemyStateMachine.GetEnemyBaseState() == EnemyStateMachine.BaseState.Wait)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    private void FixedUpdate()
    {
        if (playerOnSight && enemyStateMachine.GetEnemyBaseState() == EnemyStateMachine.BaseState.Walk)
        {
            Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
            enemyBasicMove(direction);

            lastTargetDirection = direction;
        }

        if (!playerOnSight && lastTargetDirection != Vector2.zero)
        {
            enemyMotionSlowdown();
        }

        if (enemyStateMachine.GetEnemyBaseState() == EnemyStateMachine.BaseState.Patrol)
        {
            MoveTowardsPatrolTarget();

            if (Vector2.Distance(rb.position, patrolTarget) < 0.1f)
            {
                StartCoroutine(WaitBeforeNextPatrol());
            }
        }
    }

    public EnemyBaseStats GetEnemyStats()
    {
        return enemyBaseStats;
    }

    private IEnumerator DealDamage(GameObject playerObject)
    {
        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Attack);
        player.ReceiveDamageFrom(this);
        lastAttackTime = Time.time;

        yield return new WaitForSeconds(enemyBaseStats.attackCD);

        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Walk);
    }

    void enemyMotionSlowdown()
    {
        if (enemyStateMachine.GetEnemyBaseState() != EnemyStateMachine.BaseState.SlowDown)
        {
            currentSpeed = enemyBaseStats.movementSpeed;
            enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.SlowDown);
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
            enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Wait);
            StartCoroutine(WaitBeforeNextPatrol());  // Czekamy, a¿ minie czas patrolu
        }
    }

    void ChangeToPatrolState()
    {
        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Patrol);
    }

    void enemyBasicMove(Vector2 direction)
    {
        Vector2 enemyMove = rb.position + direction * enemyBaseStats.movementSpeed * Time.fixedDeltaTime;
        rb.MovePosition(enemyMove);
    }

    void enemyPatrolMove()
    {
        Vector2 patrolCheckpointPosition = Random.insideUnitCircle * patrolRadius;
        Vector2 patrolMove = rb.position + (patrolCheckpointPosition * (enemyBaseStats.movementSpeed * 0.7f) * Time.fixedDeltaTime);
        rb.MovePosition(patrolMove);
    }

    Vector2 GetNewPatrolPoint()
    {
        return (Vector2)transform.position + Random.insideUnitCircle * patrolRadius;
    }

    float GetRandomTimeRange(float min, float max)
    {
        return Random.Range(min, max);
    }

    private void MoveTowardsPatrolTarget()
    {
        Vector2 direction = (patrolTarget - rb.position).normalized;
        rb.MovePosition(rb.position + direction * enemyBaseStats.movementSpeed * Time.fixedDeltaTime);
    }

    private IEnumerator WaitBeforeNextPatrol()
    {
        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Wait);

        yield return new WaitForSeconds(patrolFrequency);

        patrolTarget = GetNewPatrolPoint();

        ChangeToPatrolState();
    }
}
