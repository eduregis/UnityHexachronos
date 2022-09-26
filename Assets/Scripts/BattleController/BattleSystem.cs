using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public enum BattleState { 
    START,
    HERO1TURN,
    HERO2TURN,
    HERO3TURN,
    ENEMY1TURN,
    ENEMY2TURN,
    ENEMY3TURN,
    WON,
    LOST
} 

public enum MenuTargetType {
    ATTACK,
    SKILL
}

public class BattleSystem : MonoBehaviour {

    #region External variables
    [Header("Characters Stats")]
    public CharacterStats hero1;
    public CharacterStats hero2;
    public CharacterStats hero3;
    public CharacterStats enemy1;
    public CharacterStats enemy2;
    public CharacterStats enemy3;

    [Header("Character Sprites")]
    public GameObject hero1Sprite;
    public GameObject hero2Sprite;
    public GameObject hero3Sprite;
    public GameObject enemy1Sprite;
    public GameObject enemy2Sprite;
    public GameObject enemy3Sprite;

    [Header("Auxiliar Text (Will Be Removed)")]
    public TextMeshProUGUI auxText;

    [Header("Menu Panels")]
    public GameObject mainMenuPanel;
    public GameObject skillsMenuPanel;
    public GameObject selectEnemyMenuPanel;
    public GameObject selectHeroMenuPanel;

    [Header("Main Menu Buttons")]
    public Button attackButton;
    public Button skillMenuButton;
    public Button blockButton;

    [Header("Skills Menu Buttons")]
    public Button skill1Button;
    public Button skill2Button;
    public Button skill3Button;
    public Button skill4Button;

    [Header("Skills Menu Button Texts")]
    public TextMeshProUGUI skill1Text;
    public TextMeshProUGUI skill2Text;
    public TextMeshProUGUI skill3Text;
    public TextMeshProUGUI skill4Text;

    [Header("Descipriton Skill Texts")]
    public TextMeshProUGUI descriptionSkill;
    public TextMeshProUGUI nameSkill;
    public TextMeshProUGUI costSkill;
    public TextMeshProUGUI targetSkill;

    [Header("Target Arrows Menu Buttons")]
    public Button hero1Arrow;
    public Button hero2Arrow;
    public Button hero3Arrow;
    public Button enemy1Arrow;
    public Button enemy2Arrow;
    public Button enemy3Arrow;

    [Header("HUDs")]
    public BattleHUD hero1HUD;
    public BattleHUD hero2HUD;
    public BattleHUD hero3HUD;
    public BattleHUD enemy1HUD;
    public BattleHUD enemy2HUD;
    public BattleHUD enemy3HUD;

    public BattleState state;
    #endregion

    #region Control variables

    bool isInSkillsMenu = false;
    bool rotatingHeroes = false;
    MenuTargetType menuTargetType;
    List<CharacterSkill> skills;
    int selectedSkillIndex = 0;
    Color backCharColor = new(0.4f, 0.4f, 0.4f, 1f);

    Vector3 hero1Position;
    Vector3 hero2Position;
    Vector3 hero3Position;

    SpriteRenderer hero1Renderer;
    SpriteRenderer hero2Renderer;
    SpriteRenderer hero3Renderer;

    Vector3 enemy1Position;
    Vector3 enemy2Position;
    Vector3 enemy3Position;

    #endregion

    #region Time variables

    const float START_BATTLE_TIME = 1.2f;
    const float MENU_TO_ENEMYSINGLETARGET_TIME = 0.5f;
    const float MENU_TO_HEROSINGLETARGET_TIME = 0.5f;
    const float MAINMENU_TO_SKILLMENU_TIME = 0.5f;
    const float ENEMYSINGLETARGET_TO_NEXTTURN_TIME = 1f;
    const float HEROSINGLETARGET_TO_NEXTTURN_TIME = 1f;
    const float ALLENEMIES_TO_NEXTTURN_TIME = 1f;
    const float ALLHEROES_TO_NEXTTURN_TIME = 1f;
    const float SELFTARGET_TO_NEXTTURN_TIME = 1f;
    const float ENEMYTURN_TO_ENEMYATTACK_TIME = 1f;
    const float ENEMYATTACK_TO_NEXTTURN_TIME = 1f;
    const float MAINMENU_TO_BLOCKING_TIME = 0.5f;
    const float BLOCKING_TO_NEXTTURN_TIME = 0.5f;
    const float ROTATE_TO_NEXTTURN_TIME = 1f;

