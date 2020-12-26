using System;
using UnityEngine;

namespace Outcast.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression _progression = null;

        private int _currentLevel;
        
        private void Start() {
            _currentLevel = GetLevel();
            Experience experience = GetComponent<Experience>();
            if (experience != null) {
                experience.onExlerienceGained += UpdateLevel;
            }
        }

        private void UpdateLevel() {
            int newLevel = GetLevel();
            if (newLevel > _currentLevel) {
                _currentLevel = newLevel;
                print("Levelled up!");
            }
        }
        
        public float GetStat(Stat stat) {
            return _progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
            if (_currentLevel < 0) {
                _currentLevel = CalculateLevel();
            }
            return _currentLevel;
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