using UnityEngine;

public class FSMBehaviour : MonoBehaviour
{
    private State _currentState;
    
    public void Update()
    {
        if (_currentState != null) _currentState.Update();
    }
    
    public void CallMethodOnState(string methodName)
    {
        if (_currentState != null) _currentState.CallMethod(methodName);
    }

    public void ChangeStateTo(State state)
    {
        if (_currentState != null) _currentState.Exit();
        _currentState = state;
        _currentState.Enter();
    }
}