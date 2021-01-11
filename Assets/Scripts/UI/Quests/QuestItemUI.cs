using Outcast.Quests;
using TMPro;
using UnityEngine;

public class QuestItemUI : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private TextMeshProUGUI progress;
    
    public void Setup(Quest quest) {
        title.text = quest.GetTitle();
        progress.text = "0/" + quest.GetObjectiveCount();
    }
}