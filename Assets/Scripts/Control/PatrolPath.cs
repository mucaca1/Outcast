using System;
using UnityEngine;

namespace Outcast.Control {
    public class PatrolPath : MonoBehaviour {
        private float _waypointGizmoRadius = 0.3f;

        private void OnDrawGizmos() {
            for (int i = 0; i < transform.childCount; i++) {
                Gizmos.DrawSphere(transform.GetChild(i).transform.position, _waypointGizmoRadius);
                Gizmos.DrawLine(GetWaypointPosition(i), GetWaypointPosition(GetNextWayoint(i)));
            }
        }

        public int GetNextWayoint(int i) {
            return i + 1 < transform.childCount ? i + 1 : 0;
        }

        public Vector3 GetWaypointPosition(int i) {
            return transform.GetChild(i).position;
        }
    }
}