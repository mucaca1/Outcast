using System;
using System.Collections;
using Outcast.Attributes;
using Outcast.Control;
using UnityEngine;
using UnityEngine.Serialization;

namespace Outcast.Combat {
    public class WeaponPickUp : MonoBehaviour, IRaycastable {
        [FormerlySerializedAs("_weapon")] [SerializeField] private WeaponConfig weaponConfig = null;
        [SerializeField] private float hidenTime = 5f;
        [SerializeField] private float heal = 0;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                PickUp(other.gameObject);
            }
        }

        private void PickUp(GameObject subject) {
            if (weaponConfig != null) {
                subject.GetComponent<Fighter>().EquipWeapon(weaponConfig);
            }

            if (heal > 0) {
                subject.GetComponent<Health>().Heal(heal);
            }
            StartCoroutine(HideForSeconds(hidenTime));
        }

        private IEnumerator HideForSeconds(float time) {
            ShowPickup(false);
            yield return new WaitForSeconds(time);
            ShowPickup(true);
        }

        private void ShowPickup(bool show) {
            GetComponent<SphereCollider>().enabled = show;
            foreach (Transform child in transform) {
                child.gameObject.SetActive(show);
            }
        }

        public CursorType GetCursorType() {
            return CursorType.Pickup;
        }

        public bool HandleRaycast(PlayerController controller) {
            if (Input.GetMouseButtonDown(0)) {
                PickUp(controller.gameObject);
            }

            return true;
        }
    }
}