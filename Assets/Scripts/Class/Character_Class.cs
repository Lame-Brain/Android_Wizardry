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
    public Enum._Alignment alignment;
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
    public int eqWeapon, eqArmor, eqShield, eqHelmet, eqGauntlet, eqMisc;
    public bool Trebor_Honor_Guard; // ">"
    public bool Gnilda_Staff_Keeper; // "K"
    public bool Llylgamyn_Knight; // "G"
    public bool Descendent_of_Diamonds; // "D"
    public bool Star_of_Llylgamyn; // "*"
    public int Strength, IQ, Piety, Vitality, Agility, Luck;    
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
        this.character_class = Enum._Class.fighter;
        this.alignment = Enum._Alignment.neutral;
        this.ageInWeeks = Random.Range(0, 300) + 936; 
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
        this.swing_count = 1;
        this.hit_dam = new Dice(1, 2, 0); //1d2
        this.lostXYL = Vector3Int.zero;
        this.Trebor_Honor_Guard = false;
        this.Gnilda_Staff_Keeper = false;
        this.Llylgamyn_Knight = false;
        this.Descendent_of_Diamonds = false;
        this.Star_of_Llylgamyn = false;
        this.Strength = 10; this.IQ = 10; this.Piety = 10; this.Vitality = 10; this.Agility = 10; this.Luck = 10;
        this.eqWeapon = -1; this.eqArmor = -1; this.eqShield = -1; this.eqHelmet = -1; this.eqGauntlet = -1; this.eqMisc = -1;
        this.mageSpells = new int[7];
        this.mageSpellsCast = new int[7];
        this.priestSpells = new int[7];
        this.priestSpellsCast = new int[7];
        this.SpellKnown = new bool[50];
        for (int i = 0; i < 50; i++) this.SpellKnown[i] = false;
    }

    public void Load_Character(string _char)
    {
        string[] _data = _char.Split(",");
        int _n = 0;
        this.name = "";
        for (int i = 0; i < 15; i++)
        {
            if (_data[i] == "0") this.name += "";
            if (_data[i] == "1") this.name += "A";
            if (_data[i] == "2") this.name += "B";
            if (_data[i] == "3") this.name += "C";
            if (_data[i] == "4") this.name += "D";
            if (_data[i] == "5") this.name += "E";
            if (_data[i] == "6") this.name += "F";
            if (_data[i] == "7") this.name += "G";
            if (_data[i] == "8") this.name += "H";
            if (_data[i] == "9") this.name += "I";
            if (_data[i] == "10") this.name += "J";
            if (_data[i] == "11") this.name += "K";
            if (_data[i] == "12") this.name += "L";
            if (_data[i] == "13") this.name += "M";
            if (_data[i] == "14") this.name += "N";
            if (_data[i] == "15") this.name += "O";
            if (_data[i] == "16") this.name += "P";
            if (_data[i] == "17") this.name += "Q";
            if (_data[i] == "18") this.name += "R";
            if (_data[i] == "19") this.name += "S";
            if (_data[i] == "20") this.name += "T";
            if (_data[i] == "21") this.name += "U";
            if (_data[i] == "22") this.name += "V";
            if (_data[i] == "23") this.name += "W";
            if (_data[i] == "24") this.name += "X";
            if (_data[i] == "25") this.name += "Y";
            if (_data[i] == "26") this.name += "Z";
            if (_data[i] == "27") this.name += " ";
            if (_data[i] == "28") this.name += "-";
            if (_data[i] == "29") this.name += "'";
        }
        _n = 15; this.inParty = _data[_n] == "1" ?  true : false;
        _n = 16; this.location = (Enum._Locaton)int.Parse(_data[_n]);
        _n = 17; this.race = (Enum._Race)int.Parse(_data[_n]);
        _n = 18; this.character_class = (Enum._Class)int.Parse(_data[_n]);
        _n = 19; this.alignment = (Enum._Alignment)int.Parse(_data[_n]);
        _n = 20; this.ageInWeeks = int.Parse(_data[_n]);
        _n = 21; this.status = (Enum._Status)int.Parse(_data[_n]);
        _n = 22; this.Save_vs_Death = int.Parse(_data[_n]);
        _n = 23; this.Save_vs_Wand = int.Parse(_data[_n]);
        _n = 24; this.Save_vs_Breath = int.Parse(_data[_n]);
        _n = 25; this.Save_vs_Petrify = int.Parse(_data[_n]);
        _n = 26; this.Save_vs_Spell = int.Parse(_data[_n]);
        _n = 27; this.Geld = int.Parse(_data[_n]);
        _n = 28; string[] _allItems = _data[_n].Split(";");
        for (int i = 0; i < 8; i++)
        {
            string[] _itemDetails = _allItems[i].Split("||");            
            this.Inventory[i].index = int.Parse(_itemDetails[0]);
            this.Inventory[i].equipped = _itemDetails[1] == "1" ? true : false;
            this.Inventory[i].curse_active = _itemDetails[2] == "1" ? true : false;
            this.Inventory[i].identified = _itemDetails[3] == "1" ? true : false;
        }
        _n = 29; this.level = int.Parse(_data[_n]);
        _n = 30; this.xp = int.Parse(_data[_n]);
        _n = 31; this.xp_nnl = int.Parse(_data[_n]);
        _n = 32; this.HP = int.Parse(_data[_n]);
        _n = 33; this.HP_MAX = int.Parse(_data[_n]);
        _n = 34; this.hitDiceSides = int.Parse(_data[_n]);
        _n = 35; this.ArmorClass = int.Parse(_data[_n]);
        _n = 36; this.heal_points = int.Parse(_data[_n]);
        _n = 37; this.crit_hit = _data[_n] == "1" ? true : false;
        _n = 38; this.hit_dam.sides = int.Parse(_data[_n]);
        _n = 39; this.hit_dam.num = int.Parse(_data[_n]);
        _n = 40; this.hit_dam.bonus = int.Parse(_data[_n]);
        _n = 41; this.lostXYL.x = int.Parse(_data[_n]);
        _n = 42; this.lostXYL.y = int.Parse(_data[_n]);
        _n = 43; this.lostXYL.z = int.Parse(_data[_n]);
        _n = 44; this.eqWeapon = int.Parse(_data[_n]);
        _n = 45; this.eqArmor = int.Parse(_data[_n]);
        _n = 46; this.eqShield = int.Parse(_data[_n]);
        _n = 47; this.eqHelmet = int.Parse(_data[_n]);
        _n = 48; this.eqGauntlet = int.Parse(_data[_n]);
        _n = 49; this.eqMisc = int.Parse(_data[_n]);
        _n = 50; this.Trebor_Honor_Guard = _data[_n] == "1" ? true : false;
        _n = 51; this.Gnilda_Staff_Keeper = _data[_n] == "1" ? true : false;
        _n = 52; this.Llylgamyn_Knight = _data[_n] == "1" ? true : false;
        _n = 53; this.Descendent_of_Diamonds = _data[_n] == "1" ? true : false;
        _n = 54; this.Star_of_Llylgamyn = _data[_n] == "1" ? true : false;
        _n = 55; this.Strength = int.Parse(_data[_n]);
        _n = 56; this.IQ = int.Parse(_data[_n]);
        _n = 57; this.Piety = int.Parse(_data[_n]);
        _n = 58; this.Vitality = int.Parse(_data[_n]);
        _n = 59; this.Agility = int.Parse(_data[_n]);
        _n = 60; this.Luck = int.Parse(_data[_n]);

        _n = 61; string[] _mspells = _data[_n].Split(";");
        for (int i = 0; i < 7; i++) this.mageSpells[i] = int.Parse(_mspells[i]);

        _n = 62; string[] _mspellsC = _data[_n].Split(";");
        for (int i = 0; i < 7; i++) this.mageSpellsCast[i] = int.Parse(_mspellsC[i]);

        _n = 63; string[] _pspells = _data[_n].Split(";");
        for (int i = 0; i < 7; i++) this.priestSpells[i] = int.Parse(_pspells[i]);

        _n = 64; string[] _pspellsC = _data[_n].Split(";");
        for (int i = 0; i < 7; i++) this.priestSpellsCast[i] = int.Parse(_pspellsC[i]);

        _n = 65; string[] _allSpKn = _data[_n].Split(";");
        for (int i = 0; i < 50; i++) this.SpellKnown[i] = _allSpKn[i] == "1" ? true : false;
    }

    public string Save_Character()
    {
        string _result = "", _tmpNam = name.ToUpper();        
        while (_tmpNam.Length < 15) _tmpNam += "+";
        for (int i = 0; i < 15; i++)
        {
            if (_tmpNam[i].Equals('+')) _result += "0,";
            if (_tmpNam[i].Equals('A')) _result += "1,";
            if (_tmpNam[i].Equals('B')) _result += "2,";
            if (_tmpNam[i].Equals('C')) _result += "3,";
            if (_tmpNam[i].Equals('D')) _result += "4,";
            if (_tmpNam[i].Equals('E')) _result += "5,";
            if (_tmpNam[i].Equals('F')) _result += "6,";
            if (_tmpNam[i].Equals('G')) _result += "7,";
            if (_tmpNam[i].Equals('H')) _result += "8,";
            if (_tmpNam[i].Equals('I')) _result += "9,";
            if (_tmpNam[i].Equals('J')) _result += "10,";
            if (_tmpNam[i].Equals('K')) _result += "11,";
            if (_tmpNam[i].Equals('L')) _result += "12,";
            if (_tmpNam[i].Equals('M')) _result += "13,";
            if (_tmpNam[i].Equals('N')) _result += "14,";
            if (_tmpNam[i].Equals('O')) _result += "15,";
            if (_tmpNam[i].Equals('P')) _result += "16,";
            if (_tmpNam[i].Equals('Q')) _result += "17,";
            if (_tmpNam[i].Equals('R')) _result += "18,";
            if (_tmpNam[i].Equals('S')) _result += "19,";
            if (_tmpNam[i].Equals('T')) _result += "20,";
            if (_tmpNam[i].Equals('U')) _result += "21,";
            if (_tmpNam[i].Equals('V')) _result += "22,";
            if (_tmpNam[i].Equals('W')) _result += "23,";
            if (_tmpNam[i].Equals('X')) _result += "24,";
            if (_tmpNam[i].Equals('Y')) _result += "25,";
            if (_tmpNam[i].Equals('Z')) _result += "26,";
            if (_tmpNam[i].Equals(' ')) _result += "27,";
            if (_tmpNam[i].Equals('-')) _result += "28,";
            if (_tmpNam[i].Equals('\'')) _result += "29,";
        }        
        _result += inParty ? "1," : "0,";        // 15
        _result += (int)location + "," +         // 16
                   (int)race + "," +             // 17
                   (int)character_class + "," +  // 18
                   (int)alignment + "," +        // 19
                   ageInWeeks + "," +            // 20
                   (int)status + "," +           // 21
                   Save_vs_Death + "," +         // 22
                   Save_vs_Wand + "," +          // 23
                   Save_vs_Breath + "," +        // 24
                   Save_vs_Petrify + "," +       // 25
                   Save_vs_Spell + "," +         // 26
                   Geld + ",";                   // 27
        string items = "";
        for (int i = 0; i < 8; i++)
        {
            items += Inventory[i].index + "||";
            items += Inventory[i].equipped ? "1||" : "0||";
            items += Inventory[i].curse_active ? "1||" : "0||";
            items += Inventory[i].identified ? "1||;" : "0||;";
        }
        _result += items + ",";                  //28
        _result += level + ",";                  //29
        _result += xp + ",";                     //30
        _result += xp_nnl + ",";                 //31
        _result += HP + ",";                     //32
        _result += HP_MAX + ",";                 //33
        _result += hitDiceSides + ",";           //34
        _result += ArmorClass + ",";             //35
        _result += heal_points + ",";            //36
        _result += crit_hit ? "1," : "0,";       //37
        _result += hit_dam.num + ",";            //38
        _result += hit_dam.sides + ",";          //39
        _result += hit_dam.bonus + ",";          //40
        _result += lostXYL.x + ",";              //41
        _result += lostXYL.y + ",";              //42
        _result += lostXYL.z + ",";              //43
        _result += eqWeapon + ",";               //44
        _result += eqArmor + ",";                //45
        _result += eqShield + ",";               //46
        _result += eqHelmet + ",";               //47
        _result += eqGauntlet + ",";             //48
        _result += eqMisc + ",";                 //49
        _result += Trebor_Honor_Guard ? "1," : "0,"; //50
        _result += Gnilda_Staff_Keeper ? "1," : "0,"; //51
        _result += Llylgamyn_Knight ? "1," : "0,"; //52
        _result += Descendent_of_Diamonds ? "1," : "0,"; //53
        _result += Star_of_Llylgamyn ? "1," : "0,"; //54
        _result += Strength + ",";                     //55
        _result += IQ + ",";                           //56
        _result += Piety + ",";                        //57
        _result += Vitality + ",";                     //58
        _result += Agility + ",";                      //59
        _result += Luck + ",";                         //60
        string _ms = "", _mc = "", _ps = "", _pc = "";
        for (int i = 0; i < 7; i++)
        {
            _ms += mageSpells[i] + ";";
            _mc += mageSpellsCast[i] + ";";
            _ps += priestSpells[i] + ";";
            _pc += priestSpellsCast[i] + ";";
        }
        _result += _ms + ",";                    //61
        _result += _mc + ",";                    //62
        _result += _ps + ",";                    //63
        _result += _pc + ",";                    //64
        string _sk = "";
        for (int i = 0; i < SpellKnown.Length; i++)
            _sk += SpellKnown[i] ? "1;" : "0;";
        _result += _sk + ",";                    //65
        return _result;
    }

    public void EquipItem(int _slot)
    {
        //Check if something is already equipped
       int _challengeSlot = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Weapon) _challengeSlot = eqWeapon;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Armor) _challengeSlot = eqArmor;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Shield) _challengeSlot = eqShield;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Helmet) _challengeSlot = eqHelmet;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Gauntlets) _challengeSlot = eqGauntlet;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Misc) _challengeSlot = eqMisc;

        //Edge case 1: An item is already equipped, and it is cursed (cancel)
        if (_challengeSlot >= 0 && Inventory[_challengeSlot].curse_active) return;
        //Edge case 2: An item is already equipped, and it is not cursed (unequip it)
        if (_challengeSlot >= 0 && !Inventory[_challengeSlot].curse_active) UnequipItem(_challengeSlot);

        //Ok, proceed with the EQUIP!
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Weapon) eqWeapon = _slot;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Armor) eqArmor = _slot;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Shield) eqShield = _slot;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Helmet) eqHelmet = _slot;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Gauntlets) eqGauntlet = _slot;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Misc) eqMisc = _slot;
        Inventory[_slot].equipped = true;

        //Heal Points
        int _temp = GameManager.ITEM[Inventory[_slot].index].heal_pts;
        if (_temp > 0) this.heal_points += _temp;

        //Armor Mod
        //Debug.Log("AC is " + this.ArmorClass + " minus " + GameManager.ITEM[Inventory[_slot].index].armor_mod);
        this.ArmorClass -= GameManager.ITEM[Inventory[_slot].index].armor_mod;

        //Swings
        _temp = GameManager.ITEM[Inventory[_slot].index].xtra_swings;
        if (_temp > this.swing_count) this.swing_count = _temp;

        //Damage        
        this.hit_dam = GameManager.ITEM[Inventory[_slot].index].damage;

        //Crit_hit
        if (!this.crit_hit && GameManager.ITEM[Inventory[_slot].index].crit_hit) this.crit_hit = true;

        //Curse?
        if (GameManager.ITEM[Inventory[_slot].index].cursed) Inventory[_slot].curse_active = true;
        if (GameManager.ITEM[Inventory[_slot].index].item_align != Enum._Alignment.none &&
            GameManager.ITEM[Inventory[_slot].index].item_align != alignment) Inventory[_slot].curse_active = true;
        if (Inventory[_slot].curse_active) Inventory[_slot].identified = true;
    }

    public void UnequipItem(int _slot)
    {
        //This function walks the equip process backwards, does not check for curses.
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Weapon) eqWeapon = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Armor) eqArmor = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Shield) eqShield = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Helmet) eqHelmet = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Gauntlets) eqGauntlet = -1;
        if (GameManager.ITEM[Inventory[_slot].index].item_type == Enum._Item_Type.Misc) eqMisc = -1;
        Inventory[_slot].equipped = false;

        //Heal Points
        this.heal_points -= GameManager.ITEM[Inventory[_slot].index].heal_pts;

        //Armor Mod
        this.ArmorClass += GameManager.ITEM[Inventory[_slot].index].armor_mod;

        //Swings
        CalculateBaseSwings();

        //Damage        
        if(character_class == Enum._Class.ninja)
        { this.hit_dam = new Dice(2, 4, 0); }
        else { this.hit_dam = new Dice(2, 2, 0); }

        //Crit_hit
        if (character_class == Enum._Class.ninja)
        { this.crit_hit = true; }
        else { this.crit_hit = false; }
    }

    public int CalculateBaseSwings()
    {
        int _result = 0;
        
        this.swing_count = 1;
        
        if(this.character_class == Enum._Class.fighter || character_class == Enum._Class.samurai || character_class == Enum._Class.lord || character_class == Enum._Class.ninja)
            this.swing_count += Mathf.FloorToInt(this.level / 5);

        if (character_class == Enum._Class.ninja) this.swing_count++;

        return _result;
    }

    public bool HasEmptyInventorySlot()
    {
        bool _result = false;
        for (int i = 0; i < 8; i++)
            if (this.Inventory[i].index == -1) _result = true;
        return _result;
    }
    
    public int GetEmptyInventorySlot()
    {
        int _result = -1;
        for (int i = 7; i > -1; i--)
            if (this.Inventory[i].index == -1) _result = i;
        return _result;
    }
}
