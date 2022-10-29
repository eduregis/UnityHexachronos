using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using System.Linq;

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
    List<UpgradeChip> chips;
    List<String> chipSetEffects;

    public void LoadChips(List<UpgradeChip> chipToBeLoaded) {
        chips = chipToBeLoaded;
    }

    public void LoadBattleStats() {

        TriggerChipSetEffects();

        TriggerStatsChips();

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

        TriggerBattleChips();

        DebbugingHero();
    }

    public void DebbugingHero() {
        Debug.Log("Hero name: " + name + "\n Basic Stats: " + "\n   Strength: " + strength + "\n   Vitality: " + vitality +
            "\n   Intelligence: " + intelligence + "\n   Technique: " + technique + "\n   Agility: " + agility +
            "\n   Luck: " + luck + "\n Battle Stats: " + "\n   Damage: " + damage + "\n   Defense: " + defense + "\n   CritRate: "
            + critRate + "\n   CritDamage: " + critDamage + "\n   Evasion: " + evasionRate + "\n   HitRate: " + hitRate + "\n ChipSet Effects: (" + chipSetEffects.Count + ")");
        foreach (string chipSet in chipSetEffects) {
            Debug.Log("\n   " + chipSet);
        }
    }

    public void TriggerStatsChips() {
        if (chips != null) {
            foreach (UpgradeChip chip in chips) {
                if (chip.chipType is ChipType.Strength or ChipType.Intelligence or ChipType.Vitality or
                    ChipType.Technique or ChipType.Agility or ChipType.Luck) {
                    TriggerChip(chip);
                }
            }
        }
    }

    public void TriggerBattleChips() {
        if (chips != null) {
            foreach (UpgradeChip chip in chips) {
                if (chip.chipType is ChipType.Damage or ChipType.Defense or ChipType.HitRate or
                    ChipType.Evasion or ChipType.CritDamage or ChipType.CritRate) {
                    TriggerChip(chip);
                }
            }
        }
    }

    public void TriggerChipSetEffects() {

        chipSetEffects = new();

        if (chips != null) {
            foreach (ChipSet chipSet in Enum.GetValues(typeof(ChipSet))) {
                var filteredChips = chips.Where(chip => chip.chipSet == chipSet);
                if (filteredChips.Count() > 1) {
                    chipSetEffects.Add(chipSet + "+2");
                    if (filteredChips.Count() > 2) {
                        chipSetEffects.Add(chipSet + "+3");
                    }
                }
            }
        }
    }

    public void TriggerChip(UpgradeChip chip) {
        switch (chip.chipType) {
            case ChipType.Damage:
                damage = (int)CalcStatusWithChipBonus(damage, chip);
                break;
            case ChipType.Defense:
                defense = CalcStatusWithChipBonus(defense, chip);
                break;
            case ChipType.HitRate:
                hitRate = (int)CalcStatusWithChipBonus(hitRate, chip);
                break;
            case ChipType.Evasion:
                evasionRate = (int)CalcStatusWithChipBonus(evasionRate, chip);
                break;
            case ChipType.CritDamage:
                critDamage = (int)CalcStatusWithChipBonus(critDamage, chip);
                break;
            case ChipType.CritRate:
                critRate = (int)CalcStatusWithChipBonus(critRate, chip);
                break;
            case ChipType.Strength:
                strength = (int)CalcStatusWithChipBonus(strength, chip);
                break;
            case ChipType.Intelligence:
                intelligence = (int)CalcStatusWithChipBonus(intelligence, chip);
                break;
            case ChipType.Vitality:
                vitality = (int)CalcStatusWithChipBonus(vitality, chip);
                break;
            case ChipType.Technique:
                technique = (int)CalcStatusWithChipBonus(technique, chip);
                break;
            case ChipType.Agility:
                agility = (int)CalcStatusWithChipBonus(agility, chip);
                break;
            case ChipType.Luck:
                luck = (int)CalcStatusWithChipBonus(luck, chip);
                break;
        }
    }

    public float CalcStatusWithChipBonus(float value, UpgradeChip chip) {
        switch (chip.chipModifier) {
            case ChipModifier.Constant:
                return value + chip.value;
            case ChipModifier.Multiplier:
                return value * chip.value;
            case ChipModifier.AutoAttackModifier:
                return value;
        }
        return value;
    }

    public string TakeDamage(int dmg) {
        if (dmg >= life) {
            life = 0;
        } else {
            life -= dmg;
        }
        return dmg.ToString();
    }

    public string TakeHeal(int heal) {
        
        if (heal + life >= maxLife) {
            int trueHeal = (heal + life) - maxLife; 
            life = maxLife;
            return "HEAL " + trueHeal + "!";
        } else {
            life += heal;
            return "HEAL " + heal + "!";
        }
    }

    public string LoseEnergy(int cost) {
        if (cost >= energy) {
            int remainingEnergy = energy;
            energy = 0;
            return "LOSE " + remainingEnergy + " MP!";
        } else {
            energy -= cost;
            return "LOSE " + cost + " MP!";
        }
    }

    public string GainEnergy(int recoveredEnergy) {
        if (recoveredEnergy + energy >= maxEnergy) {
            int trueRecover = (recoveredEnergy + energy) - maxEnergy;
            energy = maxEnergy;
            return "RECOVER " + trueRecover + "!";
        } else {
            energy += recoveredEnergy;
            return "RECOVER " + recoveredEnergy + "!";
        }
    }

    public string ReceivingAttackDamage(CharacterStats attacker) {
        Debug.Log("Character: " + attacker.char_name + ", damage: " + attacker.damage);
        return DamageWithBuffsAndStatsApplied(attacker, attacker.damage);
    }

    public List<string> ReceivingSkillDamage(CharacterStats attacker, float damageMultiplier, int quantity, int rate) {

        List<string> messages = new();

        for (int i = 0; i < quantity; i++) {
            int random = UnityEngine.Random.Range(0, 99);
            if (random < rate) {
                int finalDamage = (int)(attacker.damage * damageMultiplier);
                Debug.Log("Damage: " + finalDamage + ", rate: " + random);
                messages.Add(DamageWithBuffsAndStatsApplied(attacker, finalDamage));
            } else {
                messages.Add("MISS!");
            }
        }

        return messages;
    }

    public string ReceivingHeal(CharacterStats healer, float healMultiplier, int rate) {

        int random = UnityEngine.Random.Range(0, 99);
        if (random < rate) {
            int heal = (int)(healer.intelligence * healMultiplier);
            Debug.Log("Heal: " + heal + ", rate: " + random);
            return TakeHeal(heal);
        } else {
            return "FAILED!";
        }
    }

    public bool IsInNegativeStatus(BuffType statusBuffType) {
        foreach (Buff buff in buffs) {
            if (buff.buffType == statusBuffType) {
                return true;
            }
        }
        return false;
    }

    private string DamageWithBuffsAndStatsApplied(CharacterStats attacker, int receivedDamage) {

        // Applying buff modifications. B.D.A. is Buff and Debuffs Applied
        int BDA_attackerHitRate = (int)GenericBuffApplier(attacker, (float)attacker.hitRate, BuffType.HitRateUp, BuffType.HitRateDown);
        int BDA_attackerCritDamage = (int)GenericBuffApplier(attacker, (float)attacker.critDamage, BuffType.CritDamageUp, BuffType.CritDamageDown);
        int BDA_attackerCritRate = (int)GenericBuffApplier(attacker, (float)attacker.critRate, BuffType.CritRateUp, BuffType.CritRateDown);
        int BDA_attackerDamage = (int)GenericBuffApplier(attacker, (float)receivedDamage, BuffType.DamageUp, BuffType.DamageDown);
        float BDA_defense = GenericBuffApplier(this, this.defense, BuffType.DefenseUp, BuffType.DefenseDown);
        int BDA_evasionRate = (int)GenericBuffApplier(this, (float)this.evasionRate, BuffType.EvasionUp, BuffType.EvasionDown);

        int maxHitValue = (BDA_attackerHitRate > 100) ? BDA_attackerHitRate : 100;
        int hitRNG = UnityEngine.Random.Range(0, maxHitValue);

        int finalDamage;

        if (hitRNG > BDA_evasionRate) {
            int critRNG = UnityEngine.Random.Range(0, 100);
            if (critRNG < BDA_attackerCritRate && !isBlocking) {
                int attackCritDamage = BDA_attackerDamage + (int)(BDA_attackerDamage * BDA_attackerCritDamage * 0.01);
                finalDamage = (int)(attackCritDamage / BDA_defense);
                Debug.Log("Character: " + attacker.char_name + "critRate: " + BDA_attackerCritRate + "critDamage: " + BDA_attackerCritDamage + ", damage: " + finalDamage);
                return TakeDamage(finalDamage) + "\n Critical!";
            } else {
                float finalDefense = (isBlocking == false) ? BDA_defense : (float)(BDA_defense * 1.5);
                finalDamage = (int)(BDA_attackerDamage / finalDefense);
                Debug.Log("Character: " + attacker.char_name + ", damage: " + finalDamage);
                return TakeDamage(finalDamage);
            }
            
        } else {
            return "MISS!";
        }
    }

    private float GenericBuffApplier(CharacterStats buffOwner, float initialValue, BuffType buffTypeUp, BuffType buffTypeDown) {
        float finalValue = (float)initialValue;

        foreach (Buff buff in buffOwner.buffs) {
            if (buff.buffType == buffTypeUp) {
                if (buff.modifier == BuffModifier.Multiplier) {
                    finalValue *= buff.value;
                } else if (buff.modifier == BuffModifier.Constant) {
                    finalValue += buff.value;
                }
            }
            if (buff.buffType == buffTypeDown) {
                if (buff.modifier == BuffModifier.Multiplier) {
                    finalValue /= buff.value;
                } else if (buff.modifier == BuffModifier.Constant) {
                    finalValue -= buff.value;
                }
            }
        }
        return finalValue;
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
        int intIncrease = 0;
        if (modifier == BuffModifier.Status) intIncrease = intelligence;
        if (random < (rate + intIncrease)) {
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

    public List<string> ApplySkill(CharacterStats skillUser, CharacterSkill skill) {

        List<string> messages = new();

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
                        messages.Add(buff.buffType.ToString());
                        buffs.Add(buff);
                    }
                    break;
                case "attack":
                    // ATTACK SYNTAX: Attack identifier (attack) - Multiplier - Quantity of attacks - Chance of success
                    List<string> attackMessages = ReceivingSkillDamage(
                        skillUser,
                        float.Parse(strlist[1]),
                        int.Parse(strlist[2]),
                        int.Parse(strlist[3]));
                    foreach (string attackMessage in attackMessages) messages.Add(attackMessage);
                    break;
                case "heal":
                    // HEAL SYNTAX: Heal identifier (heal) - Multiplier - Chance of success
                    messages.Add(ReceivingHeal(
                        skillUser,
                        float.Parse(strlist[1]),
                        int.Parse(strlist[2])));
                    break;
                case "energy":
                    // HEAL SYNTAX: Energy identifier (energy) - Value
                    int energyValue = int.Parse(strlist[1]);
                    if (energyValue > 0) messages.Add(GainEnergy(energyValue));
                    else messages.Add(LoseEnergy(energyValue));
                    break;
            }
        }
        return messages;
    }
}


