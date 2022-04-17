using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="BasicStats", menuName ="BasicStats")]

public class BasicCharacterStats : ScriptableObject
{
    public int strength;
    public int intelligence;
    public int vitality;
    public int technique;
    public int agility;
    public int luck;
}
