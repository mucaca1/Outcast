using System;
using Outcast.Quests;
using UnityEngine;

namespace Outcast.UI.Quests {
    public class QuestListUI : MonoBehaviour {

        [SerializeField] private QuestItemUI questPrefab;

        private void Start() {

            transform.DetachChildren();
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            foreach (QuestStatus status in questList.GetStatuses()) {
                QuestItemUI questInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                questInstance.Setup(status);
            }
        }
    }
}