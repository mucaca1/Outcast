using System;
using Outcast.Core;
using Outcast.Attributes;
using RPG.Saving;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

namespace Outcast.Movement {
    public class Mover : MonoBehaviour, IAction, ISaveable {
        [SerializeField] private float maxSpeed = 6f;
        [SerializeField] private float maxPathLength = 20f;
        private NavMeshAgent navMeshAgent;
        private Health _health;

        private void Awake() {
            navMeshAgent = GetComponent<NavMeshAgent>();
            _health = GetComponent<Health>();
        }

        void Update() {
            navMeshAgent.enabled = !_health.IsDead;
            UpdateAnimator();
        }

        private void UpdateAnimator() {
            Vector3 velocity = navMeshAgent.velocity;
            Vector3 localVelocity = transform.InverseTransformDirection(velocity);
            float speed = localVelocity.z;
            GetComponent<Animator>().SetFloat("fowardSpeed", speed);
        }

        public bool CanMoveTo(Vector3 destination) {
            NavMeshPath navMeshPath = new NavMeshPath();
            bool canCalculatePath = NavMesh.CalculatePath(transform.position, destination, NavMesh.AllAreas, navMeshPath);
            if (!canCalculatePath) return false;
            if (CalculateLengthPath(navMeshPath) > maxPathLength) return false;
            return true;
        }

        public void StartMoveAction(Vector3 destination, float speedFraction) {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination, speedFraction);
        }

        public void MoveTo(Vector3 destination, float speedFraction) {
            navMeshAgent.destination = destination;
            navMeshAgent.speed = maxSpeed * Mathf.Clamp01(speedFraction);
            navMeshAgent.isStopped = false;
        }

        public void Cancel() {
            navMeshAgent.isStopped = true;
        }

        public object CaptureState() {
            return new SerializableVector3(transform.position);
        }

        public void RestoreState(object state) {
            SerializableVector3 position = (SerializableVector3)state;
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.enabled = false;
            transform.position = position.ToVector();
            agent.enabled = true;
        }
        
        private float CalculateLengthPath(NavMeshPath navMeshPath) {
            float totalLength = 0f;
            for (int i = 0; i < navMeshPath.corners.Length - 1; i++) {
                totalLength += Vector3.Distance(navMeshPath.corners[i], navMeshPath.corners[i + 1]);
            }

            return totalLength;
        }
    }
}