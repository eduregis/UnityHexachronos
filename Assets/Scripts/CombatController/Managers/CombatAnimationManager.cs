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
        switch (affectType)
        {
            case AffectType.Self:
                break;
            case AffectType.EnemyTarget:
                string heroName = CombatCharManager.GetInstance().GetCharNameByIndex(characterIndex, false);
                heroesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(heroName);
                string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, true);
                enemiesSprites[0].GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
                
                for (int i = 0; i < effects.Count; i++)
                {
                    int random_x = Random.Range(0, 100);
                    int random_y = Random.Range(0, 100);
                    damageTexts[i].text = effects[i].ToString();
                    if (isEnemy)
                    {
                        damageTexts[i].transform.position = new Vector3(enemiesSprites[0].transform.position.x + random_x, enemiesSprites[0].transform.position.y + random_y + 400.0f, 1);
                    }
                    else
                    {
                        damageTexts[i].transform.position = new Vector3(heroesSprites[0].transform.position.x - random_x, heroesSprites[0].transform.position.y + random_y + 400.0f, 1);
                    }

                    
                }
                
                break;
            case AffectType.AllyTarget:
                break;
            case AffectType.AllEnemies:
                break;
            case AffectType.AllAllies:
                break;
        }

        screenTimer = 1f;
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
                    
                    heroesSprites[0].SetActive(true);
                    heroesSprites[1].SetActive(false);
                    heroesSprites[2].SetActive(false);
                    foreach (GameObject enemies in enemiesSprites)
                    {
                        enemies.SetActive(false);
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
                case AffectType.AllEnemies:
                    heroesSprites[0].SetActive(true);
                    heroesSprites[1].SetActive(false);
                    heroesSprites[2].SetActive(false);

                    enemiesSprites[0].SetActive(true);
                    enemiesSprites[1].SetActive(true);
                    enemiesSprites[2].SetActive(true);
                    break;
                case AffectType.AllyTarget:
                    heroesSprites[0].SetActive(true);
                    heroesSprites[1].SetActive(true);
                    heroesSprites[2].SetActive(false);

                    foreach (GameObject enemies in enemiesSprites)
                    {
                        enemies.SetActive(false);
                    }
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

                text.alpha = Mathf.Lerp(text.alpha, 0, 5f * Time.deltaTime);
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
