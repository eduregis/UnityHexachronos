using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BasicStats", menuName ="BasicStats")]

public class CharacterStats : ScriptableObject
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

    public void LoadBattleStats()
    {
        maxLife = ((vitality) * 5);
        life = maxLife;
        maxEnergy = 100;
        energy = maxEnergy;
        defense = 1f;
        damage = (strength + (technique / 2));
        hitRate = (50 + technique + (agility / 2) + (luck / 4));
        evasionRate = ((agility / 3) + (luck / 3) + (intelligence / 3));
        critRate = (5 + (luck / 2));
        critDamage = 50;
        isBlocking = false;
    }

    public bool TakeDamage(int dmg)
    {
        if (dmg >= life)
        {
            life = 0;
            return true;
        }
        else
        {
            life -= dmg;
            return false;
        }
    }
}


