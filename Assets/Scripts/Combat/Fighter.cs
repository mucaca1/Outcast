using System;
using System.Collections.Generic;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using GameDevTV.Utils;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;
using Outcast.Attributes;
using Outcast.Stats;
using UnityEngine.Serialization;

namespace Outcast.Combat {
    public class Fighter : MonoBehaviour, IAction, ISaveable {
        [SerializeField] private float timeBetweenAttack = 0.7f;
        [SerializeField] private Transform rightHandTransform = null;
        [SerializeField] private Transform leftHandTransform = null;
        [FormerlySerializedAs("defaultWeapon")] [SerializeField] private WeaponConfig defaultWeaponConfig = null;
        private Health target;
        private Equipment _equipment;
        private float timeSinceLastAttack = Mathf.Infinity;
        
        private WeaponConfig _currnetWeaponConfig;
        private LazyValue<Weapon> _currentWeapon;

        private void Awake() {
            _currnetWeaponConfig = defaultWeaponConfig;
            _currentWeapon = new LazyValue<Weapon>(InitializationWeapon);
            _equipment = GetComponent<Equipment>();
            if (_equipment) {
                _equipment.equipmentUpdated += UpdateWeapon;
            }
        }
        
        private void Start() {
            _currentWeapon.ForceInit();
            AttachWeapon(_currnetWeaponConfig);
        }

        private Weapon InitializationWeapon() {
            return AttachWeapon(defaultWeaponConfig);
        }

        private void Update() {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (target.IsDead) return;
            if (!GetIsInRange(target.transform)) {
                GetComponent<Mover>().MoveTo(target.transform.position, 1f);
            }
            else {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void UpdateWeapon() {
            var weaponConfig = _equipment.GetItemInSlot(EquipLocation.Weapon) as WeaponConfig;
            if (weaponConfig == null) {
                EquipWeapon(defaultWeaponConfig);
            }
            else {
                EquipWeapon(weaponConfig);
            }
        }

        public void EquipWeapon(WeaponConfig weaponConfig) {
            if (weaponConfig == null) return;
            _currentWeapon.value = AttachWeapon(weaponConfig);
        }

        private Weapon AttachWeapon(WeaponConfig weaponConfig) {
            _currnetWeaponConfig = weaponConfig;
            return weaponConfig.SpawnWeapon(rightHandTransform, leftHandTransform, GetComponent<Animator>());
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

            if (_currentWeapon.value != null) {
                _currentWeapon.value.Hit();
            }
            
            if (_currnetWeaponConfig.HasProjectile()) {
                _currnetWeaponConfig.SpawnProjectile(rightHandTransform, leftHandTransform, target, gameObject, damage);
            }
            else {
                target.TakeDamage(damage, gameObject);
            }
        }

        void Shoot() {
            Hit();
        }

        private bool GetIsInRange(Transform targetTransform) {
            return Vector3.Distance(transform.position, targetTransform.position) < _currnetWeaponConfig.WeaponRange;
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

            if (!GetComponent<Mover>().CanMoveTo(combatTarget.transform.position) &&
                !GetIsInRange(combatTarget.transform)) {
                return false;
            }
            
            Health targetHealth = combatTarget.GetComponent<Health>();
            return targetHealth != null && !targetHealth.IsDead;
        }

        public object CaptureState() {
            return _currnetWeaponConfig.name;
        }

        public void RestoreState(object state) {
            string weaponName = (string) state;
            WeaponConfig weaponConfig = UnityEngine.Resources.Load<WeaponConfig>(weaponName);
            EquipWeapon(weaponConfig);
        }
    }
}