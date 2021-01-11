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
        public void Setup(Quest quest) {
            title.text = quest.GetTitle();
            objectiveContainer.DetachChildren();
            foreach (string objective in quest.GetObjectives()) {
                GameObject objectiveInstance = Instantiate(objectivePrefab, objectiveContainer);
                TextMeshProUGUI text = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                text.text = objective;
            }
        } 
    }
}
