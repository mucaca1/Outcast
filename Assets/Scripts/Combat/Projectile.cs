using System;
using System.Numerics;
using Outcast.Core;
using Outcast.Attributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

namespace Outcast.Combat {
    public class Projectile : MonoBehaviour {
        [SerializeField] private float speed = 1f;
        [SerializeField] private bool _isHoming = true;
        [SerializeField] private GameObject hitEffect = null;
        [SerializeField] private float maxLiveTime = 10f;
        [SerializeField] private GameObject[] destroyOnHit = null;
        [SerializeField] private float lifeAfterImpact = 0.2f;
        [SerializeField] private UnityEvent onHit;
        
        private Health target;
        private float damage = 0f;
        private GameObject instigator;

        private void Start() {
            transform.LookAt(GetAimPosition());
        }

        private void Update() {
            if (target == null) return;
            if (_isHoming && !target.IsDead) {
                transform.LookAt(GetAimPosition());
            }
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private Vector3 GetAimPosition() {
            CapsuleCollider collider = target.gameObject.GetComponent<CapsuleCollider>();
            return target.gameObject.transform.position + Vector3.up * collider.height / 2;
        }

        public void SetTarget(Health target, GameObject instigator, float damage) {
            this.damage = damage;
            this.target = target;
            this.instigator = instigator;
            
            Destroy(gameObject, maxLiveTime);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return;
            if (hitEffect != null) {
                Instantiate(hitEffect, GetAimPosition(), transform.rotation);
            }

            speed = 0;
            onHit.Invoke();
            target.TakeDamage(damage, instigator);
            foreach (GameObject o in destroyOnHit) {
                Destroy(o);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}