using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CombatAnimationManager : MonoBehaviour
{
    [Header("CharacterSprites")]
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject[] heroesSprites;
    [SerializeField] private GameObject[] enemiesSprites;
    [SerializeField] private TextMeshProUGUI[] damageTexts;

    // Animation control variables
    private float screenTimer = 0;
    private AffectType affectType = AffectType.Self;
    private List<string> listEffects = new List<string>();
    private bool isEnemyAux = false;

    private int characterIndexAux = 0;
    private int targetIndexAux = 0;

    private static CombatAnimationManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CombatAnimationManager in the scene");
        }
        instance = this;
    }
    public static CombatAnimationManager GetInstance()
    {
        return instance;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    // CombatAnimationManager.GetInstance().ActiveScreen()

    public void ActiveScreen(List<string> effects, int characterIndex, int targetIndex, AffectType type, bool isEnemy)
    {
        affectType = type;
        isEnemyAux = isEnemy;
        characterIndexAux = characterIndex;
        targetIndexAux = targetIndex;

        switch (affectType)
        {
            case AffectType.Self:
                for (int i = 0; i < effects.Count; i++)
                {
                    damageTexts[i].text = effects[i].ToString();
                    if (isEnemy)
                    {
                        string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, true);
                        enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                        damageTexts[i].transform.position = new Vector3(enemiesSprites[0].transform.position.x + 100f, enemiesSprites[0].transform.position.y  + 350.0f + (i * 50.0f), 1);
                    }
                    else
                    {
                        string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, false);
                        heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                        damageTexts[i].transform.position = new Vector3(heroesSprites[0].transform.position.x + 100f, heroesSprites[0].transform.position.y + 350.0f + (i * 50.0f), 1);
                    }
                }
                break;
            case AffectType.EnemyTarget:
                for (int i = 0; i < effects.Count; i++)
                {
                    int random_x = Random.Range(0, 200);
                    int random_y = Random.Range(0, 20);
                    damageTexts[i].text = effects[i].ToString();
                    if (isEnemy)
                    {
                        string heroName = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, false);
                        heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(heroName);
                        string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, true);
                        enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                        damageTexts[i].transform.position = new Vector3(enemiesSprites[0].transform.position.x + random_x + 200f, enemiesSprites[0].transform.position.y + random_y + 350.0f + (i* 50.0f), 1);
                    }
                    else
                    {
                        string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, false);
                        heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                        string heroName = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, true);
                        enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(heroName);
                        damageTexts[i].transform.position = new Vector3(heroesSprites[0].transform.position.x - random_x + 200f, heroesSprites[0].transform.position.y + random_y + 350.0f + (i * 50.0f), 1);
                    }
                }
                break;
            case AffectType.AllyTarget:
                for (int i = 0; i < effects.Count; i++)
                {
                    int random_x = Random.Range(0, 200);
                    int random_y = Random.Range(0, 20);
                    damageTexts[i].text = effects[i].ToString();
                    if (isEnemy)
                    {
                        if (characterIndex == targetIndex)
                        {
                            string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, true);
                            enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                            damageTexts[i].transform.position = new Vector3(enemiesSprites[0].transform.position.x + random_x + 200f, enemiesSprites[0].transform.position.y + random_y + 350.0f + (i * 50.0f), 1);
                        } else
                        {
                            string hero1Name = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, true);
                            enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero1Name);
                            string hero2Name = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, true);
                            enemiesSprites[1].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero2Name);
                            damageTexts[i].transform.position = new Vector3(enemiesSprites[1].transform.position.x + random_x + 200f, enemiesSprites[1].transform.position.y + random_y + 350.0f + (i * 50.0f), 1);
                        }
                    }
                    else
                    {
                        if (characterIndex == targetIndex)
                        {
                            Debug.Log("aaaaaa");
                            heroesSprites[1].SetActive(false);
                            string heroName = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, false);
                            heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(heroName);
                            damageTexts[i].transform.position = new Vector3(heroesSprites[0].transform.position.x + random_x + 200f, heroesSprites[0].transform.position.y + random_y + 350.0f + (i * 50.0f), 1);
                        }
                        else
                        {
                            string hero1Name = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, false);
                            heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero1Name);
                            string hero2Name = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, false);
                            heroesSprites[1].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero2Name);
                            damageTexts[i].transform.position = new Vector3(heroesSprites[1].transform.position.x + random_x + 200f, heroesSprites[1].transform.position.y + random_y + 350.0f + (i * 50.0f), 1);
                        }
                    }
                }
                break;
            case AffectType.AllEnemies:
                break;
            case AffectType.AllAllies:
                break;
        }

        screenTimer = 1.5f;
    }

    // Update is called once per frame
    void Update()
    {
        TriggerScreen();
    }

    public void SetAffectType (AffectType type)
    {
        affectType = type;
    }
    public AffectType GetAffectType()
    {
        return affectType;
    }

    private void TriggerScreen()
    {
        if (screenTimer > 0)
        {
            screenTimer -= Time.deltaTime;

            overlay.SetActive(true);

            switch(affectType)
            {
                case AffectType.Self:
                    if (isEnemyAux)
                    {
                        foreach (GameObject hero in heroesSprites)
                        {
                            hero.SetActive(false);
                        }
                        enemiesSprites[0].SetActive(true);
                        enemiesSprites[1].SetActive(false);
                        enemiesSprites[2].SetActive(false);
                    } else
                    {
                        heroesSprites[0].SetActive(true);
                        heroesSprites[1].SetActive(false);
                        heroesSprites[2].SetActive(false);
                        foreach (GameObject enemies in enemiesSprites)
                        {
                            enemies.SetActive(false);
                        }
                    }
                    break;
                case AffectType.EnemyTarget:
                    heroesSprites[0].SetActive(true);
                    heroesSprites[1].SetActive(false);
                    heroesSprites[2].SetActive(false);

                    enemiesSprites[0].SetActive(true);
                    enemiesSprites[1].SetActive(false);
                    enemiesSprites[2].SetActive(false);
                    break;
                case AffectType.AllyTarget:
                    if (isEnemyAux)
                    {
                        foreach (GameObject hero in heroesSprites)
                        {
                            hero.SetActive(false);
                        }
                        enemiesSprites[0].SetActive(true);
                        enemiesSprites[1].SetActive(characterIndexAux != targetIndexAux);
                        enemiesSprites[2].SetActive(false);
                    }
                    else
                    {
                        heroesSprites[0].SetActive(true);
                        heroesSprites[1].SetActive(characterIndexAux != targetIndexAux);
                        heroesSprites[2].SetActive(false);

                        foreach (GameObject enemies in enemiesSprites)
                        {
                            enemies.SetActive(false);
                        }
                    }
                    break;
                case AffectType.AllEnemies:
                    heroesSprites[0].SetActive(true);
                    heroesSprites[1].SetActive(false);
                    heroesSprites[2].SetActive(false);

                    enemiesSprites[0].SetActive(true);
                    enemiesSprites[1].SetActive(true);
                    enemiesSprites[2].SetActive(true);
                    break;
                case AffectType.AllAllies:
                    foreach (GameObject hero in heroesSprites)
                    {
                        hero.SetActive(false);
                    }

                    foreach (GameObject enemies in enemiesSprites)
                    {
                        enemies.SetActive(false);
                    }
                    break;
            }

            foreach (TextMeshProUGUI text in damageTexts)
            {
                text.transform.position = Vector3.Lerp(text.transform.position, text.transform.position + new Vector3(0, 200, 0), 0.3f * Time.deltaTime);

                text.alpha = Mathf.Lerp(text.alpha, 0, 3.5f * Time.deltaTime);
            }

        } else
        {
            overlay.SetActive(false);

            foreach (GameObject hero in heroesSprites)
            {
                hero.SetActive(false);
            }
            foreach (GameObject enemies in enemiesSprites)
            {
                enemies.SetActive(false);
            }
            foreach(TextMeshProUGUI text in damageTexts)
            {
                text.alpha = 1;
                text.text = "";
            }
        }
    }
}
