using System;
using Outcast.Control;
using Outcast.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Outcast.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private GameObject _player;
        private void Start() {
            GetComponent<PlayableDirector>().played += DisableControll;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            _player = GameObject.FindWithTag("Player");
        }

        void DisableControll(PlayableDirector pd) {
            _player.GetComponent<ActionScheduler>().CancelCurrentAction();
            _player.GetComponent<PlayerController>().enabled = false;
        }

        void EnableControl(PlayableDirector pd) {
            _player.GetComponent<PlayerController>().enabled = true;
        }
    }
}