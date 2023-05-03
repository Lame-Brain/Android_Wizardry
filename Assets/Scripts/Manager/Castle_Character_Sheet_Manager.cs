using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle_Character_Sheet_Manager : MonoBehaviour
{
    public TextMeshProUGUI body, equip_button, viewItem_button, readMagic_button, tradeGeld_button, back_button;
    public GameObject Equip_Flow_Panel;
    public GameObject View_Item_Panel;

    private Castle_Logic_Manager _castle;

    private void OnEnable()
    {        
        body.fontSize = GameManager.FONT;
        equip_button.fontSize = GameManager.FONT;
        viewItem_button.fontSize = GameManager.FONT;
        readMagic_button.fontSize = GameManager.FONT;
        tradeGeld_button.fontSize = GameManager.FONT;
        back_button.fontSize = GameManager.FONT;
        _castle = FindObjectOfType<Castle_Logic_Manager>();
        //_input = FindObjectOfType<Castle_Button_Manager>();
        //_display = FindObjectOfType<Castle_Display_Manager>();
        //_party = FindObjectOfType<Party_Class>();

        string _txt = _castle.Selected_Character.name + " " + _castle.Selected_Character.race.ToString() + " " + _castle.Selected_Character.alignment.ToString()[0] + "-" + _castle.Selected_Character.character_class.ToString() + " ";
        if (_castle.Selected_Character.Trebor_Honor_Guard) _txt += ">";
        if (_castle.Selected_Character.Gnilda_Staff_Keeper) _txt += "K";
        if (_castle.Selected_Character.Llylgamyn_Knight) _txt += "G";
        if (_castle.Selected_Character.Descendent_of_Diamonds) _txt += "D";
        if (_castle.Selected_Character.Star_of_Llylgamyn) _txt += "*";
        _txt += "\n\n";
        //Strength Line
        if (_castle.Selected_Character.Strength < 10)
        { _txt += "    STRENGTH  " + _castle.Selected_Character.Strength + "    Geld " + _castle.Selected_Character.Geld + "\n"; }
        else
        { _txt += "    STRENGTH " + _castle.Selected_Character.Strength + "    Geld " + _castle.Selected_Character.Geld + "\n"; }

        //IQ line
        if (_castle.Selected_Character.IQ < 10)
        { _txt += "        I.Q.  " + _castle.Selected_Character.IQ + "     EXP " + _castle.Selected_Character.xp + "\n"; }
        else
        { _txt += "        I.Q. " + _castle.Selected_Character.IQ + "     EXP " + _castle.Selected_Character.xp + "\n"; }

        //Piety Line
        if (_castle.Selected_Character.Piety < 10)
        { _txt += "       PIETY  " + _castle.Selected_Character.Piety + "\n"; }
        else
        { _txt += "       PIETY " + _castle.Selected_Character.Piety + "\n"; }

        //Vitality Line
        if (_castle.Selected_Character.Vitality < 10)
        { _txt += "    VITALITY  " + _castle.Selected_Character.Vitality + "  LEVEL "; }
        else
        { _txt += "    VITALITY " + _castle.Selected_Character.Vitality + "  LEVEL "; }

        if (_castle.Selected_Character.level > 99) _txt += "";
        if (_castle.Selected_Character.level < 99 && _castle.Selected_Character.level > 9) _txt += "  ";
        if (_castle.Selected_Character.level < 9) _txt += "   ";
        _txt += _castle.Selected_Character.level;

        _txt += "     AGE";
        _txt += " " + _castle.Selected_Character.ageInWeeks / 52 + "\n";

        //Agility Line
        if (_castle.Selected_Character.Agility < 10)
        { _txt += "     AGILITY  " + _castle.Selected_Character.Agility + "  HITS "; }
        else
        { _txt += "     AGILITY " + _castle.Selected_Character.Agility + "  HITS "; }

        if (_castle.Selected_Character.HP < 999) _txt += "";
        if (_castle.Selected_Character.HP > 99 && _castle.Selected_Character.HP < 1000) _txt += " ";
        if (_castle.Selected_Character.HP > 9 && _castle.Selected_Character.HP < 100) _txt += "  ";
        if (_castle.Selected_Character.HP < 10) _txt += "   ";
        
        _txt += _castle.Selected_Character.HP.ToString();

        _txt += "/" + _castle.Selected_Character.HP_MAX;

        if (_castle.Selected_Character.HP_MAX > 999) _txt += "";
        if (_castle.Selected_Character.HP_MAX > 99 && _castle.Selected_Character.HP_MAX < 1000) _txt += "";
        if (_castle.Selected_Character.HP_MAX > 9 && _castle.Selected_Character.HP_MAX < 100) _txt += " ";
        if (_castle.Selected_Character.HP_MAX < 10) _txt += "  ";
        

        _txt += "   AC ";
        if (_castle.Selected_Character.ArmorClass < -9) _txt += "LO";
        if (_castle.Selected_Character.ArmorClass > -10 && _castle.Selected_Character.ArmorClass < 100) _txt += _castle.Selected_Character.ArmorClass;
        if (_castle.Selected_Character.ArmorClass > 99) _txt += "HI";
        _txt += "\n";

        //Luck Line
        if (_castle.Selected_Character.Luck < 10)
        { _txt += "        LUCK  " + _castle.Selected_Character.Luck + "  STATUS "; }
        else
        { _txt += "        LUCK " + _castle.Selected_Character.Luck + "  STATUS "; }
        _txt += _castle.Selected_Character.status.ToString() + "\n\n";

        //Spells
        int[] _temp = new int[7];
        for (int i = 0; i < 7; i++) _temp[i] = _castle.Selected_Character.mageSpells[i] - _castle.Selected_Character.mageSpellsCast[i];
        _txt += "        MAGE " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n";
        for (int i = 0; i < 7; i++) _temp[i] = _castle.Selected_Character.priestSpells[i] - _castle.Selected_Character.priestSpellsCast[i];
        _txt += "      PRIEST " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n\n";

        //Inventory            
        _txt += "*=EQUIP, -=CURSED, ?=UNKNOWN, #=UNUSABLE\n\n";
        string[] _18inv = new string[8];
        for (int i = 0; i < 8; i++)
        {
            if (_castle.Selected_Character.Inventory[i].index != -1)
            {
                //String displayed at start of string
                string _addChar = " ";

                //ChatGPT suggested this code:
                string charClass = _castle.Selected_Character.character_class.ToString().Substring(0, 1);
                string itemClassUse = GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].class_use;
                if (itemClassUse.IndexOf(charClass, System.StringComparison.OrdinalIgnoreCase) == -1)
                {
                    _addChar = "#";
                }
                // Replacing this code:
                //if (!GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].class_use.Contains(_castle.Selected_Character.character_class.ToString().Substring(0, 1).ToLower())) _addChar = "#";

                if (!_castle.Selected_Character.Inventory[i].identified) _addChar = "?";
                if (_castle.Selected_Character.Inventory[i].equipped) _addChar = "*";
                if (_castle.Selected_Character.Inventory[i].curse_active) _addChar = "-";
                _18inv[i] = (i + 1) + ")" + _addChar + _castle.Selected_Character.Inventory[i].ItemName();

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
        _txt = _txt.ToUpper();
        body.text = _txt;
    }

    public void Button(string _command)
    {
        if (_command == "Leave")
        {
            _castle.CurrentPage = 0;
            _castle.UpdateScreen();
            this.gameObject.SetActive(false);
        }
        if (_command == "Equip")
        {
            Equip_Flow_Panel.SetActive(true);
        }
        if (_command == "Items")
        {
            View_Item_Panel.SetActive(true);
            FindObjectOfType<Castle_Item_View_Manager>().ChooseItem();
        }
        if (_command == "Magic")
        {
            View_Item_Panel.SetActive(true);
            FindObjectOfType<Castle_Item_View_Manager>().ShowMagic();
        }
        if (_command == "Geld")
        {
            View_Item_Panel.SetActive(true);
            FindObjectOfType<Castle_Item_View_Manager>().tradeGeld = true;
            FindObjectOfType<Castle_Item_View_Manager>().ChoosePartyMember();
        }
    }
}
