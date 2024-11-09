using UnityEngine;

public class EnemyStateControl : CharacterStateControl
{
    public enum EnemyState
    {
        Stunned,
        KnockedBack,
        Patrolling,
        ChasingPlayer,
        AttackingPlayer
    }

    private EnemyState currentEnemyState = EnemyState.Patrolling;

    public EnemyState GetCurrentEnemyState()
    {
        return currentEnemyState;
    }

    public void SetEnemyState(EnemyState newState)
    {
        currentEnemyState = newState;
    }

    public bool IsChasingPlayer()
    {
        return currentEnemyState == EnemyState.ChasingPlayer;
    }

    public bool IsAttackingPlayer()
    {
        return currentEnemyState == EnemyState.AttackingPlayer;
    }
    public bool IsPatrolling()
    {
        return currentEnemyState == EnemyState.Patrolling;
    }

    //void Update()
    //{
    //    switch (currentEnemyState)
    //    {
    //        case EnemyState.Patrolling:
    //            Patrol();
    //            break;

    //        case EnemyState.ChasingPlayer:
    //            ChasePlayer();
    //            break;

    //        case EnemyState.AttackingPlayer:
    //            AttackPlayer();
    //            break;

    //        case EnemyState.Stunned:
    //            break;
    //    }
    //}

    //private void Patrol()
    //{
    //    // Logika patrolowania
    //    Debug.Log("Enemy is patrolling");
    //}

    //private void ChasePlayer()
    //{
    //    // Logika poœcigu
    //    Debug.Log("Enemy is chasing the player");
    //}

    //private void AttackPlayer()
    //{
    //    // Logika atakowania
    //    Debug.Log("Enemy is attacking the player");
    //}
}
