using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterBuffsSpriteManager : MonoBehaviour { 

    private static CharacterBuffsSpriteManager instance;
   
    [Header("Buffs Images")]
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
    public Sprite stunned;
    public Sprite bleeding;
    public Sprite taunt;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one BuffIconManager in the scene");
        }
        instance = this;
    }

    public static CharacterBuffsSpriteManager GetInstance() {
        return instance;
    }

    public Sprite BuffSpriteImage(BuffType type) {
        switch (type) {
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
            case BuffType.Stunned:
                return stunned;
            case BuffType.Bleeding:
                return bleeding;
            case BuffType.Taunt:
                return taunt;
            default:
                return attackUp;
        }
    }
}