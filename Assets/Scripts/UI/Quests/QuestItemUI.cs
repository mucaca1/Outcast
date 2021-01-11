using Outcast.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;

    private QuestStatus _questStatus;
    
    public void Setup(QuestStatus status) {
        _questStatus = status;
        title.text = status.GetQuest().GetTitle();
        progress.text = status.GetCompletedQuestCount() + "/" + status.GetQuest().GetObjectiveCount();
    }

    public QuestStatus GetQuestStatus() {
        return _questStatus;
    }
}