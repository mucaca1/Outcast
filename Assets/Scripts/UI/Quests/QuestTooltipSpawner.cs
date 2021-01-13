using GameDevTV.Core.UI.Tooltips;
using Outcast.Quests;
using TMPro;
using UnityEngine;

namespace Outcast.UI.Quests {
    public class QuestTooltipSpawner : TooltipSpawner {

        public override void UpdateTooltip(GameObject tooltip) {
            QuestStatus status = GetComponent<QuestItemUI>().GetQuestStatus();
            tooltip.GetComponent<QuestTooltipUI>().Setup(status);
        }

        public override bool CanCreateTooltip() {
            return true;
        }
    }
}