using System.Collections.Generic;
using UnityEngine;

namespace Outcast.Quests {
    [CreateAssetMenu(fileName = "New Quest", menuName = "Outcast/Quest", order = 0)]
    public class Quest : ScriptableObject {
        [SerializeField] private string[] objectives;


        public string GetTitle() {
            return name;
        }

        public int GetObjectiveCount() {
            return objectives.Length;
        }

        public IEnumerable<string> GetObjectives() {
            return objectives;
        }
    }
}