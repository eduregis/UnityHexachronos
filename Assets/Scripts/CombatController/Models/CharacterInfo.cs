using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterInfo
{
    public string char_name;
    // basic status
    public int strength;
    public int intelligence;
    public int vitality;
    public int technique;
    public int agility;
    public int luck;
    public int level;
    // advanced status
    public int life;
    public int maxLife;
    public int energy;
    public int maxEnergy;
    public int damage;
    public int hitRate;
    public int evasionRate;
    public int critRate;
    public int critDamage;

    public List<CharacterSkill> skillList;
    public List<Buff> buffList;
}