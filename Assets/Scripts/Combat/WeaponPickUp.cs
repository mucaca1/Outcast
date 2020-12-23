using System;
using System.Collections;
using UnityEngine;

namespace Outcast.Combat {
    public class WeaponPickUp : MonoBehaviour {
        [SerializeField] private Weapon _weapon = null;
        [SerializeField] private float hidenTime = 5f;

        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                StartCoroutine(HideForSeconds(hidenTime));
            }
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
    }
}