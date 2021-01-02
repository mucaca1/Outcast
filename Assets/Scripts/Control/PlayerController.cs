using System;
using System.Collections;
using Outcast.Combat;
using Outcast.Core;
using UnityEngine;
using Outcast.Movement;
using Outcast.Attributes;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace Outcast.Control {
    public class PlayerController : MonoBehaviour {
        private Health _health;

        [System.Serializable]
        struct CursorMapping {
            public CursorType type;

            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] _cursorMappings = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;

        private void Awake() {
            _health = GetComponent<Health>();
        }

        private void Update() {
            if (InteractWithUI()) return;

            if (_health.IsDead) {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithRaycastable()) return;
            if (InteractWithInput()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithRaycastable() {
            RaycastHit[] raycastHits = GetSortedRaycastHit();
            foreach (RaycastHit hit in raycastHits) {
                IRaycastable[] raycastables = hit.transform.GetComponents<IRaycastable>();
                foreach (IRaycastable raycastable in raycastables) {
                    if (raycastable.HandleRaycast(this)) {
                        SetCursor(raycastable.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] GetSortedRaycastHit() {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++) {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);
            return hits;
        }

        private bool InteractWithUI() {
            if (EventSystem.current.IsPointerOverGameObject()) {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        private bool InteractWithInput() {
            Vector3 target;
            bool hasHit = RaycastHitWalkable(out target);
            if (hasHit) {

                if (!GetComponent<Mover>().CanMoveTo(target)) {
                    return false;
                }
                
                if (Input.GetMouseButton(0)) {
                    GetComponent<Mover>().StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Move);
                return true;
            }

            return false;
        }

        private bool RaycastHitWalkable(out Vector3 target) {
            target = new Vector3();
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;
            NavMeshHit navHit;
            bool hasNavMeshHit =
                NavMesh.SamplePosition(hit.point, out navHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasNavMeshHit) return false;
            target = navHit.position;

            return true;
        }

        private void SetCursor(CursorType type) {
            CursorMapping mapping = GetCursorMapping(type);
            Cursor.SetCursor(mapping.texture, mapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType type) {
            foreach (CursorMapping mapping in _cursorMappings) {
                if (mapping.type == type) {
                    return mapping;
                }
            }

            return _cursorMappings[0];
        }

        private static Ray GetMouseRay() {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}