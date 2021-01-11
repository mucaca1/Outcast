using System.Collections.Generic;
using UnityEngine;

namespace Outcast.Quests {
    [System.Serializable]
    public class QuestStatus {
        [SerializeField] private Quest quest;
        [SerializeField] private List<string> completedObjectives = new List<string>();

        public QuestStatus(Quest quest) {
            this.quest = quest;
        }
        public Quest GetQuest() {
            return quest;
        }

        public int GetCompletedQuestCount() {
            return completedObjectives.Count;
        }

        public bool IsObjectiveComplete(string objective) {
            return completedObjectives.Contains(objective);
        }

        public void CompleteObjective(string objective) {
            if (quest.HasObjective(objective)) {
                completedObjectives.Add(objective);
            }
            
        }
    }
}