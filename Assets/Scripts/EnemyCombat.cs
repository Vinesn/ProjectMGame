using UnityEngine;
using System.Collections;

public class EnemyCombat : CharacterCombat
{
    [SerializeField] float lastAttackTime;
    [SerializeField] float attackCD = 1f;
    Coroutine damaging;
    EnemyStateControl enemyState;
    CharacterBaseStats playerStat;
    CharacterBaseStats myselfStat;

    private void Awake()
    {
        myselfStat = GetComponent<CharacterBaseStats>();
        GameObject playerObject = GameObject.FindWithTag("Player");

        if (playerObject != null)
        {
            playerStat = GetComponent<CharacterBaseStats>();
        }

    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            if (Time.time >= lastAttackTime + attackCD)
            {
                if (damaging != null)
                {
                    StopCoroutine(damaging);
                }
                damaging = StartCoroutine(DealDamageToEntity(other.gameObject));
            }
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            lastAttackTime = 0;
        }
    }
    IEnumerator DealDamageToEntity(GameObject playerObject)
    {
        enemyState.SetEnemyState(EnemyStateControl.EnemyState.AttackingPlayer);

        if (playerObject != null)
        {
            playerStat.TakeDamage(myselfStat.AttackDamage);
        }

        lastAttackTime = Time.time;

        yield return new WaitForSeconds(attackCD);

        enemyState.SetEnemyState(EnemyStateControl.EnemyState.Patrolling);
    }
}
