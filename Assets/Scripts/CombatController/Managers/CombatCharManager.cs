using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image[] heroesFullLifebars;
    [SerializeField] private Image[] heroesDamageLifebars;
    [SerializeField] private Image[] enemiesFullLifebars;
    [SerializeField] private Image[] enemiesDamageLifebars;

    private List<CharacterInfo> heroes;
    private List<CharacterInfo> enemies;

    private int heroesIndex = 0;
    private bool playerTurn = true;

    private static CombatCharManager instance;

    private float damageLifeShrinkTimer = 1f;
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
        
    }

    public void SetupCharacters()
    {
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Luca), true);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Sam), true);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.Borell), true);

        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        CreateCharacter(CharStatsManager.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
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

    public CharacterInfo GetCurrentCharacter()
    {
        if (heroesIndex == heroes.Count) { heroesIndex = 0; }
        return heroes[heroesIndex++];
    }

    // Update is called once per frame
    void Update()
    {
        CheckingDamageBars();
    }

    public void ShowEnemyTarget(int targetIndex)
    {
        for (int i = 0; i < enemiesTargets.Length; i++)
        {
            if (i == targetIndex) { enemiesTargets[i].SetActive(true); }
            else { enemiesTargets[i].SetActive(false); }
        }
    } 

    public void LoseHP( int index, bool isEnemy)
    {
        if (isEnemy)
        {
            if (enemies[index].life - 30 < 0) { enemies[index].life = 0; }
            else { enemies[index].life -= 30; }
            enemiesFullLifebars[index].fillAmount = Mathf.Clamp(((float)enemies[index].life / (float)enemies[index].maxLife), 0, 1f);
            damageLifeShrinkTimer = 1f;
        } else
        {
            if (heroes[index].life - 30 < 0) { heroes[index].life = 0; }
            else { heroes[index].life -= 30; }
            heroesFullLifebars[index].fillAmount = Mathf.Clamp(((float)heroes[index].life / (float)enemies[index].maxLife), 0, 1f);
            damageLifeShrinkTimer = 1f;
        }
        
    }

    public void CheckingDamageBars()
    {
        if (damageLifeShrinkTimer > 0)
        {
            damageLifeShrinkTimer -= Time.deltaTime;
        } else
        {
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
}
