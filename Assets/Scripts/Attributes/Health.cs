using System;
using GameDevTV.Saving;
using GameDevTV.Utils;
using Outcast.Core;
using Outcast.Stats;
using UnityEngine;
using UnityEngine.Events;

namespace Outcast.Attributes {
    public class Health : MonoBehaviour, ISaveable {
        [SerializeField] private float regeneratePercentage = 70f;

        [SerializeField] private DamageEvent takeDamage;
        [SerializeField] private UnityEvent onDie;

        private bool _isDead = false;

        [System.Serializable]
        class DamageEvent : UnityEvent<float> {
            
        }

        private LazyValue<float> _health;

        public bool IsDead => _isDead;

        private void Awake() {
            _health = new LazyValue<float>(InitializationHealth);
        }

        private float InitializationHealth() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        private void Start() {
            _health.ForceInit();
        }


        private void OnEnable() {
            BaseStats stats = GetComponent<BaseStats>();
            if (stats == null) return;

            stats.onLevelUp += RegenerateHealth;
        }

        private void OnDisable() {
            BaseStats stats = GetComponent<BaseStats>();
            if (stats == null) return;

            stats.onLevelUp -= RegenerateHealth;
        }

        private void RegenerateHealth() {
            float regeneratedPercentageHealt =
                GetComponent<BaseStats>().GetStat(Stat.Health) * (regeneratePercentage / 100);
            _health.value = Mathf.Max(_health.value, regeneratedPercentageHealt);
        }

        public void TakeDamage(float damage, GameObject instigator) {
            _health.value = Mathf.Max(_health.value - damage, 0);
            print(instigator.name + " deal demage: " + damage);

            if (_health.value == 0) {
                onDie.Invoke();
                Die();
                AwardExperience(instigator);
            }
            else {
                takeDamage.Invoke(damage);
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
            return _health.value;
        }

        public float GetPercentage() {
            return 100 * GetFraction();
        }

        public float GetFraction() {
            return _health.value / GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public float GetHealthPoints() {
            return _health.value;
        }

        public float GetMaxHealthPoints() {
            return GetComponent<BaseStats>().GetStat(Stat.Health);
        }

        public void RestoreState(object state) {
            _health.value = (float) state;

            if (_health.value == 0) {
                Die();
            }
        }

        public void Heal(float heal) {
            _health.value = Mathf.Min(_health.value + heal, GetMaxHealthPoints());
        }
    }
}