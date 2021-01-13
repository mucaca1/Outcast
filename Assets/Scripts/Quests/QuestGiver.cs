using UnityEngine;

namespace Outcast.Quests {
    public class QuestGiver : MonoBehaviour {
        [SerializeField] private Quest quest;


        public void GiveQuest() {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.AddQuest(quest);
        }
    }
}