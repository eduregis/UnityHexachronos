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

public class CharStatsManager : MonoBehaviour
{
    public List<BasicCharInfo> characters;
    private static CharStatsManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CharStatsManager in the scene");
        }
        instance = this;
    }
    public static CharStatsManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public BasicCharacterStats GetBasicStats(CharacterIdentifier identifier)
    {
        foreach (BasicCharInfo character in characters)
        {
            if(character.characterIdentifier == identifier)
            {
                return character.characterStats;
            }
        }
        return characters[0].characterStats;
    }
}
