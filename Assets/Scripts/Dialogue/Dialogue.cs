using System;
using System.Collections.Generic;
using TreeEditor;
using UnityEditor;
using UnityEngine;

namespace Dialogue {
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Dialogue", order = 0)]
    public class Dialogue : ScriptableObject, ISerializationCallbackReceiver {
        [SerializeField] private List<DialogueNode> _nodes = new List<DialogueNode>();

        private Dictionary<string, DialogueNode> _nodeLookup = new Dictionary<string, DialogueNode>();

#if UNITY_EDITOR
        private void Awake() {
        }
#endif

        private void OnValidate() {
            _nodeLookup.Clear();
            foreach (DialogueNode node in _nodes) {
                _nodeLookup[node.name] = node;
            }
        }

        public IEnumerable<DialogueNode> GetAllNodes() {
            return _nodes;
        }

        public DialogueNode GetRootNode() {
            return _nodes[0];
        }

        public IEnumerable<DialogueNode> GetAllChildren(DialogueNode dialogueNode) {
            foreach (string childId in dialogueNode.GetChildrens()) {
                if (_nodeLookup.ContainsKey(childId)) {
                    yield return _nodeLookup[childId];
                }
            }
        }

#if UNITY_EDITOR
        public void CreateNode(DialogueNode parent) {
            var newNode = MakeNode(parent);

            newNode.SetPlayerSpeaking(!parent.IsPlayerSpeaking());
            newNode.SetPosition(parent.GetRect().position + new Vector2(parent.GetRect().width + 10, 0));
            Undo.RegisterCreatedObjectUndo(newNode, "Create New Node");
            Undo.RecordObject(this, "Add New Node");

            AddNode(newNode);
        }

        private void AddNode(DialogueNode newNode) {
            _nodes.Add(newNode);

            OnValidate();
        }

        private DialogueNode MakeNode(DialogueNode parent) {
            DialogueNode newNode = ScriptableObject.CreateInstance<DialogueNode>();
            newNode.name = Guid.NewGuid().ToString();
            if (parent != null) {
                parent.AddChildren(newNode.name);
            }

            return newNode;
        }

        public void DeleteNode(DialogueNode nodeToDelete) {
            Undo.RecordObject(this, "Delete Node");
            _nodes.Remove(nodeToDelete);
            OnValidate();
            CleanDanglingChildren(nodeToDelete);
            Undo.DestroyObjectImmediate(nodeToDelete);
        }

#endif

        private void CleanDanglingChildren(DialogueNode nodeToDelete) {
            foreach (DialogueNode node in GetAllNodes()) {
                node.RemoveChildren(nodeToDelete.name);
            }
        }

        public void OnBeforeSerialize() {
#if UNITY_EDITOR
            if (_nodes == null || _nodes.Count == 0) {
                var newNode = MakeNode(null);
                AddNode(newNode);
            }

            if (AssetDatabase.GetAssetPath(this) != "") {
                foreach (DialogueNode node in GetAllNodes()) {
                    if (AssetDatabase.GetAssetPath(node) == "") {
                        AssetDatabase.AddObjectToAsset(node, this);
                    }
                }
            }
#endif
        }

        public void OnAfterDeserialize() {
        }

        public IEnumerable<DialogueNode> GetPlayerChildren(DialogueNode currentNode) {
            foreach (DialogueNode node in GetAllChildren(currentNode)) {
                if (node.IsPlayerSpeaking()) {
                    yield return node;
                }
            }
        }

        public IEnumerable<DialogueNode> GetAIChildren(DialogueNode currentNode) {
            foreach (DialogueNode node in GetAllChildren(currentNode)) {
                if (!node.IsPlayerSpeaking()) {
                    yield return node;
                }
            }
        }
    }
}