    #endregion

    // Start is called before the first frame update
    void Start() {
        state = BattleState.START;

        hero1Position = GetPosition(hero1Sprite);
        hero2Position = GetPosition(hero2Sprite);
        hero3Position = GetPosition(hero3Sprite);

        hero1Renderer = hero1Sprite.GetComponent<SpriteRenderer>();
        hero2Renderer = hero2Sprite.GetComponent<SpriteRenderer>();
        hero3Renderer = hero3Sprite.GetComponent<SpriteRenderer>();

        if (hero3 != null) {
            hero2Renderer.color = backCharColor;
            hero3Renderer.color = backCharColor;
        }

        enemy1Position = GetPosition(enemy1Sprite);
        enemy1Position = GetPosition(enemy1Sprite);
        enemy1Position = GetPosition(enemy1Sprite);

        SetupBattle();
    }

    void Update() {
        if (state == BattleState.HERO1TURN || state == BattleState.HERO2TURN || state == BattleState.HERO3TURN) {
            if (rotatingHeroes == true && hero3 != null) RotateHeroes();
            if (isInSkillsMenu == true) ShowDescriptionSkill();
        }
    }

    #region Turn functions
    void SetupBattle() {

        mainMenuPanel.SetActive(false);
        skillsMenuPanel.SetActive(false);
        selectHeroMenuPanel.SetActive(false);
        selectEnemyMenuPanel.SetActive(false);

        auxText.text = "Starting battle!";

        // setup the UI before start the battle

        if (hero1 != null) {
            hero1.LoadBattleStats();
            SpriteRenderer sptRen = hero1Sprite.GetComponent<SpriteRenderer>();
            hero1Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero1.char_name);
            hero1HUD.SetHUD(hero1);
        }

