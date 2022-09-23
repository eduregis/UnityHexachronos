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
    public Sprite Morya_Portrait;
    public Sprite BasicSoldier_Portrait;

    [Header("CharacterIdle")]
    public Sprite Luca_Idle;
    public Sprite Sam_Idle;
    public Sprite Borell_Idle;
    public Sprite Salvato_Idle;
    public Sprite Billy_Idle;
    public Sprite Dandara_Idle;
    public Sprite Morya_Idle;
    public Sprite BasicSoldier_Idle;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one CharacterCombatSpriteManager in the scene");
        }
        instance = this;
    }

    public static CharacterCombatSpriteManager GetInstance() {
        return instance;
    }

    public Sprite CharacterPortraitImage(String imageText) {
        return imageText switch {
            "Luca" => Luca_Portrait,
            "Sam" => Sam_Portrait,
            "Borell" => Borell_Portrait,
            "Salvato" => Salvato_Portrait,
            "Billy" => Billy_Portrait,
            "Dandara" => Dandara_Portrait,
            "Morya" => Morya_Portrait,
            "Basic Soldier" => BasicSoldier_Portrait,
            "Basic Lieutenant" => BasicSoldier_Portrait,
            _ => Luca_Idle,
        };
    }

    public Sprite CharacterSpriteIdleImage(String imageText) {
        return imageText switch {
            "Luca" => Luca_Idle,
            "Sam" => Sam_Idle,
            "Borell" => Borell_Idle,
            "Salvato" => Salvato_Idle,
            "Billy" => Billy_Idle,
            "Dandara" => Dandara_Idle,
            "Morya" => Morya_Idle,
            "Basic Soldier" => BasicSoldier_Idle,
            "Basic Lieutenant" => BasicSoldier_Idle,
            _ => Luca_Idle,
        };
    }
}
