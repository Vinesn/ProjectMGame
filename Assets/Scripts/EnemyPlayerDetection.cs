using UnityEngine;

public class EnemyPlayerDetection : MonoBehaviour
{
    EnemyBaseStats enemyStats;
    [SerializeField] float hitboxRadius;
    [SerializeField] bool showGizmos = true;
    GameObject player;
    public bool PlayerOnSight { get; private set; }

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        enemyStats = GetComponentInParent<EnemyBaseStats>();
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
        Collider2D detectedPlayer = Physics2D.OverlapCircle(transform.position, hitboxRadius);

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
        RaycastHit2D lineOfSight = Physics2D.Raycast(transform.position, directionToPlayer, hitboxRadius);

        if (lineOfSight.collider != null && lineOfSight.collider.CompareTag("Player"))
        {
            PlayerOnSight = true;
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

            Collider2D player = Physics2D.OverlapCircle(transform.position, hitboxRadius);
            if (player != null && player.CompareTag("Player"))
            {
                Gizmos.color = Color.green;

                Gizmos.DrawLine(transform.position, player.transform.position);
            }
        }
    }
    // Debug Gizoms poprawnie wykrywa gracza. Zepsuty jest skrypt chodzenia albo update pozycji gracza. Kolizja przy linii wzroku nie zatrzymuje Enemy chyba?

}
