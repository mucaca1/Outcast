using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Outcast.Quests {
    
    public class QuestList : MonoBehaviour {
        [SerializeField] private QuestStatus[] statuses;

        public IEnumerable<QuestStatus> GetStatuses() {
            return statuses.ToArray();
        }
    }
}