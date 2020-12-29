using System;
using GameDevTV.Utils;
using UnityEngine;

namespace Outcast.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression _progression = null;
        [SerializeField] private GameObject levelUpParticleEffectPrefab = null;
        [SerializeField] private bool shouldUseModifiers = false;
        public event Action onLevelUp;
        
        private LazyValue<int> _currentLevel;

        private Experience experience;

        private void Awake() {
            experience = GetComponent<Experience>();
            _currentLevel = new LazyValue<int>(GetLevel);
        }

        private void OnEnable() {
            if (experience != null) {
                experience.onExlerienceGained += UpdateLevel;
            }
        }

        private void OnDisable() {
            if (experience != null) {
                experience.onExlerienceGained -= UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int newLevel = CalculateLevel();
            if (newLevel > _currentLevel.value) {
                _currentLevel.value = newLevel;
                LevelUpEffect();
                onLevelUp();
            }
        }

        private void LevelUpEffect() {
            Instantiate(levelUpParticleEffectPrefab, transform);
        }

        public float GetStat(Stat stat) {
            if (shouldUseModifiers) {
                return GetBaseStat(stat);
            }
            else {
                return (GetBaseStat(stat) + GetAdditiveModifiers(stat)) * (1 + (GetPercentageModifiers(stat) / 100));
            }
        }

        private float GetPercentageModifiers(Stat stat) {
            float totalPercentage = 0f;
            
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifiers in provider.GetPercentageModifiers(stat)) {
                    totalPercentage += modifiers;
                }
            }

            return totalPercentage;
        }

        private float GetBaseStat(Stat stat) {
            return _progression.GetStat(stat, characterClass, GetLevel());
        }

        private float GetAdditiveModifiers(Stat stat) {
            float total = 0f;
            foreach (IModifierProvider provider in GetComponents<IModifierProvider>()) {
                foreach (float modifiers in provider.GetAdditiveModifiers(stat)) {
                    total += modifiers;
                }
            }

            return total;
        }

        public int GetLevel() {
            if (_currentLevel.value < 0) {
                _currentLevel.value = CalculateLevel();
            }
            return _currentLevel.value;
        }

        public int CalculateLevel() {
            Experience experience = GetComponent<Experience>();
            if (experience == null) return startingLevel;
            float currentXP = experience.GetPoints();
            int maxLevel = _progression.GetLevels(Stat.ExperienceLevelUp, characterClass);
            for (int level = 1; level <= maxLevel; level++) {
                float needetXP = _progression.GetStat(Stat.ExperienceLevelUp, characterClass, level);
                if (currentXP < needetXP) {
                    return level;
                }
            }
            return maxLevel + 1;
        }
    }
}