using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
    public class DialogueNode : ScriptableObject {
        [SerializeField] private string text;
        [SerializeField] private List<string> children = new List<string>();

        [SerializeField] private Rect rect = new Rect(100, 50, 200, 130);

        public string GetText() {
            return text;
        }

#if UNITY_EDITOR
        public void SetText(string text) {
            if (text != this.text) {
                Undo.RecordObject(this, "Change text");
                this.text = text;
            }
        }
#endif

        public List<string> GetChildrens() {
            return children;
        }

#if UNITY_EDITOR
        public void AddChildren(string childUniqueId) {
            Undo.RecordObject(this, "Add Children");
            children.Add(childUniqueId);
        }

        public void RemoveChildren(string childUniqueId) {
            Undo.RecordObject(this, "Remove Children");
            children.Remove(childUniqueId);
        }

#endif
        public Rect GetRect() {
            return rect;
        }

#if UNITY_EDITOR

        public void SetPosition(Vector2 position) {
            Undo.RecordObject(this, "Change Position");
            rect.position = position;
        }

        public void SetRect(Rect rect) {
            Undo.RecordObject(this, "Move");
            this.rect = rect;
        }
    }
#endif
}