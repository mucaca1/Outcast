using System.Collections.Generic;
using UnityEngine;

namespace Outcast.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New progression", order = 0)]
    public class Progression : ScriptableObject {
        [SerializeField] private ProgressionCharacterClass[] _characterClass = null;

        private Dictionary<CharacterClass, Dictionary<Stat, float[]>> _stats = null;

        public float GetStat(Stat stat, CharacterClass characterClass, int level) {
            BuildLookUp();

            float[] levels = _stats[characterClass][stat];

            if (levels.Length < level) {
                return 0f;
            }
            
            return levels[level - 1];
        }

        public int GetLevels(Stat stat, CharacterClass characterClass) {
            float[] levels = _stats[characterClass][stat];
            return levels.Length;
        }

        private void BuildLookUp() {
            if (_stats != null) return;

            _stats = new Dictionary<CharacterClass, Dictionary<Stat, float[]>>();
            foreach (ProgressionCharacterClass progressionCharacterClass in _characterClass) {
                var characterClass = new Dictionary<Stat, float[]>();
                foreach (ProgressionStat progressionStat in progressionCharacterClass.stats) {
                    characterClass[progressionStat.stat] = progressionStat.levels;
                }

                _stats[progressionCharacterClass.characterClass] = characterClass;
            }
        }

        [System.Serializable]
        class ProgressionCharacterClass {
            public CharacterClass characterClass;
            public ProgressionStat[] stats;
        }

        [System.Serializable]
        class ProgressionStat {
            public Stat stat;
            public float[] levels;
        }
    }
}