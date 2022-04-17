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

    [Header("Character Info")]
    [SerializeField] private List<int> skill1;
    [SerializeField] private List<int> skill2;
    [SerializeField] private List<int> skill3;

    // Control Variables
    private int APLimit;
    private int APindex;
    private List<int> arrowCodes;
    private List<int> comboList;

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
        arrowCodes = new List<int>();
        comboList = new List<int>();

        skill1 = new List<int> { 2, 1, 3 };
        skill2 = new List<int> { 2, 2, 1 };
        skill3 = new List<int> { 4, 4, 3 };

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

        HighlightSkillArrows();
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
                ClearComboArray();
                break;
            case 4:
                HidingSkillMenu();
                PrintCombo();
                // faz os combos
                actualStatus = ActualTBSStatus.SkillMenu;
                ClearComboArray();
                CallMainMenu();
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
        foreach(int x in comboList)
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
        comboList.Clear();
        APindex = 0;
    }

    public void SettingSkill(List<int> sequences, int techniqueCode)
    {
        // verifica a quantidade de posições que podemos recuar ao introduzir essa sequencia no combo.
        if (APindex != APLimit)
        {
            APindex -= CheckingChainCombo(sequences);
            bool canHighlight = false;

            // testa se a sequencia entra inteira no escopo de ações.
            if ((APindex + sequences.Count < APLimit) && techniqueCode != 0)
            {
                canHighlight = true;
                comboList.Add(techniqueCode);
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
        if (APindex > 0)
        {
            // Variáveis de controle
            // chainSize vai checar o tamanho da sequencia a ser colocada, para iterar em cima desse numero e saber se existem semelhanças entre o final do combo e o começo da sequência.
            int chainSize = sequences.Count - 1;
            // suffixIndex pega o tamanho atual do combo.
            int suffixIndex = arrowCodes.Count;
            // prefixIndex subtrai o tamanho da sequencia ( menos 1) e o tamanho atual do combo, para saber a partir de posição do combo começamos a testar a semelhança dos combos.
            int prefixIndex = suffixIndex - chainSize;
            // Essa variável recebe 'true' se a sequencia e combo coincidirem;
            bool isChained = false;
            // chainBreaker inicia com 0, e é acrescida de 1 toda vez que a sequencia não coincidir com combo, utilizada para avançar uma casa do combo antes de testar novamente.
            int chainBreaker = 0;
            
            //a cada falha de teste, a chainBreaker se aproxima de chainSize
            while(chainSize > chainBreaker)
            {
                for (int index = 0; (index + chainBreaker) < chainSize; index++)
                {
                    // testamos se o prefixo da sequencia é semelhante ao sufixo do combo.
                    isChained = (sequences[index] == arrowCodes[index + prefixIndex + chainBreaker]);

                }
                if (isChained)
                {
                    // se passar por todo o for e a variavel isChained ainda for 'true', retornamos o tamanho que o APindex deverá recuar.
                    return chainSize - chainBreaker;
                } else
                {
                    // se falhar no teste, avançamos a posição inicial testada em 1.
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
}
