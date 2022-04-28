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
            case Skill.MarkEnemy:
                MarkEnemy(charInfo, targetIndex, isEnemy);
                break;
            case Skill.KeepCalm:
                KeepCalm(charInfo, targetIndex, isEnemy);
                break;
            case Skill.SoundBomb:
                SoundBomb(charInfo, targetIndex, isEnemy);
                break;
            case Skill.Bullseye:
                Bullseye(charInfo, targetIndex, isEnemy);
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
        List<string> texts = new List<string>();

        Buff buff1 = CreateBuff(1.5f, BuffType.EvasionUp, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);
        texts.Add("Aumenta esquiva");

        Buff buff2 = CreateBuff(10, BuffType.CritRateUp, BuffModifier.Constant, 3);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
        texts.Add("Aumenta taxa crítica");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    private void DesencanaComIsso (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 0.6);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        System.Random rnd = new System.Random();

        int stunRate = rnd.Next(1, 101);
        if (stunRate > 30)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
            texts.Add("Atordoado");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void BehindYou (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 0.7);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        System.Random rnd = new System.Random();

        int bleedingRate = rnd.Next(1, 101);
        if (bleedingRate > 40)
        {
            Buff buff = CreateBuff(10, BuffType.Bleeding, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
            texts.Add("Sangrando");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private IEnumerator LetsGetThisOverWith (CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 0.65);
        for (int i = 0; i < 5; i++)
        {
            yield return new WaitForSeconds(0.05f);
            int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
            texts.Add(finalDamage.ToString());
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    // Sam Skills

    private void Taser(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 0.7);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        System.Random rnd = new System.Random();

        int stunRate = rnd.Next(1, 101);
        if (stunRate > 50)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
            texts.Add("Atordoado");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void SmokeBomb(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(1.5f, BuffType.EvasionUp, BuffModifier.Multiplier, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
            texts.Add("Aumenta esquiva");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllEnemies, isEnemy);
    }

    private void HealingPulse(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            CombatCharManager.GetInstance().GainHP(30, i, isEnemy);
            texts.Add("Curou " + 30);
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllAllies, isEnemy);
    }

    private void FinishIt(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff1 = CreateBuff(50f, BuffType.CritRateUp, BuffModifier.Constant, 1);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);
        texts.Add("Aumentou taxa crítica");

        Buff buff2 = CreateBuff(2f, BuffType.CritDamageUp, BuffModifier.Multiplier, 1);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
        texts.Add("Aumentou dano crítico");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllyTarget, isEnemy);
    }

    // Borell Skills

    private void HandShield(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff = CreateBuff(1.5f, BuffType.DefenseUp, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        texts.Add("Aumenta Defesa");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);

    }

    private void LongLiveTheRevolution(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff = CreateBuff(1.5f, BuffType.Taunt, BuffModifier.Multiplier, 3);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        texts.Add("Provocar");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    private void JustAScratch(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        CombatCharManager.GetInstance().GainHP(50, targetIndex, isEnemy);
        texts.Add("Recupera HP");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    private void PowerJab(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 2.5);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    // Billy Skills

    private IEnumerator DoubleSlash(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage1 = (int)((float)charInfo.damage * 0.7);
        int finalDamage1 = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage1, targetIndex, isEnemy);
        texts.Add(finalDamage1.ToString());

        yield return new WaitForSeconds(0.1f);

        int damage2 = (int)((float)charInfo.damage * 0.8);
        int finalDamage2 = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage2, targetIndex, isEnemy);
        texts.Add(finalDamage2.ToString());

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void BlankStare(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        System.Random rnd = new System.Random();
        int stunRate = rnd.Next(1, 101);
        if (stunRate > 50)
        {
            Buff buff = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
            texts.Add("Atordoado");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void OpenWounds(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 0.5);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        System.Random rnd = new System.Random();

        int bleedingRate = rnd.Next(1, 101);
        if (bleedingRate > 70)
        {
            Buff buff = CreateBuff(10, BuffType.Bleeding, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
            texts.Add("Sangrando");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void BerserkMode(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff1 = CreateBuff(2.5f, BuffType.DefenseDown, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);
        texts.Add("Diminui Defesa");

        Buff buff2 = CreateBuff(2.5f, BuffType.DamageUp, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
        texts.Add("Aumenta Dano");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    // Salvato Skills

    private void HealingInjection(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        CombatCharManager.GetInstance().GainHP(30, targetIndex, isEnemy);
        texts.Add("Curou " + 30);

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllyTarget, isEnemy);
    }

    private void ExtraBattery(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        CombatCharManager.GetInstance().GainEnergy(50, targetIndex, isEnemy);
        texts.Add("Recupera energia");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllyTarget, isEnemy);
    }

    private void StayFocused(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(25f, BuffType.HitRateUp, BuffModifier.Constant, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
            texts.Add("Aumenta precisão");
        }
        

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllAllies, isEnemy);
    }

    private void WordOfCommand(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff = CreateBuff(2f, BuffType.DefenseDown, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff, targetIndex, isEnemy);
        texts.Add("Diminuiu defesa");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    // Dandara Skills

    private void LetsGoGuys(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfHeroes = CombatCharManager.GetInstance().heroes.Count;
        for (int i = 0; i < numberOfHeroes; i++)
        {
            Buff buff = CreateBuff(1.3f, BuffType.DamageUp, BuffModifier.Multiplier, 3);
            CombatCharManager.GetInstance().SettingBuff(buff, i, isEnemy);
            texts.Add("Aumenta dano");
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllAllies, isEnemy);
    }

    private void EnergizedHammer(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 1.5);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void Charge(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        CombatCharManager.GetInstance().GainEnergy(50, targetIndex, isEnemy);
        texts.Add("Recupera Energia");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    private void SpinAttack(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfEnemies = CombatCharManager.GetInstance().enemies.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            int damage = (int)((float)charInfo.damage * 1.3);
            int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, i, isEnemy);
            texts.Add(finalDamage.ToString());
        }

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllEnemies, isEnemy);
    }

    // Sniper Skills
    private void MarkEnemy(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        Buff buff1 = CreateBuff(10, BuffType.Taunt, BuffModifier.Status, 2);
        CombatCharManager.GetInstance().SettingBuff(buff1, targetIndex, isEnemy);
        texts.Add("Marcado");

        Buff buff2 = CreateBuff(1.5f, BuffType.DefenseDown, BuffModifier.Multiplier, 2);
        CombatCharManager.GetInstance().SettingBuff(buff2, targetIndex, isEnemy);
        texts.Add("Diminuiu defesa");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }

    private void KeepCalm(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        CombatCharManager.GetInstance().GainEnergy(50, targetIndex, isEnemy);
        texts.Add("Recupera Energia");

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.Self, isEnemy);
    }

    private void SoundBomb(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int numberOfEnemies = CombatCharManager.GetInstance().enemies.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            Buff buff1 = CreateBuff(25f, BuffType.HitRateDown, BuffModifier.Status, 2);
            CombatCharManager.GetInstance().SettingBuff(buff1, i, isEnemy);
            texts.Add("Diminui precisão");

            System.Random rnd = new System.Random();

            int stunRate = rnd.Next(1, 101);
            if (stunRate > 30)
            {
                Buff buff2 = CreateBuff(10, BuffType.Stunned, BuffModifier.Status, 2);
                CombatCharManager.GetInstance().SettingBuff(buff2, i, isEnemy);
                texts.Add("Atordoado");
            } else
            {
                texts.Add(" ");
            }
        }

        Debug.Log(texts.Count);

       // CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.AllEnemies, isEnemy);
    }

    private void Bullseye(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        List<string> texts = new List<string>();

        int damage = (int)((float)charInfo.damage * 3.0);
        int finalDamage = CombatCharManager.GetInstance().InflictingDamage(charInfo, damage, targetIndex, isEnemy);
        texts.Add(finalDamage.ToString());

        CombatAnimationManager.GetInstance().ActiveScreen(texts, CombatCharManager.GetInstance().GetHeroesIndex(), targetIndex, AffectType.EnemyTarget, isEnemy);
    }


}

