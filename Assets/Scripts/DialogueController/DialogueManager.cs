using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private Image characterImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    private TextMeshProUGUI[] choicesText;

    private Story currentStory;

    public bool dialogueIsPlaying { get; private set; }

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one DialogueManager in the scene");
        }
        instance = this;
    }
    public static DialogueManager GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        characterImage.enabled = false;

        choicesText = new TextMeshProUGUI[choices.Length];
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {
        if(!dialogueIsPlaying)
        {
            return;
        }
        if ((Input.GetKeyDown(KeyCode.Space)) || (Input.GetKeyDown(KeyCode.KeypadEnter)))
        {
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkJSON)
    {
        currentStory = new Story(inkJSON.text);
        dialogueIsPlaying = true;
        dialogueBox.SetActive(true);
        characterImage.enabled = true;

        ContinueStory();
    }

    public IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);

        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        characterImage.enabled = false;
        dialogueText.text = "";
        nameText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // set text for the current dialogue line
            String stringText = currentStory.Continue();

            char[] separators = { '[', ']' };
            String[] strlist = stringText.Split(separators, 3, StringSplitOptions.None);

            if (strlist.Length == 3)
            {
                char[] charNameSeparators = { '_' };
                String[] charNamelist = strlist[1].Split(charNameSeparators, 3, StringSplitOptions.None);

                if (charNamelist.Length == 2)
                {
                    nameText.text = charNamelist[0];
                } else
                {
                    nameText.text = strlist[1];
                }
                characterImage.sprite = CharacterImageManager.GetInstance().ChangeCharacterImage(strlist[1]);
                dialogueText.text = strlist[2];
            }
            else
            {
                characterImage.sprite = CharacterImageManager.GetInstance().ChangeCharacterImage("");
                nameText.text = "";
                dialogueText.text = " " + stringText;
            }
            // display choices, if any, for this dialogue line
            DisplayChoices();
            
        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }
    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;

        if(currentChoices.Count > choices.Length)
        {
            Debug.LogError("More choices were given than the UI can support. Number of choices given: "
                + currentChoices.Count);
        }

        int index = 0;
        // enable and initialize the choices up to the amount of choices for this line of dialogue.
        foreach(Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;
            index++;
        }
        // go through the remaining choices the UI supports and make sure they're hidden.
        for (int i = index; i <choices.Length; i++)
        {
            choices[i].gameObject.SetActive(false);
        }
    }
}
