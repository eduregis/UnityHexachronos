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
    [Header("Characters PreFabs (Will Be Removed)")]
    public GameObject hero1Prefab;
    public GameObject hero2Prefab;
    public GameObject hero3Prefab;
    public GameObject enemy1Prefab;
    public GameObject enemy2Prefab;
    public GameObject enemy3Prefab;

    [Header("Battle Stations")]
    public Transform hero1BattleStation;
    public Transform hero2BattleStation;
    public Transform hero3BattleStation;
    public Transform enemy1BattleStation;
    public Transform enemy2BattleStation;
    public Transform enemy3BattleStation;

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

    [Header("Characters Units")]
    Unit hero1Unit;
    Unit hero2Unit;
    Unit hero3Unit;
    Unit enemy1Unit;
    Unit enemy2Unit;
    Unit enemy3Unit;

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
        GameObject hero1GO = Instantiate(hero1Prefab, hero1BattleStation);
        hero1Unit = hero1GO.GetComponent<Unit>();
        hero1HUD.SetHUD(hero1Unit);

        GameObject hero2GO = Instantiate(hero2Prefab, hero2BattleStation);
        hero2Unit = hero2GO.GetComponent<Unit>();
        hero2HUD.SetHUD(hero2Unit);

        GameObject enemy1GO = Instantiate(enemy1Prefab, enemy1BattleStation);
        enemy1Unit = enemy1GO.GetComponent<Unit>();
        enemy1HUD.SetHUD(enemy1Unit);

        GameObject enemy2GO = Instantiate(enemy2Prefab, enemy2BattleStation);
        enemy2Unit = enemy2GO.GetComponent<Unit>();
        enemy2HUD.SetHUD(enemy2Unit);

        StartCoroutine(StartBattle());
    }

    IEnumerator StartBattle()
    {
        if (hero1Unit == null)
            hero1HUD.gameObject.SetActive(false);
        if (hero2Unit == null)
            hero2HUD.gameObject.SetActive(false);
        if (hero3Unit == null)
            hero3HUD.gameObject.SetActive(false);
        if (enemy1Unit == null)
            enemy1HUD.gameObject.SetActive(false);
        if (enemy2Unit == null)
            enemy2HUD.gameObject.SetActive(false);
        if (enemy3Unit == null)
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
                enemy1Unit.TakeDamage(hero1Unit.damage);
                enemy1HUD.SetHP(enemy1Unit.currentHP);
                break;
            case 2:
                enemy2Unit.TakeDamage(hero2Unit.damage);
                enemy2HUD.SetHP(enemy2Unit.currentHP);
                break;
            case 3:
                enemy3Unit.TakeDamage(hero3Unit.damage);
                enemy3HUD.SetHP(enemy3Unit.currentHP);
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

            hero1Unit.TakeDamage(enemy1Unit.damage);

            hero1HUD.SetHP(hero1Unit.currentHP);

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
                if (hero1Unit != null)
                    return (hero1Unit.currentHP != 0);
                return false;
            case BattleState.HERO2TURN:
                if (hero2Unit != null)
                    return (hero2Unit.currentHP != 0);
                return false;
            case BattleState.HERO3TURN:
                if (hero3Unit != null)
                    return (hero3Unit.currentHP != 0);
                return false;
        }
        return true;
    }

    bool IsEnemyReadyToAct() {
        switch (state) {
            case BattleState.ENEMY1TURN:
                if (enemy1Unit != null)
                    return (enemy1Unit.currentHP != 0);
                return false;
            case BattleState.ENEMY2TURN:
                if (enemy2Unit != null)
                    return (enemy2Unit.currentHP != 0);
                return false;
            case BattleState.ENEMY3TURN:
                if (enemy3Unit != null)
                    return (enemy3Unit.currentHP != 0);
                return false;
        }
        return true;
    }

    bool IsAllHeroesDead() {
        if (hero1Unit != null) {
            if (hero1Unit.currentHP > 0)
                return false;
        }
        if (hero2Unit != null) {
            if (hero2Unit.currentHP > 0)
                return false;
        }
        if (hero3Unit != null) {
            if (hero3Unit.currentHP > 0)
                return false;
        }
        return true;
    }

    bool IsAllEnemiesDead() {
        if (enemy1Unit != null) {
            if (enemy1Unit.currentHP > 0)
                return false;
        }
        if (enemy2Unit != null) {
            if (enemy2Unit.currentHP > 0)
                return false;
        }
        if (enemy3Unit != null) {
            if (enemy3Unit.currentHP > 0)
                return false;
        }
        return true;
    }

    void CheckingAvailableEnemyTargets() {
        if (enemy1Unit == null)
            enemy1Arrow.gameObject.SetActive(false);
        if (enemy2Unit == null)
            enemy2Arrow.gameObject.SetActive(false);
        if (enemy3Unit == null)
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
                if (enemy1Unit.currentHP == 0)
                    return;
                break;
            case 2:
                if (enemy2Unit.currentHP == 0)
                    return;
                break;
            case 3:
                if (enemy3Unit.currentHP == 0)
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
