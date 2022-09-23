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

    public bool TakeHeal(int heal) {
        if (heal + life >= maxLife) {
            life = maxLife;
            return true;
        } else {
            life += heal;
            return false;
        }
    }

    public bool LoseEnergy(int cost) {
        if (cost >= energy) {
            energy = 0;
            return true;
        } else {
            energy -= cost;
            return false;
        }
    }

    public void ReceivingAttackDamage(CharacterStats attacker)
    {
        // TODO: Apply battle stats calcs in this damage
        // TODO: Apply buff modifications in this damage
        TakeDamage(attacker.damage);
    }

    public void ReceivingSkillDamage(CharacterStats attacker, float damageMultiplier, int quantity, int rate) {

    // TODO: Apply battle stats calcs in this damage
    // TODO: Apply buff modifications in this damage
        for (int i = 0; i < quantity; i++) {
            int random = UnityEngine.Random.Range(0, 99);
            if (random < rate) {
                int finalDamage = (int)(attacker.damage * damageMultiplier);
                Debug.Log("Damage: " + finalDamage + ", rate: " + random);
                TakeDamage(damage);
            }
        }
    }

    public void ReceivingHeal(CharacterStats healer, float healMultiplier, int rate) {

        // TODO: Apply battle stats calcs in this damage
        // TODO: Apply buff modifications in this damage
            int random = UnityEngine.Random.Range(0, 99);
            if (random < rate)
            {
                int heal = (int)(healer.intelligence * healMultiplier);
                Debug.Log("Heal: " + heal + ", rate: " + random);
                TakeHeal(heal);
            }
    }

    public void UpdateBuffs() {

        List<int> removedBuffsList = new();

        for (int i = buffs.Count - 1; i >= 0; i--) {
            buffs[i].duration--;
            if (buffs[i].duration < 0)
                removedBuffsList.Add(i);
        }

        foreach (int i in removedBuffsList) {
            buffs.RemoveAt(i);
        }
    }

    private Buff CreateBuff(float value, BuffType buffType, BuffModifier modifier, int duration, int rate) {

        int random = UnityEngine.Random.Range(0, 99);
        if (random < rate) {
            Buff buff = new() {
                value = value,
                buffType = buffType,
                modifier = modifier,
                duration = duration
            };
            Debug.Log("Buff( value: " + buff.value + ", buffType: " + buff.buffType + ", Modifier: " + buff.modifier + ", Duration: " + buff.duration + ")");
            return buff;
        }
        return null;
    }

    private BuffType GetBuffType(String buffTypeStr) {
        return buffTypeStr switch {
            "DamageUp" => BuffType.DamageUp,
            "DamageDown" => BuffType.DamageDown,
            "DefenseUp" => BuffType.DefenseUp,
            "DefenseDown" => BuffType.DefenseDown,
            "CritRateUp" => BuffType.CritRateUp,
            "CritRateDown" => BuffType.CritRateDown,
            "CritDamageUp" => BuffType.CritDamageUp,
            "CritDamageDown" => BuffType.CritDamageDown,
            "EvasionUp" => BuffType.EvasionUp,
            "EvasionDown" => BuffType.EvasionDown,
            "HitRateUp" => BuffType.HitRateUp,
            "HitRateDown" => BuffType.HitRateDown,
            "Stunned" => BuffType.Stunned,
            "Bleeding" => BuffType.Bleeding,
            "Taunt" => BuffType.Taunt,
            _ => BuffType.DamageUp,
        };
    }

    private BuffModifier GetBuffModifier(String buffModifierStr) {
        return buffModifierStr switch {
            "Constant" => BuffModifier.Constant,
            "Multiplier" => BuffModifier.Multiplier,
            "Status" => BuffModifier.Status,
            _ => BuffModifier.Status,
        };
    }

    public void ApplySkill(CharacterStats skillUser, CharacterSkill skill) {
        foreach (string effect in skill.skill_execution) {
            char[] separators = { '-' };
            String[] strlist = effect.Split(separators, 6, StringSplitOptions.None);
            switch (strlist[0]) {
                case "buff":
                    // BUFF SYNTAX: Buff identifier (buff) - Value - Buff type - Buff modifier - Turn duration - Chance of application
                    Buff buff = CreateBuff(
                        float.Parse(strlist[1]), 
                        GetBuffType(strlist[2]), 
                        GetBuffModifier(strlist[3]), 
                        int.Parse(strlist[4]),
                        int.Parse(strlist[5]));
                    if (buff != null) {
                        buffs.Add(buff);
                    }
                    break;
                case "attack":
                    // ATTACK SYNTAX: Attack identifier (attack) - Multiplier - Quantity of attacks - Chance of success
                    ReceivingSkillDamage(
                        skillUser,
                        float.Parse(strlist[1]),
                        int.Parse(strlist[2]),
                        int.Parse(strlist[3]));
                    break;
                case "heal":
                    // HEAL SYNTAX: Heal identifier (heal) - Multiplier - Chance of success
                    ReceivingHeal(
                        skillUser,
                        float.Parse(strlist[1]),
                        int.Parse(strlist[2]));
                    break;
            }
        }
    }
}


