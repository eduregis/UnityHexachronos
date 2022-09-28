using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ActionCanvasManager : MonoBehaviour {

    #region External variables

    public GameObject actionCanvas;

    public GameObject overlay;
    public GameObject charPrefab;
    public GameObject textPrefab;

    public Transform hero1Position;
    public Transform hero2Position;
    public Transform hero3Position;
    public Transform enemy1Position;
    public Transform enemy2Position;
    public Transform enemy3Position;
    #endregion

    #region Control variables

    List<GameObject> instances = new();
    List<GameObject> messageInstances = new();
    GameObject canvas;

    #endregion

    #region Constants
    bool isRunning = false;

    Color debuffRed =      new Color(0.85f, 0.15f, 0.15f);
    Color buffCyan =       new Color(0.15f, 0.85f, 0.85f);
    Color greenHeal =      new Color(0.15f, 0.85f, 0.15f);
    Color magentaEnergy =  new Color(0.85f, 0.15f, 0.85f);
    Color orangeCritical = new Color(0.85f, 0.50f, 0.15f);
    Color yellowNormal =   new Color(0.85f, 0.85f, 0.15f);
    Color grayError =      new Color(0.85f, 0.85f, 0.85f);
    #endregion

    private static ActionCanvasManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one ActionCanvasManager in the scene");
        }
        instance = this;
    }

    public static ActionCanvasManager GetInstance()
    {
        return instance;
    }

    private void Start() {
        canvas  = GameObject.Find("TextCanvas");
        actionCanvas.SetActive(false);
    }

    private void Update() {
        if (isRunning) {
            AnimateTexts();
        }
    }

    public void TriggerAction() {
        actionCanvas.SetActive(true);
        isRunning = true;
    }

    #region Player Action Functions
    public void TriggerPlayerToEnemySingleTargetAction(string hero_name, string enemy_name, List<string> messages) {

        TriggerAction();

        ShowHero(hero_name, hero2Position.transform.position);
        ShowEnemy(enemy_name, enemy2Position.transform.position);

        ShowMultipleMessages(messages, enemy2Position.transform.position);
    }

    public void TriggerPlayerToAllEnemiesAction(string hero_name, string enemy1_name, string enemy2_name, string enemy3_name, 
        List<string> enemy1messages, List<string> enemy2messages, List<string> enemy3messages) {

        TriggerAction();

        ShowHero(hero_name, hero2Position.transform.position);

        if (enemy1_name != "") ShowEnemy(enemy1_name, enemy1Position.transform.position);
        if (enemy2_name != "") ShowEnemy(enemy2_name, enemy2Position.transform.position);
        if (enemy3_name != "") ShowEnemy(enemy3_name, enemy3Position.transform.position);

        if (enemy1_name != "") ShowMultipleMessages(enemy1messages, enemy1Position.transform.position);
        if (enemy2_name != "") ShowMultipleMessages(enemy2messages, enemy2Position.transform.position);
        if (enemy3_name != "") ShowMultipleMessages(enemy3messages, enemy3Position.transform.position);
    }

    public void TriggerPlayerToHeroSingleTargetAction(string hero1_name, string hero2_name, List<string> messages) {

        TriggerAction();

        ShowHero(hero1_name, hero1Position.transform.position);
        ShowHero(hero2_name, hero3Position.transform.position);

        ShowMultipleMessages(messages, hero3Position.transform.position);
    }

    public void TriggerPlayerToAllHeroesAction(string hero1_name, string hero2_name, string hero3_name,
        List<string> hero1messages, List<string> hero2messages, List<string> hero3messages) {

        TriggerAction();

        if (hero1_name != "") ShowHero(hero1_name, hero1Position.transform.position);
        if (hero2_name != "") ShowHero(hero2_name, hero2Position.transform.position);
        if (hero3_name != "") ShowHero(hero3_name, hero3Position.transform.position);

        if (hero1_name != "") ShowMultipleMessages(hero1messages, hero1Position.transform.position);
        if (hero2_name != "") ShowMultipleMessages(hero2messages, hero2Position.transform.position);
        if (hero3_name != "") ShowMultipleMessages(hero3messages, hero3Position.transform.position);
    }

    public void TriggerPlayerToHeroSelfTargetAction(string hero_name, List<string> messages) {

        TriggerAction();

        ShowHero(hero_name, hero2Position.transform.position);

        ShowMultipleMessages(messages, hero2Position.transform.position);
    }
    #endregion

    #region CPU Action Functions
    public void TriggerCPUToHeroSingleTargetAction(string enemy_name, string hero_name, List<string> messages) {

        TriggerAction();

        ShowEnemy(enemy_name, enemy2Position.transform.position);
        ShowHero(hero_name, hero2Position.transform.position);

        ShowMultipleMessages(messages, hero2Position.transform.position);
    }
    #endregion

    private void ShowHero(string hero_name, Vector3 position) {
        ShowCharacter(hero_name, position, false);
    }

    private void ShowEnemy(string enemy_name, Vector3 position) {
        ShowCharacter(enemy_name, position, true);
    }

    private void ShowCharacter(string char_name, Vector3 position, bool flip) {
        GameObject charSprite = Instantiate(charPrefab, position, Quaternion.identity);
        charSprite.GetComponent<SpriteRenderer>().flipX = flip;
        charSprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(char_name);
        instances.Add(charSprite);
    }

    private void ShowMultipleMessages(List<string> messages, Vector3 position) {
        float distance = 0.5f;

        foreach (string msg in messages) {
            GameObject damageEnemySprite = Instantiate(
            textPrefab, new Vector3((float)position.x, (float)position.y + 3.5f + distance, (float)position.z), Quaternion.identity);
            damageEnemySprite.GetComponent<TextMeshProUGUI>().color = TypeOfMessage((String)msg);
            damageEnemySprite.GetComponent<TextMeshProUGUI>().text = msg;
            damageEnemySprite.GetComponent<Transform>().localScale = new(0.01f, 0.01f, 1f);
            damageEnemySprite.transform.SetParent(canvas.transform);
            messageInstances.Add(damageEnemySprite);

            distance += 0.5f;
        }
    }

    private void AnimateTexts() {
        foreach (GameObject msg in messageInstances) {
            msg.transform.position = Vector3.Lerp(
                msg.transform.position, 
                new Vector3(
                    msg.transform.position.x, 
                    msg.transform.position.y + 0.5f, 
                    msg.transform.position.z), 
                1f * Time.deltaTime);

            Color msgColor = msg.GetComponent<TextMeshProUGUI>().color;
            msgColor = Color.Lerp(
                msgColor, 
                new Color(
                    msgColor.r, 
                    msgColor.g, 
                    msgColor.b, 
                    msgColor.a - 0.2f), 
                4f * Time.deltaTime);
            msg.GetComponent<TextMeshProUGUI>().color = msgColor;
        }
    }
    private Color TypeOfMessage(String msg) {
        if (msg.IndexOf("CRITICAL", StringComparison.OrdinalIgnoreCase) >= 0) return orangeCritical;
        if (msg.IndexOf("UP", StringComparison.OrdinalIgnoreCase) >= 0)       return buffCyan;
        if (msg.IndexOf("DOWN", StringComparison.OrdinalIgnoreCase) >= 0)     return debuffRed;
        if (msg.IndexOf("HEAL", StringComparison.OrdinalIgnoreCase) >= 0)     return greenHeal;
        if (msg.IndexOf("RECOVER", StringComparison.OrdinalIgnoreCase) >= 0)  return magentaEnergy;
        if (msg.IndexOf("LOSE", StringComparison.OrdinalIgnoreCase) >= 0)     return magentaEnergy;
        if (msg.IndexOf("MISS", StringComparison.OrdinalIgnoreCase) >= 0)     return grayError;
        if (msg.IndexOf("FAILED", StringComparison.OrdinalIgnoreCase) >= 0)   return grayError;
        return yellowNormal;
    }

    public void DismissAction() {
        actionCanvas.SetActive(false);
        isRunning = false;

        foreach(GameObject inst in instances) Destroy(inst);
        foreach(GameObject inst in messageInstances) Destroy(inst);

        instances.Clear();
        messageInstances.Clear();
    }
}
