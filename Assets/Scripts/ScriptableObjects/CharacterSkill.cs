using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "CharacterSkills", menuName = "CharacterSkills")]

public class CharacterSkill : ScriptableObject
{
    public List<int> sequence;
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