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
            case Skill.Jab:
                Jab(charInfo, targetIndex, isEnemy);
                break;
            case Skill.HealingInjection:
                HealingInjection(charInfo, targetIndex, isEnemy);
                break;
            case Skill.NailBomb:
                NailBomb(charInfo, targetIndex, isEnemy);
                break;
            case Skill.EnergizedHammer:
                EnergizedHammer(charInfo, targetIndex, isEnemy);
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
        if (stunRate > 50)
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
        if (bleedingRate > 50)
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


    // Billy Skills


    // Salvato Skills


    // Dandara Skills


    // Sniper Skills



    private void Jab (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().InflictingDamage(charInfo, 30, targetIndex, isEnemy);
    }

    private void HealingInjection (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainHP(30, targetIndex, isEnemy);
        Buff buff = CreateBuff(2f, BuffType.DamageUp, BuffModifier.Constant, 1);

        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
    }

    private void NailBomb (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfEnemies = CombatCharManager.GetInstance().enemies.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            CombatCharManager.GetInstance().InflictingDamage(charInfo, 30, i, isEnemy);
        }
    }

    private void EnergizedHammer (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().InflictingDamage(charInfo, 30, targetIndex, isEnemy);
    }

   
}

