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

    public void SetHUD(CharacterStats character) {
        portrait.sprite = CharacterCombatSpriteManager.GetInstance().CharacterPortraitImage(character.char_name);
        nameText.text = character.char_name;
        hpSlider.maxValue = character.maxLife;
        hpSlider.value = character.life;
        mpSlider.maxValue = character.maxEnergy;
        mpSlider.value = character.energy;
        ReloadEffects(character.buffs);
    } 

    public void UpdateUI(int hp, int mp) {
        hpSlider.value = hp;
        mpSlider.value = mp;
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
