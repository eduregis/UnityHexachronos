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
    Block,
    EnemiesTurn
}

public class CombatUIManager : MonoBehaviour
{
    [Header("Main Menu UI")]
    [SerializeField] private GameObject[] mainMenuButtons;

    [Header("Skill Menu UI")]
    [SerializeField] private GameObject[] skillMenuButtons;
    [SerializeField] private Image[] actionPoints;
    [SerializeField] private Image[] arrows;
    [SerializeField] private GameObject backgroundSkillDescription;
    [SerializeField] private TextMeshProUGUI textSkillDescription;

    [Header("Attack Target Menu UI")]
    [SerializeField] private GameObject[] enemiesSpotted;

    [Header("Turn Indicator")]
    [SerializeField] private TextMeshProUGUI turnIndicator;

    private List<int> skill1;
    private List<int> skill2;
    private List<int> skill3;

    // Control Variables
    private int APLimit;
    private int APindex;
    private List<int> arrowCodes;
    private List<int> comboList;
    private int skillButtonIndex = 0;
    private int attackTargetIndex = 0;

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

        APLimit = actualCharacter.APSlots;
        APindex = 0;
        arrowCodes = new List<int>();
        comboList = new List<int>();

        skill1 = actualCharacter.skillList[0].sequence;
        skill2 = actualCharacter.skillList[1].sequence;
        skill3 = actualCharacter.skillList[2].sequence;
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
            case ActualTBSStatus.Skill:
                CheckingTarget();
                break;
            case ActualTBSStatus.Attack:
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
                StartCoroutine(CallAttackMenu());
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

        backgroundSkillDescription.SetActive(true);
        textSkillDescription.text = actualCharacter.skillList[0].description;

        StartCoroutine(SelectSkillMenuFirstOption());

        HighlightSkillArrows();

