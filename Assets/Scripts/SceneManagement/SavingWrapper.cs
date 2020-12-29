using System;
using System.Collections;
using RPG.Saving;
using UnityEngine;

namespace Outcast.SceneManagement {
    public class SavingWrapper : MonoBehaviour {
        private const string defaultSaveFile = "save";
        [SerializeField] private float fadeInTime = 0.2f;

        private void Awake() {
            StartCoroutine(LoadLastScene());
        }

        private IEnumerator LoadLastScene() {
            yield return GetComponent<SavingSystem>().LoadLastScene(defaultSaveFile);
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeOutImmediate();
            yield return fader.FadeIn(fadeInTime);
        }
        
        private void Update() {
            if (Input.GetKeyDown(KeyCode.L)) {
                Load();
            }
        
            if (Input.GetKeyDown(KeyCode.S)) {
                Save();
            }

            if (Input.GetKeyDown(KeyCode.D)) {
                Delete();
            }
        }

        private void Delete() {
            GetComponent<SavingSystem>().Delete(defaultSaveFile);
        }

        public void Save() {
            GetComponent<SavingSystem>().Save(defaultSaveFile);
        }

        public void Load() {
            GetComponent<SavingSystem>().Load(defaultSaveFile);
        }
    }
}