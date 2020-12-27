using System;
using UnityEngine;

namespace Outcast.Core {
    public class DestroyAfterEffect : MonoBehaviour {
        [SerializeField] private GameObject objectToDestroy = null;
        private void Update() {
            if (!GetComponent<ParticleSystem>().IsAlive()) {
                if (objectToDestroy != null) {
                    Destroy(objectToDestroy);
                }
                else {
                    Destroy(gameObject);
                }
            }
        }
    }
}