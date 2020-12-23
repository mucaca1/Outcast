using UnityEngine;

namespace Outcast.Stats {
    [CreateAssetMenu(fileName = "Progression", menuName = "Stats/New progression", order = 0)]
    public class Progression : ScriptableObject {

        [SerializeField] private ProgressionCharacterClass[] _characterClass = null;

        [System.Serializable]
        class ProgressionCharacterClass {
            [SerializeField] private CharacterClass characterClass;
            [SerializeField] private float[] life;
        }
    }
}