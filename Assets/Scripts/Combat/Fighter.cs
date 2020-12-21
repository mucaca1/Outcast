using System;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float timeBetweenAttack = 0.7f;

        [SerializeField] private GameObject weaponPrefab = null;
        [SerializeField] private Transform handTransform = null;
        
        private Health target;

        private float timeSinceLastAttack = Mathf.Infinity;

        private void Start() {
            SpawnWeapon();
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead) return;
            if (!GetIsInRange()) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        void SpawnWeapon() {
            if (weaponPrefab == null || handTransform == null) return;

            Instantiate(weaponPrefab, handTransform);
        }

        private void AttackBehaviour() {
            transform.LookAt(target.transform);
            if (timeBetweenAttack < timeSinceLastAttack) {
                // This call hit event
                TriggerAttack();
                timeSinceLastAttack = 0f;
            }
        }

        private void TriggerAttack() {
            GetComponent<Animator>().ResetTrigger("stopAttack");
            GetComponent<Animator>().SetTrigger("attack");
        }

        // Hit event called from animation
        void Hit() {
            if (target == null) return;
            target.TakeDamage(weaponDamage);
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < weaponRange;
        }

        public void Attack(GameObject combatTarget) {
            GetComponent<ActionScheduler>().StartAction(this);
            target = combatTarget.GetComponent<Health>();
        }

        public void Cancel() {
            StopAttack();
            GetComponent<Mover>().Cancel();
            target = null;
        }

        private void StopAttack() {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAttack");
        }

        public bool CanAttack(GameObject combatTarget) {
            if (combatTarget == null) return false;
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead;
        }
    }
}