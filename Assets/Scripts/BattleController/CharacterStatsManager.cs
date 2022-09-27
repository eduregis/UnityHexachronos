using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStatsManager : MonoBehaviour
{
    private static CharacterStatsManager instance;

    [Header("Characters Available")]
    public CharacterStats luca;
    public CharacterStats sam;
    public CharacterStats borell;
    public CharacterStats billy;
    public CharacterStats salvato;
    public CharacterStats dandara;
    public CharacterStats morya;
    public CharacterStats basicSoldier;
    public CharacterStats basicLieutenant;

    private void Awake() {
        if (instance != null) {
            Debug.LogWarning("Found more than one CharacterStatsManager in the scene");
        }
        instance = this;
    }

    public static CharacterStatsManager GetInstance() {
        return instance;
    }

    public CharacterStats GetCharacter(string char_name) {
        return char_name switch {
            "Luca" => luca,
            "Sam" => sam,
            "Borell" => borell,
            "Billy" => billy,
            "Salvato" => salvato,
            "Dandara" => dandara,
            "Morya" => morya,
            "BasicSoldier" => basicSoldier,
            "BasicLieutenant" => basicLieutenant,
            _ => luca,
        };
    }
}
