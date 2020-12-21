using System;
using UnityEngine;

namespace Outcast.Core {
    public class PersistentObjectSpawner : MonoBehaviour {
        [SerializeField] private GameObject permistentObjectPrefab;

        private bool wasSpawned = false;
        private void Awake() {
            if (wasSpawned) return;

            SpawnPersistentObject();

            wasSpawned = true;
        }

        private void SpawnPersistentObject() {
            GameObject persistentObject = Instantiate(permistentObjectPrefab);
            DontDestroyOnLoad(persistentObject);
        }
    }
}