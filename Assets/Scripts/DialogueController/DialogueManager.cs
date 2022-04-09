using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{

    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialogueBox;
    [SerializeField] private TextMeshProUGUI dialogueText;

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
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            // Fazer a divisão pra definir a imagem do dialogo
            dialogueText.text = currentStory.Continue();
        }
        else
        {
            ExitDialogueMode();
        }
    }
}
