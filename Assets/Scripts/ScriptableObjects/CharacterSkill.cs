using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Skill
{
    TryToCatchMe,
    DesencanaComIsso,
    BehindYou,
    LetsGetThisOverWith,
    Taser,
    SmokeBomb,
    HealingPulse,
    FinishIt,
    HandShield,
    LongLiveTheRevolution,
    JustAScratch,
    PowerJab,
    DoubleSlash,
    BlankStare,
    OpenWounds,
    BerserkMode,
    HealingInjection,
    ExtraBattery,
    StayFocused,
    WordOfCommand,
    LetsGoGuys,
    EnergizedHammer,
    Charge,
    SpinAttack,
    MarkEnemy,
    KeepCalm,
    SoundBomb,
    Bullseye
}

[CreateAssetMenu(fileName = "CharacterSkills", menuName = "CharacterSkills")]

public class CharacterSkill : ScriptableObject
{
    public Skill skill_id;
    public string skill_name;
    public string description;
    public int cost;
    public AffectType affectType;
    public bool isAffectFaint = false;
};

public enum AffectType
{
    Self,
    EnemyTarget,
    AllyTarget,
    AllEnemies,
    AllAllies
}