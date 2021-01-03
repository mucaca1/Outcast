using System.Collections.Generic;
using System.ComponentModel;
using GameDevTV.Inventories;
using Outcast.Core;
using Outcast.Attributes;
using Outcast.Stats;
using UnityEngine;

namespace Outcast.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create new weapon", order = 0)]
    public class WeaponConfig : EquipableItem, IModifierProvider {
        [SerializeField] private Weapon equipedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOveride;
        [SerializeField] float percentageBonus = 0;
        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageModifier = 0f;

        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private GameObject projectilePrefab = null;

        private static string _weaponName = "Weapon";

        public float WeaponDamage => weaponDamage;

        public float WeaponRange => weaponRange;

        public float PercentageModifier => percentageModifier;

        public Weapon SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon(rightHand, leftHand);
            var overrideAnimator = animator.runtimeAnimatorController as AnimatorOverrideController;
            Weapon weapon = null;
            if (_animatorOveride != null) {
                animator.runtimeAnimatorController = _animatorOveride;
            }
            else if (overrideAnimator != null) {
                animator.runtimeAnimatorController = overrideAnimator.runtimeAnimatorController;
            }

            if (equipedPrefab != null) {
                weapon = Instantiate(equipedPrefab, GetHand(rightHand, leftHand));
                weapon.gameObject.name = _weaponName;
            }

            return weapon;
        }

        private void DestroyOldWeapon(Transform rightHand, Transform leftHand) {
            Transform oldWeapon = rightHand.Find(_weaponName);
            if (oldWeapon == null) {
                oldWeapon = leftHand.Find(_weaponName);
                if (oldWeapon == null) return;
            }
            oldWeapon.name = "DESTROYED";
            Destroy(oldWeapon.gameObject);
        }

        public void SpawnProjectile(Transform rightHand, Transform leftHand, Health target, GameObject instigator, float calculatedDamage) {
            if (target == null) return;
            GameObject instantiatedProjectile =
                Instantiate(projectilePrefab, GetHand(rightHand, leftHand).position, Quaternion.identity);
            instantiatedProjectile.GetComponent<Projectile>().SetTarget(target, instigator, calculatedDamage);
        }

        public bool HasProjectile() {
            return projectilePrefab != null;
        }

        private Transform GetHand(Transform rightHand, Transform leftHand) {
            return isRightHanded ? rightHand : leftHand;
        }

        public IEnumerable<float> GetAdditiveModifiers(Stat stat) {
            if (stat == Stat.Damage)
            {
                yield return weaponDamage;
            }
        }

        public IEnumerable<float> GetPercentageModifiers(Stat stat) {
            if (stat == Stat.Damage)
            {
                yield return percentageBonus;
            }
        }
    }
}