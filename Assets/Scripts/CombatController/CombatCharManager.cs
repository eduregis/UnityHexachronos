using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterInfo
{
    // basic status
    public int strength;
    public int intelligence;
    public int vitality;
    public int technique;
    public int agility;
    public int luck;
    public int level;
    // advanced status
    public int maxLife;
    public int damage;
    public int hitRate;
    public int evasionRate;
    public int APSlots;
    public int critRate;
    public int critDamage;
}
public class CombatCharManager : MonoBehaviour
{

    [Header("CharacterSprites")]
    [SerializeField] private GameObject[] heroesSprites;
    [SerializeField] private GameObject[] enemiesSprites;

    private List<CharacterInfo> heroes;
    private List<CharacterInfo> enemies;

    // Start is called before the first frame update
    void Start()
    {
        heroes = new List<CharacterInfo>();
        enemies = new List<CharacterInfo>();

        SetupCharacters();

        Debug.Log(heroes.Count);
        
    }

    public void SetupCharacters()
    {
        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.Luca), true);
        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.Sam), true);
        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.Borell), true);

        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
        CreateCharacter(CharStats.GetInstance().GetBasicStats(CharacterIdentifier.BasicSoldier), false);
    }
    private void CreateCharacter(BasicCharacterStats basicStats, bool isHero)
    {
        CharacterInfo characterInfo = new CharacterInfo();
        
        characterInfo.strength = basicStats.strength;
        characterInfo.intelligence = basicStats.intelligence;
        characterInfo.vitality = basicStats.vitality;
        characterInfo.technique = basicStats.technique;
        characterInfo.agility = basicStats.agility;
        characterInfo.luck = basicStats.luck;
        characterInfo.level = basicStats.level;

        characterInfo.maxLife = ((basicStats.vitality + basicStats.level ) * 5);
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
