using System;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float timeBetweenAttack = 0.7f;
        private Transform target;

        private float timeSinceLastAttack = 0f;

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (timeSinceLastAttack < 0) timeSinceLastAttack = 0f;
            if (target == null) return;
            if (!GetIsInRange()) {
                timeSinceLastAttack = 0f;
                GetComponent<Mover>().MoveTo(target.position);
            }
            else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour() {
            if (timeBetweenAttack < timeSinceLastAttack) {
                // This call hit event
                GetComponent<Animator>().SetTrigger("attack");
                timeSinceLastAttack = 0f;
            }
        }
        
        // Hit event called from animation
        void Hit() {
            target.GetComponent<Health>().TakeDamage(weaponDamage);
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.position) < weaponRange;
        }

        public void Attack(CombatTarget combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.transform;
        }

        public void Cancel() {
            target = null;
        }
    }
}