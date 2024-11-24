using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    public enum BaseState
    {
        Patrol,
        Pursue,
        SlowDown,
        Attack,
        Damaged,
        Wait
    }

    private BaseState currentState = BaseState.Patrol;

    private void Update()
    {
        Debug.Log(currentState.ToString());
    }

    public BaseState GetEnemyBaseState()
    {
        return currentState;
    }

    public void SetEnemyState(BaseState newState)
    {
        currentState = newState;
    }
}
