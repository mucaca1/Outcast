using Outcast.Core;
using UnityEngine;

namespace Outcast.Combat {
    [CreateAssetMenu(fileName = "Weapon", menuName = "Weapons/Create new weapon", order = 0)]
    public class Weapon : ScriptableObject {
        [SerializeField] private GameObject equipedPrefab = null;
        [SerializeField] private AnimatorOverrideController _animatorOveride;

        [SerializeField] private float weaponRange = 2f;
        [SerializeField] private float weaponDamage = 5f;

        [SerializeField] private bool isRightHanded = true;
        [SerializeField] private GameObject projectilePrefab = null;

        public float WeaponDamage => weaponDamage;

        public float WeaponRange => weaponRange;

        public void SpawnWeapon(Transform rightHand, Transform leftHand, Animator animator) {
            if (_animatorOveride != null)
                animator.runtimeAnimatorController = _animatorOveride;
            if (equipedPrefab != null)
                Instantiate(equipedPrefab, GetHand(rightHand, leftHand));
        }

        public void SpawnProjectile(Transform rightHand, Transform leftHand, Health target) {
            if (target == null) return;
            GameObject instantiatedProjectile =
                Instantiate(projectilePrefab, GetHand(rightHand, leftHand).position, Quaternion.identity);
            instantiatedProjectile.GetComponent<Projectile>().SetTarget(target, weaponRange);
        }

        public bool HasProjectile() {
            return projectilePrefab != null;
        }

        private Transform GetHand(Transform rightHand, Transform leftHand) {
            return isRightHanded ? rightHand : leftHand;
        }
    }
}