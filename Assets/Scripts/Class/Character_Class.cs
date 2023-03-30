using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Character_Class
{
    public string name;
    public bool inParty;
    public Enum._Locaton location;
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
    public int[] mageSpells, mageSpellsCast;
    public int[] priestSpells, priestSpellsCast;
    public bool[] SpellKnown;

    public Character_Class()
    {
        this.name = "";
        this.inParty = false;
        this.location = Enum._Locaton.Roster;
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
        for (int i = 0; i < Inventory.Length; i++) this.Inventory[i] = new Item();
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
        this.mageSpells = new int[7];
        this.mageSpellsCast = new int[7];
        this.priestSpells = new int[7];
        this.priestSpellsCast = new int[7];
        this.SpellKnown = new bool[49];
    }

    public void Load_Character(string _char)
    {
        string[] _data = _char.Split(",");
        int _n = 0;

        _n = 0;  this.name = _data[_n];
        _n = 1; this.inParty = _data[_n] == "1" ?  true : false;
        _n = 2; this.location = (Enum._Locaton)int.Parse(_data[_n]);
        _n = 3; this.race = (Enum._Race)int.Parse(_data[_n]);
        _n = 4; this.character_class = (Enum._Class)int.Parse(_data[_n]);
        _n = 5; this.ageInWeeks = int.Parse(_data[_n]);
        _n = 6; this.status = (Enum._Status)int.Parse(_data[_n]);
        _n = 7; this.Save_vs_Death = int.Parse(_data[_n]);
        _n = 8; this.Save_vs_Wand = int.Parse(_data[_n]);
        _n = 9; this.Save_vs_Breath = int.Parse(_data[_n]);
        _n = 10; this.Save_vs_Petrify = int.Parse(_data[_n]);
        _n = 11; this.Save_vs_Spell = int.Parse(_data[_n]);
        _n = 12; this.Geld = int.Parse(_data[_n]);
        _n = 13; string[] _allItems = _data[_n].Split(";");
        for (int i = 0; i < _allItems.Length; i++)
        {
            string[] _itemDetails = _allItems[i].Split("||");
            this.Inventory[i].index = int.Parse(_itemDetails[0]);
            this.Inventory[i].equipped = _itemDetails[1] == "1" ? true : false;
            this.Inventory[i].curse_active = _itemDetails[2] == "1" ? true : false;
            this.Inventory[i].identified = _itemDetails[3] == "1" ? true : false;
        }
        _n = 14; this.level = int.Parse(_data[_n]);
        _n = 15; this.xp = int.Parse(_data[_n]);
        _n = 16; this.xp_nnl = int.Parse(_data[_n]);
        _n = 17; this.HP = int.Parse(_data[_n]);
        _n = 18; this.HP_MAX = int.Parse(_data[_n]);
        _n = 19; this.hitDiceSides = int.Parse(_data[_n]);
        _n = 20; this.ArmorClass = int.Parse(_data[_n]);
        _n = 21; this.heal_points = int.Parse(_data[_n]);
        _n = 22; this.crit_hit = _data[_n] == "1" ? true : false;
        _n = 23; this.hit_dam.sides = int.Parse(_data[_n]);
        _n = 24; this.hit_dam.num = int.Parse(_data[_n]);
        _n = 25; this.hit_dam.bonus = int.Parse(_data[_n]);
        _n = 26; this.lostXYL.x = int.Parse(_data[_n]);
        _n = 27; this.lostXYL.y = int.Parse(_data[_n]);
        _n = 28; this.lostXYL.z = int.Parse(_data[_n]);
        _n = 29; this.Trebor_Honor_Guard = _data[_n] == "1" ? true : false;
        _n = 30; this.Gnilda_Staff_Keeper = _data[_n] == "1" ? true : false;
        _n = 31; this.Llylgamyn_Knight = _data[_n] == "1" ? true : false;
        _n = 32; this.Descendent_of_Diamonds = _data[_n] == "1" ? true : false;
        _n = 33; this.Star_of_Llylgamyn = _data[_n] == "1" ? true : false;

        _n = 34; string[] _mspells = _data[_n].Split(";");
        for (int i = 0; i < _mspells.Length; i++) this.mageSpells[i] = int.Parse(_mspells[i]);

        _n = 35; string[] _mspellsC = _data[_n].Split(";");
        for (int i = 0; i < _mspellsC.Length; i++) this.mageSpellsCast[i] = int.Parse(_mspellsC[i]);

        _n = 36; string[] _pspells = _data[_n].Split(";");
        for (int i = 0; i < _pspells.Length; i++) this.priestSpells[i] = int.Parse(_pspells[i]);

        _n = 37; string[] _pspellsC = _data[_n].Split(";");
        for (int i = 0; i < _pspellsC.Length; i++) this.priestSpellsCast[i] = int.Parse(_pspellsC[i]);

        _n = 38; string[] _allSpKn = _data[_n].Split(";");
        for (int i = 0; i < _allSpKn.Length; i++) this.SpellKnown[i] = _allSpKn[i] == "1" ? true : false;
    }

    public string Save_Character()
    {
        string _result = "", _tmpNam = name.ToUpper();
        while (_tmpNam.Length < 15) _tmpNam += " ";
        for (int i = 0; i < 15; i++)
        {
            if (_tmpNam[i].Equals(" ")) _result += "0,";
            if (_tmpNam[i].Equals("A")) _result += "1,";
            if (_tmpNam[i].Equals("B")) _result += "2,";
            if (_tmpNam[i].Equals("C")) _result += "3,";
            if (_tmpNam[i].Equals("D")) _result += "4,";
            if (_tmpNam[i].Equals("E")) _result += "5,";
            if (_tmpNam[i].Equals("F")) _result += "6,";
            if (_tmpNam[i].Equals("G")) _result += "7,";
            if (_tmpNam[i].Equals("H")) _result += "8,";
            if (_tmpNam[i].Equals("I")) _result += "9,";
            if (_tmpNam[i].Equals("J")) _result += "10,";
            if (_tmpNam[i].Equals("K")) _result += "11,";
            if (_tmpNam[i].Equals("L")) _result += "12,";
            if (_tmpNam[i].Equals("M")) _result += "13,";
            if (_tmpNam[i].Equals("N")) _result += "14,";
            if (_tmpNam[i].Equals("O")) _result += "15,";
            if (_tmpNam[i].Equals("P")) _result += "16,";
            if (_tmpNam[i].Equals("Q")) _result += "17,";
            if (_tmpNam[i].Equals("R")) _result += "18,";
            if (_tmpNam[i].Equals("S")) _result += "19,";
            if (_tmpNam[i].Equals("T")) _result += "20,";
            if (_tmpNam[i].Equals("U")) _result += "21,";
            if (_tmpNam[i].Equals("V")) _result += "22,";
            if (_tmpNam[i].Equals("W")) _result += "23,";
            if (_tmpNam[i].Equals("X")) _result += "24,";
            if (_tmpNam[i].Equals("Y")) _result += "25,";
            if (_tmpNam[i].Equals("Z")) _result += "26,";
        }
                                                 // 0
        _result += inParty ? "1," : "0,";        // 1
        _result += (int)location + "," +         // 2
                   (int)race + "," +             // 3
                   (int)character_class + "," +  // 4
                   ageInWeeks + "," +            // 5
                   (int)status + "," +           // 6
                   Save_vs_Death + "," +         // 7
                   Save_vs_Wand + "," +          // 8
                   Save_vs_Breath + "," +        // 9
                   Save_vs_Petrify + "," +       // 10
                   Save_vs_Spell + "," +         // 11
                   Geld + ",";                   // 12
        string items = "";
        for (int i = 0; i < 8; i++)
        {
            items += Inventory[i].index + "||";
            items += Inventory[i].equipped ? "1||" : "0||";
            items += Inventory[i].curse_active ? "1||" : "0||";
            items += Inventory[i].identified ? "1||;" : "0||;";
        }
        _result += items + ",";                  //13
        _result += level + ",";                  //14
        _result += xp + ",";                     //15
        _result += xp_nnl + ",";                 //16
        _result += HP + ",";                     //17
        _result += HP_MAX + ",";                 //18
        _result += hitDiceSides + ",";           //19
        _result += ArmorClass + ",";             //20
        _result += heal_points + ",";            //21
        _result += crit_hit ? "1," : "0,";       //22
        _result += hit_dam.num + ",";            //23
        _result += hit_dam.sides + ",";          //24
        _result += hit_dam.bonus + ",";          //25
        _result += lostXYL.x + ",";              //26
        _result += lostXYL.y + ",";              //27
        _result += lostXYL.z + ",";              //28
        _result += Trebor_Honor_Guard ? "1," : "0,"; //29
        _result += Gnilda_Staff_Keeper ? "1," : "0,"; //30
        _result += Llylgamyn_Knight ? "1," : "0,"; //31
        _result += Descendent_of_Diamonds ? "1," : "0,"; //32
        _result += Star_of_Llylgamyn ? "1," : "0,"; //33
        string _ms = "", _mc = "", _ps = "", _pc = "";
        for (int i = 0; i < 7; i++)
        {
            _ms += mageSpells[i] + ";";
            _mc += mageSpellsCast[i] + ";";
            _ps += priestSpells[i] + ";";
            _pc += priestSpellsCast[i] + ";";
        }
        _result += _ms + ",";  //34
        _result += _mc + ",";  //35
        _result += _ps + ",";  //36
        _result += _pc + ",";  //37
        string _sk = "";
        for (int i = 0; i < SpellKnown.Length; i++)
            _sk += SpellKnown[i] ? "1;" : "0;";
        _result += _sk + ","; //38
        return _result;
    }
}
