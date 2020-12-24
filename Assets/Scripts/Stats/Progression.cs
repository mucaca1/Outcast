using UnityEngine;

namespace Outcast.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New progression", order = 0)]
    public class Progression : ScriptableObject {

        [SerializeField] private ProgressionCharacterClass[] _characterClass = null;
        
        public float GetHealth(CharacterClass characterClass, int level) {
            foreach (ProgressionCharacterClass progressionCharacterClass in _characterClass) {
                // if (progressionCharacterClass.GetCharacterClass() == characterClass) {
                //     return progressionCharacterClass.GetLifeByIndex(level - 1);
                // }
            }
            return 0f;
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