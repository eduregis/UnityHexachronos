using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON1;
    [SerializeField] private TextAsset inkJSON2;

    private int handler = 0;

    // Update is called once per frame
    void Update() {
        if (!DialogueManager.GetInstance().dialogueIsPlaying) {
            switch(handler) {
                case 0:
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON1);
                    handler = 1;
                    break;
                case 1:
                    DialogueManager.GetInstance().EnterDialogueMode(inkJSON2);
                    handler = 2;
                    break;
                default:
                    Debug.Log(DialogueManager.GetInstance().choicesIndexes);
                    break;
            }
        }
    }

    public void LoadGame(string input) {
        SceneManager.LoadScene("BattleScene");
    }
}
