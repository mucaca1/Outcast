using System;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.UI.DamageText {
    public class DamageText : MonoBehaviour {
        [SerializeField] private Text damageText = null;

        public void SetValue(float damage) {
            damageText.text = String.Format("{0:0}", damage);
        }
    }
}