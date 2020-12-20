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
        private Mover _mover;
        private Health _health;

        private Vector3 _guardPosition;

        private void Start() {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            _guardPosition = transform.position;
        }

        private void Update() {
            
            if (_health.IsDead) return;
            
            if (InAttackRangeOfThePlayer() && _fighter.CanAttack(_player)) {
                _fighter.Attack(_player);
            }
            else {
                _mover.StartMoveAction(_guardPosition);
            }
        }

        private bool InAttackRangeOfThePlayer() {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return distanceToPlayer < chaseDistance;
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}