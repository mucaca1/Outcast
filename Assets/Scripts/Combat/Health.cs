using UnityEngine;

namespace Outcast.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float health = 100;

        private bool _isDead = false;

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
        }
    }
}