        turnIndicator.text = "";
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
        backgroundSkillDescription.SetActive(false);
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
            case 0:
                SettingSkill(skill1, 1);
                break;
            case 1:
                SettingSkill(skill2, 2);
                break;
            case 2:
                SettingSkill(skill3, 3);
                break;
            // Erase the entire combo
            case 3:
                comboList.Clear();
                ClearComboArray();
                break;
            case 4:
                HidingSkillMenu();
                PrintCombo();
                // faz os combos
                actualStatus = ActualTBSStatus.SkillMenu;
                ClearComboArray();
                StartCoroutine(ExecutingCombo());
                break;
            default:
                break;
        }
    }

    public void HighlightSkillArrows()
    {
        SkillArrowsManager.GetInstance().ShowSkillCombo(skill1, 1, CheckingChainCombo(skill1));
        SkillArrowsManager.GetInstance().ShowSkillCombo(skill2, 2, CheckingChainCombo(skill2));
        SkillArrowsManager.GetInstance().ShowSkillCombo(skill3, 3, CheckingChainCombo(skill3));
    }

    public void PrintCombo()
    {
        String combo = "";
        foreach (int x in comboList)
        {
            combo += (" " + x.ToString());
        }
        Debug.Log(combo);
    }

    public void ClearComboArray()
    {
        List<int> sequences = new List<int>();
        APindex = 0;
        sequences = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
        SettingSkill(sequences, 0);
        arrowCodes.Clear();
        APindex = 0;
    }

    public void SettingSkill(List<int> sequences, int techniqueCode)
    {
        // verifica a quantidade de posi��es que podemos recuar ao introduzir essa sequencia no combo.
        if (APindex != APLimit)
        {
            APindex -= CheckingChainCombo(sequences);
            bool canHighlight = false;

            // testa se a sequencia entra inteira no escopo de a��es.
            if ((APindex + sequences.Count <= APLimit))
            {
                if (techniqueCode != 0)
                {
                    canHighlight = true;
                    comboList.Add(techniqueCode);
                }

            }

            foreach (int sequence in sequences)
            {
                if (APindex < APLimit)
                {
                    SettingArrow(APindex, sequence);
                    APindex++;
                }
                else
                {
                    break;
                }
            }
            if (canHighlight) { HighlightSkillArrows(); }
        }
    }

    public int CheckingChainCombo(List<int> sequences)
    {
        if ((APindex > 0) && (APindex + 1 < APLimit))
        {
            // Vari�veis de controle
            // chainSize vai checar o tamanho da sequencia a ser colocada, para iterar em cima desse numero e saber se existem semelhan�as entre o final do combo e o come�o da sequ�ncia.
            int chainSize = sequences.Count - 1;
            // suffixIndex pega o tamanho atual do combo.
            int suffixIndex = arrowCodes.Count;
            // prefixIndex subtrai o tamanho da sequencia ( menos 1) e o tamanho atual do combo, para saber a partir de posi��o do combo come�amos a testar a semelhan�a dos combos.
            int prefixIndex = suffixIndex - chainSize;
            // Essa vari�vel recebe 'true' se a sequencia e combo coincidirem;
            bool isChained = false;
            // chainBreaker inicia com 0, e � acrescida de 1 toda vez que a sequencia n�o coincidir com combo, utilizada para avan�ar uma casa do combo antes de testar novamente.
            int chainBreaker = 0;

            //a cada falha de teste, a chainBreaker se aproxima de chainSize
            while (chainSize > chainBreaker)
            {
                for (int index = 0; (index + chainBreaker) < chainSize; index++)
                {
                    // testamos se o prefixo da sequencia � semelhante ao sufixo do combo.
                    isChained = (sequences[index] == arrowCodes[index + prefixIndex + chainBreaker]);

                }
                if (isChained)
                {
                    // se passar por todo o for e a variavel isChained ainda for 'true', retornamos o tamanho que o APindex dever� recuar.
                    return chainSize - chainBreaker;
                } else
                {
                    // se falhar no teste, avan�amos a posi��o inicial testada em 1.
                    chainBreaker++;
                }
            }
        }
        return 0;
    }

    public void SettingArrow(int index, int spriteCode)
    {
        arrowCodes.Add(spriteCode);
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
                arrows[index].color = new Color(arrows[index].color.r, arrows[index].color.g, arrows[index].color.b, 0f);
                break;
        }
    }

    private void CheckingSkillDescription() 
    { 
        for (int i = 0; i < skillMenuButtons.Length - 2; i++)
        {
            if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skillMenuButtons[i].gameObject))
            {
                if (i != skillButtonIndex)
                {
                    skillButtonIndex = i;
                    textSkillDescription.text = actualCharacter.skillList[i].description;
                }
            }
        }
    }

    public IEnumerator ExecutingCombo()
    {
        actualStatus = ActualTBSStatus.Skill;
        yield return new WaitForSeconds(0.5f);
        StartCoroutine(CallAttackMenu());
        /*foreach (int combo in comboList)
        {
            turnIndicator.text = "Executando t�cnica: " + combo;
            yield return new WaitForSeconds(1.0f);
        }
        comboList.Clear();
        StartCoroutine(UpdateCurrentCharacter());
        yield return new WaitForSeconds(0.2f);
        StartCoroutine(CallMainMenu());*/
    }


    // Attacking Methods
    private IEnumerator CallAttackMenu()
    {
        turnIndicator.text = "Selecione um alvo";
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
        Debug.Log(actualStatus);
        if (actualStatus == ActualTBSStatus.Attack)
        {
            StartCoroutine(Attacking(attackTargetMenuButtonIndex));
        } else if (actualStatus == ActualTBSStatus.Skill)
        {
            Debug.Log("vapo");
        }


    }

    private IEnumerator Attacking(int attackTargetMenuButtonIndex)
    {
        turnIndicator.text = "Atacando";
        CombatCharManager.GetInstance().ShowEnemyTarget(-1);
        yield return new WaitForSeconds(0.5f);
        CombatCharManager.GetInstance().LoseHP(actualCharacter, attackTargetMenuButtonIndex, true);
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
            turnIndicator.text = "A��o do inimigo: " + enemy.char_name;

            System.Random rnd = new System.Random();
            int targetIndex = rnd.Next(0, heroes.Count - 1);

            CombatCharManager.GetInstance().LoseHP(enemy, targetIndex, false);
            yield return new WaitForSeconds(1.0f);
            turnIndicator.text = "";
            yield return new WaitForSeconds(0.1f);
        }
        CombatCharManager.GetInstance().SetPlayerTurn();
    }

}