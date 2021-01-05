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
        [SerializeField] private Transform choiceRoot;
        [SerializeField] private GameObject choicePrefab;
        [SerializeField] private GameObject AIResponse;
        [SerializeField] private Button quitutton;
        [SerializeField] private TextMeshProUGUI conversantName;

        private void Start() {
            _playerConversant = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerConversant>();
            _playerConversant.ONConversationUpdated += UpdateUI;
            nextButton.onClick.AddListener(() => _playerConversant.Next());
            quitutton.onClick.AddListener(() => _playerConversant.Quit());
            UpdateUI();
        }

        private void UpdateUI() {
            gameObject.SetActive(_playerConversant.IsActive());
            if (!_playerConversant.IsActive()) return;

            conversantName.text = _playerConversant.GetCurrentConversantName();
            AIResponse.SetActive(!_playerConversant.IsChoosing());
            choiceRoot.gameObject.SetActive(_playerConversant.IsChoosing());

            if (_playerConversant.IsChoosing()) {
                BuildChoiceList();
            }
            else {
                AIText.text = _playerConversant.GetText();
                nextButton.gameObject.SetActive(_playerConversant.HasNext());
            }

        }

        private void BuildChoiceList() {
            foreach (Transform child in choiceRoot) {
                Destroy(child.gameObject);
            }

            foreach (DialogueNode choice in _playerConversant.GetChoices()) {
                GameObject choiceButton = Instantiate(choicePrefab, choiceRoot);
                choiceButton.GetComponentInChildren<TextMeshProUGUI>().text = choice.GetText();
                Button button = choiceButton.GetComponentInChildren<Button>();
                button.onClick.AddListener(() => {
                    _playerConversant.SelectChoice(choice);
                });
            }
        }
    }
}