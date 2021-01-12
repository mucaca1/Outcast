using System;
using System.Collections.Generic;
using System.Linq;
using GameDevTV.Inventories;
using GameDevTV.Saving;
using UnityEngine;

namespace Outcast.Quests {
    
    public class QuestList : MonoBehaviour, ISaveable {
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
            if (status.IsComplete()) {
                GiveReward(quest);
            }
            ONUpdate?.Invoke();
        }

        private void GiveReward(Quest quest) {
            foreach (var reward in quest.GetRewards()) {
                bool success = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                if (!success) {
                    GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                }
            }
        }

        private QuestStatus GetQuestStatus(Quest quest) {
            foreach (QuestStatus status in statuses) {
                if (status.GetQuest() == quest) {
                    return status;
                }
            }

            return null;
        }

        public object CaptureState() {
            List<object> state = new List<object>();
            foreach (var statuse in statuses) {
                state.Add(statuse.CaptureState());
            }

            return state;
        }

        public void RestoreState(object state) {
            List<object> stateList = state as List<object>;
            if (state == null) return;

            statuses.Clear();
            foreach (object objectState in stateList) {
                statuses.Add(new QuestStatus(objectState));
            }
        }
    }
}