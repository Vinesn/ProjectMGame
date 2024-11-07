using System.Collections;
using UnityEngine;

public class CharacterKnockback : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool isKnockedBack;
    private bool isStunned;
    private Coroutine knockbackCoroutine;
    private bool canMove = true;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void KnockbackCharacter(Vector2 CharacterPosition, float stunTime, float knockBackTime, float knockbackForce)
    {
        if (isKnockedBack) return;

        Vector2 knockbackDirection = (transform.position - (Vector3)CharacterPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        isKnockedBack = true;

        if (knockbackCoroutine != null)
        {
            StopCoroutine(knockbackCoroutine);
        }

        knockbackCoroutine = StartCoroutine(KnockingbackCoroutine(stunTime, knockBackTime));
    }

    private IEnumerator KnockingbackCoroutine(float stunTime, float knockBackTime)
    {
        yield return new WaitForSeconds(knockBackTime);
        rb.linearVelocity = Vector2.zero;
        isKnockedBack = false;

        StartStun(stunTime);
    }

    private void StartStun(float stunTime)
    {
        canMove = false;
        isStunned = true;

        StartCoroutine(EndStun(stunTime));
    }

    private IEnumerator EndStun(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);

        isStunned = false;
        canMove = true;
    }

    public bool IsStunned()
    {
        return isStunned;
    }

    public void SetCanMove(bool value)
    {
        canMove = value;
    }

    public bool CanMove()
    {
        return canMove;
    }
}
