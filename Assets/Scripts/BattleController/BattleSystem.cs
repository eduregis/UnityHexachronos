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
    MenuTargetType menuTargetType;
    List<CharacterSkill> skills;
    int selectedSkillIndex = 0;

    #endregion

    // Start is called before the first frame update
    void Start() {
        state = BattleState.START;
        SetupBattle();
    }

    void Update() {
        if ((state == BattleState.HERO1TURN || state == BattleState.HERO2TURN || state == BattleState.HERO3TURN) && isInSkillsMenu) {
            ShowDescriptionSkill();
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

        yield return new WaitForSeconds(1.2f);

        StartCoroutine(PlayerTurn());
    }

    IEnumerator PlayerTurn() {
        if (IsHeroReadyToAct()) {
            if (state == BattleState.HERO1TURN) auxText.text = hero1.char_name + ": Your turn!";
            if (state == BattleState.HERO2TURN) auxText.text = hero2.char_name + ": Your turn!";
            if (state == BattleState.HERO3TURN) auxText.text = hero3.char_name + ": Your turn!";

            mainMenuPanel.SetActive(true);

            EventSystem.current.SetSelectedGameObject(null);
            yield return new WaitForEndOfFrame();
            EventSystem.current.SetSelectedGameObject(attackButton.gameObject);
        } else {
            NextTurn();
        }
    }

    IEnumerator PlayerSelectAttackSingleTarget() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        selectEnemyMenuPanel.SetActive(true);

        auxText.text = "Select a enemy target!";

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(enemy1Arrow.gameObject);

        CheckingAvailableEnemyTargets();
    }

    IEnumerator PlayerSelectHeroSingleTarget() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        selectHeroMenuPanel.SetActive(true);

        auxText.text = "Select a hero target!";

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(hero1Arrow.gameObject);

        CheckingAvailableHeroTargets();
    }
    IEnumerator PlayerSelectSkills() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

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

        switch (menuTargetType) {
            case MenuTargetType.ATTACK:
                switch (TargetId) {
                    case 1:
                        enemy1.ReceivingAttackDamage(skillUser);
                        enemy1HUD.UpdateUI(enemy1);
                        break;
                    case 2:
                        enemy2.ReceivingAttackDamage(skillUser);
                        enemy2HUD.UpdateUI(enemy2);
                        break;
                    case 3:
                        enemy3.ReceivingAttackDamage(skillUser);
                        enemy3HUD.UpdateUI(enemy3);
                        break;
                    default:
                        break;
                }
                auxText.text = "The attack is successful!";
                break;
            case MenuTargetType.SKILL:
                switch (TargetId) {
                    case 1:
                        enemy1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        enemy1HUD.UpdateUI(enemy1);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy1"; 
                        break;
                    case 2:
                        enemy2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        enemy2HUD.UpdateUI(enemy2);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy2";
                        break;
                    case 3:
                        enemy3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
                        enemy3HUD.UpdateUI(enemy3);
                        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in enemy3";
                        break;
                    default:
                        break;
                }
                break;
        }

        yield return new WaitForSeconds(1f);

        if (IsAllEnemiesDead()) {
            state = BattleState.WON;
            EndBattle();
        } else {
            NextTurn();
        }
    }

    IEnumerator PlayerActionHeroTarget(int TargetId)
    {
        selectHeroMenuPanel.SetActive(false);
        CharacterStats skillUser = hero1;

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

        yield return new WaitForSeconds(1f);

        if (IsAllHeroesDead()) {
            state = BattleState.LOST;
            EndBattle();
        } else {
            NextTurn();
        }
    }

    IEnumerator PlayerActionAllEnemies() {
        CharacterStats skillUser = hero1;

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

        if (enemy1 != null && enemy1.life > 0) {
            enemy1.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy1HUD.UpdateUI(enemy1);
        }

        if (enemy2 != null && enemy2.life > 0) {
            enemy2.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy2HUD.UpdateUI(enemy2);
        }

        if (enemy3 != null && enemy3.life > 0) {
            enemy3.ApplySkill(skillUser, skills[selectedSkillIndex - 1]);
            enemy3HUD.UpdateUI(enemy3);
        }

        auxText.text = "used " + skills[selectedSkillIndex - 1].skill_name + " in all enemies";

        yield return new WaitForSeconds(1f);

        if (IsAllEnemiesDead()) {
            state = BattleState.WON;
            EndBattle();
        }
        else {
            NextTurn();
        }
    }

    IEnumerator PlayerActionAllHeroes() {
        CharacterStats skillUser = hero1;

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

        yield return new WaitForSeconds(1f);

        NextTurn();
    }

    IEnumerator PlayerActionSelf() {

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

        yield return new WaitForSeconds(1f);

        NextTurn();
    }

    IEnumerator EnemyTurn() {
        if (IsEnemyReadyToAct()) {
            auxText.text = state + ": attacks!";

            yield return new WaitForSeconds(1f);

            CharacterStats heroTarget = ChoosingATargetHero();

            switch (state) {
                case BattleState.ENEMY1TURN:
                    heroTarget.ReceivingAttackDamage(enemy1);
                    break;
                case BattleState.ENEMY2TURN:
                    heroTarget.ReceivingAttackDamage(enemy2);
                    break;
                case BattleState.ENEMY3TURN:
                    heroTarget.ReceivingAttackDamage(enemy3);
                    break;
            }

            UpdateUI();

            yield return new WaitForSeconds(1f);

            if (IsAllHeroesDead()) {
                state = BattleState.LOST;
                EndBattle();
            } else {
                NextTurn();
            }
        } else {
            NextTurn();
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

    IEnumerator Blocking() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

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

        yield return new WaitForSeconds(0.5f);

        NextTurn();
    }

    void NextTurn() {
        switch (state) {
            case BattleState.HERO1TURN:
                state = BattleState.HERO2TURN;
                if (hero2 != null) {
                    hero2.isBlocking = false;
                    hero2.UpdateBuffs();
                    hero2HUD.UpdateUI(hero2);
                }
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO2TURN:
                state = BattleState.HERO3TURN;
                if (hero3 != null) {
                    hero3.isBlocking = false;
                    hero3.UpdateBuffs();
                    hero3HUD.UpdateUI(hero3);
                }
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO3TURN:
                state = BattleState.ENEMY1TURN;
                if (enemy1 != null) {
                    enemy1.isBlocking = false;
                    enemy1.UpdateBuffs();
                    enemy1HUD.UpdateUI(enemy1);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY1TURN:
                state = BattleState.ENEMY2TURN;
                if (enemy2 != null) {
                    enemy2.isBlocking = false;
                    enemy2.UpdateBuffs();
                    enemy2HUD.UpdateUI(enemy2);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY2TURN:
                state = BattleState.ENEMY3TURN;
                if (enemy3 != null) {
                    enemy3.isBlocking = false;
                    enemy3.UpdateBuffs();
                    enemy3HUD.UpdateUI(enemy3);
                }
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY3TURN:
                state = BattleState.HERO1TURN;
                if (hero1 != null) {
                    hero1.isBlocking = false;
                    hero1.UpdateBuffs();
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
