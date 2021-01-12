using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outcast.Quests {
    [System.Serializable]
    public class QuestStatus {
        [SerializeField] private Quest quest;
        [SerializeField] private List<string> completedObjectives = new List<string>();

        [System.Serializable]
        class QuestStatusRecord {
            public string questName;
            public List<string> completedObjectives;
        }

        public QuestStatus(Quest quest) {
            this.quest = quest;
        }

        public QuestStatus(object quest) {
            QuestStatusRecord record = quest as QuestStatusRecord;
            if (record == null) return;
            this.quest = Quest.GetMyName(record.questName);
            completedObjectives = record.completedObjectives;
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

        public object CaptureState() {
            QuestStatusRecord record = new QuestStatusRecord();
            record.questName = quest.GetTitle();
            record.completedObjectives = completedObjectives;
            return record;
        }

        public bool IsComplete() {
            foreach (var objective in quest.GetObjectives()) {
                if (!completedObjectives.Contains(objective.reference)) {
                    return false;
                }
            }

            return true;
        }
    }
}