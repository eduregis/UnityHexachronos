using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueBattleDataBridge : MonoBehaviour {

    public static string hero1_Name = "";
    public static string hero2_Name = "";
    public static string hero3_Name = "";
    public static string enemy1_Name = "";
    public static string enemy2_Name = "";
    public static string enemy3_Name = "";

    public void SetCharactersToBattle (
        string hero1Name = "", string hero2Name = "", string hero3Name = "", 
        string enemy1Name = "", string enemy2Name = "",  string enemy3Name = "") {
        hero1_Name = hero1Name;
        hero2_Name = hero2Name;
        hero3_Name = hero3Name;
        enemy1_Name = enemy1Name;
        enemy2_Name = enemy2Name;
        enemy3_Name = enemy3Name;
    }

    public string GetCharactersToBattle(string char_slot) {
        return char_slot switch {
            "hero1_name" => hero1_Name,
            "hero2_Name" => hero2_Name,
            "hero3_Name" => hero3_Name,
            "enemy1_Name" => enemy1_Name,
            "enemy2_Name" => enemy2_Name,
            "enemy3_Name" => enemy3_Name,
            _ => "",
        };
    }
}
