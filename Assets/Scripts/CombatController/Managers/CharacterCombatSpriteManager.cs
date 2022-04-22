using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterCombatSpriteManager : MonoBehaviour
{
    private static CharacterCombatSpriteManager instance;

    [Header("CharacterPortraits")]
    public Sprite Luca_Portrait;
    public Sprite Sam_Portrait;
    public Sprite Borell_Portrait;
    public Sprite BasicSoldier_Portrait;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CharacterCombatSpriteManager in the scene");
        }
        instance = this;
    }
    public static CharacterCombatSpriteManager GetInstance()
    {
        return instance;
    }

    public Sprite CharacterSpriteImage(String imageText)
    {
        switch (imageText)
        {
            case "Luca_Portrait":
                return Luca_Portrait;
            case "Sam_Portrait":
                return Sam_Portrait;
            default:
                return Luca_Portrait;
        }
    }
}
