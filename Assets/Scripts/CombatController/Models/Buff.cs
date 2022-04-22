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
    int value;
    BuffType type;
    int duration;
}
