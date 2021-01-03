using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
    [System.Serializable]
    public class DialogueNode {
        public string uniqueID;
        public string text;
        public string[] children;

        public Rect rect;
    }
}