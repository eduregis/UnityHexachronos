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

    [Header("SkillMenu UI")]
    [SerializeField] private GameObject[] skillMenuButtons;
    [SerializeField] private Image[] actionPoints;

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
        for (int i = 0; i < actionPoints.Length; i++)
        {
            actionPoints[i].enabled = true;
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
        switch (skillMenuButtonIndex)
        {
            case 4:
                HidingSkillMenu();
                actualStatus = ActualTBSStatus.SkillMenu;
                CallMainMenu();
                break;
            default:
                break;
        }
    }
}
