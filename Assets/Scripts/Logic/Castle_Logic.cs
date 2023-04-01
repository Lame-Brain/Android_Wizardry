using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Logic : MonoBehaviour
{
    public enum ts { Market, Inn_Intro, Inn, Tavern, Tavern_Remove, Shop, Temple, Exit, Training, Inspect, See_Mem, Item_Info }
    public ts townStatus;
    public Character_Class _selected_character;
    public int _selectedRoster;

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
                        _input.Create_Button(_party.LookUp_PartyMember(i).name, "" + i);
            }
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

    }
}
