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
                    UpdateUI();
                });
            }
        }
    }
}