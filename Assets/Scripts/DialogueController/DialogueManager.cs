using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour {
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image characterImage;
    [SerializeField] private GameObject transitionPanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;


    const float FADEIN_TRANSITION_TIME = 0.5f;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }
    private bool dialogueIsWriting;
    public bool isFadeInTransition;
    public bool isFadeOutTransition;

    private static DialogueManager instance;

    public String choicesIndexes;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;
        choicesIndexes = "";
    }
    public static DialogueManager GetInstance() {
        return instance;
    }

    private void Start() {

        isFadeInTransition = true;
        isFadeOutTransition = false;
        dialogueIsPlaying = false;
        dialogueIsWriting = false;
        dialogueBox.SetActive(false);
        characterImage.enabled = false;

        transitionPanel.SetActive(true);

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices) {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }

        StartCoroutine(FinishFadeInTransition());
    }

    private void Update() {
        if (isFadeInTransition) {
            FadeInTransition();
        } else if (isFadeOutTransition) {
            FadeOutTransition();
        } else { 
            if(!dialogueIsPlaying) {
                return;
            }
            if(InputManager.GetInstance().GetSubmitPressed()) {
                if(!dialogueIsWriting) {
                    ContinueStory();
                } else {
                    dialogueIsWriting = false;
                }
            }
        }
    }

    public IEnumerator EnterDialogueMode(TextAsset inkJSON) {

        dialogueIsPlaying = true;

        yield return new WaitForSeconds(FADEIN_TRANSITION_TIME);

        currentStory = new Story(inkJSON.text);
        dialogueBox.SetActive(true);
        characterImage.enabled = true;

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode() {
        yield return new WaitForSeconds(FADEIN_TRANSITION_TIME);

        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        characterImage.enabled = false;
        dialogueText.text = "";
        nameText.text = "";
    }

    private void ContinueStory() {
        HidingChoices();
        if (currentStory.canContinue) {
            // set text for the current dialogue line
            String stringText = currentStory.Continue();

            char[] separators = { '[', ']' };
            String[] strlist = stringText.Split(separators, 3, StringSplitOptions.None);

            if (strlist.Length == 3) {
                char[] charNameSeparators = { '_' };
                String[] charNamelist = strlist[1].Split(charNameSeparators, 3, StringSplitOptions.None);

                if (charNamelist.Length == 2) {
                    nameText.text = charNamelist[0];
                } else {
                    nameText.text = strlist[1];
                }
                characterImage.sprite = CharacterImageManager.GetInstance().ChangeCharacterImage(strlist[1]);
               StartCoroutine(FillText(strlist[2]));
            }
            else {
                characterImage.sprite = CharacterImageManager.GetInstance().ChangeCharacterImage("");
                nameText.text = "";
                StartCoroutine(FillText(" " + stringText));
            }
        } else {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private IEnumerator FillText(String text) {
        dialogueIsWriting = true;
        dialogueText.text = "";
        foreach (char c in text) {
            if(dialogueIsWriting) {
                dialogueText.text += c;
                yield return new WaitForSeconds(0.025f);
            } else {
                dialogueText.text = text;
                break;
            }
        }
        dialogueIsWriting = false;
        // display choices, if any, for this dialogue line
        DisplayChoices();
    }

    private void DisplayChoices() {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length) {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue.
        foreach(Choice choice in currentChoices) {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden.
        for (int i = index; i <choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
    }

    private void HidingChoices() {
        for (int i = 0; i < choices.Length; i++) {
            choices[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator SelectFirstChoice() {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int choiceIndex) {
        currentStory.ChooseChoiceIndex(choiceIndex);
        choicesIndexes += ("" + choiceIndex);
    }

    public IEnumerator FinishFadeInTransition() {

        yield return new WaitForSeconds(FADEIN_TRANSITION_TIME);

        isFadeInTransition = false;
    }

    public void FadeInTransition() {
        Color panelColor = transitionPanel.GetComponent<Image>().color;
        panelColor = Color.Lerp(
            panelColor,
            new Color(
                panelColor.r,
                panelColor.g,
                panelColor.b,
                panelColor.a - 0.5f),
            4f * Time.deltaTime);
        transitionPanel.GetComponent<Image>().color = panelColor;
    }
    public void FadeOutTransition() {
        Color panelColor = transitionPanel.GetComponent<Image>().color;
        panelColor = Color.Lerp(
            panelColor,
            new Color(
                panelColor.r,
                panelColor.g,
                panelColor.b,
                panelColor.a + 0.5f),
            4f * Time.deltaTime);
        transitionPanel.GetComponent<Image>().color = panelColor;
    }
}
