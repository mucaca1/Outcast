using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue {
    public class PlayerConversant : MonoBehaviour {
        [SerializeField] private Dialogue testDialogue;
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode = null;

        private bool _isChoosing = false;

        public event Action ONConversationUpdated;

        private IEnumerator Start() {
            yield return new WaitForSeconds(4);
            StartDialogue(testDialogue);
        }

        public void StartDialogue(Dialogue newDialogue) {
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            ONConversationUpdated();
        }

        public bool IsActive() {
            return _currentDialogue != null;
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

        public void Quit() {
            _currentDialogue = null;
            _currentNode = null;
            _isChoosing = false;
            ONConversationUpdated();
        }

        public void Next() {
            int numPlayerResponsies = _currentDialogue.GetPlayerChildren(_currentNode).Count();
            if (numPlayerResponsies > 0) {
                _isChoosing = true;
                ONConversationUpdated();
                return;
            }

            DialogueNode[] nodes = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            int randomIndex = Random.Range(0, nodes.Length);
            _currentNode = nodes[randomIndex];
            ONConversationUpdated();
        }

        public IEnumerable<DialogueNode> GetChoices() {
            return _currentDialogue.GetPlayerChildren(_currentNode);
        }

        public bool HasNext() {
            return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
        }
    }
}