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
        return type switch {
            BuffType.DamageUp => attackUp,
            BuffType.DamageDown => attackDown,
            BuffType.DefenseUp => defenseUp,
            BuffType.DefenseDown => defenseDown,
            BuffType.CritRateUp => critRateUp,
            BuffType.CritRateDown => critRateDown,
            BuffType.CritDamageUp => critDamageUp,
            BuffType.CritDamageDown => critDamageDown,
            BuffType.HitRateUp => hitRateUp,
            BuffType.HitRateDown => hitRateDown,
            BuffType.EvasionUp => evasionUp,
            BuffType.EvasionDown => evasionDown,
            BuffType.Stunned => stunned,
            BuffType.Bleeding => bleeding,
            BuffType.Taunt => taunt,
            _ => attackUp,
        };
    }
}