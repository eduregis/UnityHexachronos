using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CombatAnimationManager : MonoBehaviour
{
    [Header("CharacterSprites")]
    [SerializeField] private GameObject overlay;
    [SerializeField] private GameObject[] heroesSprites;
    [SerializeField] private GameObject[] enemiesSprites;

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
                heroesSprites[1].GetComponent<Image>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(heroName);
                string enemyName = CombatCharManager.GetInstance().GetCharNameByIndex(targetIndex, true);
                enemiesSprites[1].GetComponent<Image>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemyName);
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
                    heroesSprites[0].SetActive(false);
                    heroesSprites[1].SetActive(true);
                    heroesSprites[2].SetActive(false);
                    foreach (GameObject enemies in enemiesSprites)
                    {
                        enemies.SetActive(false);
                    }
                    break;
                case AffectType.EnemyTarget:
                    heroesSprites[0].SetActive(false);
                    heroesSprites[1].SetActive(true);
                    heroesSprites[2].SetActive(false);

                    enemiesSprites[0].SetActive(false);
                    enemiesSprites[1].SetActive(true);
                    enemiesSprites[2].SetActive(false);
                    break;
                case AffectType.AllEnemies:
                    heroesSprites[0].SetActive(false);
                    heroesSprites[1].SetActive(true);
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
        }
    }
}
