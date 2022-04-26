using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum BuffType
{
    DamageUp,
    DamageDown,
    DefenseUp,
    DefenseDown,
    CritRateUp,
    CritRateDown,
    CritDamageUp,
    CritDamageDown,
    EvasionUp,
    EvasionDown,
    HitRateUp,
    HitRateDown,
    Stunned,
    Bleeding,
    Taunt
}

public enum BuffModifier
{
    Multiplier,
    Constant,
    Status
}
public class Buff 
{
    public float value;
    public BuffType buffType;
    public BuffModifier modifier;
    public int duration;
}
