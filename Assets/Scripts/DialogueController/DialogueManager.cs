using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private GameObject characterImage;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI nameText;

    private Story currentStory;

    private bool dialogueIsPlaying;

    private static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found morethan one DialogueManager in the scene");
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


        ContinueStory();
    }

    public void ExitDialogueMode()
    {
        dialogueIsPlaying = false;
        dialogueBox.SetActive(false);
        characterImage. SetActive(false);
        dialogueText.text = "";
        nameText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
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
                    // TODO: chamar método pra mudar imagem
                } else
                {
                    nameText.text = strlist[1];
                }
                dialogueText.text = strlist[2];
            }
            else
            {
                nameText.text = "";
                dialogueText.text = " " + stringText;
            }
            
            
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
