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
    NailBomb,
    EnergizedHammer
}

[CreateAssetMenu(fileName = "CharacterSkills", menuName = "CharacterSkills")]

public class CharacterSkill : ScriptableObject
{
    public Skill skill_id;
    public string skill_name;
    public string description;
    public int cost;
    public AffectType affectType;
};

public enum AffectType
{
    Self,
    EnemyTarget,
    AllyTarget,
    AllEnemies,
    AllAllies
}