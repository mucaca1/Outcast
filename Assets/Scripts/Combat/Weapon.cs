using System.ComponentModel;
using Outcast.Core;
using Outcast.Resources;
using UnityEngine;

namespace Outcast.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create new weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] private GameObject equipedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOveride;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;
        [SerializeField] private float percentageModifier = 0f;

        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private GameObject projectilePrefab = null;

        private static string _weaponName = "Weapon";

        public float WeaponDamage => weaponDamage;

        public float WeaponRange => weaponRange;

        public float PercentageModifier => percentageModifier;

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator) {
            DestroyOldWeapon(rightHand, leftHand);
            var overrideAnimator = animator.runtimeAnimatorController as AnimatorOverrideController;
            if (_animatorOveride != null) {
                animator.runtimeAnimatorController = _animatorOveride;
            }
            else if (overrideAnimator != null) {
                animator.runtimeAnimatorController = overrideAnimator.runtimeAnimatorController;
            }

            if (equipedPrefab != null) {
                GameObject weapon = Instantiate(equipedPrefab, GetHand(rightHand, leftHand));
                weapon.name = _weaponName;
            }
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
    }
}