using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Logic : MonoBehaviour
{
    public enum ts { Market, Inn_Intro, Inn, Tavern, Shop, Temple, Exit, Training, Inspect, See_Mem, Item_Info }
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
            _input.Enable_Button(_input.Advntr_Inn_button);
            _input.Enable_Button(_input.Glgmsh_Tavern_button);
            _input.Enable_Button(_input.Bltc_TP_button);
            _input.Enable_Button(_input.Tmpl_CANT_button);
            _input.Enable_Button(_input.Edge_of_Town_button);
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
            {
                if (!_party.EmptySlot(i))
                {
                    GameObject _go = Instantiate(_input.Name_button, _input.transform);
                    _go.tag = "Temp_Button";
                    _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = Game_Logic.PARTY.LookUp_PartyMember(i).name;
                    _go.GetComponent<Name_Button_Controller>().String = "" + i;
                    _go.SetActive(true);
                }
            }
            _input.Enable_Button_Last(_input.Leave_button);
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
            _input.Enable_Button(_input.Stables_button);
            _input.Enable_Button(_input.Cot_button);
            _input.Enable_Button(_input.EconR_button);
            _input.Enable_Button(_input.MerchS_button);
            _input.Enable_Button(_input.RoyalS_button);
            _input.Enable_Button_Last(_input.Leave_button);
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
            if(_party.EmptySlot(5)) _input.Enable_Button(_input.AddMember_button);
            if (!_party.EmptySlot(0))
            {
                _input.Enable_Button(_input.RemMember_button);
                for (int i = 0; i < 6; i++)
                {
                    if (!_party.EmptySlot(i))
                    {
                        GameObject _go = Instantiate(_input.Name_button, _input.transform);
                        _go.tag = "Temp_Button";
                        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = Game_Logic.PARTY.LookUp_PartyMember(i).name;
                        _go.GetComponent<Name_Button_Controller>().String = "" + i;
                        _go.SetActive(true);
                    }
                }
            }
            _input.Enable_Button_Last(_input.Leave_button);
        }

    }
}
