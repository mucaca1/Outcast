using Outcast.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;

    private Quest _quest;
    
    public void Setup(Quest quest) {
        _quest = quest;
        title.text = quest.GetTitle();
        progress.text = "0/" + quest.GetObjectiveCount();
    }

    public Quest GetQuest() {
        return _quest;
    }
}