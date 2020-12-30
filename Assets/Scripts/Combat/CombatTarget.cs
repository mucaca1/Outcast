using Outcast.Control;
using Outcast.Core;
using Outcast.Resources;
using UnityEngine;

namespace Outcast.Combat {
    [RequireComponent(typeof(Health))]
    public class CombatTarget : MonoBehaviour, IRaycastable {
        public CursorType GetCursorType() {
            return CursorType.Combat;
        }

        public bool HandleRaycast(PlayerController controller) {
            if (!controller.GetComponent<Fighter>().CanAttack(gameObject)) return false;

            if (Input.GetMouseButton(0)) {
                controller.GetComponent<Fighter>().Attack(gameObject);
            }

            return true;
        }
    }
}