using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Character_Class
{
    public string name;
    public bool inMaze;
    public Enum._Race race;
    public Enum._Class character_class;
    public int ageInWeeks;
    public Enum._Status status;
    public int Save_vs_Death, Save_vs_Wand, Save_vs_Breath, Save_vs_Petrify, Save_vs_Spell; //these are better when they are more negative. the lower the better?
    public int Geld;
    public Item[] Inventory;
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

    public Character_Class()
    {
        this.name = "";
        this.inMaze = false;
        this.race = Enum._Race.none;
        this.character_class = Enum._Class.none;
        this.ageInWeeks = 52 * 18;
        this.status = Enum._Status.OK;
        this.Save_vs_Death = 0;
        this.Save_vs_Wand = 0;
        this.Save_vs_Breath = 0;
        this.Save_vs_Petrify = 0;
        this.Save_vs_Spell = 0;
        this.Geld = 0;
        this.Inventory = new Item[8];
        this.level = 1;
        this.xp = 0;
        this.xp_nnl = 1000000000;
        this.HP = 10;
        this.HP_MAX = 10;
        this.hitDiceSides = 6;
        this.ArmorClass = 10;
        this.heal_points = 0;
        this.crit_hit = false;
        this.hit_dam = new Dice(1, 2, 0); //1d2
        this.lostXYL = Vector3Int.zero;
        this.Trebor_Honor_Guard = false;
        this.Gnilda_Staff_Keeper = false;
        this.Llylgamyn_Knight = false;
        this.Descendent_of_Diamonds = false;
        this.Star_of_Llylgamyn = false;
        this.SpellKnown = new bool[50];
        this.mageSpells = new int[7];
        this.mageSpellsCast = new int[7];
        this.priestSpells = new int[7];
        this.priestSpellsCast = new int[7];
    }

    public void Assign_Character(string _char)
    {
        string[] _data = _char.Split(",");
        int _n = 0;

        _n = 0;  this.name = _data[_n];
        _n = 1; this.inMaze = _data[_n] == "1" ?  true : false;
        _n = 2; this.race = (Enum._Race)int.Parse(_data[_n]);
        _n = 3; this.character_class = (Enum._Class)int.Parse(_data[_n]);
        _n = 4; this.ageInWeeks = int.Parse(_data[_n]);
        _n = 5; this.status = (Enum._Status)int.Parse(_data[_n]);
        _n = 6; this.Save_vs_Death = int.Parse(_data[_n]);
        _n = 7; this.Save_vs_Wand = int.Parse(_data[_n]);
        _n = 8; this.Save_vs_Breath = int.Parse(_data[_n]);
        _n = 9; this.Save_vs_Petrify = int.Parse(_data[_n]);
        _n = 10; this.Save_vs_Spell = int.Parse(_data[_n]);
        _n = 11; this.Geld = int.Parse(_data[_n]);
        _n = 12; string[] _allItems = _data[_n].Split(";");
        for (int i = 0; i < _allItems.Length; i++)
        {
            string[] _itemDetails = _allItems[i].Split("||");
            this.Inventory[i].index = int.Parse(_itemDetails[0]);
            this.Inventory[i].equipped = _itemDetails[1] == "1" ? true : false;
            this.Inventory[i].curse_active = _itemDetails[2] == "1" ? true : false;
            this.Inventory[i].identified = _itemDetails[3] == "1" ? true : false;
        }
        _n = 13; this.level = int.Parse(_data[_n]);
        _n = 14; this.xp = int.Parse(_data[_n]);
        _n = 15; this.xp_nnl = int.Parse(_data[_n]);
        _n = 16; this.HP = int.Parse(_data[_n]);
        _n = 17; this.HP_MAX = int.Parse(_data[_n]);
        _n = 18; this.hitDiceSides = int.Parse(_data[_n]);
        _n = 19; this.ArmorClass = int.Parse(_data[_n]);
        _n = 20; this.heal_points = int.Parse(_data[_n]);
        _n = 21; this.crit_hit = _data[_n] == "1" ? true : false;
        _n = 22; this.hit_dam.sides = int.Parse(_data[_n]);
        _n = 23; this.hit_dam.num = int.Parse(_data[_n]);
        _n = 24; this.hit_dam.bonus = int.Parse(_data[_n]);
        _n = 25; this.lostXYL.x = int.Parse(_data[_n]);
        _n = 26; this.lostXYL.y = int.Parse(_data[_n]);
        _n = 27; this.lostXYL.z = int.Parse(_data[_n]);
        _n = 28; this.Trebor_Honor_Guard = _data[_n] == "1" ? true : false;
        _n = 29; this.Gnilda_Staff_Keeper = _data[_n] == "1" ? true : false;
        _n = 30; this.Llylgamyn_Knight = _data[_n] == "1" ? true : false;
        _n = 31; this.Descendent_of_Diamonds = _data[_n] == "1" ? true : false;
        _n = 32; this.Star_of_Llylgamyn = _data[_n] == "1" ? true : false;

        _n = 33; string[] _allSpKn = _data[_n].Split(";");
        for (int i = 0; i < _allSpKn.Length; i++) this.SpellKnown[i] = _allSpKn[i] == "1" ? true : false;

        _n = 34; string[] _mspells = _data[_n].Split(";");
        for (int i = 0; i < _mspells.Length; i++) this.mageSpells[i] = int.Parse(_mspells[i]);

        _n = 35; string[] _mspellsC = _data[_n].Split(";");
        for (int i = 0; i < _mspellsC.Length; i++) this.mageSpellsCast[i] = int.Parse(_mspellsC[i]);

        _n = 36; string[] _pspells = _data[_n].Split(";");
        for (int i = 0; i < _pspells.Length; i++) this.priestSpells[i] = int.Parse(_pspells[i]);

        _n = 37; string[] _pspellsC = _data[_n].Split(";");
        for (int i = 0; i < _pspellsC.Length; i++) this.priestSpellsCast[i] = int.Parse(_pspellsC[i]);
    }
}
