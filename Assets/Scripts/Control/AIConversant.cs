using Dialogue;
using Outcast.Control;
using UnityEngine;

namespace Control {
    public class AIConversant : MonoBehaviour, IRaycastable {
        [SerializeField] private string characterName;

        [SerializeField] private Dialogue.Dialogue _dialogue = null;
        public CursorType GetCursorType() {
            return CursorType.Dialogue;
        }

        public string GetName() {
            return characterName;
        }

        public bool HandleRaycast(PlayerController controller) {
            if (_dialogue == null) return false;
            if (Input.GetMouseButtonDown(0)) {
                controller.GetComponent<PlayerConversant>().StartDialogue(this, _dialogue);
            }
            return true;
        }
    }
}