using UnityEngine;

public class EnemyBaseStats : MonoBehaviour
{
    public float MaxHealthPoints = 100.0f;
    public float CurrentHealth { get; private set; }
    public float AttackDamage = 10f;
    public float attackCD = 2f;
    public float Defense = 5f;
    public float movementSpeed = 900f;
    public float sightRadius = 1f;

    private void Start()
    {
        CurrentHealth = MaxHealthPoints;
    }

    public void TakeDamage(float damage)
    {
        float appliedDamage = Mathf.Clamp(damage - Defense, 0, float.MaxValue);
        CurrentHealth -= appliedDamage;
        Debug.Log(gameObject.name + " HP zmieni³o siê: " + CurrentHealth);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    public void Heal(float healAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount, 0, MaxHealthPoints);
        Debug.Log(gameObject.name + " zosta³ uleczony. Aktualne HP: " + CurrentHealth);
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " zgin¹³.");
        Destroy(gameObject);
    }
}
