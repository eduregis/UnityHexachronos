using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        characters = new List<BasicCharInfo>();
    }

    public CharacterStats GetBasicStats(CharacterIdentifier identifier)
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
