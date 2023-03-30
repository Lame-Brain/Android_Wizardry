using System.Collections;
using System.Collections.Generic;
using BlobberEngine;

public class Monster_Class
{
    public string name_unk, names_unk, name, names;
    public int pic;
    public Dice group_size, HP;
    public Enum._Class monster_class;
    public int ArmorClass;
    public Dice[] attack;
    public int xp;
    public int drain_amnt;
    public int heal_pts;
    public int reward1, reward2;
    public int ally_index, ally_perc;
    public int mage_spells, priest_spells;
    public int unique;    
    public Enum._Damage_type breathe, resist, immune;
    public string special;
    public string weapon_style; //swings, thrusts, stabs, slashes, chops [or] tears, rips, gnaws, bites, claws [or both]
    public bool resist_friendly;
}
