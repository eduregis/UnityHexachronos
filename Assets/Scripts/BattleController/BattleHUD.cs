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

    public void SetHUD(CharacterStats character) {
        portrait.sprite = CharacterCombatSpriteManager.GetInstance().CharacterPortraitImage(character.char_name);
        nameText.text = character.char_name;
        hpSlider.maxValue = character.maxLife;
        hpSlider.value = character.life;
        mpSlider.maxValue = character.maxEnergy;
        mpSlider.value = character.energy;
    } 

    public void SetHP(int hp) {
        hpSlider.value = hp;
    }

    public void SetMP(int mp) {
        mpSlider.value = mp;
    }
}
