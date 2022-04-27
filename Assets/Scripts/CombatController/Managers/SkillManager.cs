using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class SkillManager : MonoBehaviour
{
    private static SkillManager instance;
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one SkillManager in the scene");
        }
        instance = this;
    }
    public static SkillManager GetInstance()
    {
        return instance;
    }

    public void TriggeringSkill(Skill skill_id, CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        switch (skill_id)
        {
            case Skill.TryToCatchMe:
                TryToCatchMe(charInfo, targetIndex, isEnemy);
                break;
            case Skill.DesencanaComIsso:
                DesencanaComIsso(charInfo, targetIndex, isEnemy);
                break;
            case Skill.BehindYou:
                BehindYou(charInfo, targetIndex, isEnemy);
                break;
            case Skill.LetsGetThisOverWith:
                StartCoroutine(LetsGetThisOverWith(charInfo, targetIndex, isEnemy));
                break;
            case Skill.Taser:
                Taser(charInfo, targetIndex, isEnemy);
                break;
            case Skill.SmokeBomb:
                SmokeBomb(charInfo, targetIndex, isEnemy);
                break;
            case Skill.HealingPulse:
                HealingPulse(charInfo, targetIndex, isEnemy);
                break;
            case Skill.FinishIt:
                FinishIt(charInfo, targetIndex, isEnemy);
                break;
            case Skill.HandShield:
                HandShield(charInfo, targetIndex, isEnemy);
                break;
            case Skill.LongLiveTheRevolution:
                LongLiveTheRevolution(charInfo, targetIndex, isEnemy);
                break;
            case Skill.JustAScratch:
                JustAScratch(charInfo, targetIndex, isEnemy);
                break;
            case Skill.PowerJab:
                PowerJab(charInfo, targetIndex, isEnemy);
                break;
            case Skill.DoubleSlash:
                StartCoroutine(DoubleSlash(charInfo, targetIndex, isEnemy));
                break;
            case Skill.BlankStare:
                BlankStare(charInfo, targetIndex, isEnemy);
                break;
            case Skill.OpenWounds:
                OpenWounds(charInfo, targetIndex, isEnemy);
                break;
            case Skill.BerserkMode:
                BerserkMode(charInfo, targetIndex, isEnemy);
                break;
            case Skill.HealingInjection:
                HealingInjection(charInfo, targetIndex, isEnemy);
                break;
            case Skill.ExtraBattery:
                ExtraBattery(charInfo, targetIndex, isEnemy);
                break;
            case Skill.StayFocused:
                StayFocused(charInfo, targetIndex, isEnemy);
                break;
            case Skill.WordOfCommand:
                WordOfCommand(charInfo, targetIndex, isEnemy);
                break;
            case Skill.LetsGoGuys:
                LetsGoGuys(charInfo, targetIndex, isEnemy);
                break;
            case Skill.EnergizedHammer:
                EnergizedHammer(charInfo, targetIndex, isEnemy);
                break;
            case Skill.Charge:
                Charge(charInfo, targetIndex, isEnemy);
                break;
            case Skill.SpinAttack:
                SpinAttack(charInfo, targetIndex, isEnemy);
                break;
        }
    }

    private Buff CreateBuff(float value, BuffType buffType, BuffModifier modifier, int duration)
    {
        Buff buff = new Buff();
        buff.value = value;
        buff.buffType = buffType;
        buff.modifier = modifier;
        buff.duration = duration;
        return buff;
    }

    // Lucca Skills

    private void TryToCatchMe (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff1 = CreateBuff(1.5f, BuffType.EvasionUp, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);

        Buff buff2 = CreateBuff(10, BuffType.CritRateUp, BuffModifier.Constant, 3);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
    }

    private void DesencanaComIsso (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 0.6);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);

        System.Random rnd = new System.Random();

        int stunRate = rnd.Next(1, 101);
        if (stunRate > 30)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        }
    }

    private void BehindYou (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 0.7);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);

        System.Random rnd = new System.Random();

        int bleedingRate = rnd.Next(1, 101);
        if (bleedingRate > 40)
        {
            Buff buff = CreateBuff(10, BuffType.Bleeding, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        }
    }

    private IEnumerator LetsGetThisOverWith (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 0.65);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.1f);
            CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        }
    }

    // Sam Skills

    private void Taser(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 0.7);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);

        System.Random rnd = new System.Random();

        int stunRate = rnd.Next(1, 101);
        if (stunRate > 50)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        }
    }

    private void SmokeBomb(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(1.5f, BuffType.EvasionUp, BuffModifier.Multiplier, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
        }
    }

    private void HealingPulse(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            CombatCharManager.GetInstance().GainHP(30, i, isEnemy);
        }
    }

    private void FinishIt(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff1 = CreateBuff(50f, BuffType.CritRateUp, BuffModifier.Constant, 1);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);

        Buff buff2 = CreateBuff(2f, BuffType.CritDamageUp, BuffModifier.Multiplier, 1);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
    }

    // Borell Skills

    private void HandShield(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff = CreateBuff(1.5f, BuffType.DefenseUp, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
    }

    private void LongLiveTheRevolution(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff = CreateBuff(1.5f, BuffType.Taunt, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
    }

    private void JustAScratch(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainHP(50, targetIndex, isEnemy);
    }

    private void PowerJab(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 2.5);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
    }

    // Billy Skills

    private IEnumerator DoubleSlash(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage1 = (int)((float)charInfo.damage * 0.7);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage1, targetIndex, isEnemy);

        yield return new WaitForSeconds(0.1f);

        int damage2 = (int)((float)charInfo.damage * 0.8);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage2, targetIndex, isEnemy);
    }

    private void BlankStare(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        System.Random rnd = new System.Random();
        int stunRate = rnd.Next(1, 101);
        if (stunRate > 50)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        }
    }

    private void OpenWounds(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 0.5);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);

        System.Random rnd = new System.Random();

        int bleedingRate = rnd.Next(1, 101);
        if (bleedingRate > 70)
        {
            Buff buff = CreateBuff(10, BuffType.Bleeding, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        }
    }

    private void BerserkMode(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff1 = CreateBuff(2.5f, BuffType.DefenseDown, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);

        Buff buff2 = CreateBuff(2.5f, BuffType.DamageUp, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
    }

    // Salvato Skills

    private void HealingInjection(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainHP(30, targetIndex, isEnemy);
    }

    private void ExtraBattery(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainEnergy(50, targetIndex, isEnemy);
    }

    private void StayFocused(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(25f, BuffType.HitRateUp, BuffModifier.Constant, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
        }
    }

    private void WordOfCommand(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        Buff buff = CreateBuff(2f, BuffType.DefenseDown, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
    }

    // Dandara Skills

    private void LetsGoGuys(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(1.3f, BuffType.DamageUp, BuffModifier.Multiplier, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
        }
    }

    private void EnergizedHammer(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int damage = (int)((float)charInfo.damage * 1.5);
        CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
    }

    private void Charge(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainEnergy(50, targetIndex, isEnemy);
    }

    private void SpinAttack(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfEnemies = CombatCharManager.GetInstance().enemies.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int damage = (int)((float)charInfo.damage * 1.3);
            CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, i, isEnemy);
        }

    
}

    // Sniper Skills



}

