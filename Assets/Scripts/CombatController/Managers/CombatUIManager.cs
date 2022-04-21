using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public enum ActualTBSStatus
{
    MainMenu,
    Attack,
    SkillMenu,
    Skill,
    AllySingleTargetSkill,
    AllyMultiTargetSkill,
    EnemySingleTargetSkill,
    EnemyMultiTargetSkill,
    Block,
    EnemiesTurn
}

public class CombatUIManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private GameObject[] mainMenuButtons;

    [Header("Skill Menu UI")]
    [SerializeField] private GameObject[] skillMenuButtons;
    [SerializeField] private GameObject backgroundSkillDescription;
    [SerializeField] private TextMeshProUGUI textSkillDescription;

    [Header("Attack Target Menu UI")]
    [SerializeField] private GameObject[] enemiesSpotted;

    [Header("Turn Indicator")]
    [SerializeField] private TextMeshProUGUI turnIndicator;


    // Control Variables

    private int skillButtonIndex = 1;
    private int attackTargetIndex = 0;

    private int selectedSkill = -1;

    private CharacterInfo actualCharacter;
    private int numberOfAllies = 0;
    private int numberOfEnemies = 0;

    private static CombatUIManager instance;
    private ActualTBSStatus actualStatus;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Found more than one CombatUIManager in the scene");
        }
        instance = this;
    }
    public static CombatUIManager GetInstance()
    {
        return instance;
    }

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Setup());
    }

    public IEnumerator Setup()
    {
        HidingSkillMenu();

        yield return new WaitForSeconds(0.5f);
        GetCurrentCharacter();

        numberOfAllies = CombatCharManager.GetInstance().GetNumberOfAllies();
        numberOfEnemies = CombatCharManager.GetInstance().GetNumberOfEnemies();

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CallMainMenu());
        actualStatus = ActualTBSStatus.MainMenu;
    }

    public void GetCurrentCharacter()
    {

        actualCharacter = CombatCharManager.GetInstance().GetCurrentCharacter();

        Debug.Log(actualCharacter.char_name);
    }

    public IEnumerator UpdateCurrentCharacter()
    {
        CombatCharManager.GetInstance().RotateCharacters();

        yield return new WaitForSeconds(0.5f);

        GetCurrentCharacter();
    }

    // Update is called once per frame
    void Update()
    {
        switch (actualStatus)
        {
            case ActualTBSStatus.MainMenu:
                break;
            case ActualTBSStatus.SkillMenu:
                CheckingSkillDescription();
                break;
            case ActualTBSStatus.Attack:
                CheckingTarget();
                break;
            case ActualTBSStatus.EnemySingleTargetSkill:
                CheckingTarget();
                break;
            case ActualTBSStatus.EnemiesTurn:
                CheckingIsPlayerTurn();
                break;
            default:
                break;
        }
    }

    // Main Menu Methods
    private IEnumerator CallMainMenu()
    {
        if (CombatCharManager.GetInstance().IsPlayerTurn())
        {
            actualStatus = ActualTBSStatus.MainMenu;

            turnIndicator.text = "Menu Principal";

            yield return new WaitForSeconds(1.0f);
            for (int i = 0; i < mainMenuButtons.Length; i++)
            {
                mainMenuButtons[i].gameObject.SetActive(true);
            }
            StartCoroutine(SelectMainMenuFirstOption());

            turnIndicator.text = "";
        } else
        {
            actualStatus = ActualTBSStatus.EnemiesTurn;
            yield return new WaitForSeconds(1.0f);
            turnIndicator.text = "Turno dos inimigos";

            StartCoroutine(EnemyTurnActions());
        }


    }

    private void HidingMainMenu()
    {
        for (int i = 0; i < mainMenuButtons.Length; i++)
        {
            mainMenuButtons[i].gameObject.SetActive(false);
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
            case 0:
                HidingMainMenu();
                actualStatus = ActualTBSStatus.Attack;
                StartCoroutine(CallEnemyTargetMenu());
                break;
            case 1:
                HidingMainMenu();
                actualStatus = ActualTBSStatus.SkillMenu;
                StartCoroutine(CallSkillMenu());
                break;
            case 2:
                HidingMainMenu();
                actualStatus = ActualTBSStatus.Block;
                StartCoroutine(Blocking());
                break;
            default:
                break;
        }
    }

    // Skill Menu Methods
    private IEnumerator CallSkillMenu()
    {
        turnIndicator.text = "Menu de Skills";

        yield return new WaitForSeconds(1.0f);

        for (int i = 0; i < skillMenuButtons.Length; i++)
        {
            skillMenuButtons[i].gameObject.SetActive(true);
        }

        backgroundSkillDescription.SetActive(true);
        textSkillDescription.text = actualCharacter.skillList[0].description;

        StartCoroutine(SelectSkillMenuFirstOption());

        turnIndicator.text = "";
    }

    private void HidingSkillMenu()
    {
        for (int i = 0; i < skillMenuButtons.Length; i++)
        {
            skillMenuButtons[i].gameObject.SetActive(false);
        }
        backgroundSkillDescription.SetActive(false);
    }

    private IEnumerator SelectSkillMenuFirstOption()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(skillMenuButtons[1].gameObject);
    }

    public void ChooseSkillMenuButton(int skillMenuButtonIndex)
    {
        if (skillMenuButtonIndex == 0)
        {
            HidingSkillMenu();
            actualStatus = ActualTBSStatus.Attack;
            StartCoroutine(CallMainMenu());
        } else
        {
            actualStatus = ActualTBSStatus.Skill;
            StartCoroutine(ExecutingSkill(skillMenuButtonIndex));
        }


    }

    private void CheckingSkillDescription() 
    { 
        for (int i = 0; i < skillMenuButtons.Length; i++)
        {
            if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skillMenuButtons[i].gameObject))
            {
                if (i == 0)
                {
                    textSkillDescription.text = "";
                }
                if (i != (skillButtonIndex))
                {
                    skillButtonIndex = i;
                    if (i != 0) { textSkillDescription.text = actualCharacter.skillList[i - 1].description; } 
                }
            }
        }
    }

    public IEnumerator ExecutingSkill(int skillIndex)
    {
        HidingSkillMenu();
        actualStatus = ActualTBSStatus.Skill;
        yield return new WaitForSeconds(0.5f);
        selectedSkill = skillIndex - 1;
        // checar antes qual o input da skill
        switch(actualCharacter.skillList[selectedSkill].affectType)
        {
            case AffectType.EnemyTarget:
                actualStatus = ActualTBSStatus.EnemySingleTargetSkill;
                StartCoroutine(CallEnemyTargetMenu());
                break;
            default:
                break;
        }
    }


    // Attacking Methods
    private IEnumerator CallEnemyTargetMenu()
    {
        if (actualStatus == ActualTBSStatus.Attack)
        {
            turnIndicator.text = "Selecione um alvo";
        }
        else if (actualStatus == ActualTBSStatus.EnemySingleTargetSkill)
        {
            turnIndicator.text = "Selecione um alvo da skill";
        }
            
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < numberOfEnemies; i++)
        {
            enemiesSpotted[i].gameObject.SetActive(true);
        }
        for (int i = numberOfEnemies; i < enemiesSpotted.Length; i++)
        {
            enemiesSpotted[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectAttackTargetMenuFirstOption());
    }

    private void HidingAttackTargetMenu()
    {
        for (int i = 0; i < enemiesSpotted.Length; i++)
        {
            enemiesSpotted[i].gameObject.SetActive(false);
        }
    }

    public void ChooseAttackTargetMenuButton(int attackTargetMenuButtonIndex)
    {
        if (actualStatus == ActualTBSStatus.Attack)
        {
            StartCoroutine(Attacking(attackTargetMenuButtonIndex));
        }
        else if (actualStatus == ActualTBSStatus.EnemySingleTargetSkill) {
            StartCoroutine(ApllyingEnemySingleTargetSkill(attackTargetMenuButtonIndex));
        }
    }

    private IEnumerator Attacking(int attackTargetMenuButtonIndex)
    {
        turnIndicator.text = "Atacando";
        CombatCharManager.GetInstance().ShowEnemyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        CombatCharManager.GetInstance().BasicAttack(actualCharacter, attackTargetMenuButtonIndex, true);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingAttackTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    private IEnumerator ApllyingEnemySingleTargetSkill(int attackTargetMenuButtonIndex)
    {
        CombatCharManager.GetInstance().ShowEnemyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        SkillManager.GetInstance().TriggerringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, attackTargetMenuButtonIndex, true);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingAttackTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    private void CheckingTarget()
    {
        for (int i = 0; i < enemiesSpotted.Length; i++)
        {
            if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, enemiesSpotted[i].gameObject))
            {
                if (i != attackTargetIndex)
                {
                    attackTargetIndex = i;
                    CombatCharManager.GetInstance().ShowEnemyTarget(i);
                }
            }
        }
    }

    private IEnumerator SelectAttackTargetMenuFirstOption()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(enemiesSpotted[0].gameObject);
        CombatCharManager.GetInstance().ShowEnemyTarget(0);
    }

    // Blocking Methods
    private IEnumerator Blocking()
    {
        turnIndicator.text = "Bloqueando";
        yield return new WaitForSeconds(1.0f);
        turnIndicator.text = "";
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    // Enemeies Turn Methods
    private void CheckingIsPlayerTurn()
    {
        if (CombatCharManager.GetInstance().IsPlayerTurn())
        {
            actualStatus = ActualTBSStatus.MainMenu;
            StartCoroutine(CallMainMenu());
        }
    }

    private IEnumerator EnemyTurnActions()
    {
        List<CharacterInfo> heroes = CombatCharManager.GetInstance().heroes;

        foreach (CharacterInfo enemy in CombatCharManager.GetInstance().enemies)
        {
            turnIndicator.text = "Ação do inimigo: " + enemy.char_name;

            System.Random rnd = new System.Random();
            int targetIndex = rnd.Next(0, heroes.Count - 1);

            CombatCharManager.GetInstance().BasicAttack(enemy, targetIndex, false);
            yield return new WaitForSeconds(1.0f);
            turnIndicator.text = "";
            yield return new WaitForSeconds(0.1f);
        }
        CombatCharManager.GetInstance().SetPlayerTurn();
    }

}