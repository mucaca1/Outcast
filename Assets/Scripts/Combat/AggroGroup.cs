using System;
using UnityEngine;

namespace Outcast.Combat {
    public class AggroGroup : MonoBehaviour {

        [SerializeField] private Fighter[] _fighters;
        [SerializeField] private bool _activateOnStart = false;

        private void Start() {
            Active(_activateOnStart);
        }

        public void Active(bool shouldActive) {
            foreach (Fighter enemy in _fighters) {
                CombatTarget target = enemy.GetComponent<CombatTarget>();
                if (target != null) {
                    target.enabled = shouldActive;
                }
                enemy.enabled = shouldActive;
            }
        }
    }
}