using UnityEngine;
using UnityEngine.Events;

namespace Outcast.Combat {
    public class Weapon : MonoBehaviour {
        [SerializeField] private UnityEvent onHit;
        public void Hit() {
            onHit.Invoke();
        }
    }
}