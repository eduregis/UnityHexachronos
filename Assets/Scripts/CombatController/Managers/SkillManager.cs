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

    private void Jab(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().LoseHP(charInfo, 30, targetIndex, isEnemy);
    }

    private void HealingInjection(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().GainHP(30, targetIndex, isEnemy);
    }
    private void NailBomb(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        int numberOfEnemies = CombatCharManager.GetInstance().enemies.Count;
        for (int i = 0; i < numberOfEnemies; i++)
        {
            CombatCharManager.GetInstance().LoseHP(charInfo, 30, i, isEnemy);
        }
        
    }

    private void EnergizedHammer(CharacterInfo charInfo, int targetIndex, bool isEnemy)
    {
        CombatCharManager.GetInstance().LoseHP(charInfo, 30, targetIndex, isEnemy);
    }
}

