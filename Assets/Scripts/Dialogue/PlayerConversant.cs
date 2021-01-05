using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Dialogue {
    public class PlayerConversant : MonoBehaviour {
        private Dialogue _currentDialogue;
        private DialogueNode _currentNode = null;

        private bool _isChoosing = false;

        public event Action ONConversationUpdated;

        public void StartDialogue(Dialogue newDialogue) {
            _currentDialogue = newDialogue;
            _currentNode = _currentDialogue.GetRootNode();
            TriggerEnterAction();
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
            TriggerEnterAction();
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
            TriggerExitAction();
            ONConversationUpdated();
        }

        public void Next() {
            int numPlayerResponsies = _currentDialogue.GetPlayerChildren(_currentNode).Count();
            if (numPlayerResponsies > 0) {
                _isChoosing = true;
                TriggerExitAction();
                ONConversationUpdated();
                return;
            }

            DialogueNode[] nodes = _currentDialogue.GetAIChildren(_currentNode).ToArray();
            if (nodes.Length != 0) {
                int randomIndex = Random.Range(0, nodes.Length);
                TriggerExitAction();
                _currentNode = nodes[randomIndex];
                TriggerEnterAction();
            }

            ONConversationUpdated();
        }

        public IEnumerable<DialogueNode> GetChoices() {
            return _currentDialogue.GetPlayerChildren(_currentNode);
        }

        public bool HasNext() {
            return _currentDialogue.GetAllChildren(_currentNode).Count() > 0;
        }

        private void TriggerEnterAction() {
            if (_currentNode != null && _currentNode.GetOnEnterAction() != "") {
                Debug.Log(_currentNode.GetOnEnterAction());
            }
        }

        private void TriggerExitAction() {
            if (_currentNode != null && _currentNode.GetOnExitAction() != "") {
                Debug.Log(_currentNode.GetOnExitAction());
            }
        }
    }
}