using System.Collections.Generic;
using UnityEngine;

namespace Outcast.Quests {
    [CreateAssetMenu(fileName = "New Quest", menuName = "Outcast/Quest", order = 0)]
    public class Quest : ScriptableObject {
        [SerializeField] private List<string> objectives = new List<string>();


        public string GetTitle() {
            return name;
        }

        public int GetObjectiveCount() {
            return objectives.Count;
        }

        public IEnumerable<string> GetObjectives() {
            return objectives;
        }

        public bool HasObjective(string objective) {
            return objectives.Contains(objective);
        }

        public static Quest GetMyName(string questName) {
            foreach (Quest quest in Resources.LoadAll<Quest>("")) {
                if (quest.name == questName) {
                    return quest;
                }
            }

            return null;
        }
    }
}