using UnityEngine;

public class EnemyDetection : MonoBehaviour
{
    Transform playerTransform;
    public Vector2 playerPosition { get; private set; }
    bool playerOnSight = false;
    public float sightRange = 0.5f;

    void Start()
    {
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerTransform = playerObject.transform;
            playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
        }
    }

    public bool IsPlayerInSight()
    {
        if (playerTransform != null)
        {
            playerPosition = new Vector2(playerTransform.position.x, playerTransform.position.y);
            float distanceToPlayer = (playerPosition - (Vector2)transform.position).sqrMagnitude;
            playerOnSight = distanceToPlayer < (sightRange * sightRange);
        }

        return playerOnSight;
    }
}
