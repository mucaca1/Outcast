using System;
using Outcast.Core;
using Outcast.Stats;
using RPG.Saving;
using UnityEngine;

namespace Outcast.Resources {
    public class Health : MonoBehaviour, ISaveable {

        [SerializeField] private float regeneratePercentage = 70f;

        private float _health = -1f;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Start() {
            if (_health < 0) {
                _health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }

            BaseStats stats = GetComponent<BaseStats>();
            if (stats == null) return;

            stats.onLevelUp += RegenerateHealth;
        }

        private void RegenerateHealth() {
            float regeneratedPercentageHealt = GetComponent<BaseStats>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            _health = Mathf.Max(_health, regeneratedPercentageHealt);
        }

        public void TakeDamage(float demage, GameObject instigator) {
            _health = Mathf.Max(_health - demage, 0);
            print(_health);
            if (_health == 0) {
                Die();
                AwardExperience(instigator);
            }
        }

        private void AwardExperience(GameObject instigator) {
            Experience experience = instigator.GetComponent<Experience>();
            if (experience == null) return;
            experience.GainExperience(GetComponent<BaseStats>().GetStat(Stat.ExperienceReward));
        }

        private void Die() {
            if (_isDead) return;
            _isDead = true;
            GetComponent<Animator>().SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public object CaptureState() {
            return _health;
        }

        public float GetPercentage() {
            return 100 * (_health / GetComponent<BaseStats>().GetStat(Stat.Health));
        }

        public float GetHealthPoints() {
            return _health;
        }

        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void RestoreState(object state) {
            _health = (float) state;

            if (_health == 0) {
                Die();
            }
        }
    }
}