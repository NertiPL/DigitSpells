using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpellType
{
    Fire,
    Water,
    Earth,
    Lightning,
    Healing
}

public enum Difficulty
{
    Easy, //minimum 2 numbers
    Medium, // minimum 3 numbers
    Hard, // minimum 5 numbers
    Impossible // minimum 8 numbers
}


[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell", order = 0)]
public class Spells : ScriptableObject
{
    public float cd; //cooldown
    public string name;
    public string description;
    public int lvl;
    public SpellType spellType;
    public Difficulty difficulty;
    public Sprite sprite;
    public GameObject prefab;
    public bool onTopOnStaff;

    public float LvlChanges(float dmgOrHeal)
    {
        return dmgOrHeal * (1+lvl/2);
    }
}

