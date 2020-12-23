using System;
using UnityEngine;

namespace Outcast.Core {
    public class DestroyAfterEffect : MonoBehaviour {
        private void Update() {
            if (!GetComponent<ParticleSystem>().IsAlive()) {
                Destroy(gameObject);
            }
        }
    }
}