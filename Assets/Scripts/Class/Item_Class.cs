using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Item_Class : MonoBehaviour
{
    public string name, name_unk;
    public Enum._Item_Type item_type;
    public Enum._Alignment item_align;
    public bool cursed;
    public string special;
    public int change_to;
    public int change_chance;
    public float price;
    public int store_stock;
    public int spell_pwr;
    public string class_use;
    public int heal_pts;
    public Dice damage;
    public int armor_mod;
    public int hit_mod;
    public int xtra_swings;
    public bool crit_hit;
    public string wep_vs_type;
    public string ac_vs_type;
}
