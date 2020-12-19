using UnityEngine;

namespace Outcast.Combat {
    public class Health : MonoBehaviour {
        [SerializeField] private float health;

        public void TakeDamage(float demage) {
            health = Mathf.Max(health - demage, 0);
            print(health);
        }
    }
}