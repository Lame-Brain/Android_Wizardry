using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Character_Class : MonoBehaviour
{
    public string name;
    public bool inMaze;
    public Enum._Race race;
    public Enum._Class character_class;
    public int AgeinWeeks;
    public Enum._Status status;
    public int Save_vs_Death, Save_vs_Wand, Save_vs_Breath, Save_vs_Petrify, Save_vs_Spell; //these are better when they are more negative. the lower the better?
    public int Geld;
    //public Items[] Inventory;
    public int level;
    public int xp;
    public int xp_nnl;
    public int HP;
    public int HP_MAX;
    public bool[] SpellKnown;
    public int[] mageSpells, mageSpellsCast;
    public int[] priestSpells, priestSpellsCast;
    public int hitDiceSides;
    public int ArmorClass;
    public int heal_points;
    public bool crit_hit;
    public int swing_count;
    public Dice hit_dam;
    public Vector3Int lostXYL;
    public bool Trebor_Honor_Guard; // ">"
    public bool Gnilda_Staff_Keeper; // "K"
    public bool Llylgamyn_Knight; // "G"
    public bool Descendent_of_Diamonds; // "D"
    public bool Star_of_Llylgamyn; // "*"
    // ">*KDG"
}
