using System;
using Outcast.SceneManagement;
using UnityEngine;

namespace Outcast.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] private GameObject permistentObjectPrefab;

        private static bool _wasSpawned = false;

        private void Awake() {
            if (_wasSpawned) {
                return;
            }

            SpawnPersistentObject();
            _wasSpawned = true;
        }

        private void SpawnPersistentObject() {
            GameObject persistentObject = Instantiate(permistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}