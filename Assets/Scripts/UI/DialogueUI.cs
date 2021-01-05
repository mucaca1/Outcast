using System;
using Dialogue;
using TMPro;
using UnityEngine;

namespace Outcast.UI {
    public class DialogueUI : MonoBehaviour {
        private PlayerConversant _playerConversant;

        [SerializeField] private TextMeshProUGUI AIText;

        private void Start() {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            AIText.text = _playerConversant.GetText();
        }
    }
}