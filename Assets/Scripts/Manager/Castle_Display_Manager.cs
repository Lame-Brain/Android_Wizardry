using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Display_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Display;
    public Castle_Pop_Up_Manager PopUp_Panel;
    public Castle_Text_Input_Manager TextInput_Panel;
    public Castle_Character_Sheet_Manager Character_Sheet;
    public Castle_Trade_Panel_Manager Trade_Panel;

    private string location_string, flavor_string;

    public void Update_Display(string location_string_, string flavor_string_)
    {
        location_string = location_string_;
        flavor_string = flavor_string_;

        //build party string
        string[] _partyText = new string[6];
        for (int i = 0; i < 6; i++)
        {
            _partyText[i] = "";
            if (!GameManager.PARTY.EmptySlot(i))
            {
                Character_Class me = GameManager.PARTY.LookUp_PartyMember(i);

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
                string _hpMax = me.HP_MAX.ToString();
                if (me.HP > 9999) _hpMax = "lots";
                if (me.HP < 1000) _hpMax = " " + _hpMax;
                if (me.HP < 100) _hpMax = " " + _hpMax;
                if (me.HP < 10) _hpMax = " " + _hpMax;

                //Status replacment, for spacing
                string _stat = me.status.ToString();
                if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hpMax;

                _partyText[i] = " " + i + " " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                    me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                    _ac + " " + _hp + " " + _stat + "\n";
            }
            else
            {
                _partyText[i] = "\n";
            }
        }

        //build static info
        int _spaces = 30 - location_string.Length;
        string _output = "+--------------------------------------+\n" +
                         "| castle";
        for (int i = 0; i < _spaces; i++) _output += " ";
        _output += location_string + " |\n+--------------------------------------+\n" +
                                     " # character name  class ac hits status \n";
        _output += _partyText[0];
        _output += _partyText[1];
        _output += _partyText[2];
        _output += _partyText[3];
        _output += _partyText[4];
        _output += _partyText[5];
        _output += "+--------------------------------------+\n";

        //Add flavor string
        _output += flavor_string;

        //output the string
        _output = _output.ToUpper();
        _output = _output.Replace("[D]", "d");        
        Display.fontSize = GameManager.FONT;
        Display.text = _output;
    }

    public void Refresh_Display()
    {
        Update_Display(location_string, flavor_string);
    }
}
