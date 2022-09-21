using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CharacterIdentifier
{
    Luca = 0,
    Borell = 1,
    Sam = 2,
    Salvato = 3,
    Billy = 4,
    Dandara = 5,
    Morya = 6,
    BasicSoldier = 7,
    BasicLiutenant = 8
}

[System.Serializable]
public class BasicCharInfo
{
    public CharacterStats characterStats;
    public CharacterIdentifier characterIdentifier;
}