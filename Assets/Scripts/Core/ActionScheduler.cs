using UnityEngine;

namespace Outcast.Core {
    public class ActionScheduler : MonoBehaviour {
        private IAction _currentAction;
        
        public void StartAction(IAction action) {
            if (action == _currentAction) return;
            _currentAction?.Cancel();
            _currentAction = action;
        }

        public void CancelCurrentAction() {
            StartAction(null);
        }
    }
}