using System;
using System.Collections.Generic;
using GameDevTV.Utils;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;
using Outcast.Attributes;
using Outcast.Stats;
using RPG.Saving;
using UnityEngine.Serialization;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable, IModifierProvider {
        [SerializeField] private float timeBetweenAttack = 0.7f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [FormerlySerializedAs("defaultWeapon")] [SerializeField] private WeaponConfig defaultWeaponConfig = null;
        private Health target;

        private float timeSinceLastAttack = Mathf.Infinity;
        
        private LazyValue<WeaponConfig> _currnetWeapon;

        private void Awake() {
            _currnetWeapon = new LazyValue<WeaponConfig>(InitializationWeapon);
        }
        
        private void Start() {
            _currnetWeapon.ForceInit();
        }

        private WeaponConfig InitializationWeapon() {
            AttachWeapon(defaultWeaponConfig);
            return defaultWeaponConfig;
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

        public void EquipWeapon(WeaponConfig weaponConfig) {
            if (weaponConfig == null) return;
            AttachWeapon(weaponConfig);
        }

        private void AttachWeapon(WeaponConfig weaponConfig) {
            _currnetWeapon.value = weaponConfig;
            weaponConfig.SpawnWeapon(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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

        public Health GetTarget() {
            return target;
        }

        // Hit event called from animation
        void Hit() {
            if (target == null) return;
            float damage = GetComponent<BaseStats>().GetStat(Stat.Damage);
            if (_currnetWeapon.value.HasProjectile()) {
                _currnetWeapon.value.SpawnProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else {
                target.TakeDamage(damage, gameObject);
            }
        }

        void Shoot() {
            Hit();
        }

        private bool GetIsInRange() {
            return Vector3.Distance(transform.position, target.transform.position) < _currnetWeapon.value.WeaponRange;
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

        public object CaptureState() {
            return _currnetWeapon.value.name;
        }

        public void RestoreState(object state) {
            string weaponName = (string) state;
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return _currnetWeapon.value.WeaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage) {
                yield return _currnetWeapon.value.PercentageModifier;
            }
        }
    }
}