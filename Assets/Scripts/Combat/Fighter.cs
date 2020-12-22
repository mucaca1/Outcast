using System;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour, IAction {
        [SerializeField] private float timeBetweenAttack = 0.7f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [SerializeField] private Weapon defaultWeapon = null;
        private Health target;

        private float timeSinceLastAttack = Mathf.Infinity;
        private Animator _animator;
        private Weapon _currnetWeapon = null;

        private void Start() {
            _animator = GetComponent<Animator>();
            EquipWeapon(defaultWeapon);
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

        public void EquipWeapon(Weapon weapon) {
            if (weapon == null) return;
            _currnetWeapon = weapon;
            weapon.SpawnWeapon(rightHandTransform, leftHandTransform, _animator);
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
            if (_currnetWeapon.HasProjectile()) {
                _currnetWeapon.SpawnProjectile(rightHandTransform, leftHandTransform, target);
            }
            else {
                target.TakeDamage(_currnetWeapon.WeaponDamage);
            }
        }

        void Shoot() {
            Hit();
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < _currnetWeapon.WeaponRange;
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