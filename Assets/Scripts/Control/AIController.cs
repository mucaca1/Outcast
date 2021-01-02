using System;
using GameDevTV.Utils;
using Outcast.Combat;
using Outcast.Core;
using Outcast.Movement;
using Outcast.Attributes;
using UnityEngine;
using UnityEngine.AI;

namespace Outcast.Control {
    public class AIController : MonoBehaviour {
        [SerializeField] private float chaseDistance = 5f;
        [SerializeField] private float suspicionTime = 3f;
        [SerializeField] private PatrolPath patrolPath;
        [SerializeField] private float waypointTolerance = 1f;
        [SerializeField] private float waipontDwellTime = 2f;
        [Range(0, 1)] [SerializeField] private float patrolSpeedFraction = 0.2f;
        [SerializeField] private float aggrevateTime = 3f;
        [SerializeField] private float soutDistance = 4f;

        private GameObject _player;
        private Fighter _fighter;
        private Mover _mover;
        private Health _health;

        private LazyValue<Vector3> _guardPosition;
        private float _lastSinceLastSawPlayer = Mathf.Infinity;
        private float _timeSinceArrivedAtWaypoint = Mathf.Infinity;
        private float _timeSinceAggrevateTime = Mathf.Infinity;

        private int _currentWaypointIndex = 0;

        private void Awake() {
            _player = GameObject.FindWithTag("Player");
            _fighter = GetComponent<Fighter>();
            _health = GetComponent<Health>();
            _mover = GetComponent<Mover>();
            
            _guardPosition = new LazyValue<Vector3>(InitializeGuardPosition);
        }

        private void Start() {
            _guardPosition.ForceInit();
        }

        private Vector3 InitializeGuardPosition() {
            return transform.position;
        }

        private void Update() {
            if (_health.IsDead) return;

            if (IsAggrevated() && _fighter.CanAttack(_player)) {
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

        public void Aggrevate() {
            _timeSinceAggrevateTime = 0f;
        }

        private void UpdateTimers() {
            _lastSinceLastSawPlayer += Time.deltaTime;
            _timeSinceArrivedAtWaypoint += Time.deltaTime;
            _timeSinceAggrevateTime += Time.deltaTime;
        }

        private void PatrolBehaviour() {
            Vector3 nextPosition = _guardPosition.value;
            if (patrolPath != null) {
                if (AtWaypoint()) {
                    _timeSinceArrivedAtWaypoint = 0f;
                    CycleWaypoint();
                }

                nextPosition = GetCurrentWaypoint();
            }

            if (_timeSinceArrivedAtWaypoint > waipontDwellTime) {
                _mover.StartMoveAction(nextPosition, patrolSpeedFraction);
            }
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

            AggrevateNearbyEnemies();
        }

        private void AggrevateNearbyEnemies() {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, soutDistance, Vector3.up, 0);

            foreach (RaycastHit hit in hits) {
                AIController controller = hit.collider.GetComponent<AIController>();
                if (controller == null) continue;
                controller.Aggrevate();
            }
        }

        private bool IsAggrevated() {
            float distanceToPlayer = Vector3.Distance(_player.transform.position, transform.position);
            return (distanceToPlayer < chaseDistance || _timeSinceAggrevateTime < aggrevateTime);
        }

        private void OnDrawGizmosSelected() {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}