using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour {
    public TextMeshProUGUI nameText;
    public Slider hpSlider;
    public Slider mpSlider;
    public Image portrait;
    public Image[] effects;

    public bool isHighlighted = false;

    #region Time Variables
    const float HIGHLIGHT_TRANSITION_TIME = 4f;
    const float SLIDER_TRANSITION_TIME = 4f;
    #endregion

    #region Control Variables
    private Vector3 scaleHighlight = new(1.1f, 1.1f, 1f);
    private Vector3 scaleNormal = new(1.0f, 1.0f, 1f);
    private float newLife;
    private float newEnergy;
    private bool isCharacterSetted = false;
    #endregion

    public void Update() {
        if (isHighlighted) {
            ScaleToHighlight();
        } else {
            ScaleToNormal();
        }
        if (isCharacterSetted) {
            if (newLife != hpSlider.value) SlidingHP();
            if (newEnergy != mpSlider.value) SlidingMP();
        }
    }

    public void SetHUD(CharacterStats character) {
        portrait.sprite = CharacterCombatSpriteManager.GetInstance().CharacterPortraitImage(character.char_name);
        nameText.text = character.char_name;
        hpSlider.maxValue = character.maxLife;
        hpSlider.value = character.life;
        newLife = character.life;
        mpSlider.maxValue = character.maxEnergy;
        mpSlider.value = character.energy;
        newEnergy = character.energy;
        isCharacterSetted = true;
        ReloadEffects(character.buffs);
    } 

    public void ScaleToHighlight() {
        if(transform.localScale != scaleHighlight) {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleHighlight, HIGHLIGHT_TRANSITION_TIME * Time.deltaTime);
        }
    }

    public void ScaleToNormal() {
        if (transform.localScale != scaleNormal) {
            transform.localScale = Vector3.Lerp(transform.localScale, scaleNormal, HIGHLIGHT_TRANSITION_TIME * Time.deltaTime);
        }
    }

    public void SlidingHP() {
        hpSlider.value = Mathf.Lerp(hpSlider.value, newLife, SLIDER_TRANSITION_TIME * Time.deltaTime);
    }

    public void SlidingMP() {
        mpSlider.value = Mathf.Lerp(mpSlider.value, newEnergy, SLIDER_TRANSITION_TIME * Time.deltaTime);
    }

    public void UpdateUI(CharacterStats character) {
        newLife = character.life;
        newEnergy = character.energy;
        ReloadEffects(character.buffs);
    }

    public void ReloadEffects(List<Buff> buffs) {
        for (int i = 0; i < buffs.Count; i++) {
            effects[i].gameObject.SetActive(true);
            effects[i].sprite = CharacterBuffsSpriteManager.GetInstance().BuffSpriteImage(buffs[i].buffType);
        }
        for (int i = buffs.Count; i < effects.Length; i++) {
            effects[i].gameObject.SetActive(false);
        }
    }
}
