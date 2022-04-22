using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterInfo
{
    public string char_name;
    // basic status
    public int strength;
    public int intelligence;
    public int vitality;
    public int technique;
    public int agility;
    public int luck;
    public int level;
    // advanced status
    public int life;
    public int maxLife;
    public int damage;
    public int hitRate;
    public int evasionRate;
    public int APSlots;
    public int critRate;
    public int critDamage;

    public List<CharacterSkill> skillList;
}
public class CombatCharManager : MonoBehaviour
{

    [Header("CharacterSprites")]
    [SerializeField] private GameObject[] heroesSprites;
    [SerializeField] private GameObject[] heroesTargets;
    [SerializeField] private GameObject[] enemiesSprites;
    [SerializeField] private GameObject[] enemiesTargets;

    [Header("CharacterLifebars")]
    [SerializeField] private GameObject[] heroesHUD;
    [SerializeField] private Image[] heroesPortraits;
    [SerializeField] private Image[] heroesFullLifebars;
    [SerializeField] private Image[] heroesDamageLifebars;
    [SerializeField] private Image[] heroesEmptyLifebars;
    [SerializeField] private Image[] enemiesPortraits;
    [SerializeField] private Image[] enemiesFullLifebars;
    [SerializeField] private Image[] enemiesDamageLifebars;
    [SerializeField] private Image[] enemiesEmptyLifebars;

    public List<CharacterInfo> heroes;
    public List<CharacterInfo> enemies;

    private int heroesIndex = 0;
    private bool playerTurn = true;

    private static CombatCharManager instance;

    // Animation control variables
    private float damageLifeShrinkTimer = 0;
    private float gainLifeShrinkTimer = 0;
    private float rotateCharTimer = 0;
    private float hudAnimateTimer = 0;

    Vector3 pos1;
    Vector3 pos2;
    Vector3 pos3;
    Vector3 scaleMain = new Vector3(120,120,1);
    Vector3 scaleNormal = new Vector3(100,100,1);

