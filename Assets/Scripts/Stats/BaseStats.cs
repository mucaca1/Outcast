using System;
using UnityEngine;

namespace Outcast.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression _progression = null;

        private void Update() {
            if (gameObject.tag == "Player") {
                print(GetLevel());
            }
        }

        public float GetStat(Stat stat) {
            return _progression.GetStat(stat, characterClass, GetLevel());
        }

        public int GetLevel() {
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