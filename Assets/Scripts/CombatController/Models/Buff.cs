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
    HitRateDown
}
public class Buff 
{
    public int value;
    public BuffType buffType;
    public int duration;
}
