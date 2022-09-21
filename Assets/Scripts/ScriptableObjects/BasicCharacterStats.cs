using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BasicStats", menuName ="BasicStats")]

public class BasicCharacterStats : ScriptableObject
{
    public string char_name;

    [Header("Characters Basic Stats")]
    public int strength;
    public int intelligence;
    public int vitality;
    public int technique;
    public int agility;
    public int luck;

    [Header("Characters Battle Stats")]
    public int life;
    public int maxLife;
    public int energy;
    public int maxEnergy;
    public float defense;
    public int damage;
    public int hitRate;
    public int evasionRate;
    public int critRate;
    public int critDamage;
    public bool isBlocking;

    public List<CharacterSkill> skills;
}


