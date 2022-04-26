using System.Collections;
using System.Collections.Generic;
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
    SelfTargetSkill,
    Block,
    EnemiesTurn
}

public class CombatUIManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private GameObject[] mainMenuButtons;
    [SerializeField] private Image charPortrait;

    [Header("Skill Menu UI")]
    [SerializeField] private GameObject[] skillMenuButtons;
    [SerializeField] private GameObject backgroundSkillDescription;
    [SerializeField] private TextMeshProUGUI textSkillDescription;

    [Header("Attack Target Menu UI")]
    [SerializeField] private GameObject[] enemiesSpotted;

    [Header("Allies Target Menu UI")]
    [SerializeField] private GameObject[] heroesSpotted;

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
        StartCoroutine(GetCurrentCharacter());

        numberOfAllies = CombatCharManager.GetInstance().GetNumberOfAllies();
        numberOfEnemies = CombatCharManager.GetInstance().GetNumberOfEnemies();

        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CallMainMenu());
        actualStatus = ActualTBSStatus.MainMenu;
    }

    public IEnumerator GetCurrentCharacter()
    {
        actualCharacter = CombatCharManager.GetInstance().GetCurrentCharacter();

        yield return new WaitForSeconds(0.2f);

        if (!IsTheHeroAbleToFight())
        {
            StartCoroutine(PassTurn());
        }
    }

    public IEnumerator UpdateCurrentCharacter()
    {
        CombatCharManager.GetInstance().GoToNextCharacter();

        if (CombatCharManager.GetInstance().IsPlayerTurn())
        {
            CombatCharManager.GetInstance().RotateCharacters();
        }

        yield return new WaitForSeconds(0.5f);

        StartCoroutine(GetCurrentCharacter());

        TranslateAllySpotted();

        if (CombatCharManager.GetInstance().IsItLastHero())
        {
            CombatCharManager.GetInstance().BuffListHeroIterator();
            TestingHeroesNegativeStatus();
        }
    }

    private void TranslateAllySpotted()
    {
        Debug.Log(actualCharacter.char_name);

        int numberOfAllies = CombatCharManager.GetInstance().GetNumberOfAllies();

        Vector3 pos_0 = heroesSpotted[0].transform.position;
        Vector3 pos_1 = heroesSpotted[1].transform.position;
        Vector3 pos_2 = heroesSpotted[2].transform.position;

        if (numberOfAllies == 3)
        {
            heroesSpotted[0].transform.position = pos_2;
            heroesSpotted[1].transform.position = pos_0;
            heroesSpotted[2].transform.position = pos_1;
        } else if (numberOfAllies == 2)
        {
            heroesSpotted[1].transform.position = pos_0;
            heroesSpotted[0].transform.position = pos_1;
        }
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
                CheckingEnemyTarget();
                break;
            case ActualTBSStatus.EnemySingleTargetSkill:
                CheckingEnemyTarget();
                break;
            case ActualTBSStatus.AllySingleTargetSkill:
                CheckingAllyTarget();
                break;
            case ActualTBSStatus.SelfTargetSkill:
                CheckingAllyTarget();
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

            if (IsTheHeroAbleToFight())
            {
                for (int i = 0; i < mainMenuButtons.Length; i++)
                {
                    mainMenuButtons[i].gameObject.SetActive(true);
                }
                StartCoroutine(SelectMainMenuFirstOption());
            }
            turnIndicator.text = "";
        }
        else
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
        switch (actualCharacter.skillList[selectedSkill].affectType)
        {
            case AffectType.EnemyTarget:
                actualStatus = ActualTBSStatus.EnemySingleTargetSkill;
                StartCoroutine(CallEnemyTargetMenu());
                break;
            case AffectType.AllyTarget:
                actualStatus = ActualTBSStatus.AllySingleTargetSkill;
                StartCoroutine(CallAllyTargetMenu());
                break;
            case AffectType.AllEnemies:
                actualStatus = ActualTBSStatus.EnemyMultiTargetSkill;
                StartCoroutine(CallAllEnemiesTargetMenu());
                break;
            case AffectType.AllAllies:
                actualStatus = ActualTBSStatus.AllyMultiTargetSkill;
                StartCoroutine(CallAllAlliesTargetMenu());
                break;
            case AffectType.Self:
                actualStatus = ActualTBSStatus.SelfTargetSkill;
                StartCoroutine(CallSelfTargetMenu());
                break;
            default:
                break;
        }
    }

    // Skill With Ally Targets
    private IEnumerator CallAllyTargetMenu()
    {
        turnIndicator.text = "Selecione um alvo da skill";
        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < numberOfAllies; i++)
        {
            heroesSpotted[i].gameObject.SetActive(true);
        }
        for (int i = numberOfAllies; i < heroesSpotted.Length; i++)
        {
            heroesSpotted[i].gameObject.SetActive(false);
        }
        StartCoroutine(SelectAllyTargetMenuFirstOption());
    }

    private void HidingAllyTargetMenu()
    {
        for (int i = 0; i < heroesSpotted.Length; i++)
        {
            heroesSpotted[i].gameObject.SetActive(false);
        }
    }

    public void ChooseAllyTargetMenuButton(int allyTargetMenuButtonIndex)
    {
        if (actualStatus == ActualTBSStatus.AllySingleTargetSkill)
        {
            StartCoroutine(ApllyingAllySingleTargetSkill(allyTargetMenuButtonIndex));
        }
        else if (actualStatus == ActualTBSStatus.AllyMultiTargetSkill)
        {
            StartCoroutine(ApllyingAlliesMultiTargetSkill());
        }
        else if (actualStatus == ActualTBSStatus.SelfTargetSkill)
        {
            StartCoroutine(ApllyingSelfTargetSkill());
        }
    }

    private IEnumerator ApllyingAllySingleTargetSkill(int allyTargetMenuButtonIndex)
    {
        CombatCharManager.GetInstance().ShowAllyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        SkillManager.GetInstance().TriggeringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, allyTargetMenuButtonIndex, false);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingAllyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    private void CheckingAllyTarget()
    {
        for (int i = 0; i < heroesSpotted.Length; i++)
        {
            if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, heroesSpotted[i].gameObject))
            {
                if (i != attackTargetIndex)
                {
                    attackTargetIndex = i;
                    CombatCharManager.GetInstance().ShowAllyTarget(i);
                }
            }
        }
    }

    private IEnumerator SelectAllyTargetMenuFirstOption()
    {
        // Event System requires we clear it first, then wait
        // for at least one frame before we set the current selected object.
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(heroesSpotted[0].gameObject);
        CombatCharManager.GetInstance().ShowAllyTarget(0);
    }

    // Skill With Multi Enemies Targets

    private IEnumerator CallAllEnemiesTargetMenu()
    {
        turnIndicator.text = "A skill marca todos os alvos";

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < enemiesSpotted.Length; i++)
        {
            enemiesSpotted[i].gameObject.SetActive(true);
        }
        StartCoroutine(SelectAllAttackTargetMenu());
    }

    private IEnumerator SelectAllAttackTargetMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(enemiesSpotted[0].gameObject);
        CombatCharManager.GetInstance().ShowAllEnemiesTarget();
    }

    private IEnumerator ApllyingEnemyMultiTargetSkill()
    {
        CombatCharManager.GetInstance().ShowEnemyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        SkillManager.GetInstance().TriggeringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, 0, true);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingEnemyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    // Skill With Self Targets

    private IEnumerator CallSelfTargetMenu()
    {
        turnIndicator.text = "A skill marca a si mesmo";

        yield return new WaitForSeconds(1.0f);

        int targetIndex = CombatCharManager.GetInstance().GetHeroesIndex();

        for (int i = 0; i < heroesSpotted.Length; i++)
        {
            if (i == targetIndex)
            {
                heroesSpotted[i].gameObject.SetActive(true);
            } else
            {
                heroesSpotted[i].gameObject.SetActive(false);
            }
            
        }
        StartCoroutine(SelectSelfTargetMenu(targetIndex));
    }

    private IEnumerator SelectSelfTargetMenu(int targetIndex)
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(heroesSpotted[targetIndex].gameObject);
        CombatCharManager.GetInstance().ShowAllyTarget(targetIndex);
    }

    private IEnumerator ApllyingSelfTargetSkill()
    {
        CombatCharManager.GetInstance().ShowAllyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        int targetIndex = CombatCharManager.GetInstance().GetHeroesIndex();
        SkillManager.GetInstance().TriggeringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, targetIndex, false);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingAllyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    // Skill With Multi Allies Targets

    private IEnumerator CallAllAlliesTargetMenu()
    {
        turnIndicator.text = "A skill marca todos os alvos";

        yield return new WaitForSeconds(1.0f);
        for (int i = 0; i < heroesSpotted.Length; i++)
        {
            heroesSpotted[i].gameObject.SetActive(true);
        }
        StartCoroutine(SelectAllAlliesTargetMenu());
    }

    private IEnumerator SelectAllAlliesTargetMenu()
    {
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(heroesSpotted[0].gameObject);
        CombatCharManager.GetInstance().ShowAllAlliesTarget();
    }

    private IEnumerator ApllyingAlliesMultiTargetSkill()
    {
        CombatCharManager.GetInstance().ShowAllyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        SkillManager.GetInstance().TriggeringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, 0, false);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingAllyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
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

    private void HidingEnemyTargetMenu()
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
        else if (actualStatus == ActualTBSStatus.EnemyMultiTargetSkill)
        {
            StartCoroutine(ApllyingEnemyMultiTargetSkill());
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
        HidingEnemyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    private IEnumerator ApllyingEnemySingleTargetSkill(int attackTargetMenuButtonIndex)
    {
        CombatCharManager.GetInstance().ShowEnemyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        SkillManager.GetInstance().TriggeringSkill(actualCharacter.skillList[selectedSkill].skill_id, actualCharacter, attackTargetMenuButtonIndex, true);
        turnIndicator.text = "";
        yield return new WaitForSeconds(0.5f);
        HidingEnemyTargetMenu();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());
    }

    private void CheckingEnemyTarget()
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

    private IEnumerator PassTurn()
    {
        yield return new WaitForSeconds(1.0f);
        turnIndicator.text = "";
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        if (!CombatCharManager.GetInstance().IsItLastHero())
        {
            StartCoroutine(CallMainMenu());
        }
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
        HidingMainMenu();

        List<CharacterInfo> heroes = CombatCharManager.GetInstance().heroes;

        foreach (CharacterInfo enemy in CombatCharManager.GetInstance().enemies)
        {
            CombatCharManager.GetInstance().RotateEnemies();

            bool isAbleTo = true;

            yield return new WaitForSeconds(1.0f);

            CombatCharManager.GetInstance().BuffListEnemyIterator();

            if (enemy.life == 0)
            {
                isAbleTo = false;
            }
            else 
            {
                foreach (Buff buff in enemy.buffList)
                {
                    if (buff.modifier == BuffModifier.Status)
                    {
                        if (buff.buffType == BuffType.Stunned)
                        {
                            isAbleTo = false;
                        }
                    }
                    if (buff.buffType == BuffType.Bleeding)
                    {
                        CombatCharManager.GetInstance().LoseHP((int)buff.value, CombatCharManager.GetInstance().GetEnemiesIndex(), true);
                    }
                }
            }

            if (isAbleTo)
            {
                turnIndicator.text = "Ação do inimigo: " + enemy.char_name;
                int targetIndex = Random.Range(0, heroes.Count);

                for (int i = 0; i < heroes.Count; i++)
                {
                    if (CombatCharManager.GetInstance().IsTauntActive(i))
                    {
                        targetIndex = i;
                    }
                }

                CombatCharManager.GetInstance().BasicAttack(enemy, targetIndex, false);
            }

            yield return new WaitForSeconds(1.0f);
            CombatCharManager.GetInstance().GoToNextEnemy();
            turnIndicator.text = "";
            yield return new WaitForSeconds(1.0f);
        }

        CombatCharManager.GetInstance().RotateEnemies();
        CombatCharManager.GetInstance().RotateCharacters();
        CombatCharManager.GetInstance().SetPlayerTurn();

        CombatCharManager.GetInstance().BuffListHeroIterator();
        TestingHeroesNegativeStatus();

        if (!IsTheHeroAbleToFight())
        {
            StartCoroutine(PassTurn());
        }
    }

    public bool IsTheHeroAbleToFight()
    {
        if (actualCharacter.life == 0)
        {
            return false;
        }
        else
        {
            foreach (Buff buff in actualCharacter.buffList)
            {
                if (buff.modifier == BuffModifier.Status)
                {
                    if (buff.buffType == BuffType.Stunned)
                    {
                        return false;
                    } 
                }
            }
        }
        return true;
    }


    public void TestingHeroesNegativeStatus ()
    {
        foreach (Buff buff in actualCharacter.buffList)
        {
            if (buff.modifier == BuffModifier.Status)
            {
                if (buff.buffType == BuffType.Bleeding)
                {
                    CombatCharManager.GetInstance().LoseHP((int)buff.value, CombatCharManager.GetInstance().GetHeroesIndex(), false);
                }
            }
        }
    }

}