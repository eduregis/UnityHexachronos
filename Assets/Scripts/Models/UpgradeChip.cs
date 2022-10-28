using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ChipType {
    Damage,
    Defense,
    CritRate,
    CritDamage,
    Evasion,
    HitRate,
    Strength,
    Intelligence,
    Vitality,
    Technique,
    Agility,
    Luck,
    Corruption
}

public enum ChipModifier {
    Multiplier,
    Constant,
    AutoAttackModifier
}

public enum ChipSet {
    Pyrang, // Vermelho
    Kyra, // Verde
    Oby, // Verde ou azul
    Una, // Preto
    Tinga, // Branco
    Obyeteh, // Azul Claro
    Yuba // Amarelo
}

public class UpgradeChip {
    public float value;
    public ChipType chipType;
    public ChipModifier chipModifier;
    public ChipSet chipSet;
}
