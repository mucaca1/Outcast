using System;
using Outcast.Combat;
using Outcast.Core;
using Outcast.Movement;
using UnityEngine;
using UnityEngine.AI;

namespace Outcast.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waipontDwellTime = 2f;

        private GameObject _player;
        private Fighter _fighter;
        private Mover _mover;
        private Health _health;

        private Vector3 _guardPosition;
        private float _lastSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;

        private int _currentWaypointIndex = 0;

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
                AttackBehaviour();
            }
            else if (_lastSinceLastSawPlayer < suspicionTime) {
                SuspicionBehaviour();
            }
            else {
                PatrolBehaviour();
            }

            UpdateTimers();
        }

        private void UpdateTimers() {
            _lastSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = _guardPosition;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    _timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }

                if (_timeSinceArrivedAtWaypoint > waipontDwellTime) {
                    nextPosition = GetCurrentWaypoint();
                }
            }

            _mover.StartMoveAction(nextPosition);
        }

        private Vector3 GetCurrentWaypoint() {
            return patrolPath.GetWaypointPosition(_currentWaypointIndex);
        }

        private void CycleWaypoint() {
            _currentWaypointIndex = patrolPath.GetNextWayoint(_currentWaypointIndex);
        }

        private bool AtWaypoint() {
            float distance = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distance < waypointTolerance;
        }

        private void SuspicionBehaviour() {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackBehaviour() {
            _lastSinceLastSawPlayer = 0f;
            _fighter.Attack(_player);
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