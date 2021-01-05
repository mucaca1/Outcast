using System;
using Dialogue;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Outcast.UI {
    public class DialogueUI : MonoBehaviour {
        private PlayerConversant _playerConversant;

        [SerializeField] private TextMeshProUGUI AIText;
        [SerializeField] private Button nextButton;

        private void Start() {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            nextButton.onClick.AddListener(Next);
            UpdateUI();
        }

        private void Next() {
            _playerConversant.Next();
            UpdateUI();
        }

        private void UpdateUI() {
            AIText.text = _playerConversant.GetText();
            nextButton.gameObject.SetActive(_playerConversant.HasNext());
        }
    }
}