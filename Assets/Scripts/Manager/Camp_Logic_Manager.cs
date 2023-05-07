using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Logic_Manager : MonoBehaviour
{
    public enum Camp_Logic_States { main = 0 }
    public Camp_Logic_States state;
    public Button_Manager[] button;
    public TMPro.TextMeshProUGUI Message;


    private void OnEnable()
    {
        Message.fontSize = GameManager.FONT;
        for (int i = 0; i < button.Length; i++)
            button[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().fontSize = GameManager.FONT;
        ClearButtons();
        state = Camp_Logic_States.main;
        UpdateScreen();
    }

    public void ClearButtons()
    {
        for (int i = 0; i < button.Length; i++) button[i].UpdateButton("", "");
    }
    public void SetButton(int _num, string _name, string _command)
    {
        if (_num < 0) _num = 0;
        if (_num > button.Length - 1) _num = button.Length - 1;
        button[_num].UpdateButton(_name, _command);
    }

    public void UpdateScreen()
    {
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

                _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                    me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                    _ac + " " + _hp + " " + _stat + "\n";
            }
            else
            {
                _partyText[i] = "\n";
            }
        }

        //build static info
        string _output = "+--------------------------------------+\n" +
                         "| labyrinth                       camp |\n" +
                         "+--------------------------------------+\n" +
                         "# character name  class ac hits status \n";
        _output += _partyText[0];
        _output += _partyText[1];
        _output += _partyText[2];
        _output += _partyText[3];
        _output += _partyText[4];
        _output += _partyText[5];
        _output += "+--------------------------------------+\n";

        if(state == Camp_Logic_States.main)
        {
            //              1234567890123456789012345678901234567890
            _output += "\n\nYour party takes a moment to stop and   \n" +
                           "rest.";
            SetButton(0, "Reorder Party", "reorder_party");
            SetButton(2, "Inspect Party Member", "inspect_member");
            SetButton(6, "Disband Party", "disband_party");
            SetButton(10, "Break Camp", "break_camp");
        }





        //output the string
        _output = _output.ToUpper();
        _output = _output.Replace("[D]", "d");
        Message.text = _output;
    }



    public void Button_Press_Received(string _command)
    {
    }

}
