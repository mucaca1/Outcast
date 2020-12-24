using System;
using Outcast.Combat;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;
using Outcast.Resources;

namespace Outcast.Control {
    public class PlayerController : MonoBehaviour {

        private Health _health;

        private void Start() {
            _health = GetComponent<Health>();
        }

        private void Update() {
            if (_health.IsDead) return;
            
            if (InteractWithCombat()) return;
            if (InteractWithInput()) return;
            print("Nothing to do...");
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

                return true;
            }

            return false;
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}