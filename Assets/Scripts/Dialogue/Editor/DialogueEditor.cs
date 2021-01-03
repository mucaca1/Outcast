using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        private Dialogue _selectedDialogue = null;

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
                EditorGUILayout.LabelField(_selectedDialogue.name);
            }
        }
    }
}