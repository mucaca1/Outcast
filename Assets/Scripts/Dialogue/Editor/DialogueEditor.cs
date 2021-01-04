using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor {
    public class DialogueEditor : EditorWindow {
        private Dialogue _selectedDialogue = null;
        private GUIStyle _nodeStyle;
        private DialogueNode _draggingNode = null;

        private Vector2 _draggingOffset;

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditor() {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line) {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null) {
                ShowDialogueEditor();
                return true;
            }

            return false;
        }

        private void OnEnable() {
            Selection.selectionChanged += OnSelectionChanged;

            _nodeStyle = new GUIStyle();
            _nodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            _nodeStyle.padding = new RectOffset(20, 20, 20, 20);
            _nodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged() {
            Dialogue dialogue = Selection.activeObject as Dialogue;
            if (dialogue != null) {
                _selectedDialogue = dialogue;
                Repaint();
            }
        }

        private void OnGUI() {
            if (_selectedDialogue == null) {
                EditorGUILayout.LabelField("No Dialogue selected.");
            }
            else {
                ProcessEvents();
                EditorGUILayout.LabelField(_selectedDialogue.name);
                foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                    DrawConnections(node);
                }
            }
        }

        private void ProcessEvents() {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null) {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition);
                if (_draggingNode != null) {
                    _draggingOffset = _draggingNode.rect.position - Event.current.mousePosition;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null) {
                Undo.RecordObject(_selectedDialogue, "Change Node Position");
                _draggingNode.rect.position = Event.current.mousePosition + _draggingOffset;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null) {
                _draggingNode = null;
            }
        }

        private void DrawNode(DialogueNode node) {
            GUILayout.BeginArea(node.rect, _nodeStyle);
            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);
            EditorGUI.BeginChangeCheck();
            string newId = EditorGUILayout.TextField(node.uniqueID);
            string newText = EditorGUILayout.TextField(node.text);
            if (EditorGUI.EndChangeCheck()) {
                Undo.RecordObject(_selectedDialogue, "Update Dialogue Text");
                node.uniqueID = newId;
                node.text = newText;
            }

            foreach (DialogueNode childrenNode in _selectedDialogue.GetAllChildren(node)) {
                EditorGUILayout.LabelField(childrenNode.text);
            }

            GUILayout.EndArea();
        }
        
        private void DrawConnections(DialogueNode node) {
            Vector3 startPosition = new Vector2(node.rect.xMax, node.rect.center.y);
            foreach (DialogueNode children in _selectedDialogue.GetAllChildren(node)) {
                Vector3 endPosition = new Vector2(children.rect.xMin, children.rect.center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0f;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }
        
        private DialogueNode GetNodeAtPoint(Vector2 point) {
            DialogueNode returnValue = null;
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) {
                if (node.rect.Contains(point)) {
                    returnValue = node;
                }
            }

            return returnValue;
        }
    }
}