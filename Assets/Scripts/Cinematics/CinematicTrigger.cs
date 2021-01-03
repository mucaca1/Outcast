using System;
using GameDevTV.Saving;
using UnityEngine;
using UnityEngine.Playables;

namespace Outcast.Cinematics {
    public class CinematicTrigger : MonoBehaviour, ISaveable {
        private bool _wasPlayed = false;

        private void OnTriggerEnter(Collider other) {
            if (!_wasPlayed && other.tag == "Player") {
                _wasPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }

        public object CaptureState() {
            return _wasPlayed;
        }

        public void RestoreState(object state) {
            _wasPlayed = (bool) state;
        }
    }
}