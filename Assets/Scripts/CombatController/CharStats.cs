using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterIdentifier
{
    Luca = 0,
    Borell = 1,
    Sam = 2,
    Salvato = 3,
    Billy = 4,
    Dandara = 5,
    Sniper = 6,
    BasicSoldier = 7,
    BasicLiutenant = 8
}

[System.Serializable]
public class BasicCharInfo
{
    public BasicCharacterStats characterStats;
    public CharacterIdentifier characterIdentifier;
}

public class CharStats : MonoBehaviour
{
    public List<BasicCharInfo> characters;
    private static CharStats instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CharStatsManager in the scene");
        }
        instance = this;
    }
    public static CharStats GetInstance()
    {
        return instance;
    }

    private void Start()
    {
        characters = new List<BasicCharInfo>();
    }

    public BasicCharacterStats GetBasicStats(CharacterIdentifier identifier)
    {
        foreach (BasicCharInfo character in characters)
        {
            if(character.characterIdentifier == identifier)
            {
                Debug.Log(character.characterStats.strength);
                return character.characterStats;
            }
        }
        return characters[0].characterStats;
    }
}
