using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public enum ActualTBSStatus
{
    MainMenu,
    Attack,
    SkillMenu,
    Skill,
    Block
}

public class CombatManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private GameObject[] mainMenuButtons;

    [Header("Skill Menu UI")]
    [SerializeField] private GameObject[] skillMenuButtons;
    [SerializeField] private Image[] actionPoints;
    [SerializeField] private Image[] arrows;

    // Control Variables
    private int APLimit;
    private int APindex;

    private static CombatManager instance;
    private ActualTBSStatus actualStatus;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CombatManager in the scene");
        }
        instance = this;
    }
    public static CombatManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        APLimit = 5;
        APindex = 0;
        HidingSkillMenu();
        CallMainMenu();
        actualStatus = ActualTBSStatus.MainMenu;
    }

    // Update is called once per frame
    void Update()
    {
        if (InputManager.GetInstance().GetSubmitPressed())
        {
            switch (actualStatus)
            {
                case ActualTBSStatus.MainMenu:
                    break;
                case ActualTBSStatus.SkillMenu:
                    break;
                default:
                    break;
            }
        }
    }

    private void CallMainMenu()
    {
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].gameObject.SetActive(true);
        }
        StartCoroutine(SelectMainMenuFirstOption());
    }
    private void HidingMainMenu()
    {
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].gameObject.SetActive(false);
        }
    }

    private void CallSkillMenu()
    {
        for (int i = 0; i < APLimit; i++)
        {
            actionPoints[i].enabled = true;
            arrows[i].enabled = true;
        }
        for (int i = APLimit; i < actionPoints.Length; i++)
        {
            actionPoints[i].enabled = false;
            arrows[i].enabled = false;
        }
        for (int i = 0; i < skillMenuButtons.Length; i++)
        {
            skillMenuButtons[i].gameObject.SetActive(true);
        }
        StartCoroutine(SelectSkillMenuFirstOption());
    }
    private void HidingSkillMenu()
    {
        for (int i = 0; i < actionPoints.Length; i++)
        {
            actionPoints[i].enabled = false;
            arrows[i].enabled = false;
        }
        for (int i = 0; i < skillMenuButtons.Length; i++)
        {
            skillMenuButtons[i].gameObject.SetActive(false);
        }
    }

    private IEnumerator SelectMainMenuFirstOption()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(mainMenuButtons[0].gameObject);
    }

    public void ChooseMainMenuButton(int mainMenuButtonIndex)
    {
        switch (mainMenuButtonIndex)
        {
            case 1:
                HidingMainMenu();
                actualStatus = ActualTBSStatus.SkillMenu;
                CallSkillMenu();
                break;
            default:
                break;
        }
    }

    private IEnumerator SelectSkillMenuFirstOption()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(skillMenuButtons[0].gameObject);
    }

    public void ChooseSkillMenuButton(int skillMenuButtonIndex)
    {
        int[] sequences = new int[0];
        switch (skillMenuButtonIndex)
        {
            case 0:
                sequences = new int[] { 1, 2, 3 };
                SettingSkill(sequences);
                break;
            case 1:
                sequences = new int[] { 2, 2, 1 };
                SettingSkill(sequences);
                break;
            case 2:
                sequences = new int[] { 4, 4, 3 };
                SettingSkill(sequences);
                break;
            // Erase the entire combo
            case 3:
                APindex = 0;
                sequences = new int[] {0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
                SettingSkill(sequences);
                APindex = 0;
                break;
            case 4:
                HidingSkillMenu();
                actualStatus = ActualTBSStatus.SkillMenu;
                CallMainMenu();
                break;
            default:
                break;
        }
    }

    public void SettingSkill(int[] sequences)
    {
        foreach (int sequence in sequences)
        {
            if (APindex < APLimit)
            {
                SettingArrow(APindex, sequence);
                APindex++;
            } else
            {
                break;
            }
            
        }
        ArrowSpriteManager.GetInstance().ChangeArrowSprite("");
    }

    public void SettingArrow(int index, int spriteCode)
    {
        arrows[index].color = new Color(arrows[index].color.r, arrows[index].color.g, arrows[index].color.b, 1f);
        switch (spriteCode)
        {
            case 1:
                arrows[index].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_up");
                break;
            case 2:
                arrows[index].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_left");
                break;
            case 3:
                arrows[index].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_right");
                break;
            case 4:
                arrows[index].sprite = ArrowSpriteManager.GetInstance().ChangeArrowSprite("arrow_down");
                break;
            default:
                Debug.Log("apaga");
                arrows[index].color = new Color(arrows[index].color.r, arrows[index].color.g, arrows[index].color.b, 0f);
                break;

        }
        
    }
}
