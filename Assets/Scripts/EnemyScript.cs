using UnityEngine;
using System.Collections;
using Unity.Cinemachine;

public class EnemyScript : MonoBehaviour
{
    public float sightRange = 1f;
    public float searchAttentionSpan = 0.1f;
    public float moveSpeed = 0.5f;
    public float enemyDamage = 10f;
    public float attackCD = 2f;
    public float knockBackTime = 1f;
    public float knockbackForce = 2f;
    public float stunTime = 1f;
    private Rigidbody2D rb;
    private PlayerControl player;
    private Coroutine damaging;
    private bool playerOnSight;
    private bool canMove = true;
    private bool isSlowingDown = false;
    private float lastAttackTime;
    private float currentSpeed;
    private Vector2 lastTargetDirection = Vector2.zero;

    //DODAÆ KOLIZJE ENEMY Z ENEMY (Hitbox jako child)

    //private bool isRoaming = true;
    private Vector2 currentVelocity = Vector2.zero;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            player = playerObject.GetComponent<PlayerControl>();
        }
        else
        {
            Debug.Log("Nie znaleziono Obiektu Gracz.");
        }
    }

    void Update()
    {
        if (player != null)
        {
            Vector2 playerLocation = player.playerMapLocation;
            float distanceToPlayer = (playerLocation - (Vector2)transform.position).sqrMagnitude;
            playerOnSight = distanceToPlayer < sightRange*sightRange;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCD)
            {
                if (damaging != null)
                    {
                        StopCoroutine(damaging);
                    }
                damaging = StartCoroutine(DealDamage(other.gameObject));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastAttackTime = 0;
        }
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
            Vector2 direction = (player.playerMapLocation - rb.position).normalized;
            Vector2 enemyMove = rb.position + direction * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(enemyMove);

            lastTargetDirection = direction;
            isSlowingDown = false;
        }
        if (!playerOnSight && lastTargetDirection != Vector2.zero)
        {
            if (!isSlowingDown)
            {
                currentSpeed = moveSpeed;
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

    private IEnumerator DealDamage(GameObject playerObject)
    {
        canMove = false;

        if (playerObject != null)
        {
            player.TakeDamage(this);
        }

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(attackCD);

        canMove = true;
    }
}