        if (hero2 != null) {
            hero2.LoadBattleStats();
            hero2Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero2.char_name);
            hero2HUD.SetHUD(hero2);
        }

        if (hero3 != null) {
            hero3.LoadBattleStats();
            hero3Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(hero3.char_name);
            hero3HUD.SetHUD(hero3);
        }

        if (enemy1 != null) {
            enemy1.LoadBattleStats();
            enemy1Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemy1.char_name);
            enemy1HUD.SetHUD(enemy1);
        }

        if (enemy2 != null) {
            enemy2.LoadBattleStats();
            enemy2Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemy2.char_name);
            enemy2HUD.SetHUD(enemy2);
        }

        if (enemy3 != null) {
            enemy3.LoadBattleStats();
            enemy3Sprite.GetComponent<SpriteRenderer>().sprite = CharacterCombatSpriteManager.GetInstance().CharacterSpriteIdleImage(enemy3.char_name);
            enemy3HUD.SetHUD(enemy3);
        }

        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        if (hero1 == null)
            hero1HUD.gameObject.SetActive(false);
        if (hero2 == null)
            hero2HUD.gameObject.SetActive(false);
        if (hero3 == null)
            hero3HUD.gameObject.SetActive(false);
        if (enemy1 == null)
            enemy1HUD.gameObject.SetActive(false);
        if (enemy2 == null)
            enemy2HUD.gameObject.SetActive(false);
        if (enemy3 == null)
            enemy3HUD.gameObject.SetActive(false);

        state = BattleState.HERO1TURN;

        yield return new WaitForSeconds(START_BATTLE_TIME);

        hero1HUD.isHighlighted = true;

        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn() {
        if (IsHeroReadyToAct()) {

            CharacterStats hero = hero1;

            switch(state) {
                case BattleState.HERO1TURN:
                    hero = hero1;
                    auxText.text = hero1.char_name + ": Your turn!";
                    break;
                case BattleState.HERO2TURN:
                    hero = hero2;
                    auxText.text = hero2.char_name + ": Your turn!";
                    break;
                case BattleState.HERO3TURN:
                    hero = hero3;
                    auxText.text = hero3.char_name + ": Your turn!";
                    break;
            }

            if (IsInNegativeStatus(BuffType.Stunned)) {
                auxText.text = state + " stunned!";
                StartCoroutine(NextTurn());
            } else {
                if (IsInNegativeStatus(BuffType.Bleeding)) {
                    hero.TakeDamage(10);
                    auxText.text = state + " bleeding!";
                    UpdateUI();
                }
                mainMenuPanel.SetActive(true);

                EventSystem.current.SetSelectedGameObject(null);
                yield return new WaitForEndOfFrame();
                EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
            }
        } else {
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator PlayerSelectAttackSingleTarget() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(MENU_TO_ENEMYSINGLETARGET_TIME);

        selectEnemyMenuPanel.SetActive(true);

        auxText.text = "Select a enemy target!";

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(enemy1Arrow.gameObject);

        CheckingAvailableEnemyTargets();
    }

    IEnumerator PlayerSelectHeroSingleTarget() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(MENU_TO_HEROSINGLETARGET_TIME);

        selectHeroMenuPanel.SetActive(true);

        auxText.text = "Select a hero target!";

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(hero1Arrow.gameObject);

        CheckingAvailableHeroTargets();
    }
    
    IEnumerator PlayerSelectSkills() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(MAINMENU_TO_SKILLMENU_TIME);

        skillsMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(skill1Button.gameObject);

        if (skills[0] != null)
            skill1Text.text = skills[0].skill_name;
        if (skills[1] != null)
            skill2Text.text = skills[1].skill_name;
        if (skills[2] != null)
            skill3Text.text = skills[2].skill_name;
        if (skills[3] != null)
            skill4Text.text = skills[3].skill_name;
    }

    IEnumerator PlayerActionEnemyTarget(int TargetId)
    {
        selectEnemyMenuPanel.SetActive(false);
        CharacterStats skillUser = hero1;

        switch(state) {
            case BattleState.HERO1TURN:
                skillUser = hero1;
                break;
            case BattleState.HERO2TURN:
                skillUser = hero2;
                break;
            case BattleState.HERO3TURN:
                skillUser = hero3;
                break;
        }

        List<string> messages = new();

        switch (menuTargetType) {
            case MenuTargetType.ATTACK:
                switch (TargetId) {
                    case 1:
                        messages.Add(enemy1.ReceivingAttackDamage(skillUser).ToString());
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy1.char_name, messages);
                        enemy1HUD.UpdateUI(enemy1);
                        break;
                    case 2:
                        messages.Add(enemy2.ReceivingAttackDamage(skillUser).ToString());
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy2.char_name, messages);
                        enemy2HUD.UpdateUI(enemy2);
                        break;
                    case 3:
                        messages.Add(enemy3.ReceivingAttackDamage(skillUser).ToString());
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy3.char_name, messages);
                        enemy3HUD.UpdateUI(enemy3);
                        break;
                    default:
                        break;
                }
                auxText.text = "Attacking!";
                break;
            case MenuTargetType.SKILL:

                ActionCanvasManager.GetInstance().TriggerAction();

                List<string> skillMessages;

                switch (TargetId) {
                    case 1:
                        skillMessages = enemy1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        foreach (string msg in skillMessages) messages.Add(msg);
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy1.char_name, messages);
                        enemy1HUD.UpdateUI(enemy1);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy1"; 
                        break;
                    case 2:
                        skillMessages = enemy2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        foreach (string msg in skillMessages) messages.Add(msg);
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy2.char_name, messages);
                        enemy2HUD.UpdateUI(enemy2);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy2";
                        break;
                    case 3:
                        skillMessages = enemy3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        foreach (string msg in skillMessages) messages.Add(msg);
                        ActionCanvasManager.GetInstance().TriggerEnemySingleTargetAction(skillUser.char_name, enemy3.char_name, messages);
                        enemy3HUD.UpdateUI(enemy3);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy3";
                        break;
                    default:
                        break;
                }
                break;
        }

        yield return new WaitForSeconds(ENEMYSINGLETARGET_TO_NEXTTURN_TIME);

        ActionCanvasManager.GetInstance().DismissAction();

        if (IsAllEnemiesDead()) {
            state = BattleState.WON;
            EndBattle();
        } else {
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator PlayerActionHeroTarget(int TargetId)
    {
        selectHeroMenuPanel.SetActive(false);
        CharacterStats skillUser = hero1;

        ActionCanvasManager.GetInstance().TriggerAction();

        switch (state)
        {
            case BattleState.HERO1TURN:
                skillUser = hero1;
                break;
            case BattleState.HERO2TURN:
                skillUser = hero2;
                break;
            case BattleState.HERO3TURN:
                skillUser = hero3;
                break;
        }

        switch (TargetId) {
            case 1:
                hero1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                hero1HUD.UpdateUI(hero1);
                auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in hero1";
                break;
            case 2:
                hero2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                hero2HUD.UpdateUI(hero2);
                auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in hero2";
                break;
            case 3:
                hero3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                hero3HUD.UpdateUI(hero3);
                auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in hero3";
                break;
            default:
                break;
        }

        yield return new WaitForSeconds(HEROSINGLETARGET_TO_NEXTTURN_TIME);

        ActionCanvasManager.GetInstance().DismissAction();

        if (IsAllHeroesDead()) {
            state = BattleState.LOST;
            EndBattle();
        } else {
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator PlayerActionAllEnemies() {
        CharacterStats skillUser = hero1;

        ActionCanvasManager.GetInstance().TriggerAction();

        switch (state) {
            case BattleState.HERO1TURN:
                skillUser = hero1;
                break;
            case BattleState.HERO2TURN:
                skillUser = hero2;
                break;
            case BattleState.HERO3TURN:
                skillUser = hero3;
                break;
        }

        string enemy1Name = "";
        string enemy2Name = "";
        string enemy3Name = "";

        List<string> enemy1Messages = new();
        List<string> enemy2Messages = new();
        List<string> enemy3Messages = new();

        if (enemy1 != null && enemy1.life > 0) {
            enemy1Name = enemy1.char_name;
            enemy1Messages = enemy1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy1HUD.UpdateUI(enemy1);
        }

        if (enemy2 != null && enemy2.life > 0) {
            enemy2Name = enemy2.char_name;
            enemy2Messages = enemy2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy2HUD.UpdateUI(enemy2);
        }

        if (enemy3 != null && enemy3.life > 0) {
            enemy3Name = enemy3.char_name;
            enemy3Messages = enemy3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy3HUD.UpdateUI(enemy3);
        }

        ActionCanvasManager.GetInstance().TriggerAllEnemiesAction(skillUser.char_name, enemy1Name, enemy2Name, enemy3Name, enemy1Messages, enemy2Messages, enemy3Messages);

        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in all enemies";

        yield return new WaitForSeconds(ALLENEMIES_TO_NEXTTURN_TIME);

        ActionCanvasManager.GetInstance().DismissAction();

        if (IsAllEnemiesDead()) {
            state = BattleState.WON;
            EndBattle();
        }
        else {
            StartCoroutine(NextTurn());
        }
    }

    IEnumerator PlayerActionAllHeroes() {
        CharacterStats skillUser = hero1;

        ActionCanvasManager.GetInstance().TriggerAction();

        switch (state) {
            case BattleState.HERO1TURN:
                skillUser = hero1;
                break;
            case BattleState.HERO2TURN:
                skillUser = hero2;
                break;
            case BattleState.HERO3TURN:
                skillUser = hero3;
                break;
        }

        if (hero1 != null && hero1.life > 0) {
            hero1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            hero1HUD.UpdateUI(hero1);
        }

        if (hero2 != null && hero2.life > 0) {
            hero2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            hero2HUD.UpdateUI(hero2);
        }

        if (hero3 != null && hero3.life > 0) {
            hero3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            hero3HUD.UpdateUI(hero3);
        }

        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in all heroes";

        yield return new WaitForSeconds(ALLHEROES_TO_NEXTTURN_TIME);

        ActionCanvasManager.GetInstance().DismissAction();

        StartCoroutine(NextTurn());
    }

    IEnumerator PlayerActionSelf() {

        ActionCanvasManager.GetInstance().TriggerAction();

        switch (state) {
            case BattleState.HERO1TURN:
                if (hero1 != null && hero1.life > 0) {
                    hero1.ApplySkill(hero1, skills[selectedSkillIndex - 1]);
                    hero1HUD.UpdateUI(hero1);
                }
                break;
            case BattleState.HERO2TURN:
                if (hero2 != null && hero2.life > 0) {
                    hero2.ApplySkill(hero2, skills[selectedSkillIndex - 1]);
                    hero2HUD.UpdateUI(hero2);
                }
                break;
            case BattleState.HERO3TURN:
                if (hero3 != null && hero3.life > 0) {
                    hero3.ApplySkill(hero3, skills[selectedSkillIndex - 1]);
                    hero3HUD.UpdateUI(hero3);
                }
                break;
        }

        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " inself";

        yield return new WaitForSeconds(SELFTARGET_TO_NEXTTURN_TIME);

        ActionCanvasManager.GetInstance().DismissAction();

        StartCoroutine(NextTurn());
    }

    IEnumerator EnemyTurn() {
        if (IsEnemyReadyToAct()) {
            auxText.text = state + " turn!";

            CharacterStats enemy = enemy1;

            switch(state) {
                case BattleState.ENEMY1TURN:
                    enemy = enemy1;
                    break;
                case BattleState.ENEMY2TURN:
                    enemy = enemy2;
                    break;
                case BattleState.ENEMY3TURN:
                    enemy = enemy3;
                    break;
            }

            if (IsInNegativeStatus(BuffType.Bleeding)) {
                enemy.TakeDamage(10);
                auxText.text = state + " bleeding!";
                UpdateUI();
            }
            
            yield return new WaitForSeconds(ENEMYTURN_TO_ENEMYATTACK_TIME);

            if (!IsInNegativeStatus(BuffType.Stunned)) {
                CharacterStats heroTarget = ChoosingATargetHero();
                heroTarget.ReceivingAttackDamage(enemy);
            } else {
                auxText.text = state + "  stunned!";
            }

            UpdateUI();

            yield return new WaitForSeconds(ENEMYATTACK_TO_NEXTTURN_TIME);

            if (IsAllHeroesDead()) {
                state = BattleState.LOST;
                EndBattle();
            } else {
                StartCoroutine(NextTurn());
            }
        } else {
            StartCoroutine(NextTurn());
        }
    }

    private void UpdateUI() {
        if (hero1 != null) hero1HUD.UpdateUI(hero1);
        if (hero2 != null) hero2HUD.UpdateUI(hero2);
        if (hero3 != null) hero3HUD.UpdateUI(hero3);
        if (enemy1 != null) enemy1HUD.UpdateUI(enemy1);
        if (enemy2 != null) enemy2HUD.UpdateUI(enemy2);
        if (enemy3 != null) enemy3HUD.UpdateUI(enemy3);
    }

    private void RotateHeroes() {
        switch (state) {
            case BattleState.HERO1TURN:
                RenderingAndRotateCharacter(hero1Sprite, hero1Renderer, hero3Position, 3);
                RenderingAndRotateCharacter(hero2Sprite, hero2Renderer, hero1Position, 1);
                RenderingAndRotateCharacter(hero3Sprite, hero3Renderer, hero2Position, 2);
                break;
            case BattleState.HERO2TURN:
                RenderingAndRotateCharacter(hero1Sprite, hero1Renderer, hero2Position, 2);
                RenderingAndRotateCharacter(hero2Sprite, hero2Renderer, hero3Position, 3);
                RenderingAndRotateCharacter(hero3Sprite, hero3Renderer, hero1Position, 1);
                break;
            case BattleState.HERO3TURN:
                RenderingAndRotateCharacter(hero1Sprite, hero1Renderer, hero1Position, 1);
                RenderingAndRotateCharacter(hero2Sprite, hero2Renderer, hero2Position, 2);
                RenderingAndRotateCharacter(hero3Sprite, hero3Renderer, hero3Position, 3);
                break;
        }
    }

    private void RenderingAndRotateCharacter(GameObject character, SpriteRenderer renderer, Vector3 position, int newTurn) {
        character.transform.position = Vector3.Lerp(character.transform.position, position, 10f * Time.deltaTime);
        switch (newTurn) {
            case 1:
                renderer.sortingOrder = -8;
                renderer.color = Color.Lerp(renderer.color, Color.white, 4f * Time.deltaTime);
                break;
            case 2:
                renderer.sortingOrder = -9;
                renderer.color = Color.Lerp(renderer.color, backCharColor, 4f * Time.deltaTime);
                break;
            case 3:
                renderer.sortingOrder = -10;
                renderer.color = Color.Lerp(renderer.color, backCharColor, 4f * Time.deltaTime);
                break;
        }

    }

    IEnumerator Blocking() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(MAINMENU_TO_BLOCKING_TIME);

        auxText.text = "Blocking!";

        switch (state) {
            case BattleState.HERO1TURN:
                hero1.isBlocking = true;
                break;
            case BattleState.HERO2TURN:
                hero2.isBlocking = true;
                break;
            case BattleState.HERO3TURN:
                hero3.isBlocking = true;
                break;
        }

        yield return new WaitForSeconds(BLOCKING_TO_NEXTTURN_TIME);
        StartCoroutine(NextTurn());
    }

    IEnumerator NextTurn() {

        if (state == BattleState.HERO1TURN || state == BattleState.HERO2TURN || state == BattleState.HERO3TURN) {
            if (hero3 != null) {
                rotatingHeroes = true;
                yield return new WaitForSeconds(ROTATE_TO_NEXTTURN_TIME);
                RepositioningHeroTargets();
                rotatingHeroes = false;
            }
        }

        switch (state) {
            case BattleState.HERO1TURN:
                hero1HUD.isHighlighted = false;
                state = BattleState.HERO2TURN;
                if (hero2 != null) {
                    hero2.isBlocking = false;
                    hero2.UpdateBuffs();
                    hero2HUD.isHighlighted = true;
                    hero2HUD.UpdateUI(hero2);
                }
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO2TURN:
                hero2HUD.isHighlighted = false;
                state = BattleState.HERO3TURN;
                if (hero3 != null) {
                    hero3.isBlocking = false;
                    hero3.UpdateBuffs();
                    hero3HUD.isHighlighted = true;
                    hero3HUD.UpdateUI(hero3);
                }
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO3TURN:
                hero3HUD.isHighlighted = false;
                state = BattleState.ENEMY1TURN;
                if (enemy1 != null) {
                    enemy1.isBlocking = false;
                    enemy1.UpdateBuffs();
                    enemy1HUD.isHighlighted = true;
                    enemy1HUD.UpdateUI(enemy1);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY1TURN:
                enemy1HUD.isHighlighted = false;
                state = BattleState.ENEMY2TURN;
                if (enemy2 != null) {
                    enemy2.isBlocking = false;
                    enemy2.UpdateBuffs();
                    enemy2HUD.isHighlighted = true;
                    enemy2HUD.UpdateUI(enemy2);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY2TURN:
                enemy2HUD.isHighlighted = false;
                state = BattleState.ENEMY3TURN;
                if (enemy3 != null) {
                    enemy3.isBlocking = false;
                    enemy3.UpdateBuffs();
                    enemy3HUD.isHighlighted = true;
                    enemy3HUD.UpdateUI(enemy3);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY3TURN:
                enemy3HUD.isHighlighted = false;
                state = BattleState.HERO1TURN;
                if (hero1 != null) {
                    hero1.isBlocking = false;
                    hero1.UpdateBuffs();
                    hero1HUD.isHighlighted = true;
                    hero1HUD.UpdateUI(hero1);
                }
                StartCoroutine(PlayerTurn());
                break;
        }
    }

    void EndBattle() {
        if (state == BattleState.WON) {
            auxText.text = "You won the battle!";
        } else if (state == BattleState.LOST) {
            auxText.text = "You were defeated!";
        }
    }

    #endregion

    #region Check functions

    private Vector3 GetPosition(GameObject sprite) {
        return new Vector3(sprite.transform.position.x, sprite.transform.position.y, sprite.transform.position.z);
    }

    bool IsInNegativeStatus(BuffType statusBuffType) {
        switch (state) {
            case BattleState.HERO1TURN:
                return (hero1 != null && hero1.IsInNegativeStatus(statusBuffType));
            case BattleState.HERO2TURN:
                return (hero2 != null && hero2.IsInNegativeStatus(statusBuffType));
            case BattleState.HERO3TURN:
                return (hero3 != null && hero3.IsInNegativeStatus(statusBuffType));
            case BattleState.ENEMY1TURN:
                return (enemy1 != null && enemy1.IsInNegativeStatus(statusBuffType));
            case BattleState.ENEMY2TURN:
                return (enemy2 != null && enemy2.IsInNegativeStatus(statusBuffType));
            case BattleState.ENEMY3TURN:
                return (enemy3 != null && enemy3.IsInNegativeStatus(statusBuffType));
        }
        return false;
    }

    bool IsHeroReadyToAct() {
        switch (state) {
            case BattleState.HERO1TURN:
                if (hero1 != null)
                    return (hero1.life != 0);
                return false;
            case BattleState.HERO2TURN:
                if (hero2 != null)
                    return (hero2.life != 0);
                return false;
            case BattleState.HERO3TURN:
                if (hero3 != null)
                    return (hero3.life != 0);
                return false;
        }
        return true;
    }

    bool IsEnemyReadyToAct() {
        switch (state) {
            case BattleState.ENEMY1TURN:
                if (enemy1 != null)
                    return (enemy1.life != 0);
                return false;
            case BattleState.ENEMY2TURN:
                if (enemy2 != null)
                    return (enemy2.life != 0);
                return false;
            case BattleState.ENEMY3TURN:
                if (enemy3 != null)
                    return (enemy3.life != 0);
                return false;
        }
        return true;
    }

    bool IsAllHeroesDead() {
        if (hero1 != null) {
            if (hero1.life > 0)
                return false;
        }
        if (hero2 != null) {
            if (hero2.life > 0)
                return false;
        }
        if (hero3 != null) {
            if (hero3.life > 0)
                return false;
        }
        return true;
    }

    bool IsAllEnemiesDead() {
        if (enemy1 != null) {
            if (enemy1.life > 0)
                return false;
        }
        if (enemy2 != null) {
            if (enemy2.life > 0)
                return false;
        }
        if (enemy3 != null) {
            if (enemy3.life > 0)
                return false;
        }
        
        return true;
    }

    void CheckingAvailableEnemyTargets() {
        if (enemy1 == null)
            enemy1Arrow.gameObject.SetActive(false);
        if (enemy2 == null)
            enemy2Arrow.gameObject.SetActive(false);
        if (enemy3 == null)
            enemy3Arrow.gameObject.SetActive(false);
    }

    void CheckingAvailableHeroTargets() {
        if (hero1 == null)
            hero1Arrow.gameObject.SetActive(false);
        if (hero2 == null)
            hero2Arrow.gameObject.SetActive(false);
        if (hero3 == null)
            hero3Arrow.gameObject.SetActive(false);
    }

    void ShowDescriptionSkill() {
        if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skill1Button.gameObject)) {
            descriptionSkill.text = skills[0].description;
            nameSkill.text = skills[0].skill_name;
            costSkill.text = "Cost: " + skills[0].cost;
            targetSkill.text = "Targets: " + skills[0].affectType;
        } else if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skill2Button.gameObject)) {
            descriptionSkill.text = skills[1].description;
            nameSkill.text = skills[1].skill_name;
            costSkill.text = "Cost: " + skills[1].cost;
            targetSkill.text = "Targets: " + skills[1].affectType;
        } else if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skill3Button.gameObject)) {
            descriptionSkill.text = skills[2].description;
            nameSkill.text = skills[2].skill_name;
            costSkill.text = "Cost: " + skills[2].cost;
            targetSkill.text = "Targets: " + skills[2].affectType;
        } else if (GameObject.ReferenceEquals(EventSystem.current.currentSelectedGameObject, skill4Button.gameObject)) {
            descriptionSkill.text = skills[3].description;
            nameSkill.text = skills[3].skill_name;
            costSkill.text = "Cost: " + skills[3].cost;
            targetSkill.text = "Targets: " + skills[3].affectType;
        }
    }

    private bool HaveEnoughEnergy(int TargetId) {
        switch (state) {
            case BattleState.HERO1TURN:
                if (skills[TargetId].cost <= hero1.energy) {
                    hero1.LoseEnergy(skills[TargetId].cost);
                    hero1HUD.UpdateUI(hero1);
                    return true;
                }
                return false;
            case BattleState.HERO2TURN:
                if (skills[TargetId].cost <= hero2.energy) {
                    hero2.LoseEnergy(skills[TargetId].cost);
                    hero2HUD.UpdateUI(hero2);
                    return true;
                }
                return false;
            case BattleState.HERO3TURN:
                if (skills[TargetId].cost <= hero3.energy) {
                    hero3.LoseEnergy(skills[TargetId].cost);
                    hero3HUD.UpdateUI(hero3);
                    return true;
                }
                return false;
        }
        return false;
    }

    private CharacterStats ChoosingATargetHero() {

        if (hero1.IsInNegativeStatus(BuffType.Taunt) == true) return hero1;
        if (hero2.IsInNegativeStatus(BuffType.Taunt) == true) return hero2;
        if (hero3.IsInNegativeStatus(BuffType.Taunt) == true) return hero3;

        while (true) {
            int targetRNG = UnityEngine.Random.Range(1, 4);
            switch (targetRNG) {
                case 1:
                    if (hero1 != null && hero1.life > 0)
                        return hero1;
                    break;
                case 2:
                    if (hero2 != null && hero2.life > 0)
                        return hero2;
                    break;
                case 3:
                    if (hero3 != null && hero3.life > 0)
                        return hero3;
                    break;
            }
        }
    }

    private void RepositioningHeroTargets() {
        Vector3 pos1 = GetPosition(hero1Arrow.gameObject);
        Vector3 pos2 = GetPosition(hero2Arrow.gameObject);
        Vector3 pos3 = GetPosition(hero3Arrow.gameObject);

        hero1Arrow.transform.position = pos3;
        hero2Arrow.transform.position = pos1;
        hero3Arrow.transform.position = pos2;
    }

    #endregion

    #region Button functions
    public void OnAttackButton() {

        menuTargetType = MenuTargetType.ATTACK;

        StartCoroutine(PlayerSelectAttackSingleTarget());
    }

    public void OnSelectEnemyTargetButton(int TargetId) {
        switch (TargetId) {
            case 1:
                if (enemy1.life == 0)
                    return;
                break;
            case 2:
                if (enemy2.life == 0)
                    return;
                break;
            case 3:
                if (enemy3.life == 0)
                    return;
                break;
            default:
                break;
        }
        StartCoroutine(PlayerActionEnemyTarget(TargetId));
    }

    public void OnSelectHeroesTargetButton(int TargetId) {
        switch (TargetId) {
            case 1:
                if (hero1.life == 0)
                    return;
                break;
            case 2:
                if (hero2.life == 0)
                    return;
                break;
            case 3:
                if (hero3.life == 0)
                    return;
                break;
            default:
                break;
        }
        StartCoroutine(PlayerActionHeroTarget(TargetId));
    }

    public void OnSkillsButton() {

        if (state == BattleState.HERO1TURN) {
            skills = hero1.skills;
        } else if (state == BattleState.HERO2TURN) {
            skills = hero2.skills;
        } else if (state == BattleState.HERO3TURN) {
            skills = hero3.skills;
        }

        StartCoroutine(PlayerSelectSkills());
        isInSkillsMenu = true;
        auxText.text = "Using skills!";
    }

    public void OnSelectedSkill(int TargetId) {
        
        if(HaveEnoughEnergy(TargetId - 1)) {
            menuTargetType = MenuTargetType.SKILL;
            selectedSkillIndex = TargetId;

            switch (skills[TargetId - 1].affectType) {
                case AffectType.EnemyTarget:
                    isInSkillsMenu = false;
                    skillsMenuPanel.SetActive(false);
                    StartCoroutine(PlayerSelectAttackSingleTarget());
                    break;
                case AffectType.AllEnemies:
                    isInSkillsMenu = false;
                    skillsMenuPanel.SetActive(false);
                    StartCoroutine(PlayerActionAllEnemies());
                    break;
                case AffectType.AllyTarget:
                    isInSkillsMenu = false;
                    skillsMenuPanel.SetActive(false);
                    StartCoroutine(PlayerSelectHeroSingleTarget());
                    break;
                case AffectType.AllAllies:
                    isInSkillsMenu = false;
                    skillsMenuPanel.SetActive(false);
                    StartCoroutine(PlayerActionAllHeroes());
                    break;
                case AffectType.Self:
                    isInSkillsMenu = false;
                    skillsMenuPanel.SetActive(false);
                    StartCoroutine(PlayerActionSelf());
                    break;
                default:
                    break;
            }
        }
    }

    public void OnBlockButton() {
        StartCoroutine(Blocking());
    }

    #endregion
}
