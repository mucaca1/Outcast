using System;
using Outcast.Combat;
using UnityEngine;
using Outcast.Movement;

namespace Outcast.Control {
    public class PlayerController : MonoBehaviour {
        private void Update() {
            if (InteractWithCombat()) return;
            if (InteractWithInput()) return;
            print("Nothing to do...");
        }

        private bool InteractWithCombat() {
            RaycastHit[] raycastHits = Physics.RaycastAll(GetMouseRay());
            foreach (RaycastHit hit in raycastHits) {
                CombatTarget target = hit.transform.GetComponent<CombatTarget>();
                if (!GetComponent<Fighter>().CanAttack(target)) continue;

                if (Input.GetMouseButtonDown(0)) {
                    GetComponent<Fighter>().Attack(target);
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
                    GetComponent<Mover>().StartMoveAction(hit.point);
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