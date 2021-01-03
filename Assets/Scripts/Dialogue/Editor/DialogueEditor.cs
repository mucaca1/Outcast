using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Dialogue.Editor {
    public class DialogueEditor : EditorWindow {

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowDialogueEditor() {
            GetWindow(typeof(DialogueEditor), false, "Dialogue Editor");
        }

        [OnOpenAssetAttribute(1)]
        public static bool OnOpenAsset(int instanceID, int line) {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null) {
                ShowDialogueEditor();
                return true;
            }
            return false;
        }

        private void OnGUI() {
            EditorGUILayout.LabelField("Hello world");
            EditorGUILayout.LabelField("Ahoj svet");
            EditorGUILayout.LabelField("Nazdar světe");
        }
    }
}