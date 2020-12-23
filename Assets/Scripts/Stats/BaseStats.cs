using System;
using UnityEngine;

namespace Outcast.Stats {
    public class BaseStats : MonoBehaviour {
        [Range(1, 99)]
        [SerializeField] private int startingLevel = 1;
        [SerializeField] private CharacterClass characterClass;
        [SerializeField] private Progression _progression = null;
    }
}