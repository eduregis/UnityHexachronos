using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Skill
{
    Jab,
    NailBomb,
    HealingInjection,
    EnergizedHammer,
    SmokeBomb
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