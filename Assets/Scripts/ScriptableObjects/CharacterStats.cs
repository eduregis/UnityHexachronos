using System.Collections;
using System.Collections.Generic;
using System;
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
    public List<Buff> buffs;

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
        buffs = new List<Buff>();
    }

    public bool TakeDamage(int dmg) {
        if (dmg >= life) {
            life = 0;
            return true;
        } else {
            life -= dmg;
            return false;
        }
    }

    private Buff CreateBuff(float value, BuffType buffType, BuffModifier modifier, int duration) {
        Buff buff = new Buff();
        buff.value = value;
        buff.buffType = buffType;
        buff.modifier = modifier;
        buff.duration = duration;
        return buff;
    }

    public void ApplySkill(CharacterSkill skill) {
        Debug.Log("Buffs: " + buffs.Count);
        foreach (string effect in skill.skill_execution) {
            char[] separators = { '-' };
            String[] strlist = effect.Split(separators, 5, StringSplitOptions.None);
            switch (strlist[0]) {
                case "buff":
                    Debug.Log("value: " + float.Parse(strlist[1]) + ", buff type: " + strlist[2] + ", buff modifier: " + strlist[3] + ", duration: " + strlist[4]);
                    Buff buff = CreateBuff(float.Parse(strlist[1]), BuffType.DefenseDown, BuffModifier.Multiplier, int.Parse(strlist[4]));
                    buffs.Add(buff);
                    break;
                case "attack":
                    break;
                case "heal":
                    break;
            }
        }
        Debug.Log("Buffs: " + buffs.Count);
    }
}


