using System;
using Outcast.Combat;
using Outcast.Core;
using Outcast.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Outcast.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;

        private GameObject _player;
        private Fighter _fighter;
        private Health _health;

        private void Start() {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
        }

        private void Update() {
            
            if (_health.IsDead) return;
            
            if (InAttackRangeOfThePlayer() && _fighter.CanAttack(_player)) {
                _fighter.Attack(_player);
            }
            else {
                _fighter.Cancel();
            }
        }

        private bool InAttackRangeOfThePlayer() {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }
    }
}