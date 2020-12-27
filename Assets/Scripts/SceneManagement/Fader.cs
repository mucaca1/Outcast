using System;
using System.Collections;
using UnityEngine;

namespace Outcast.SceneManagement {
    public class Fader : MonoBehaviour {
        private CanvasGroup _canvasGroup;

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            _canvasGroup.alpha = 1f;
        }

       public IEnumerator FadeOut(float time) {
            while (_canvasGroup.alpha < 1f) {
                _canvasGroup.alpha += Time.deltaTime / time;
                yield return null;
            }
        }
        
        public IEnumerator FadeIn(float time) {
            while (_canvasGroup.alpha > 0) {
                _canvasGroup.alpha -= Time.deltaTime / time;
                yield return null;
            }
        }
    }
}