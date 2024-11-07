using UnityEngine;

public class PlayerCombat : MonoBehaviour
{
    private CharacterBaseStats playerStats;
    private CharacterKnockback playerKnockback;

    void Start()
    {
        playerStats = GetComponent<CharacterBaseStats>();
        playerKnockback = GetComponent<CharacterKnockback>();
    }

    public void TakeDamage(EnemyScript enemy)
    {
        playerStats.TakeDamage(enemy.enemyDamage);

        if (!playerKnockback.IsStunned())
        {
            playerKnockback.KnockbackCharacter(enemy.transform.position, enemy.stunTime, enemy.knockBackTime, enemy.knockbackForce);
        }
    }
}
