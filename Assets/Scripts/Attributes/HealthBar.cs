using System;
using UnityEngine;

namespace Outcast.Attributes {
    public class HealthBar : MonoBehaviour {
        [SerializeField] private Health _healthComponent = null;
        [SerializeField] private RectTransform foreground = null;
        [SerializeField] private Canvas rootCanvas = null;
         
        private void Update() {
            float fraction = _healthComponent.GetFraction();
            if (Mathf.Approximately(fraction, 0) || Mathf.Approximately(fraction, 1)) {
                rootCanvas.enabled = false;
            }

            rootCanvas.enabled = true;
            foreground.localScale = new Vector3(fraction, 1f, 1f);
        }
    }
}