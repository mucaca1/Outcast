using System;
using UnityEngine;
using UnityEngine.Playables;

namespace Outcast.Cinematics {
    public class CinematicTrigger : MonoBehaviour {

        private bool _wasPlayed = false;
        private void OnTriggerEnter(Collider other) {
            if (!_wasPlayed && other.tag == "Player") {
                _wasPlayed = true;
                GetComponent<PlayableDirector>().Play();
            }
        }
    }
}