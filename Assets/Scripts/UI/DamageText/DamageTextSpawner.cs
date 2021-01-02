using System;
using UnityEngine;

namespace Outcast.UI.DamageText {
    public class DamageTextSpawner : MonoBehaviour {
        [SerializeField] private DamageText _damageText;

        public void Spawn(float damage) {
            DamageText text = Instantiate<DamageText>(_damageText, transform);
            text.SetValue(damage);
        }
    }
}