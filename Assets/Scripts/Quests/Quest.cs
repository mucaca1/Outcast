﻿using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

namespace Outcast.Quests {
    [CreateAssetMenu(fileName = "New Quest", menuName = "Outcast/Quest", order = 0)]
    public class Quest : ScriptableObject {
        [SerializeField] private List<Objective> objectives = new List<Objective>();
        [SerializeField] private List<Reward> rewards = new List<Reward>();

        [System.Serializable]
        public class Reward {
            [Min(1)]
            public int number;
            public InventoryItem item;
        }

        [System.Serializable]
        public class Objective {
            public string reference;
            public string description;
        }


        public string GetTitle() {
            return name;
        }

        public int GetObjectiveCount() {
            return objectives.Count;
        }

        public IEnumerable<Objective> GetObjectives() {
            return objectives;
        }

        public bool HasObjective(string objectiveRef) {
            foreach (var objective in objectives) {
                if (objective.reference == objectiveRef) {
                    return true;
                }
            }

            return false;
        }

        public static Quest GetMyName(string questName) {
            foreach (Quest quest in Resources.LoadAll<Quest>("")) {
                if (quest.name == questName) {
                    return quest;
                }
            }

            return null;
        }

        public IEnumerable<Reward> GetRewards() {
            return rewards;
        }
    }
}