    Vector3 HUDMain = new Vector3(1.7f, 1.7f, 1);
    Vector3 HUDNormal = new Vector3(1.5f, 1.5f, 1);
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CombatCharManager in the scene");
        }
        instance = this;
    }
    public static CombatCharManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        heroes = new List<CharacterInfo>();
        enemies = new List<CharacterInfo>();

        SetupCharacters();

        UpdateUI();
        
    }

    public void SetupCharacters()
    {
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Luca), true);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Sam), true);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Borell), true);

        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        //CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
    }
    private void CreateCharacter(BasicCharacterStats basicStats, bool isHero)
    {
        CharacterInfo characterInfo = new CharacterInfo();
        
        characterInfo.char_name = basicStats.char_name;

        characterInfo.strength = basicStats.strength;
        characterInfo.intelligence = basicStats.intelligence;
        characterInfo.vitality = basicStats.vitality;
        characterInfo.technique = basicStats.technique;
        characterInfo.agility = basicStats.agility;
        characterInfo.luck = basicStats.luck;
        characterInfo.level = basicStats.level;
        characterInfo.skillList = basicStats.skills;

        characterInfo.maxLife = ((basicStats.vitality + basicStats.level ) * 5);
        characterInfo.life = characterInfo.maxLife;
        characterInfo.damage = (basicStats.strength + (basicStats.technique / 2) + (basicStats.level / 5));
        characterInfo.hitRate = (50 + basicStats.technique + (basicStats.agility / 2) + (basicStats.luck / 4));
        characterInfo.evasionRate = ((basicStats.agility / 3) + (basicStats.luck / 3) + (basicStats.intelligence / 3));
        characterInfo.APSlots = ((basicStats.technique / 5) + (basicStats.level / 7) - 1);
        characterInfo.critRate = (5 + (basicStats.luck / 2));
        characterInfo.critDamage = 50;

        if (isHero) {
            heroes.Add(characterInfo);
        } else
        {
            enemies.Add(characterInfo);
        }
    }

    private void UpdateUI()
    {
        for (int i = heroes.Count; i < heroesSprites.Length; i++)
        {
            heroesSprites[i].SetActive(false);
            heroesTargets[i].SetActive(false);
            heroesDamageLifebars[i].enabled = false;
            heroesFullLifebars[i].enabled = false;
            heroesEmptyLifebars[i].enabled = false;
            heroesPortraits[i].enabled = false;
        }
        for (int i = enemies.Count; i < enemiesSprites.Length; i++)
        {
            enemiesSprites[i].SetActive(false);
            enemiesTargets[i].SetActive(false);
            enemiesDamageLifebars[i].enabled = false;
            enemiesFullLifebars[i].enabled = false;
            enemiesEmptyLifebars[i].enabled = false;
            enemiesPortraits[i].enabled = false;
        }
    }

    public void RotateCharacters()
    {
        ;

        pos1 = heroesSprites[0].transform.position;
        pos2 = heroesSprites[1].transform.position;
        pos3 = heroesSprites[2].transform.position;

        GoToNextCharacter();

        rotateCharTimer = 1f;
    }

    public CharacterInfo GetCurrentCharacter()
    {
        hudAnimateTimer = 1f;
        return heroes[heroesIndex];
    }

    public void GoToNextCharacter()
    {
        if (heroesIndex == heroes.Count - 1)
        {
            playerTurn = false;
            heroesIndex = 0;
        } else
        {
            playerTurn = true;
            heroesIndex++;
        }
    }
    
    public bool IsPlayerTurn()
    {
        return playerTurn;
    }

    public void SetPlayerTurn()
    {
        playerTurn = true;
    }
    public int GetNumberOfAllies()
    {
        return heroes.Count;
    }

    public int GetNumberOfEnemies()
    {
        return enemies.Count;
    }

    // Update is called once per frame
    void Update()
    {
        MovingSpriteCharsIfNeeded();
        AnimatingHUDIfNeeded();
        CheckingDamageBars();
        CheckingGainBars();
    }

    public void ShowEnemyTarget(int targetIndex)
    {
        for (int i = 0; i < enemiesTargets.Length; i++)
        {
            if (i == targetIndex) { enemiesTargets[i].SetActive(true); }
            else { enemiesTargets[i].SetActive(false); }
        }
    }

    public void ShowAllEnemiesTarget()
    {
        for (int i = 0; i < enemiesTargets.Length; i++)
        {
             enemiesTargets[i].SetActive(true); 
        }
    }

    public void ShowAllyTarget(int targetIndex)
    {
        for (int i = 0; i < heroesTargets.Length; i++)
        {
            if (i == targetIndex) { heroesTargets[i].SetActive(true); }
            else { heroesTargets[i].SetActive(false); }
        }
    }

    public void ShowAllAlliesTarget()
    {
        for (int i = 0; i < heroesTargets.Length; i++)
        {
            heroesTargets[i].SetActive(true);
        }
    }

    public void HideAllTargets()
    {
        for (int i = 0; i < enemiesTargets.Length; i++)
        {
            enemiesTargets[i].SetActive(false);
        }
    }

    public void BasicAttack(CharacterInfo attackChar, int index, bool isEnemy)
    {
        LoseHP(attackChar, attackChar.damage, index, isEnemy);
    }


    public void GainHP(int hpGain, int index, bool isEnemy)
    {
        if (isEnemy)
        {
            if (enemies[index].life + hpGain > enemies[index].maxLife) { enemies[index].life = enemies[index].maxLife; }
            else { enemies[index].life += hpGain; }
            enemiesDamageLifebars[index].fillAmount = Mathf.Clamp(((float)enemies[index].life / (float)enemies[index].maxLife), 0, 1f);
            gainLifeShrinkTimer = 1f;
        }
        else
        {
            if (heroes[index].life + hpGain > heroes[index].maxLife) { heroes[index].life = heroes[index].maxLife; }
            else { heroes[index].life += hpGain; }
            heroesDamageLifebars[index].fillAmount = Mathf.Clamp(((float)heroes[index].life / (float)heroes[index].maxLife), 0, 1f);
            gainLifeShrinkTimer = 1f;
        }
    }
    public void LoseHP(CharacterInfo attackChar, int basicDamage, int index, bool isEnemy)
    {
        System.Random rnd = new System.Random();

        int hitRate = rnd.Next(1, 101);
        int evasionRate = rnd.Next(1, 101);
        int critRate = rnd.Next(1, 101);

        int damage = basicDamage;

        if (critRate < attackChar.critRate)
        {
            damage = damage + (damage * (int)((float)attackChar.critDamage) / 100);
        }

        if (isEnemy)
        {
            if ((hitRate < attackChar.hitRate) || (evasionRate > attackChar.evasionRate)) {
                if (enemies[index].life - damage < 0) { enemies[index].life = 0; }
                else { enemies[index].life -= damage; }
                enemiesFullLifebars[index].fillAmount = Mathf.Clamp(adjustHexagonBarPercentage((float)enemies[index].life, (float)enemies[index].maxLife), 0, 1f);
                damageLifeShrinkTimer = 1f;
                Debug.Log(attackChar.char_name + " causou " + damage + " (" + basicDamage + ") pontos de dano em " + enemies[index].char_name);
            } else
            {
                Debug.Log("Errou");
            }
        } else
        {
            if ((hitRate < attackChar.hitRate) || (evasionRate > attackChar.evasionRate))
            {
                if (heroes[index].life - damage < 0) { heroes[index].life = 0; }
                else { heroes[index].life -= damage; }
                heroesFullLifebars[index].fillAmount = Mathf.Clamp(adjustHexagonBarPercentage((float)heroes[index].life, (float)heroes[index].maxLife), 0, 1f);
                damageLifeShrinkTimer = 1f;
                Debug.Log(attackChar.char_name + " causou " + damage + " (" + basicDamage + ") pontos de dano em " + heroes[index].char_name);
            }
            else
            {
                Debug.Log("Errou");
            }
        }
    }

    public float adjustHexagonBarPercentage(float actualValue, float maxValue)
    {
        return (actualValue / maxValue);
    }

    public void MovingSpriteCharsIfNeeded()
    {
        if (rotateCharTimer > 0)
        {
            rotateCharTimer -= Time.deltaTime;

            if (heroes.Count == 2)
            {
                heroesSprites[0].transform.position = Vector3.Lerp(heroesSprites[0].transform.position, pos2, 7f * Time.deltaTime);
                heroesSprites[1].transform.position = Vector3.Lerp(heroesSprites[1].transform.position, pos1, 7f * Time.deltaTime);
            }
            else if (heroes.Count == 3)
            {
                heroesSprites[0].transform.position = Vector3.Lerp(heroesSprites[0].transform.position, pos3, 7f * Time.deltaTime);
                heroesSprites[1].transform.position = Vector3.Lerp(heroesSprites[1].transform.position, pos1, 7f * Time.deltaTime);
                heroesSprites[2].transform.position = Vector3.Lerp(heroesSprites[2].transform.position, pos2, 7f * Time.deltaTime);
            }
            for (int i = 0; i < heroes.Count; i++)
            {
                if (i == heroesIndex)
                {
                    heroesSprites[i].transform.localScale = Vector3.Lerp(heroesSprites[i].transform.localScale, scaleMain, 4f * Time.deltaTime);
                    SpriteRenderer spr = heroesSprites[i].GetComponent<SpriteRenderer>();
                    spr.sortingOrder = 4;
                    spr.color = Color.Lerp(spr.color, Color.white, 4f * Time.deltaTime);
                }
                else
                {
                    heroesSprites[i].transform.localScale = Vector3.Lerp(heroesSprites[i].transform.localScale, scaleNormal, 4f * Time.deltaTime);
                    SpriteRenderer spr = heroesSprites[i].GetComponent<SpriteRenderer>();
                    spr.sortingOrder = 3;
                    spr.color = Color.Lerp(spr.color, Color.gray, 4f * Time.deltaTime);
                }
            }
        }
    }

    public void AnimatingHUDIfNeeded()
    {
        if (hudAnimateTimer > 0)
        {
            hudAnimateTimer -= Time.deltaTime;
            
            for (int i = 0; i < heroesHUD.Length; i++)
            {
                if (i == heroesIndex)
                {
                    heroesHUD[i].transform.localScale = Vector3.Lerp(heroesHUD[i].transform.localScale, HUDMain, 8f * Time.deltaTime);
                }
                else
                {
                    heroesHUD[i].transform.localScale = Vector3.Lerp(heroesHUD[i].transform.localScale, HUDNormal, 8f * Time.deltaTime);
                }
            }
        }
    }
    public void CheckingDamageBars()
    {
        if (damageLifeShrinkTimer > 0)
        {
            damageLifeShrinkTimer -= Time.deltaTime;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (enemiesFullLifebars[i].fillAmount < enemiesDamageLifebars[i].fillAmount)
                {
                    enemiesDamageLifebars[i].fillAmount -= 0.5f * Time.deltaTime;
                }
            }
            for (var i = 0; i < heroes.Count; i++)
            {
                if (heroesFullLifebars[i].fillAmount < heroesDamageLifebars[i].fillAmount)
                {
                    heroesDamageLifebars[i].fillAmount -= 0.5f * Time.deltaTime;
                }
            }
        } 
    }

    public void CheckingGainBars()
    {
        if (gainLifeShrinkTimer > 0)
        {
            gainLifeShrinkTimer -= Time.deltaTime;

            for (var i = 0; i < enemies.Count; i++)
            {
                if (enemiesFullLifebars[i].fillAmount < enemiesDamageLifebars[i].fillAmount)
                {
                    enemiesFullLifebars[i].fillAmount += 0.5f * Time.deltaTime;
                }
            }
            for (var i = 0; i < heroes.Count; i++)
            {
                if (heroesFullLifebars[i].fillAmount < heroesDamageLifebars[i].fillAmount)
                {
                    heroesFullLifebars[i].fillAmount += 0.5f * Time.deltaTime;
                }
            }
        }
    }
}
