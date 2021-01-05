using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue {
    public class PlayerConversant : MonoBehaviour {
        [SerializeField] private Dialogue currentDialogue;
        private DialogueNode _currentNode = null;

        private bool _isChoosing = false;

        private void Awake() {
            _currentNode = currentDialogue.GetRootNode();
        }

        public string GetText() {
            if (_currentNode == null) {
                return "";
            }

            return _currentNode.GetText();
        }

        public void SelectChoice(DialogueNode chosenNode) {
            _currentNode = chosenNode;
            _isChoosing = false;
            Next();
        }

        public bool IsChoosing() {
            return _isChoosing;
        }

        public void Next() {
            int numPlayerResponsies = currentDialogue.GetPlayerChildren(_currentNode).Count();
            if (numPlayerResponsies > 0) {
                _isChoosing = true;
                return;
            }

            DialogueNode[] nodes = currentDialogue.GetAIChildren(_currentNode).ToArray();
            int randomIndex = Random.Range(0, nodes.Length);
            _currentNode = nodes[randomIndex];
        }

        public IEnumerable<DialogueNode> GetChoices() {
            return currentDialogue.GetPlayerChildren(_currentNode);
        }

        public bool HasNext() {
            return currentDialogue.GetAllChildren(_currentNode).Count() > 0;
        }
    }
}