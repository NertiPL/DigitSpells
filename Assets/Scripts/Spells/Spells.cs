using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum SpellType
{
    Fire,
    Water,
    Earth,
    Air,
    Thunder
}

public enum Difficulty
{
    Easy,
    Medium,
    Hard,
    Impossible
}


[CreateAssetMenu(fileName = "NewSpell", menuName = "Spell", order = 0)]
public class Spells : ScriptableObject
{
    public string name;
    public string description;
    public int lvl;
    public SpellType spellType;
    public Difficulty difficulty;
    public Sprite sprite;
    public GameObject prefab;
}
