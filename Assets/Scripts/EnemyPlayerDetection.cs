using UnityEngine;

public class EnemyPlayerDetection : MonoBehaviour
{
    EnemyBaseStats enemyStats;
    [SerializeField] float hitboxRadius;
    [SerializeField] bool showGizmos = true;
    GameObject player;
    public bool PlayerOnSight { get; private set; }
    int layerMask;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponentInParent<EnemyBaseStats>();
        layerMask = LayerMask.GetMask("PlayerPresence", "Obstacles");
    }

    private void Start()
    {
        hitboxRadius = enemyStats.sightRadius;
    }

    private void FixedUpdate()
    {
        DetectPlayerInSightArea();
    }

    void DetectPlayerInSightArea()
    {
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, hitboxRadius, layerMask);

        if (detectedPlayer != null && detectedPlayer.CompareTag("Player"))
        {
            DetectPlayerSightLine(detectedPlayer);
            Debug.Log("Detected Player in area!");
        }
        else
        {
            Debug.Log("No player in area");
        }
    }
    void DetectPlayerSightLine(Collider2D detectedPlayer)
    {
        Vector2 directionToPlayer = (detectedPlayer.transform.position - transform.position).normalized;
        RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, directionToPlayer, hitboxRadius, layerMask);

        if (lineOfSight.collider != null)
        {
            PlayerOnSight = lineOfSight.collider.CompareTag("Player");
        }
        else
        {
            PlayerOnSight = false;
        }
    }

    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), hitboxRadius);
            Collider2D detectedObject = Physics2D.OverlapCircle(transform.position, hitboxRadius);

            if (detectedObject != null)
            {
                if (detectedObject.CompareTag("Player"))
                {
                    Gizmos.color = Color.green;
                    Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), hitboxRadius);

                    Vector2 directionToObject = (detectedObject.transform.position - transform.position).normalized;
                    RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, directionToObject, hitboxRadius);
                    if (lineOfSight.collider != null)
                    {
                        if (lineOfSight.collider.CompareTag("Player"))
                        {
                            Gizmos.color = Color.green;
                            Gizmos.DrawRay(transform.position, directionToObject * hitboxRadius);
                        }
                        else
                        {
                            Gizmos.color = Color.yellow;
                            Gizmos.DrawRay(transform.position, directionToObject * hitboxRadius);
                        }
                    }
                    else
                    {
                        Gizmos.color = Color.red;
                        Gizmos.DrawRay(transform.position, directionToObject * hitboxRadius);
                    }
                }
                else
                {
                    Gizmos.color = Color.yellow;
                    Gizmos.DrawWireSphere(new Vector2(transform.position.x, transform.position.y), hitboxRadius);
                }
            }
        }
    }
    // PLAYER TAG W MAIN NODE I CHILD NODE POWODUJ• B£•D, NIE MOZE ZNALEè∆ KONTROLERA W HITBOXIE PLAYERA, TRZEBA JAKOS ZROBIC PRZERZUTE LOGIKI DO PARENTA NIE NA CHILD?? IDK XD

}