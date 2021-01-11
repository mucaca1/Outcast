using GameDevTV.Core.UI.Tooltips;
using Outcast.Quests;
using TMPro;
using UnityEngine;

namespace Outcast.UI.Quests {
    public class QuestTooltipSpawner : TooltipSpawner {

        public override void UpdateTooltip(GameObject tooltip) {
            Quest quest = GetComponent<QuestItemUI>().GetQuest();
            tooltip.GetComponent<QuestTooltipUI>().Setup(quest);
        }

        public override bool CanCreateTooltip() {
            return true;
        }
    }
}