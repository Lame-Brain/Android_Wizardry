using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Logic_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Message, up_btn_txt, left_btn_txt, down_btn_txt, right_btn_txt, one_btn_txt, two_btn_txt;
    public TextAsset[] Level_data;

    public GameObject Camp_Screen, Light_Icon, Shield_Icon;

    public Magic_Logic_Controller Magic;

    public void StartDungeon()
    {
        Message.fontSize = GameManager.FONT;
        up_btn_txt.fontSize = GameManager.FONT;
        left_btn_txt.fontSize = GameManager.FONT;
        down_btn_txt.fontSize = GameManager.FONT;
        right_btn_txt.fontSize = GameManager.FONT;
        one_btn_txt.fontSize = GameManager.FONT;
        two_btn_txt.fontSize = GameManager.FONT;
        SetButtonText(1, "Examine");
        SetButtonText(2, "Camp");
        UpdateMessge();
    }

    public void SetButtonText(int _buttonNum, string _newText)
    {
        if (_buttonNum == 1) one_btn_txt.text = _newText.ToUpper();
        if (_buttonNum == 2) two_btn_txt.text = _newText.ToUpper();
    }
    public void UpdateMessge(string _newText = "")
    {
        string _output = "";

        if(_newText == "")
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
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison != 0) _stat = "POISON";
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison == 0) _stat = _hpMax;

                    _partyText[i] = _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
                else
                {
                    _partyText[i] = "\n";
                }
            }
            //build static info
            _output = "character name  class ac hits status\n";
            _output += _partyText[0];
            _output += _partyText[1];
            _output += _partyText[2];
            _output += _partyText[3];
            _output += _partyText[4];
            _output += _partyText[5];
        }



        Message.text = _output;
    }

    public void ButtonPressReceived(string _command)
    {
        if(_command == "make_camp")
        {
            Camp_Screen.SetActive(true);
        }
    }
}
