using System;
using Outcast.Core;
using Outcast.Stats;
using RPG.Saving;
using UnityEngine;

namespace Outcast.Resources {
    public class Health : MonoBehaviour, ISaveable {
        private float _health = -1f;

        private bool _isDead = false;

        public bool IsDead => _isDead;

        private void Start() {
            if (_health < 0) {
                _health = GetComponent<BaseStats>().GetStat(Stat.Health);
            }
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

        public void RestoreState(object state) {
            _health = (float) state;

            if (_health == 0) {
                Die();
            }
        }
    }
}