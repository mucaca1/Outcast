using System;
using System.Linq;
using UnityEngine;

namespace Dialogue {
    public class PlayerConversant : MonoBehaviour {
        [SerializeField] private Dialogue currentDialogue;
        private DialogueNode _currentNode = null;

        private void Awake() {
            _currentNode = currentDialogue.GetRootNode();
        }

        public string GetText() {
            if (_currentNode == null) {
                return "";
            }

            return _currentNode.GetText();
        }

        public void Next() {
            _currentNode = currentDialogue.GetAllChildren(_currentNode).ToArray()[0];
        }

        public bool HasNext() {
            return currentDialogue.GetAllChildren(_currentNode).Count() > 0;
        }
    }
}