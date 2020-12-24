﻿using UnityEngine;

namespace Outcast.Resources {
    public class Experience : MonoBehaviour {

        [SerializeField] private float experiencePoints = 0f;

        public void GainExperience(float experience) {
            experiencePoints += experience;
        }
    }
}