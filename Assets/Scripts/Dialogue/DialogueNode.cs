using System;
using System.Collections;
using System.Collections.Generic;
using Outcast.Core;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
    public class DialogueNode : ScriptableObject {
        [SerializeField] private bool isPlayerSpeaking = false;
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new List<string>();

        [SerializeField] private Rect rect = new Rect(100, 50, 200, 130);

        [SerializeField] private string onEnterAction;
        [SerializeField] private string onExitAction;
        [SerializeField] private Condition condition;

        public string GetText() {
            return text;
        }

        public List<string> GetChildrens() {
            return children;
        }

        public Rect GetRect() {
            return rect;
        }

        public bool IsPlayerSpeaking() {
            return isPlayerSpeaking;
        }

        public string GetOnEnterAction() {
            return onEnterAction;
        }
        
        public string GetOnExitAction() {
            return onExitAction;
        }

#if UNITY_EDITOR
        
        public void SetPlayerSpeaking(bool isPlayerSpeaking) {
            Undo.RecordObject(this, "Change PlayerSpeaking");
            this.isPlayerSpeaking = isPlayerSpeaking;
            EditorUtility.SetDirty(this);
        }
        
        public void SetText(string text) {
            if (text != this.text) {
                Undo.RecordObject(this, "Change text");
                this.text = text;
                EditorUtility.SetDirty(this);
            }
        }

        public void AddChildren(string childUniqueId) {
            Undo.RecordObject(this, "Add Children");
            children.Add(childUniqueId);
            EditorUtility.SetDirty(this);
        }

        public void RemoveChildren(string childUniqueId) {
            Undo.RecordObject(this, "Remove Children");
            children.Remove(childUniqueId);
            EditorUtility.SetDirty(this);
        }

        public void SetPosition(Vector2 position) {
            Undo.RecordObject(this, "Change Position");
            rect.position = position;
            EditorUtility.SetDirty(this);
        }

        public void SetRect(Rect rect) {
            Undo.RecordObject(this, "Move");
            this.rect = rect;
            EditorUtility.SetDirty(this);
        }
#endif
        public bool ChceckCondition(IEnumerable<IPredicateEvaluator> evaluators) {
            return condition.Check(evaluators);
        }
    }
}