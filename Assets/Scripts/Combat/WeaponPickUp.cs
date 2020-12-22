using System;
using UnityEngine;

namespace Outcast.Combat {
    public class WeaponPickUp : MonoBehaviour {
        [SerializeField] private Weapon _weapon = null; 
        private void OnTriggerEnter(Collider other) {
            if (other.gameObject.tag == "Player") {
                other.GetComponent<Fighter>().EquipWeapon(_weapon);
                Destroy(gameObject);
            }
        }
    }
}