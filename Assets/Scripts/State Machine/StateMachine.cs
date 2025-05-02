using System.Collections;
using UnityEngine;

public abstract class StateMachine : MonoBehaviour
{
    private State currentState;

    private void Update()
    {
        currentState?.Tick(Time.deltaTime);
    }
    public void SwitchStateTo(State state)
    {
        currentState?.Exit();
        currentState = state;
        currentState?.Enter();
    }
}
