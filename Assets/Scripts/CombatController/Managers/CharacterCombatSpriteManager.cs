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
    public Sprite Salvato_Portrait;
    public Sprite Billy_Portrait;
    public Sprite Dandara_Portrait;
    public Sprite Sniper_Portrait;
    public Sprite BasicSoldier_Portrait;

    [Header("CharacterIdle")]
    public Sprite Luca_Idle;
    public Sprite Sam_Idle;
    public Sprite Borell_Idle;
    public Sprite Salvato_Idle;
    public Sprite Billy_Idle;
    public Sprite Dandara_Idle;
    public Sprite Sniper_Idle;
    public Sprite BasicSoldier_Idle;

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

    public Sprite CharacterPortraitImage(String imageText)
    {
        switch (imageText)
        {
            case "Luca":
                return Luca_Portrait;
            case "Sam":
                return Sam_Portrait;
            case "Borell":
                return Borell_Portrait;
            case "Salvato":
                return Salvato_Portrait;
            case "Billy":
                return Billy_Portrait;
            case "Dandara":
                return Dandara_Portrait;
            case "Sniper":
                return Sniper_Portrait;
            case "Basic Soldier":
                return BasicSoldier_Portrait;
            case "Basic Lieutenant":
                return BasicSoldier_Portrait;
            default:
                return Luca_Idle;
        }
    }

    public Sprite CharacterSpriteIdleImage(String imageText)
    {
        switch (imageText)
        {
            case "Luca":
                return Luca_Idle;
            case "Sam":
                return Sam_Idle;
            case "Borell":
                return Borell_Idle;
            case "Salvato":
                return Salvato_Idle;
            case "Billy":
                return Billy_Idle;
            case "Dandara":
                return Dandara_Idle;
            case "Sniper":
                return Sniper_Idle;
            case "Basic Soldier":
                return BasicSoldier_Idle;
            case "Basic Lieutenant":
                return BasicSoldier_Idle;
            default:
                return Luca_Idle;
        }
    }
}
