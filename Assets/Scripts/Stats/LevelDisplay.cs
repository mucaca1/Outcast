﻿using System;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.Stats {
    public class LevelDisplay : MonoBehaviour {
        private BaseStats _baseStats;

        private void Awake() {
            _baseStats = GameObject.FindWithTag("Player").GetComponent<BaseStats>();
        }

        private void Update() {
            GetComponent<Text>().text = String.Format("{0:0}",_baseStats.GetLevel());
        }
    }
}