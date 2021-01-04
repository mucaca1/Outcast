using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Dialogue {
    [System.Serializable]
    public class DialogueNode {
        public string uniqueID;
        public string text;
        public List<string> children = new List<string>();

        public Rect rect = new Rect(100, 50, 200, 130);
    }
}