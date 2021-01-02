using System;
using System.Collections;
using Outcast.Control;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

namespace Outcast.SceneManagement {
    public class Portal : MonoBehaviour {

        enum DestinationIdentifier {
            A, B, C, D, E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOutTime = 0.5f;
        [SerializeField] private float fadeInTime = 1.5f;
        [SerializeField] private float fadeWaitTime = 0.5f;
        private void OnTriggerEnter(Collider other) {
            if (other.tag == "Player") {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition() {
            if (sceneToLoad < 0) {
                Debug.LogError("Portal does not has select scene");
                yield break;
            }
            DontDestroyOnLoad(gameObject);

            PlayerController playerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            playerController.enabled = false;
            
            SavingWrapper wrapper = FindObjectOfType<SavingWrapper>();

            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);
            
            wrapper.Save();
            
            yield return SceneManager.LoadSceneAsync(sceneToLoad);
            PlayerController newPlayerController = GameObject.FindWithTag("Player").GetComponent<PlayerController>();
            newPlayerController.enabled = false;
            
            wrapper.Load();

            Portal otherPortal = GetAnotherPortal();
            UpdatePlayer(otherPortal);

            wrapper.Save();
            
            yield return new WaitForSeconds(fadeWaitTime);
            fader.FadeIn(fadeInTime);
            
            newPlayerController.enabled = true;
            
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal) {
            GameObject player = GameObject.FindWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetAnotherPortal() {
            foreach (Portal portal in FindObjectsOfType<Portal>()) {
                if (portal == this) continue;
                if (portal.destination == this.destination)
                    return portal;
            }

            return null;
        }
    }
}