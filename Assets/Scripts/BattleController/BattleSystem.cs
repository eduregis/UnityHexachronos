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

public class BattleSystem : MonoBehaviour
{
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

    // Start is called before the first frame update
    void Start() {
        state = BattleState.START;
        SetupBattle();
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
            auxText.text = state + ": Your turn!";

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

        auxText.text = "Select a target!";

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(enemy1Arrow.gameObject);

        CheckingAvailableEnemyTargets();
    }

    IEnumerator PlayerSelectSkills() {
        mainMenuPanel.SetActive(false);

        yield return new WaitForSeconds(0.5f);

        skillsMenuPanel.SetActive(true);

        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        EventSystem.current.SetSelectedGameObject(skill1Button.gameObject);
    }

    IEnumerator PlayerActionEnemyTarget(int TargetId)
    {
        selectEnemyMenuPanel.SetActive(false);

        yield return new WaitForSeconds(1f);

        switch (TargetId){
            case 1:
                enemy1.TakeDamage(hero1.damage);
                enemy1HUD.SetHP(enemy1.life);
                break;
            case 2:
                enemy2.TakeDamage(hero2.damage);
                enemy2HUD.SetHP(enemy2.life);
                break;
            case 3:
                enemy3.TakeDamage(hero3.damage);
                enemy3HUD.SetHP(enemy3.life);
                break;
            default:
                break;
        }
        
        auxText.text = "The attack is successful!";

        yield return new WaitForSeconds(1f);

        if (IsAllEnemiesDead())
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            NextTurn();
        }
    }

    IEnumerator EnemyTurn() {
        if (IsEnemyReadyToAct()) {
            auxText.text = state + ": attacks!";

            yield return new WaitForSeconds(1f);

            hero1.TakeDamage(enemy1.damage);

            hero1HUD.SetHP(hero1.life);

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

    void NextTurn() {
        switch (state) {
            case BattleState.HERO1TURN:
                state = BattleState.HERO2TURN;
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO2TURN:
                state = BattleState.HERO3TURN;
                StartCoroutine(PlayerTurn());
                break;
            case BattleState.HERO3TURN:
                state = BattleState.ENEMY1TURN;
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY1TURN:
                state = BattleState.ENEMY2TURN;
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY2TURN:
                state = BattleState.ENEMY3TURN;
                StartCoroutine(EnemyTurn());
                break;
            case BattleState.ENEMY3TURN:
                state = BattleState.HERO1TURN;
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

    #endregion

    #region Button functions
    public void OnAttackButton() {
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

    public void OnSkillsButton() {
        StartCoroutine(PlayerSelectSkills());
        auxText.text = "Using skills!";
    }

    public void OnSelectedSkill(int TargetId)
    {
        auxText.text = "Using skill " + TargetId;
    }
    public void OnBlockButton() {
        auxText.text = "Blocking!";
    }

    #endregion
}
