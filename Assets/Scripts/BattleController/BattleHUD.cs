using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public Slider hpSlider;

    public void SetHUD(CharacterStats character)
    {
        nameText.text = character.char_name;
        hpSlider.maxValue = character.maxLife;
        hpSlider.value = character.life;
    } 

    public void SetHP(int hp)
    {
        hpSlider.value = hp;
    }
}
