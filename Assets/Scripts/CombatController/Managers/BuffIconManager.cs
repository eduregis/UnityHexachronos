using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffIconManager : MonoBehaviour
{
    private static BuffIconManager instance;

    [Header("Buffs UI")]
    [SerializeField] private Image[] char1Buffs;
    [SerializeField] private Image[] char2Buffs;
    [SerializeField] private Image[] char3Buffs;

    [Header("Buffs Images")]
    public Sprite attackUp;
    public Sprite attackDown;
    public Sprite defenseUp;
    public Sprite defenseDown;
    public Sprite critRateUp;
    public Sprite critRateDown;
    public Sprite critDamageUp;
    public Sprite critDamageDown;
    public Sprite hitRateUp;
    public Sprite hitRateDown;
    public Sprite evasionUp;
    public Sprite evasionDown;
    public Sprite stunned;
    public Sprite bleeding;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one BuffIconManager in the scene");
        }
        instance = this;
    }
    public static BuffIconManager GetInstance()
    {
        return instance;
    }

    public void UpdateUI(CharacterInfo characterInfo, int index)
    {
        for (int i = 0; i < characterInfo.buffList.Count; i++)
        {
            switch (index)
            {
                case 0:
                    char1Buffs[i].gameObject.SetActive(true);
                    char1Buffs[i].sprite = BuffSpriteImage(characterInfo.buffList[i].buffType);
                    break;
                case 1:
                    char2Buffs[i].gameObject.SetActive(true);
                    char2Buffs[i].sprite = BuffSpriteImage(characterInfo.buffList[i].buffType);
                    break;
                case 2:
                    char3Buffs[i].gameObject.SetActive(true);
                    char3Buffs[i].sprite = BuffSpriteImage(characterInfo.buffList[i].buffType);
                    break;
                default:
                    break;
            }
        }
        for (int i = characterInfo.buffList.Count; i < char1Buffs.Length; i++)
        {
            switch (index)
            {
                case 0:
                    char1Buffs[i].gameObject.SetActive(false);
                    break;
                case 1:
                    char2Buffs[i].gameObject.SetActive(false);
                    break;
                case 2:
                    char3Buffs[i].gameObject.SetActive(false);
                    break;
                default:
                    break;
            }
        }
    }

    public Sprite BuffSpriteImage(BuffType type)
    {
        switch (type)
        {
            case BuffType.DamageUp:
                return attackUp;
            case BuffType.DamageDown:
                return attackDown;
            case BuffType.DefenseUp:
                return defenseUp;
            case BuffType.DefenseDown:
                return defenseDown;
            case BuffType.CritRateUp:
                return critRateUp;
            case BuffType.CritRateDown:
                return critRateDown;
            case BuffType.CritDamageUp:
                return critDamageUp;
            case BuffType.CritDamageDown:
                return critDamageDown;
            case BuffType.HitRateUp:
                return hitRateUp;
            case BuffType.HitRateDown:
                return hitRateDown;
            case BuffType.EvasionUp:
                return evasionUp;
            case BuffType.EvasionDown:
                return evasionDown;
            case BuffType.Stunned:
                return stunned;
            case BuffType.Bleeding:
                return bleeding;
            default:
                return attackUp;
        }
    }
}
