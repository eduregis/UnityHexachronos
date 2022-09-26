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

        GameObject hero1Sprite = Instantiate(charPrefab, hero2Position.transform.position, Quaternion.identity);
        hero1Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero_name);
        instances.Add(hero1Sprite);

        GameObject enemy1Sprite = Instantiate(charPrefab, enemy2Position.transform.position, Quaternion.identity);
        enemy1Sprite.GetComponent<SpriteRenderer>().flipX = true;
        enemy1Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemy_name);
        instances.Add(enemy1Sprite);

        float distance = 0.5f;
        
        foreach (string msg in messages) {
            GameObject damageEnemySprite = Instantiate(
            textPrefab,
            new Vector3(
                (float)enemy2Position.transform.position.x,
                (float)enemy2Position.transform.position.y + 3.5f + distance,
                (float)enemy2Position.transform.position.z),
            Quaternion.identity);
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
