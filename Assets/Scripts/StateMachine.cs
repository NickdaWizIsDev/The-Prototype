using UnityEngine;

public class StateMachine : MonoBehaviour
{
    public State state;

    public void Set(State newState, bool forceReset = false)
    {
        if(state != newState || forceReset)
        {
            state?.Exit();
            state = newState;
            state.Initialise(this);
            state.Enter();
        }
    }
}