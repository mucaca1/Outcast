using System;
using Outcast.Quests;
using UnityEngine;

namespace Outcast.UI.Quests {
    public class QuestListUI : MonoBehaviour {
        [SerializeField] private Quest[] tempQuest;

        [SerializeField] private QuestItemUI questPrefab;

        private void Start() {

            transform.DetachChildren();
            
            foreach (Quest quest in tempQuest) {
                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(quest);
            }
        }
    }
}