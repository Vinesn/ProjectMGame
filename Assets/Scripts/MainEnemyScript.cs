using UnityEngine;

[RequireComponent(typeof(EnemyMovement))]
[RequireComponent(typeof(EnemyCombat))]
[RequireComponent(typeof(EnemyDetection))]
[RequireComponent(typeof(CharacterBaseStats))]
[RequireComponent(typeof(EnemyStateControl))]
public class Enemy : MonoBehaviour
{
    EnemyMovement enemyMovement;
    EnemyDetection enemyDetection;
    EnemyCombat enemyCombat;

    private void Awake()
    {
        enemyMovement = GetComponent<EnemyMovement>();
        enemyDetection = GetComponent<EnemyDetection>();
        enemyCombat = GetComponent<EnemyCombat>();
    }

    private void FixedUpdate()
    {
        if (enemyDetection.IsPlayerInSight())
        {
            enemyMovement.MoveTowardsPlayer(enemyDetection.playerPosition);
        }
        else
        {
            enemyMovement.SlowDownAfterLosingSight();
        }
    }
}
