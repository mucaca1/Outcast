using System;
using System.Collections;
using UnityEngine;

namespace Outcast.SceneManagement {
    public class Fader : MonoBehaviour {
        private CanvasGroup _canvasGroup;
        private Coroutine _actualRunningCoroutine = null;

        private void Awake() {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void FadeOutImmediate() {
            _canvasGroup.alpha = 1f;
        }

       public IEnumerator FadeOut(float time) {
           return Fade(1, time);
       }
       
       public IEnumerator FadeIn(float time) {
           return Fade(0, time);
       }

       private IEnumerator FadeRoutine(float target, float time) {
           while (Mathf.Approximately(_canvasGroup.alpha, target)) {
               _canvasGroup.alpha = Mathf.MoveTowards(_canvasGroup.alpha, target, Time.deltaTime / time);
               yield return null;
           }
       }

       public IEnumerator Fade(float target, float time) {
           if (_actualRunningCoroutine != null) {
               StopCoroutine(_actualRunningCoroutine);
           }

           _actualRunningCoroutine = StartCoroutine(FadeRoutine(target ,time));
           yield return _actualRunningCoroutine;
       }
    }
}