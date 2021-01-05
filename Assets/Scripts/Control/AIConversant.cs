using Dialogue;
using Outcast.Control;
using UnityEngine;

namespace Control {
    public class AIConversant : MonoBehaviour, IRaycastable {

        [SerializeField] private Dialogue.Dialogue _dialogue = null;
        public CursorType GetCursorType() {
            return CursorType.Dialogue;
        }

        public bool HandleRaycast(PlayerController controller) {
            if (_dialogue == null) return false;
            if (Input.GetMouseButtonDown(0)) {
                controller.GetComponent<PlayerConversant>().StartDialogue(_dialogue);
            }
            return true;
        }
    }
}