using System;
using Outcast.Control;
using Outcast.Core;
using UnityEngine;
using UnityEngine.Playables;

namespace Outcast.Cinematics {
    public class CinematicControlRemover : MonoBehaviour {
        private GameObject _player;

        private void Awake() {
            _player = GameObject.FindWithTag("Player");
        }

        private void OnEnable() {
            GetComponent<PlayableDirector>().played += DisableControll;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable() {
            GetComponent<PlayableDirector>().played -= DisableControll;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
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