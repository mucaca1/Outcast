using System;
using UnityEngine;
using Outcast.Movement;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour {
        [SerializeField] private float weaponRange = 2f;
        private Transform target;

        private void Update() {
            if (target == null) return;
            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else {
                GetComponent<Mover>().Stop();
            }
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}