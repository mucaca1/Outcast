using System;
using System.Numerics;
using Outcast.Core;
using UnityEngine;
using UnityEngine.UI;
using Vector3 = UnityEngine.Vector3;

namespace Outcast.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 1f;

        private Health target;
        private float demage = 0f;

        private void Update() {
            if (target == null) return;
            transform.LookAt(GetAimPosition());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimPosition() {
            CapsuleCollider collider = target.gameObject.GetComponent<CapsuleCollider>();
            return target.gameObject.transform.position + Vector3.up * collider.height / 2;
        }

        public void SetTarget(Health target, float demage) {
            this.demage = demage;
            this.target = target;
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            
            target.TakeDamage(demage);
            Destroy(gameObject);
        }
    }
}