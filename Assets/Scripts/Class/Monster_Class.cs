using System.Collections;
using System.Collections.Generic;
using BlobberEngine;

public class Monster_Class
{
    public int index;
    public int pic;
    public string name_unk, names_unk, name, names;
    public Dice group_size, HitDice;
    public Enum._Class monster_class;
    public int ArmorClass;
    public List<Dice> attack = new List<Dice>();
    public int reward1, reward2;
    public int partner_chance, partner_index;
    public int mage_spells, priest_spells;
    public int spell_resist;
    public string elem_resist;
    public string abilities;
    public int xp;
    public int unique;    
    public string special;
    public string weapon_style; //swings, thrusts, stabs, slashes, chops [or] tears, rips, gnaws, bites, claws [or both]
    public bool resist_friendly;
}
