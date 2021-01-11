using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outcast.Quests {
    
    public class QuestList : MonoBehaviour {
        [SerializeField] private List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action ONUpdate;

        public void AddQuest(Quest quest) {
            if (HasQuest(quest)) return;
            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            ONUpdate?.Invoke();
        }

        private bool HasQuest(Quest quest) {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses() {
            return statuses;
        }

        public void CompleteObjective(Quest quest, string objective) {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            ONUpdate?.Invoke();
        }

        private QuestStatus GetQuestStatus(Quest quest) {
            foreach (QuestStatus status in statuses) {
                if (status.GetQuest() == quest) {
                    return status;
                }
            }

            return null;
        }
    }
}