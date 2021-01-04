﻿using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor {
    public class DialogueEditor : EditorWindow {
        [NonSerialized] private Dialogue _selectedDialogue = null;
        [NonSerialized] private GUIStyle _nodeStyle;
        [NonSerialized] private DialogueNode _draggingNode = null;
        [NonSerialized] private DialogueNode _creatingNode = null;
        [NonSerialized] private DialogueNode _nodeToDelete = null;

        [NonSerialized] private DialogueNode _linkingParentNode = null;
        
        private Vector2 _scrollPosition = new Vector2(0, 0);

        [NonSerialized] private bool _draggingCanvas = false;
        [NonReorderable] private Vector2 _draggingCanvasOffset;

        private Vector2 _draggingOffset;
        private const float _canvasSize = 5000;
        private const float _backgroundSize = 50;

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
                _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);
                Rect canvas = GUILayoutUtility.GetRect(_canvasSize, _canvasSize);
                Rect texCoords = new Rect(0, 0, _canvasSize / _backgroundSize, _canvasSize / _backgroundSize);
                Texture2D background = Resources.Load("background") as Texture2D;
                GUI.DrawTextureWithTexCoords(canvas, background, texCoords);
                
                EditorGUILayout.LabelField(_selectedDialogue.name);
                foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) {
                    DrawNode(node);
                    DrawConnections(node);
                }
                
                EditorGUILayout.EndScrollView();

                if (_creatingNode != null) {
                    _selectedDialogue.CreateNode(_creatingNode);
                    _creatingNode = null;
                }

                if (_nodeToDelete != null) {
                    _selectedDialogue.DeleteNode(_nodeToDelete);
                    _nodeToDelete = null;
                }
            }
        }

        private void ProcessEvents() {
            if (Event.current.type == EventType.MouseDown && _draggingNode == null) {
                _draggingNode = GetNodeAtPoint(Event.current.mousePosition + _scrollPosition);
                if (_draggingNode != null) {
                    _draggingOffset = _draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = _draggingNode;
                }
                else {
                    _draggingCanvas = true;
                    _draggingCanvasOffset = Event.current.mousePosition + _scrollPosition;
                    Selection.activeObject = _selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingNode != null) {
                _draggingNode.SetPosition(Event.current.mousePosition + _draggingOffset);
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && _draggingCanvas) {
                _scrollPosition = _draggingCanvasOffset - Event.current.mousePosition;
                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingNode != null) {
                _draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && _draggingCanvas) {
                _draggingCanvas = false;
            }
        }

        private void DrawNode(DialogueNode node) {
            GUILayout.BeginArea(node.GetRect(), _nodeStyle);
            EditorGUILayout.LabelField("Node:", EditorStyles.whiteLabel);

            EditorGUILayout.LabelField(node.name);
            node.SetText(EditorGUILayout.TextField(node.GetText()));


            GUILayout.BeginHorizontal();
            if (GUILayout.Button("x")) {
                _nodeToDelete = node;
            }

            DrawLinkButtons(node);

            if (GUILayout.Button("+")) {
                _creatingNode = node;
            }

            GUILayout.EndHorizontal();

            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node) {
            if (_linkingParentNode == null) {
                if (GUILayout.Button("link")) {
                    _linkingParentNode = node;
                }
            }
            else if (_linkingParentNode == node) {
                if (GUILayout.Button("cancel")) {
                    _linkingParentNode = null;
                }
            }
            else if (_linkingParentNode.GetChildrens().Contains(node.name)) {
                if (GUILayout.Button("unlink")) {
                    _linkingParentNode.RemoveChildren(node.name);
                    _linkingParentNode = null;
                }
            }
            else {
                if (GUILayout.Button("child")) {
                    _linkingParentNode.AddChildren(node.name);
                    _linkingParentNode = null;
                }
            }
        }

        private void DrawConnections(DialogueNode node) {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode children in _selectedDialogue.GetAllChildren(node)) {
                Vector3 endPosition = new Vector2(children.GetRect().xMin, children.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0f;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset,
                    endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }

        private DialogueNode GetNodeAtPoint(Vector2 point) {
            DialogueNode returnValue = null;
            foreach (DialogueNode node in _selectedDialogue.GetAllNodes()) {
                if (node.GetRect().Contains(point)) {
                    returnValue = node;
                }
            }

            return returnValue;
        }
    }
}