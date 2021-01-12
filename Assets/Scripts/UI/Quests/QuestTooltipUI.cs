using System.Collections;
using System.Collections.Generic;
using Outcast.Quests;
using TMPro;
using UnityEngine;

namespace Outcast.UI.Quests {
    public class QuestTooltipUI : MonoBehaviour {
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private Transform objectiveContainer;
        [SerializeField] private GameObject objectivePrefab;
        [SerializeField] private GameObject objectiveIncompletedPrefab;
        public void Setup(QuestStatus status) {
            title.text = status.GetQuest().GetTitle();
            objectiveContainer.DetachChildren();
            foreach (Quest.Objective objective in status.GetQuest().GetObjectives()) {
                GameObject prefab = status.IsObjectiveComplete(objective.reference)
                    ? objectivePrefab
                    : objectiveIncompletedPrefab;
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI text = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                text.text = objective.description;
            }
        } 
    }
}
