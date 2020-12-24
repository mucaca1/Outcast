﻿using System;
using Outcast.Resources;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.Combat {
    public class EnemyHealthDisplay : MonoBehaviour {
        private Fighter _fighter;

        private void Awake() {
            _fighter = GameObject.FindWithTag("Player").GetComponent<Fighter>();
        }

        private void Update() {
            Health target = _fighter.GetTarget();
            if (target == null) {
                GetComponent<Text>().text = "N/A";
            }
            else {
                GetComponent<Text>().text = String.Format("{0:0}%", target.GetPercentage().ToString());   
            }
        }
    }
}