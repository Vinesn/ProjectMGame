using UnityEngine;

public class EnemyPlayerDetection : MonoBehaviour
{
    EnemyBaseStats enemyStats;
    [SerializeField] float hitboxRadius;
    CircleCollider2D triggerHitbox;
    public bool PlayerOnSight { get; private set; }

    private void Awake()
    {
        enemyStats = GetComponentInParent<EnemyBaseStats>();
        triggerHitbox = GetComponent<CircleCollider2D>();
    }

    private void Start()
    {
        hitboxRadius = enemyStats.sightRadius;
        triggerHitbox.radius = hitboxRadius;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOnSight = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerOnSight = false;
        }
    }
}
