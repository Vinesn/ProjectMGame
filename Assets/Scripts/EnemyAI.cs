using UnityEngine;
using System.Collections;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{
    public float searchAttentionSpan = 0.1f;
    public float knockBackTime = 1f;
    public float knockbackForce = 2f;
    public float stunTime = 1f;
    [SerializeField] private float patrolRadius = 2f;
    [SerializeField] private float patrolFrequency = 5f;
    [SerializeField][Range(0.1f, 1.0f)] private float patrolMoveSpeed = 0.4f;
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

    [Header("PathFinder")]
    public Transform target;
    public float nextWaypointDistance = 3f;
    public float speed = 300f;
    public float pathUpdateSeconds = 0.5f;
    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    bool reachedEndOfPath = false;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        enemyBaseStats = GetComponent<EnemyBaseStats>();
        enemyStateMachine = GetComponent<EnemyStateMachine>();
        playerDetection = GetComponentInChildren<EnemyPlayerDetection>();
    }

    void Start()
    {
        GameObject[] playerObject = GameObject.FindGameObjectsWithTag("Player");

        if (playerObject != null)
        {
            foreach (GameObject obj in playerObject)
            {
                player = obj.GetComponent<PlayerController>();
                if (player != null)
                {
                    break;
                }
            }
        }
        else
        {
            Debug.Log("Nie znaleziono Obiektu Gracz.");
        }
        //patrolTarget = GetNewPatrolPoint();

        InvokeRepeating("UpdatePath", 0f, pathUpdateSeconds);
    }
    void UpdatePath()
    {
        if (seeker.IsDone())
        {
        seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }           
    }
    void PathFollow()
    {
        if (path == null)
        {
            return;
        }
        if (currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.linearVelocity = Vector2.SmoothDamp(rb.linearVelocity, force, ref currentVelocity, 0.5f); ;
        
        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    void Update()
    {
        //if (playerDetection != null)
        //{
        //    playerOnSight = playerDetection.PlayerOnSight;
        //}

        //if (playerOnSight)
        //{
        //    enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Pursue);
        //}
    }

    private void FixedUpdate()
    {
        PathFollow();

        //if (playerOnSight && enemyStateMachine.GetEnemyBaseState() == EnemyStateMachine.BaseState.Pursue)
        //{
        //    Vector2 direction = ((Vector2)player.transform.position - rb.position).normalized;
        //    EnemyBasicMove(direction, enemyBaseStats.movementSpeed);

        //    lastTargetDirection = direction;
        //}

        //if (!playerOnSight && lastTargetDirection != Vector2.zero)
        //{
        //    enemyMotionSlowdown();
        //}

        //if (enemyStateMachine.GetEnemyBaseState() == EnemyStateMachine.BaseState.Patrol)
        //{
        //    MoveTowardsPatrolTarget();

        //    if (Vector2.Distance(rb.position, patrolTarget) < 0.1f)
        //    {
        //        StartCoroutine(WaitBeforeNextPatrol());
        //    }
        //}
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

        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Pursue);
    }
    void enemyMotionSlowdown()
    {
        if (enemyStateMachine.GetEnemyBaseState() != EnemyStateMachine.BaseState.SlowDown)
        {
            enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.SlowDown);
            currentSpeed = enemyBaseStats.movementSpeed;
        }
        if (currentSpeed > 0)
        {
            currentSpeed -= Time.deltaTime * searchAttentionSpan;
            Vector2 enemyMove = rb.position + lastTargetDirection * currentSpeed * Time.fixedDeltaTime;
            rb.MovePosition(enemyMove);
        }

        if (currentSpeed <= 0)
        {
            Debug.Log("Speed reached 0, switching to Patrol state");
            lastTargetDirection = Vector2.zero;
            enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Wait);
            StartCoroutine(WaitBeforeNextPatrol());
        }
    }

    void ChangeToPatrolState()
    {
        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Patrol);
    }

    void EnemyBasicMove(Vector2 direction, float speed)
    {
        Vector2 enemyMove = rb.position + direction * speed * Time.fixedDeltaTime;
        rb.MovePosition(enemyMove);
    }

    Vector2 GetNewPatrolPoint()
    {
        Vector2 newPatrolPoint = (Vector2)transform.position + Random.insideUnitCircle * patrolRadius;
        return newPatrolPoint;
    }

    float GetRandomTimeRange(float min, float max)
    {
        return Random.Range(min, max);
    }

    private void MoveTowardsPatrolTarget()
    {
        Vector2 direction = (patrolTarget - rb.position).normalized;
        EnemyBasicMove(direction, (enemyBaseStats.movementSpeed * patrolMoveSpeed));
    }

    private IEnumerator WaitBeforeNextPatrol()
    {
        enemyStateMachine.SetEnemyState(EnemyStateMachine.BaseState.Wait);
        rb.MovePosition(transform.position);
        yield return new WaitForSeconds(patrolFrequency);

        patrolTarget = GetNewPatrolPoint();

        ChangeToPatrolState();
    }
}