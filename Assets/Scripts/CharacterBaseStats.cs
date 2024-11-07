using UnityEngine;

public class CharacterBaseStats : MonoBehaviour
{
    public float MaxHealthPoints = 100.0f;
    public float CurrentHealth { get; private set; }
    public float AttackDamage = 10f;
    public float Defense = 5f;

    private void Awake()
    {
        CurrentHealth = MaxHealthPoints;
    }
    public void TakeDamage(float damage)
    {
        //Wz�r na Damage (WZ�R, MIN WARTO��, MAX WARTO��)
        float appliedDamage = Mathf.Clamp(damage - Defense, 0, float.MaxValue);

        CurrentHealth -= appliedDamage;

        if (CurrentHealth <= 0)
        {
            Debug.Log(gameObject.name + " HP zmieni�o si�: " + CurrentHealth);
        }
    }
    public void Heal(float healAmount)
    {
        //Wz�r na heal (WZ�R, MIN WARTO��, MAX WARTO��)
        CurrentHealth = Mathf.Clamp(CurrentHealth + healAmount, 0, MaxHealthPoints);
        Debug.Log(gameObject.name + " zosta� uleczony. Aktualne HP: " + CurrentHealth);
    }

    protected virtual void Die()
    {
        Debug.Log(gameObject.name + " zgin��.");
    }
}