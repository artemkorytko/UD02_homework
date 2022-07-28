using UnityEngine;

namespace Viking_scripts
{
    public class StateMachine : MonoBehaviour
    {
        private State _currentState;

        public void Initialize(State startState)
        {
            _currentState = startState;
            _currentState.Enter();
        }

        public void ChangeState(State newState)
        {
            _currentState.Exit();
            _currentState = newState;
            _currentState.Enter();
        }
    }
}