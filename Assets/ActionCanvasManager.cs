using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ActionCanvasManager : MonoBehaviour
{
    public GameObject actionCanvas;

    public GameObject overlay;
    public GameObject charPrefab;
    public GameObject textPrefab;
    List<GameObject> instances = new();

    public Transform hero1Position;
    public Transform hero2Position;
    public Transform hero3Position;
    public Transform enemy1Position;
    public Transform enemy2Position;
    public Transform enemy3Position;
    GameObject canvas;

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

    public void TriggerAction() {
        actionCanvas.SetActive(true);
    }

    public void TriggerEnemySingleTargetAction(string hero_name, string enemy_name, List<string> messages) {
        actionCanvas.SetActive(true);

        ShowHero(hero_name, hero2Position.transform.position);

        ShowEnemy(enemy_name, enemy2Position.transform.position);

        ShowMultipleMessages(messages, enemy2Position.transform.position);
    }

    public void TriggerAllEnemiesAction(string hero_name, string enemy1_name, string enemy2_name, string enemy3_name, 
        List<string> enemy1messages, List<string> enemy2messages, List<string> enemy3messages) {

        ShowHero(hero_name, hero2Position.transform.position);

        if (enemy1_name != "") ShowEnemy(enemy1_name, enemy1Position.transform.position);
        if (enemy2_name != "") ShowEnemy(enemy2_name, enemy2Position.transform.position);
        if (enemy3_name != "") ShowEnemy(enemy3_name, enemy3Position.transform.position);

        if (enemy1_name != "") ShowMultipleMessages(enemy1messages, enemy1Position.transform.position);
        if (enemy2_name != "") ShowMultipleMessages(enemy2messages, enemy2Position.transform.position);
        if (enemy3_name != "") ShowMultipleMessages(enemy3messages, enemy3Position.transform.position);
    }

    private void ShowHero(string hero_name, Vector3 position) {
        GameObject heroSprite = Instantiate(charPrefab, position, Quaternion.identity);
        heroSprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero_name);
        instances.Add(heroSprite);
    }

    private void ShowEnemy(string enemy_name, Vector3 position) {
        GameObject enemySprite = Instantiate(charPrefab, position, Quaternion.identity);
        enemySprite.GetComponent<SpriteRenderer>().flipX = true;
        enemySprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemy_name);
        instances.Add(enemySprite);
    }

    private void ShowMultipleMessages(List<string> messages, Vector3 position) {
        float distance = 0.5f;

        foreach (string msg in messages) {
            GameObject damageEnemySprite = Instantiate(
            textPrefab, new Vector3((float)position.x, (float)position.y + 3.5f + distance, (float)position.z), Quaternion.identity);
            damageEnemySprite.GetComponent<TextMeshProUGUI>().text = msg;
            damageEnemySprite.GetComponent<Transform>().localScale = new(0.01f, 0.01f, 1f);
            damageEnemySprite.transform.SetParent(canvas.transform);
            instances.Add(damageEnemySprite);

            distance += 0.5f;
        }
    }

    public void DismissAction() {
        actionCanvas.SetActive(false);

        foreach(GameObject inst in instances) {
            Destroy(inst);
        }

        instances.Clear();
    }
}
