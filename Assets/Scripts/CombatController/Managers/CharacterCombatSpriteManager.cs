using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterCombatSpriteManager : MonoBehaviour
{
    private static CharacterCombatSpriteManager instance;

    public Sprite Luca_Portrait;
    public Sprite Sam_Portrait;
    public Sprite Borell_Portrait;
    public Sprite BasicSoldier_Portrait;

    public Sprite attackUp;
    public Sprite attackDown;
    public Sprite defenseUp;
    public Sprite defenseDown;
    public Sprite critRateUp;
    public Sprite critRateDown;
    public Sprite critDamageUp;
    public Sprite critDamageDown;
    public Sprite hitRateUp;
    public Sprite hitRateDown;
    public Sprite evasionUp;
    public Sprite evasionDown;

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

    public Sprite BuffSpriteImage(BuffType type)
    {
        switch (type)
        {
            case BuffType.DamageUp:
                return attackUp;
            case BuffType.DamageDown:
                return attackDown;
            case BuffType.DefenseUp:
                return defenseUp;
            case BuffType.DefenseDown:
                return defenseDown;
            case BuffType.CritRateUp:
                return critRateUp;
            case BuffType.CritRateDown:
                return critRateDown;
            case BuffType.CritDamageUp:
                return critDamageUp;
            case BuffType.CritDamageDown:
                return critDamageDown;
            case BuffType.HitRateUp:
                return hitRateUp;
            case BuffType.HitRateDown:
                return hitRateDown;
            case BuffType.EvasionUp:
                return evasionUp;
            case BuffType.EvasionDown:
                return evasionDown;
            default:
                return attackUp;
        }
    }
}
