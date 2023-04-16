using System.Collections;
using System.Collections.Generic;
//using System;
using UnityEngine;
using BlobberEngine;

public class Castle_Logic : MonoBehaviour
{
    public enum ts { Market, Inn_Intro, Inn, Tavern, Tavern_Remove, View_Char, View_Item, Shop_Intro, Shop, Temple, Exit, Training, Inspect, See_Mem, Item_Info }
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
            _input.Create_Button_Last("TRAINING HALL","Guild_Button");
            _input.Create_Button_Last("MAZE ENTRANCE","Maze_Button");
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
            if (!_party.EmptySlot(0)) _txt += "          Remove a member," +
                                            "\n           Inspect a member," +
                                            "\n           Divvy party Geld,\n";
            _txt +=                         "\nor press Leave to return to the castle.";
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
            _input.Create_Button("Divvy Geld", "Divvy_Geld");
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
                    if (itemClassUse.IndexOf(charClass, System.StringComparison.OrdinalIgnoreCase) == -1)
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
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>> ADVENTURER GUILD <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Training)
        {
            string _txt = "+--------------------------------------+\n" +
                          "|Castle                  Training Hall |\n" +
                          "+--------------------------------------+\n\n" +
                          "Here you can recruit new adventurers    \n" +
                          "view adventurers in the roster, change  \n" +
                          "their names, change their class, or     \n" +
                          "retire them from the roster             \n";

            _display.Update_Text_Screen(_txt);

            _input.Clear_Buttons();
            _input.Create_Button("Make Character", "Make_Character");
            _input.Create_Button("View Roster", "Show_Roster");
            _input.Create_Button("Lookup Character", "Inspect_Hero");
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  INSPECT CHARACTER <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Inspect)
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
                if (_selected_character.Inventory[i].index != -1)
                {
                    //String displayed at start of string
                    string _addChar = " ";

                    //ChatGPT suggested this code:
                    string charClass = _selected_character.character_class.ToString().Substring(0, 1);
                    string itemClassUse = Game_Logic.ITEM[_selected_character.Inventory[i].index].class_use;
                    if (itemClassUse.IndexOf(charClass, System.StringComparison.OrdinalIgnoreCase) == -1)
                    {
                        _addChar = "#";
                    }
                    // Replacing this code:
                    //if (!Game_Logic.ITEM[_selected_character.Inventory[i].index].class_use.Contains(_selected_character.character_class.ToString().Substring(0, 1).ToLower())) _addChar = "#";

                    if (!_selected_character.Inventory[i].identified) _addChar = "?";
                    if (_selected_character.Inventory[i].equipped) _addChar = "*";
                    if (_selected_character.Inventory[i].curse_active) _addChar = "-";
                    _18inv[i] = (i + 1) + ")" + _addChar + _selected_character.Inventory[i].ItemName();

                    //Bound string to 20 characters, no more, no less
                    if (_18inv[i].Length > 19) _18inv[i] = _18inv[i].Substring(0, 20);
                    while (_18inv[i].Length < 20) _18inv[i] += " ";
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

            _display.Update_Text_Screen(_txt);
            _input.Clear_Buttons();
            _input.Create_Button("Rename Character", "Rename_Button");
            _input.Create_Button("Learn New Class", "ReClass_Button");
            _input.Create_Button("Retire Character", "Retire_Button");
            _input.Create_Button_Last("BACK", "Leave_Button");
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  SHOP INTRO <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Shop_Intro)
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
                                        "| Castle                          Shop |\n" +
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
                                        "           It's a small shop!           \n\n" +
                                        "           Who will Enter?              ");
            _input.Clear_Buttons();
            for (int i = 0; i < 6; i++)
                if (!_party.EmptySlot(i))
                    _input.Create_Button(_party.LookUp_PartyMember(i).name, "Character:" + i);
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }
        //>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>>  SHOP  <<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
        if (townStatus == ts.Shop)
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
                                        "| Castle                          Shop |\n" +
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
                                        "      Welcome " + _selected_character.name + "\n" +
                                        "     You Have:   " + _selected_character.Geld + " g.\n\n" +
                                        "You May Pool your Party's geld,\n" +
                                        "        Buy an Item,\n" +
                                        "        Sell and Item, Have an item\n" +
                                        "        Uncursed, or have an item\n" +
                                        "        Identified, or leave.") ;
            _input.Clear_Buttons();
            _input.Create_Button("Pool Geld", "Pool_Geld");
            _input.Create_Button("Buy", "Buy_item");
            _input.Create_Button("Sell", "Sell_item");
            _input.Create_Button("Uncurse", "Uncurse_item");
            _input.Create_Button("Identify", "Identify_item");
            _input.Create_Button_Last("LEAVE", "Leave_Button");
        }
    }



    #region Change_Class
    public void ChangeCharacterClass(Character_Class _char, Enum._Class _newClass)
    {
        //Change Attributes
        switch (_char.race)
        {
            case Enum._Race.human:
                _char.Strength = 8;
                _char.IQ = 8;
                _char.Piety = 5;
                _char.Vitality = 8;
                _char.Agility = 8;
                _char.Luck = 9;
                break;
            case Enum._Race.elf:
                _char.Strength = 7;
                _char.IQ = 10;
                _char.Piety = 10;
                _char.Vitality = 6;
                _char.Agility = 9;
                _char.Luck = 6;
                break;
            case Enum._Race.dwarf:
                _char.Strength = 10;
                _char.IQ = 7;
                _char.Piety = 10;
                _char.Vitality = 10;
                _char.Agility = 5;
                _char.Luck = 6;
                break;
            case Enum._Race.gnome:
                _char.Strength = 7;
                _char.IQ = 7;
                _char.Piety = 10;
                _char.Vitality = 8;
                _char.Agility = 10;
                _char.Luck = 7;
                break;
            case Enum._Race.hobbit:
                _char.Strength = 5;
                _char.IQ = 7;
                _char.Piety = 7;
                _char.Vitality = 6;
                _char.Agility = 10;
                _char.Luck = 15;
                break;
        }
        //Set new class
        _char.character_class = _newClass;
        //Unequip items
        for (int i = 0; i < 8; i++)
            if (_char.Inventory[i].index > -1 && !_char.Inventory[i].curse_active) _char.UnequipItem(i);
        //New NNL
        _char.xp_nnl = new XPTable().LookupNNL(1, _newClass);
        //Hit Dice
        switch (_char.character_class)
        {
            case Enum._Class.fighter:
                _char.hitDiceSides = 10;
                break;
            case Enum._Class.mage:
                _char.hitDiceSides = 4;
                break;
            case Enum._Class.priest:
                _char.hitDiceSides = 8;
                break;
            case Enum._Class.thief:
                _char.hitDiceSides = 6;
                break;
            case Enum._Class.bishop:
                _char.hitDiceSides = 6;
                break;
            case Enum._Class.samurai:
                _char.hitDiceSides = 8;
                break;
            case Enum._Class.lord:
                _char.hitDiceSides = 10;
                break;
            case Enum._Class.ninja:
                _char.hitDiceSides = 6;
                _char.hit_dam = new Dice(2, 4, 0);
                break;
            default:
                _char.hitDiceSides = 8;
                break;
        }
        int _new_HP = Random.Range(0, _char.hitDiceSides) + 1, _deltaHP = _char.HP_MAX - _char.HP; 
        Debug.Log("MAX " + _char.HP_MAX + " - HP " + _char.HP + " = " + _deltaHP);
        if (_char.Vitality == 3) _new_HP -= 2; //Adjust for vitality
        if (_char.Vitality == 4 || _char.Vitality == 5) _new_HP--;
        if (_char.Vitality == 16) _new_HP++;
        if (_char.Vitality == 17) _new_HP += 2;
        if (_char.Vitality == 18) _new_HP += 3;
        if(_char.character_class == Enum._Class.samurai)
        {
            _new_HP += Random.Range(0, _char.hitDiceSides) + 1;
            if (_char.Vitality == 3) _new_HP -= 2; //Adjust for vitality
            if (_char.Vitality == 4 || _char.Vitality == 5) _new_HP--;
            if (_char.Vitality == 16) _new_HP++;
            if (_char.Vitality == 17) _new_HP += 2;
            if (_char.Vitality == 18) _new_HP += 3;
        }
        if (_char.HP_MAX < _new_HP)
        {
            _char.HP_MAX = _new_HP;
            _char.HP = _new_HP - _deltaHP;
        }
        //Spells and Magicka
        if(_char.character_class == Enum._Class.mage || _char.character_class == Enum._Class.bishop)
        {
            if(!_char.SpellKnown[30]) _char.SpellKnown[30] = true;
            if(!_char.SpellKnown[31]) _char.SpellKnown[31] = true;
            if (_char.mageSpells[0] < 2) _char.mageSpells[0] = 2;
        }
        if (_char.character_class == Enum._Class.priest)
        {
            if (!_char.SpellKnown[0]) _char.SpellKnown[0] = true;
            if (!_char.SpellKnown[1]) _char.SpellKnown[1] = true;
            if (_char.priestSpells[0] < 2) _char.priestSpells[0] = 2;
        }
            //Age
            _char.ageInWeeks += ((Random.Range(0, 3) + 1) * 52) + 44;
    }
    #endregion


    #region LEVEL_UP
    public void LevelUpCharacter(int _n)
    {
        Character_Class _me = Game_Logic.ROSTER[_n];
        Display_Screen_Controller _display = FindObjectOfType<Display_Screen_Controller>();
        bool _newSpells = false;
        int _deltaStr = 0, _deltaIQ = 0, _deltaPi = 0, _deltaVit = 0, _deltaAgi = 0, _deltaLk = 0;

        //Increase level and assign new nnl        
        _me.level++; _me.xp_nnl = new XPTable().LookupNNL(_me.level, _me.character_class);

        //Each attrib has a 75% chance to change
        int VAL = 65;
        bool _strChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _IQChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _PiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _VitChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _AgiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _LkChange = Random.Range(0, 101) <= VAL ? true : false;

        //if attrib changes, there is a chance it decreases, otherwise it increases
        float _downOdds = (_me.ageInWeeks / 52) / 130f; Debug.Log("Odds of decreasing " + _downOdds * 100);
        if (_strChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaStr = -1; }
            else { _deltaStr = 1; }
        if (_IQChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaIQ = -1; }
            else { _deltaIQ = 1; }
        if (_PiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaPi = -1; }
            else { _deltaPi = 1; }
        if (_VitChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaVit = -1; }
            else { _deltaVit = 1; }
        if (_AgiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaAgi = -1; }
            else { _deltaAgi = 1; }
        if (_LkChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaLk = -1; }
            else { _deltaLk = 1; }
        // If Attribs are 18 and delta is -1, 5 in 6 chance that they do not change
        if (_me.Strength == 18 && _deltaStr == -1 && Random.Range(0, 6) > 0) _deltaStr = 0;
        if (_me.IQ == 18 && _deltaIQ == -1 && Random.Range(0, 6) > 0) _deltaIQ = 0;
        if (_me.Piety == 18 && _deltaPi == -1 && Random.Range(0, 6) > 0) _deltaPi = 0;
        if (_me.Vitality == 18 && _deltaVit == -1 && Random.Range(0, 6) > 0) _deltaVit = 0;
        if (_me.Agility == 18 && _deltaAgi == -1 && Random.Range(0, 6) > 0) _deltaAgi = 0;
        if (_me.Luck == 18 && _deltaLk == -1 && Random.Range(0, 6) > 0) _deltaLk = 0;
        //Apply attrib changes
        _me.Strength += _deltaStr;
        _me.IQ += _deltaIQ;
        _me.Piety += _deltaPi;
        _me.Vitality += _deltaVit;
        _me.Agility += _deltaAgi;
        _me.Luck += _deltaLk;
        //Bound Attribs
        if (_me.Strength > 18) { _me.Strength = 18; _deltaStr = 0; }
        if (_me.Strength < 1) { _me.Strength = 1; _deltaStr = 0; }
        if (_me.IQ > 18) { _me.IQ = 18; _deltaIQ = 0; }
        if (_me.IQ < 1) { _me.IQ = 1; _deltaIQ = 0; }
        if (_me.Piety > 18) { _me.Piety = 18; _deltaPi = 0; }
        if (_me.Piety < 1) { _me.Piety = 1; _deltaPi = 0; }
        if (_me.Vitality > 18) { _me.Vitality = 18; _deltaVit = 0; }
        if (_me.Agility > 18) { _me.Agility = 18; _deltaAgi = 0; }
        if (_me.Agility < 1) { _me.Agility = 1; _deltaAgi = 0; }
        if (_me.Luck > 18) { _me.Luck = 18; _deltaLk = 0; }
        if (_me.Luck < 1) { _me.Luck = 1; _deltaLk = 0; }
        //Vitality below 3 is a special case
        if (_me.Vitality < 3)
        {
            Debug.Log("RIP");
            Castle_Logic _cl = FindObjectOfType<Castle_Logic>();
            _me.Vitality = 1; _deltaVit = 0;
            _me.status = BlobberEngine.Enum._Status.dead;
            _me.location = BlobberEngine.Enum._Locaton.Temple;
            _me.inParty = false;
            Game_Logic.PARTY.RemoveMember(_cl._selectedRoster);
            string _RIP = _me.name + " has died of old age.".ToUpper();
            _display.PopUpMessage(_RIP);
            _cl._selected_character = null;
            _cl._selectedRoster = -1;
            _cl.townStatus = Castle_Logic.ts.Inn_Intro;
            _cl.Update_Screen();
            return;
        }
        //Swing Count
        _me.CalculateBaseSwings();
        if (_me.eqWeapon > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings;
        if (_me.eqArmor > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings;
        if (_me.eqShield > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqShield].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqShield].index].xtra_swings;
        if (_me.eqHelmet > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings;
        if (_me.eqGauntlet > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings;
        if (_me.eqMisc > -1 && Game_Logic.ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings > _me.swing_count) _me.swing_count = Game_Logic.ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings;

        //ReRoll HP_MAX
        int _bonus = 0; //Bonus hitpoints based on vitality
        if (_me.Vitality == 3) _bonus = -3;
        if (_me.Vitality == 5 || _me.Vitality == 4) _bonus = -1;
        if (_me.Vitality == 16) _bonus = 1;
        if (_me.Vitality == 17) _bonus = 2;
        if (_me.Vitality == 18) _bonus = 3;
        int _hpDelta = _me.HP_MAX - _me.HP;
        _me.HP_MAX++;
        int _newHP = 0; for (int i = 0; i < _me.level; i++) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //roll once per level
        if (_me.character_class == Enum._Class.samurai) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //Samurai get 1 extra roll
        if (_newHP > _me.HP_MAX) _me.HP_MAX = _newHP; //assign newHP if it is greater than HP_MAX + 1
        _me.HP = _me.HP_MAX - _hpDelta; //Gaining new HP_Max should not widen the gap between HP and HP_Max

        //Wizardry
        int a = 0, b = 0;
        //Mage Spells
        if (_me.character_class == Enum._Class.mage || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.samurai)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
            //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.mage) { a = 0; b = 2; }
            if (_me.character_class == Enum._Class.bishop) { a = 0; b = 4; }
            if (_me.character_class == Enum._Class.samurai) { a = 3; b = 3; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.mageSpells[i])
                {
                    _newSpells = true;
                    _me.mageSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.IQ / 30);
            for (int i = 29; i < 50; i++)
                if (_me.mageSpells[Game_Logic.SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + Game_Logic.SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[29]) _SpC[0]++;
            if (_me.SpellKnown[30]) _SpC[0]++;
            if (_me.SpellKnown[31]) _SpC[0]++;
            if (_me.SpellKnown[32]) _SpC[0]++;
            if (_me.SpellKnown[33]) _SpC[1]++;
            if (_me.SpellKnown[34]) _SpC[1]++;
            if (_me.SpellKnown[35]) _SpC[2]++;
            if (_me.SpellKnown[36]) _SpC[2]++;
            if (_me.SpellKnown[37]) _SpC[3]++;
            if (_me.SpellKnown[38]) _SpC[3]++;
            if (_me.SpellKnown[39]) _SpC[3]++;
            if (_me.SpellKnown[40]) _SpC[4]++;
            if (_me.SpellKnown[41]) _SpC[4]++;
            if (_me.SpellKnown[42]) _SpC[4]++;
            if (_me.SpellKnown[43]) _SpC[5]++;
            if (_me.SpellKnown[44]) _SpC[5]++;
            if (_me.SpellKnown[45]) _SpC[5]++;
            if (_me.SpellKnown[46]) _SpC[5]++;
            if (_me.SpellKnown[47]) _SpC[6]++;
            if (_me.SpellKnown[48]) _SpC[6]++;
            if (_me.SpellKnown[49]) _SpC[6]++;
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.mageSpells[i] < _SpC[i]) _me.mageSpells[i] = _SpC[i];
        }
        //Priest Spells
        if (_me.character_class == Enum._Class.priest || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.lord)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
            //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.priest) { a = 0; b = 2; }
            if (_me.character_class == Enum._Class.bishop) { a = 3; b = 4; }
            if (_me.character_class == Enum._Class.lord) { a = 3; b = 2; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.priestSpells[i])
                {
                    _newSpells = true;
                    _me.priestSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.Piety / 30);
            for (int i = 0; i < 30; i++)
                if (_me.priestSpells[Game_Logic.SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + Game_Logic.SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[0]) _SpC[0]++;
            if (_me.SpellKnown[1]) _SpC[0]++;
            if (_me.SpellKnown[2]) _SpC[0]++;
            if (_me.SpellKnown[3]) _SpC[0]++;
            if (_me.SpellKnown[4]) _SpC[0]++;
            if (_me.SpellKnown[5]) _SpC[1]++;
            if (_me.SpellKnown[6]) _SpC[1]++;
            if (_me.SpellKnown[7]) _SpC[1]++;
            if (_me.SpellKnown[8]) _SpC[1]++;
            if (_me.SpellKnown[9]) _SpC[2]++;
            if (_me.SpellKnown[10]) _SpC[2]++;
            if (_me.SpellKnown[11]) _SpC[2]++;
            if (_me.SpellKnown[12]) _SpC[2]++;
            if (_me.SpellKnown[13]) _SpC[3]++;
            if (_me.SpellKnown[14]) _SpC[3]++;
            if (_me.SpellKnown[15]) _SpC[3]++;
            if (_me.SpellKnown[16]) _SpC[3]++;
            if (_me.SpellKnown[17]) _SpC[4]++;
            if (_me.SpellKnown[18]) _SpC[4]++;
            if (_me.SpellKnown[19]) _SpC[4]++;
            if (_me.SpellKnown[20]) _SpC[4]++;
            if (_me.SpellKnown[21]) _SpC[4]++;
            if (_me.SpellKnown[22]) _SpC[4]++;
            if (_me.SpellKnown[23]) _SpC[5]++;
            if (_me.SpellKnown[24]) _SpC[5]++;
            if (_me.SpellKnown[25]) _SpC[5]++;
            if (_me.SpellKnown[26]) _SpC[5]++;
            if (_me.SpellKnown[27]) _SpC[6]++;
            if (_me.SpellKnown[28]) _SpC[6]++;
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.priestSpells[i] < _SpC[i]) _me.priestSpells[i] = _SpC[i];
        }

        string _levelUpMessage = _me.name + " has leveled up!";
        if (_newSpells) _levelUpMessage += "\n" + _me.name + " learned new spells!!!";
        if (_deltaStr < 0) _levelUpMessage += "\n" + _me.name + " has lost Strength.";
        if (_deltaStr > 0) _levelUpMessage += "\n" + _me.name + " has gained Strength.";
        if (_deltaIQ < 0) _levelUpMessage += "\n" + _me.name + " has lost I.Q.";
        if (_deltaIQ > 0) _levelUpMessage += "\n" + _me.name + " has gained I.Q.";
        if (_deltaPi < 0) _levelUpMessage += "\n" + _me.name + " has lost Piety.";
        if (_deltaPi > 0) _levelUpMessage += "\n" + _me.name + " has gained Piety.";
        if (_deltaVit < 0) _levelUpMessage += "\n" + _me.name + " has lost Vitality.";
        if (_deltaVit > 0) _levelUpMessage += "\n" + _me.name + " has gained Vitality.";
        if (_deltaAgi < 0) _levelUpMessage += "\n" + _me.name + " has lost Agility.";
        if (_deltaAgi > 0) _levelUpMessage += "\n" + _me.name + " has gained Agility.";
        if (_deltaLk < 0) _levelUpMessage += "\n" + _me.name + " has lost Luck.";
        if (_deltaLk > 0) _levelUpMessage += "\n" + _me.name + " has gained Luck.";
        _levelUpMessage = _levelUpMessage.ToUpper();
        _display.PopUpMessage(_levelUpMessage);
    }
    #endregion

}
