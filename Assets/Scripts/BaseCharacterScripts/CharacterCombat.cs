using UnityEngine;

[RequireComponent(typeof(CharacterBaseStats))]
[RequireComponent(typeof(CharacterStateControl))]
[RequireComponent(typeof(CharacterKnockback))]
public class CharacterCombat : MonoBehaviour
{
    CharacterBaseStats charStats;
    CharacterStateControl charStateControl;
    CharacterKnockback applyKnockback;

    void Awake()
    {
        charStats = GetComponent<CharacterBaseStats>();
        charStateControl = GetComponent<CharacterStateControl>();
        applyKnockback = GetComponent<CharacterKnockback>();
    }

    public void DealDamageToEntity(EnemyScript entity)
    {
        //Na póŸniej
    }

    public void TakeDamageFromEntity(EnemyScript entity)
    {
        charStats.TakeDamage(entity.enemyDamage);

        if (!charStateControl.IsStunned())
        {
            applyKnockback.KnockbackCharacter(entity.transform.position, entity.stunTime, entity.knockBackTime, entity.knockbackForce);
        }

        if (charStats.CurrentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " zgin¹³.");
        //Logika œmierci vvv
    }
}
