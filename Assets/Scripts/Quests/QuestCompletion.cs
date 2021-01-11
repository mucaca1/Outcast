using UnityEngine;

namespace Outcast.Quests {
    public class QuestCompletion : MonoBehaviour {
        [SerializeField] private Quest quest;
        [SerializeField] private string objective;

        public void CompleteObjective() {
            QuestList questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.CompleteObjective(quest, objective);
        }
    }
}