using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Castle_Logic : MonoBehaviour
{
    public enum ts { Market, Inn_Intro, Inn, Tavern, Tavern_Remove, View_Char, View_Item, Shop, Temple, Exit, Training, Inspect, See_Mem, Item_Info }
    public ts townStatus;
    public Character_Class _selected_character;
    public int _selectedRoster;
    public int _selectedInventorySlot;
    public int _selectedItemIndex;

    private Display_Screen_Controller _display;
    private Input_Screen_Controller _input;
    private Party_Class _party;


    private void Start()
    {
        _display = FindObjectOfType<Display_Screen_Controller>();
        _input = FindObjectOfType<Input_Screen_Controller>();
        _party = FindObjectOfType<Party_Class>();
        _selected_character = null;
        _selectedRoster = -1;

        townStatus = ts.Market;
        Update_Screen();
    }

    public void Update_Screen()
    {
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  MARKET <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if(townStatus == ts.Market)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);
                    
                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";
                    
                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }
            
            _display.Update_Text_Screen("+--------------------------------------+\n" +
                                        "| Castle                        Market |\n" +
                                        "+----------- Current party: -----------+\n" +
                                        "                                        \n" +
                                        " # character name  class ac hits status \n" +
                                        _partyText[0] +
                                        _partyText[1] +
                                        _partyText[2] +
                                        _partyText[3] +
                                        _partyText[4] +
                                        _partyText[5] +
                                        "+--------------------------------------+\n" +
                                        "                                        \n" +
                                        "               you may go to:           \n" +
                                        "                                        \n" +
                                        "   The adventurer's inn, gilgamesh's    \n" +
                                        "   tavern, Boltac's Trading post, the   \n" +
                                        "   temple of cant, or edge of town.");
            _input.Clear_Buttons();
            _input.Create_Button("INN", "Inn_Button");
            _input.Create_Button("TAVERN", "Tavern_Button");
            _input.Create_Button("TRADE POST","Boltac_Button");
            _input.Create_Button("TEMPLE","Temple_Button");
            _input.Create_Button_Last("EDGE OF TOWN","Edge_of_Town_Button");
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  INN INTRO <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Inn_Intro)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }

            _display.Update_Text_Screen("+--------------------------------------+\n" +
                                        "| Castle                           Inn |\n" +
                                        "+----------- Current party: -----------+\n" +
                                        "                                        \n" +
                                        " # character name  class ac hits status \n" +
                                        _partyText[0] +
                                        _partyText[1] +
                                        _partyText[2] +
                                        _partyText[3] +
                                        _partyText[4] +
                                        _partyText[5] +
                                        "+--------------------------------------+\n" +
                                        "                                        \n" +
                                        "                                        \n" +
                                        "             Who will Stay?          ");
            _input.Clear_Buttons();
            for (int i = 0; i < 6; i++)
                if (!_party.EmptySlot(i)) 
                    _input.Create_Button(_party.LookUp_PartyMember(i).name, "" + i);
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  INN <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Inn)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }

            _display.Update_Text_Screen("+--------------------------------------+\n" +
                                        "| Castle                           Inn |\n" +
                                        "+----------- Current party: -----------+\n" +
                                        "                                        \n" +
                                        " # character name  class ac hits status \n" +
                                        _partyText[0] +
                                        _partyText[1] +
                                        _partyText[2] +
                                        _partyText[3] +
                                        _partyText[4] +
                                        _partyText[5] +
                                        "+--------------------------------------+\n" +
                                        "                                        \n" +
                                        "   Welcome " + _selected_character.name + ". We have:\n" +
                                        "                                        \n" +
                                        "   The Stables ( free! )\n" +
                                        "   Cots. 10 g/week. \n" +
                                        "   Econonomy Rooms. 50g/week.\n" +
                                        "   Merchant Suites. 200 g/week.\n" +
                                        "   Royal Suites. 500 g/week. \n" +
                                        "   Leave.");
            _input.Clear_Buttons();
            _input.Create_Button("STABLES","Stables");
            _input.Create_Button("COTS","Cots");
            _input.Create_Button("ECONOMY ROOM","Economy");
            _input.Create_Button("MERCHANT SUITE","Merchant");
            _input.Create_Button("ROTAL SUITES","Royal");
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  TAVERN <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Tavern)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }
            string _txt =  "+--------------------------------------+\n" +
                           "| Castle                        Tavern |\n" +
                           "+----------- Current party: -----------+\n" +
                           "                                        \n" +
                           " # character name  class ac hits status \n" +
                           _partyText[0] +
                           _partyText[1] +
                           _partyText[2] +
                           _partyText[3] +
                           _partyText[4] +
                           _partyText[5] +
                           "+--------------------------------------+\n" +
                           "                                        \n" +
                           "   You May ";
            if (_party.EmptySlot(5)) _txt += "Add a member,\n ";
            if (!_party.EmptySlot(0)) _txt += "          Remove a member,\n           Inspect a member,\n";
            _txt += "\nor press Leave to return to the castle.";
            _display.Update_Text_Screen(_txt);

            _input.Clear_Buttons();
            if(_party.EmptySlot(5)) _input.Create_Button("ADD MEMBER", "Add_Member");
            if (!_party.EmptySlot(0))
            {
                _input.Create_Button("REMOVE MEMBER", "Rem_Member");
                for (int i = 0; i < 6; i++)
                    if (!_party.EmptySlot(i))
                        _input.Create_Button("VIEW " + _party.LookUp_PartyMember(i).name, "View:" + i);
            }
            _input.Create_Button("View Roster", "Show_Roster");
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }

    
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  TAVERN REMOVE <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Tavern_Remove)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }
            string _txt = "+--------------------------------------+\n" +
                           "| Castle                        Tavern |\n" +
                           "+----------- Current party: -----------+\n" +
                           "                                        \n" +
                           " # character name  class ac hits status \n" +
                           _partyText[0] +
                           _partyText[1] +
                           _partyText[2] +
                           _partyText[3] +
                           _partyText[4] +
                           _partyText[5] +
                           "+--------------------------------------+\n" +
                           "                                        \n" +
                           "   Who would you like to remove? ";
            _display.Update_Text_Screen(_txt);

            _input.Clear_Buttons();
            for (int i = 0; i < 6; i++)
                if (!_party.EmptySlot(i))
                    _input.Create_Button(_party.LookUp_PartyMember(i).name, "Char:" + i);
            _input.Create_Button_Last("DONE", "Leave_Button");
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  VIEW CHARACTER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.View_Char)
        {            
            string _txt = _selected_character.name + " " + _selected_character.race.ToString() + " " + _selected_character.alignment.ToString()[0] + "-" + _selected_character.character_class.ToString() + " ";
            if (_selected_character.Trebor_Honor_Guard) _txt += ">";
            if (_selected_character.Gnilda_Staff_Keeper) _txt += "K";
            if (_selected_character.Llylgamyn_Knight) _txt += "G";
            if (_selected_character.Descendent_of_Diamonds) _txt += "D";
            if (_selected_character.Star_of_Llylgamyn) _txt += "*";
            _txt += "\n\n";
            //Strength Line
            if (_selected_character.Strength < 10)
            { _txt += "    STRENGTH  " + _selected_character.Strength + "    Geld " + _selected_character.Geld + "\n"; }
            else
            { _txt += "    STRENGTH " + _selected_character.Strength + "    Geld " + _selected_character.Geld + "\n"; }

            //IQ line
            if (_selected_character.IQ < 10)
            { _txt += "        I.Q.  " + _selected_character.IQ + "     EXP " + _selected_character.xp + "\n"; }
            else
            { _txt += "        I.Q. " + _selected_character.IQ + "     EXP " + _selected_character.xp + "\n"; }

            //Piety Line
            if (_selected_character.Piety < 10)
            { _txt += "       PIETY  " + _selected_character.Piety + "\n"; }
            else
            { _txt += "       PIETY " + _selected_character.Piety + "\n"; }

            //Vitality Line
            if (_selected_character.Vitality < 10)
            { _txt += "    VITALITY  " + _selected_character.Vitality + "   LEVEL"; }
            else
            { _txt += "    VITALITY " + _selected_character.Vitality + "   LEVEL"; }
            if (_selected_character.level < 10) _txt += " ";
            if (_selected_character.level < 100) _txt += " ";
            if (_selected_character.level > 99) _txt += " ";
            _txt += _selected_character.level + "       AGE";
            _txt += " " + _selected_character.ageInWeeks / 52 + "\n";

            //Agility Line
            if (_selected_character.Agility < 10)
            { _txt += "     AGILITY  " + _selected_character.Agility + "    HITS " + _selected_character.HP; }
            else
            { _txt += "     AGILITY " + _selected_character.Agility + "    HITS " + _selected_character.HP; }
            if (_selected_character.HP < 10) _txt += "  ";
            if (_selected_character.HP > 9 && _selected_character.HP < 100) _txt += " ";
            if (_selected_character.HP > 99) _txt += "";
            _txt += "/" + _selected_character.HP_MAX;
            if (_selected_character.HP_MAX < 10) _txt += "    ";
            if (_selected_character.HP_MAX > 9 && _selected_character.HP_MAX < 100) _txt += "   ";
            if (_selected_character.HP_MAX > 99) _txt += "  ";
            _txt += "AC  ";
            if (_selected_character.ArmorClass > 99) _txt += "HI";
            if (_selected_character.ArmorClass < 100 && _selected_character.ArmorClass > 9) _txt += _selected_character.ArmorClass.ToString();
            if (_selected_character.ArmorClass < 10 && _selected_character.ArmorClass > -1) _txt += " " + _selected_character.ArmorClass;
            if (_selected_character.ArmorClass < 0 && _selected_character.ArmorClass > -10) _txt += _selected_character.ArmorClass.ToString();
            if (_selected_character.ArmorClass < -9) _txt += "LO";
            _txt += "\n";

            //Luck Line
            if (_selected_character.Luck < 10)
            { _txt += "        LUCK  " + _selected_character.Luck + "  STATUS "; }
            else
            { _txt += "        LUCK " + _selected_character.Luck + "  STATUS "; }
            _txt += _selected_character.status.ToString() + "\n\n";

            //Spells
            int[] _temp = new int[7];
            for (int i = 0; i < 7; i++) _temp[i] = _selected_character.mageSpells[i] - _selected_character.mageSpellsCast[i];
            _txt += "        MAGE " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n";
            for (int i = 0; i < 7; i++) _temp[i] = _selected_character.priestSpells[i] - _selected_character.priestSpellsCast[i];
            _txt += "      PRIEST " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n\n";

            //Inventory            
            _txt += "*=EQUIP, -=CURSED, ?=UNKNOWN, #=UNUSABLE\n\n";
            string[] _18inv = new string[8];
            for (int i = 0; i < 8; i++)
            {
                if(_selected_character.Inventory[i].index != -1)
                {
                    //String displayed at start of string
                    string _addChar = " ";

                    //ChatGPT suggested this code:
                    string charClass = _selected_character.character_class.ToString().Substring(0, 1);                    
                    string itemClassUse = Game_Logic.ITEM[_selected_character.Inventory[i].index].class_use;
                    if (itemClassUse.IndexOf(charClass, StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        _addChar = "#";
                    }
                    // Replacing this code:
                    //if (!Game_Logic.ITEM[_selected_character.Inventory[i].index].class_use.Contains(_selected_character.character_class.ToString().Substring(0, 1).ToLower())) _addChar = "#";

                    if (!_selected_character.Inventory[i].identified) _addChar = "?";
                    if (_selected_character.Inventory[i].equipped) _addChar = "*";
                    if (_selected_character.Inventory[i].curse_active) _addChar = "-";
                    _18inv[i] = (i+1) + ")" + _addChar + _selected_character.Inventory[i].ItemName();

                    //Bound string to 20 characters, no more, no less
                    if (_18inv[i].Length > 19) _18inv[i] = _18inv[i].Substring(0, 20);
                    while(_18inv[i].Length < 20) _18inv[i] += " ";
                }
                else
                {
                    _18inv[i] = "                    ";
                }
            }
            _txt += _18inv[0] + _18inv[1] + "\n";
            _txt += _18inv[2] + _18inv[3] + "\n";
            _txt += _18inv[4] + _18inv[5] + "\n";
            _txt += _18inv[6] + _18inv[7] + "\n\n";

            //instructions
            _txt += "You may View an Item, Trade Geld,\n" +
                    "        Read Spell books, or leave.";

            _display.Update_Text_Screen(_txt);
            _input.Clear_Buttons();
            _input.Create_Button("View Item", "View_Item");
            _input.Create_Button("Trade Geld", "Trade_Geld");
            _input.Create_Button("Read Spells", "Read_Magic");
            _input.Create_Button_Last("DONE", "Leave_Button");
        }

        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  VIEW ITEM <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if(townStatus == ts.View_Item)
        {
            string _txt = _selected_character.Inventory[_selectedInventorySlot].ItemName() + "    ";
            switch (Game_Logic.ITEM[_selectedItemIndex].item_type)
            {
                //Weapon, Armor, Helmet, Gauntlets, Special, Misc, Consumable
                case BlobberEngine.Enum._Item_Type.Weapon:
                    _txt += "This is a Weapon.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Helmet:
                    _txt += "This is a Helmet.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Shield:
                    _txt += "This is a Shield.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Armor:
                    _txt += "This is Armor.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Gauntlets:
                    _txt += "This is a set of Gloves.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Special:
                    _txt += "This is a Special Item.\n";
                    break;
                case BlobberEngine.Enum._Item_Type.Consumable:
                    _txt += "This is a Consumable Item.\n";
                    break;
                default:
                    _txt += "This is a Misc Item.\n";
                    break;
            }
            if (_selected_character.Inventory[_selectedInventorySlot].identified)
            {
                _txt += "\n Value: " + Game_Logic.ITEM[_selectedItemIndex].price + "g\n";
                if (Game_Logic.ITEM[_selectedItemIndex].damage.num > 0)
                {
                    _txt += "\n";
                    if (Game_Logic.ITEM[_selectedItemIndex].hit_mod < 0) _txt += "(ToHit " + Game_Logic.ITEM[_selectedItemIndex].hit_mod + ") ";
                    if (Game_Logic.ITEM[_selectedItemIndex].hit_mod >= 0) _txt += "(ToHit +" + Game_Logic.ITEM[_selectedItemIndex].hit_mod + ") ";
                    _txt += "Damage = " + Game_Logic.ITEM[_selectedItemIndex].damage.num + "d" + Game_Logic.ITEM[_selectedItemIndex].damage.sides;
                    if (Game_Logic.ITEM[_selectedItemIndex].damage.bonus < 0) _txt += Game_Logic.ITEM[_selectedItemIndex].damage.bonus;
                    if (Game_Logic.ITEM[_selectedItemIndex].damage.bonus > 0) _txt += "+" + Game_Logic.ITEM[_selectedItemIndex].damage.bonus;
                    if (Game_Logic.ITEM[_selectedItemIndex].xtra_swings > 1) _txt += "x" + Game_Logic.ITEM[_selectedItemIndex].xtra_swings;
                    _txt += "\n";
                }
                if (Game_Logic.ITEM[_selectedItemIndex].armor_mod > 0) _txt += "\n Armor: " + Game_Logic.ITEM[_selectedItemIndex].armor_mod + "\n";
                if (Game_Logic.ITEM[_selectedItemIndex].spell != "") _txt += "\n When invoked, casts: " + Game_Logic.ITEM[_selectedItemIndex].spell + ".\n";
                if (Game_Logic.ITEM[_selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
                if (Game_Logic.ITEM[_selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
                if (Game_Logic.ITEM[_selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
                if (Game_Logic.ITEM[_selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("z")) _txt += "<None>\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("f")) _txt += "Fighter\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("m")) _txt += "Mage\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("p")) _txt += "Priest\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("t")) _txt += "Thief\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("b")) _txt += "Bishop\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("s")) _txt += "Samurai\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("l")) _txt += "Lord\n";
                if (Game_Logic.ITEM[_selectedItemIndex].class_use.Contains("n")) _txt += "Ninja\n";

                if (_selected_character.Inventory[_selectedInventorySlot].equipped) _txt += "\nThis item is equipped.\n";
                if (!_selected_character.Inventory[_selectedInventorySlot].equipped)
                {
                    string _s = "";
                    if (Game_Logic.ITEM[_selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Weapon && _selected_character.eqWeapon > -1) _s = _selected_character.Inventory[_selected_character.eqWeapon].ItemName();
                    if (Game_Logic.ITEM[_selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Armor && _selected_character.eqArmor > -1) _s = _selected_character.Inventory[_selected_character.eqArmor].ItemName();
                    if (Game_Logic.ITEM[_selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Shield && _selected_character.eqShield > -1) _s = _selected_character.Inventory[_selected_character.eqShield].ItemName();
                    if (Game_Logic.ITEM[_selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Helmet && _selected_character.eqHelmet > -1) _s = _selected_character.Inventory[_selected_character.eqHelmet].ItemName();
                    if (Game_Logic.ITEM[_selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Gauntlets && _selected_character.eqGauntlet > -1) _s = _selected_character.Inventory[_selected_character.eqGauntlet].ItemName();
                    _txt += "\n" + _s + " is equipped to this slot.";
                }
            }
            else
            {
                _txt += "\n Identify this item to learn more about it.\n\n\n";
            }
            _txt += "\n\nYou can Equip/Unequip Item, Trade Item,\n" +
                         "       Trash Item, Identify Item, or Leave.";

            _display.Update_Text_Screen(_txt);
            _input.Clear_Buttons();
            _input.Create_Button("Equip/ Unequip", "Equip_Item");
            _input.Create_Button("Trade Item", "Trade_Item");
            _input.Create_Button("Use Item", "Use_Item");
            _input.Create_Button("Trash Item", "Trash_Item");
            _input.Create_Button("Identify Item", "ID_Item");
            _input.Create_Button_Last("Leave", "Leave_Button");
        }
    }
}
