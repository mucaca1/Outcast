﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.Attributes {
    public class HealthDisplay : MonoBehaviour {
        private Health _health;

        private void Awake() {
            _health = GameObject.FindWithTag("Player").GetComponent<Health>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}/{1:0}",_health.GetHealthPoints(), _health.GetMaxHealthPoints());
        }
    }
}