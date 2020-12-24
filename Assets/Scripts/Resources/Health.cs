using System;
using Outcast.Core;
using Outcast.Stats;
using RPG.Saving;
using UnityEngine;

namespace Outcast.Resources {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float health = 100;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Start() {
            health = GetComponent<BaseStats>().GetHealth();
        }

        public void TakeDamage(float demage) {
            health = Mathf.Max(health - demage, 0);
            print(health);
            if (health == 0) {
                Die();
            }
        }

        private void Die() {
            if (_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() {
            return health;
        }

        public float GetPercentage() {
            return 100 * (health / GetComponent<BaseStats>().GetHealth());
        }

        public void RestoreState(object state) {
            health = (float) state;
            
            if (health == 0) {
                Die();
            }
        }
    }
}