using System;
using System.Collections;
using Outcast.Control;
using UnityEngine;

namespace Outcast.Combat {
    public class WeaponPickUp : MonoBehaviour, IRaycastable {
        [SerializeField] private Weapon _weapon = null;
        [SerializeField] private float hidenTime = 5f;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                PickUp(other.GetComponent<Fighter>());
            }
        }

        private void PickUp(Fighter other) {
            other.EquipWeapon(_weapon);
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
                PickUp(controller.GetComponent<Fighter>());
            }

            return true;
        }
    }
}