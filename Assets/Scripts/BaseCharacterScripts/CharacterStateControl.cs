using UnityEngine;

public class CharacterStateControl : MonoBehaviour
{
    public enum CharacterState
    {
        Normal,
        Stunned,
        KnockedBack
    }

    private CharacterState currentState = CharacterState.Normal;

    public CharacterState GetCurrentState()
    {
        return currentState;
    }

    public void SetState(CharacterState newState)
    {
        currentState = newState;
    }

    public bool CanMove()
    {
        return currentState == CharacterState.Normal;
    }

    public bool IsStunned()
    {
        return currentState == CharacterState.Stunned;
    }

    public bool IsKnockedBack()
    {
        return currentState == CharacterState.KnockedBack;
    }
}
