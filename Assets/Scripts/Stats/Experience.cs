using System;
using GameDevTV.Saving;
using UnityEngine;

namespace Outcast.Stats {
    public class Experience : MonoBehaviour, ISaveable {

        [SerializeField] private float experiencePoints = 0f;
        public event Action onExlerienceGained;

        public void GainExperience(float experience) {
            experiencePoints += experience;
            onExlerienceGained();
        }

        public object CaptureState() {
            return experiencePoints;
        }

        public void RestoreState(object state) {
            experiencePoints = (float) state;
        }

        public float GetPoints() {
            return experiencePoints;
        }
    }
}