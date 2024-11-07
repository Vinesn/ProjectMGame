using System.Collections;
using UnityEngine;

public class CharacterKnockback : MonoBehaviour
{
    Rigidbody2D rb;
    Coroutine knockbackCoroutine;
    CharacterStateControl charStateControl;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        charStateControl = GetComponent<CharacterStateControl>();
    }

    public void KnockbackCharacter(Vector2 characterPosition, float stunTime, float knockBackTime, float knockbackForce)
    {
        if (charStateControl.IsKnockedBack()) return;

        Vector2 knockbackDirection = (transform.position - (Vector3)characterPosition).normalized;
        rb.AddForce(knockbackDirection * knockbackForce, ForceMode2D.Impulse);
        charStateControl.SetState(CharacterStateControl.CharacterState.KnockedBack);

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
        charStateControl.SetState(CharacterStateControl.CharacterState.Stunned);

        StartCoroutine(EndStun(stunTime));
    }

    private IEnumerator EndStun(float stunTime)
    {
        yield return new WaitForSeconds(stunTime);
        charStateControl.SetState(CharacterStateControl.CharacterState.Normal);
    }
}
