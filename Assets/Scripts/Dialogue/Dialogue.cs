using System;
using System.Collections.Generic;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject {
        [SerializeField]
        private List<DialogueNode> _nodes;

        private Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

        #if UNITY_EDITOR
        private void Awake() {
            if (_nodes == null || _nodes.Count == 0) {
                _nodes = new List<DialogueNode>();
                _nodes.Add(new DialogueNode());
            }
            OnValidate();
        }
        #endif

        private void OnValidate() {
            _nodeLookup.Clear();
            foreach (DialogueNode node in _nodes) {
                _nodeLookup[node.uniqueID] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes() {
            return _nodes;
        }

        public DialogueNode GetRootNode() {
            return _nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode dialogueNode) {
            foreach (string childId in dialogueNode.children) {
                if (_nodeLookup.ContainsKey(childId)) {
                    yield return _nodeLookup[childId];
                }
            }
        }
    }
}