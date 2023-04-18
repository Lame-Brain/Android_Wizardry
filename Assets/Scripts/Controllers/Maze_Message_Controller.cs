using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Maze_Message_Controller : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Message;

    public void Show_Status_Message()
    {
        string[] _charLines = new string[6];
        for (int i = 0; i < 6; i++)
        {
            
            if(Game_Logic.TEXT_FONT == 0) Game_Logic.TEXT_FONT = 40.1f;
            Message.fontSize = Game_Logic.TEXT_FONT;

            if (!Game_Logic.PARTY.EmptySlot(i))
            {
                string _start = (i + 1) + " ";

                Character_Class _thisChar = Game_Logic.PARTY.LookUp_PartyMember(i);

                string _name = _thisChar.name; 
                while (_name.Length < 16) _name += " ";

                string _class = _thisChar.alignment.ToString().Substring(0, 1) + "-" + _thisChar.character_class.ToString().Substring(0, 3) + " ";

                string _ac = "";
                _ac = _thisChar.ArmorClass.ToString();
                if (_thisChar.ArmorClass > 10) _ac = "hi";
                if (_thisChar.ArmorClass < 10) _ac = " " + _thisChar.ArmorClass;
                if (_thisChar.ArmorClass < -9) _ac = "Lo";

                string _hits = "    " + _thisChar.HP.ToString();
                if (_thisChar.HP > 9) _hits = "   " + _thisChar.HP.ToString();
                if (_thisChar.HP > 99) _hits = "  " + _thisChar.HP.ToString();
                if (_thisChar.HP > 999) _hits = " " + _thisChar.HP.ToString();
                if (_thisChar.HP > 9999) _hits = " lots";

                string _status = " / " + _thisChar.HP_MAX;
                if (_thisChar.status != BlobberEngine.Enum._Status.OK) _status = _thisChar.status.ToString();

                _charLines[i] = _start + _name + _class + _ac + _hits + _status + "\n\n";
            }
            else
            {
                _charLines[i] = (i + 1) + "...\n\n";
            }
        }
        string _txt = "\n\n# character name  class ac hits status \n\n";
        for (int i = 0; i < 6; i++) _txt += _charLines[i];

        Message.text = _txt;
    }
}
