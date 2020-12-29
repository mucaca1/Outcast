using System;
using Outcast.Combat;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;
using Outcast.Resources;

namespace Outcast.Control {
    public class PlayerController : MonoBehaviour {

        private Health _health;

        enum CursorType {
            None,
            Move,
            Combat
        }

        [System.Serializable]
        struct CursorMapping {
            public CursorType type;

            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] _cursorMappings = null;

        private void Awake() {
            _health = GetComponent<Health>();
        }

        private void Update() {
            if (_health.IsDead) return;
            
            if (InteractWithCombat()) return;
            if (InteractWithInput()) return;
            
            SetCursor(CursorType.None);
        }

        private bool InteractWithCombat() {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in raycastHits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (target == null) continue;
                if (!GetComponent<Fighter>().CanAttack(target.gameObject)) continue;

                if (Input.GetMouseButton(0)) {
                    GetComponent<Fighter>().Attack(target.gameObject);
                }

                SetCursor(CursorType.Combat);
                return true;
            }

            return false;
        }

        private bool InteractWithInput() {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit) {
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(hit.point, 1f);
                }

                SetCursor(CursorType.Move);
                return true;
            }

            return false;
        }

        private void SetCursor(CursorType type) {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {

            foreach (CursorMapping mapping in _cursorMappings) {
                if (mapping.type == type) {
                    return mapping;
                }
            }

            return _cursorMappings[0];
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}