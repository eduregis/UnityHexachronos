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

    private void Start() {
        handler = DialogueBattleDataBridge.dialogueHandler;
    }

    void Update() {
        if (!DialogueManager.GetInstance().dialogueIsPlaying) {
            Debug.Log("Handler: " + handler);
            switch (handler) {
                case 0:
                    StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON1));
                    handler = 1;
                    break;
                case 1:
                    Debug.Log("Choices: " + DialogueManager.GetInstance().choicesIndexes);
                    DialogueManager.GetInstance().isFadeOutTransition = true;
                    StartCoroutine(LoadBattleScene("Luca", "Borell", "Sam", "BasicSoldier", "BasicSoldier", ""));
                    break;
                case 2:
                    StartCoroutine(DialogueManager.GetInstance().EnterDialogueMode(inkJSON2));
                    handler = 3;
                    break;
                default:
                    break;
            }
        }
    }

    public IEnumerator LoadBattleScene(string hero1Name, string hero2Name, string hero3Name, string enemy1Name, string enemy2Name, string enemy3Name) {

        yield return new WaitForSeconds(0.5f);

        DialogueBattleDataBridge.hero1_Name = hero1Name;
        DialogueBattleDataBridge.hero2_Name = hero2Name;
        DialogueBattleDataBridge.hero3_Name = hero3Name;
        DialogueBattleDataBridge.enemy1_Name = enemy1Name;
        DialogueBattleDataBridge.enemy2_Name = enemy2Name;
        DialogueBattleDataBridge.enemy3_Name = enemy3Name;

        DialogueBattleDataBridge.dialogueHandler = handler + 1;

        SceneManager.LoadScene("BattleScene");
    }

}
