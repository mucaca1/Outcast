﻿using System;
using System.Numerics;
using Outcast.Core;
using UnityEngine;
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
        
        private Health target;
        private float demage = 0f;

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

        public void SetTarget(Health target, float demage) {
            this.demage = demage;
            this.target = target;
            
            Destroy(gameObject, maxLiveTime);
        }

        private void OnTriggerEnter(Collider other) {
            if (other.GetComponent<Health>() != target) return;
            if (target.IsDead) return;
            if (hitEffect != null) {
                Instantiate(hitEffect, GetAimPosition(), transform.rotation);
            }

            speed = 0;
            
            target.TakeDamage(demage);
            foreach (GameObject o in destroyOnHit) {
                Destroy(o);
            }
            Destroy(gameObject, lifeAfterImpact);
        }
    }
}