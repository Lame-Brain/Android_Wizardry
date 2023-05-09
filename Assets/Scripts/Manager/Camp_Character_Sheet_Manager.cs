using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Camp_Character_Sheet_Manager : MonoBehaviour
{
    public TextMeshProUGUI body, equip_button, viewItem_button, readMagic_button, back_button, cast_button, identify_button;
    //public GameObject Equip_Button_GO, ViewItem_Button_GO, ReadMagic_Button_GO;
    public GameObject Equip_Flow_Panel;
    public GameObject View_Item_Panel;
    public GameObject Cast_Spell_Panel, Cast_on_Who_panel;
    public TMP_InputField input;
    public TextMeshProUGUI InputPlaceHolder;
    public TextMeshProUGUI InputText;

    private Camp_Logic_Manager _camp;
    private Magic_Logic_Controller _magic;
    private Spell_Class _selected_spell;

    private void OnEnable()
    {
        body.fontSize = GameManager.FONT;
        equip_button.fontSize = GameManager.FONT;
        viewItem_button.fontSize = GameManager.FONT;
        readMagic_button.fontSize = GameManager.FONT;
        back_button.fontSize = GameManager.FONT;
        cast_button.fontSize = GameManager.FONT;
        identify_button.fontSize = GameManager.FONT;
        InputPlaceHolder.fontSize = GameManager.FONT;
        InputText.fontSize = GameManager.FONT;
        input.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
        input.text = "";
        _camp = FindObjectOfType<Camp_Logic_Manager>();
        _magic = FindObjectOfType<Magic_Logic_Controller>();
    }

    public void UpdateScreen()
    {
        this.gameObject.SetActive(true);

        string _txt = _camp.Selected_Character.name + " " + _camp.Selected_Character.race.ToString() + " " + _camp.Selected_Character.alignment.ToString()[0] + "-" + _camp.Selected_Character.character_class.ToString() + " ";
        if (_camp.Selected_Character.Trebor_Honor_Guard) _txt += ">";
        if (_camp.Selected_Character.Gnilda_Staff_Keeper) _txt += "K";
        if (_camp.Selected_Character.Llylgamyn_Knight) _txt += "G";
        if (_camp.Selected_Character.Descendent_of_Diamonds) _txt += "D";
        if (_camp.Selected_Character.Star_of_Llylgamyn) _txt += "*";
        _txt += "\n\n";
        //Strength Line
        if (_camp.Selected_Character.Strength < 10)
        { _txt += "    STRENGTH  " + _camp.Selected_Character.Strength + "    Geld " + _camp.Selected_Character.Geld + "\n"; }
        else
        { _txt += "    STRENGTH " + _camp.Selected_Character.Strength + "    Geld " + _camp.Selected_Character.Geld + "\n"; }

        //IQ line
        if (_camp.Selected_Character.IQ < 10)
        { _txt += "        I.Q.  " + _camp.Selected_Character.IQ + "     EXP " + _camp.Selected_Character.xp + "\n"; }
        else
        { _txt += "        I.Q. " + _camp.Selected_Character.IQ + "     EXP " + _camp.Selected_Character.xp + "\n"; }

        //Piety Line
        if (_camp.Selected_Character.Piety < 10)
        { _txt += "       PIETY  " + _camp.Selected_Character.Piety + "\n"; }
        else
        { _txt += "       PIETY " + _camp.Selected_Character.Piety + "\n"; }

        //Vitality Line
        if (_camp.Selected_Character.Vitality < 10)
        { _txt += "    VITALITY  " + _camp.Selected_Character.Vitality + "  LEVEL "; }
        else
        { _txt += "    VITALITY " + _camp.Selected_Character.Vitality + "  LEVEL "; }

        if (_camp.Selected_Character.level > 99) _txt += "";
        if (_camp.Selected_Character.level < 99 && _camp.Selected_Character.level > 9) _txt += "  ";
        if (_camp.Selected_Character.level < 9) _txt += "   ";
        _txt += _camp.Selected_Character.level;

        _txt += "     AGE";
        _txt += " " + _camp.Selected_Character.ageInWeeks / 52 + "\n";

        //Agility Line
        if (_camp.Selected_Character.Agility < 10)
        { _txt += "     AGILITY  " + _camp.Selected_Character.Agility + "  HITS "; }
        else
        { _txt += "     AGILITY " + _camp.Selected_Character.Agility + "  HITS "; }

        if (_camp.Selected_Character.HP < 999) _txt += "";
        if (_camp.Selected_Character.HP > 99 && _camp.Selected_Character.HP < 1000) _txt += " ";
        if (_camp.Selected_Character.HP > 9 && _camp.Selected_Character.HP < 100) _txt += "  ";
        if (_camp.Selected_Character.HP < 10) _txt += "   ";

        _txt += _camp.Selected_Character.HP.ToString();

        _txt += "/" + _camp.Selected_Character.HP_MAX;

        if (_camp.Selected_Character.HP_MAX > 999) _txt += "";
        if (_camp.Selected_Character.HP_MAX > 99 && _camp.Selected_Character.HP_MAX < 1000) _txt += "";
        if (_camp.Selected_Character.HP_MAX > 9 && _camp.Selected_Character.HP_MAX < 100) _txt += " ";
        if (_camp.Selected_Character.HP_MAX < 10) _txt += "  ";


        _txt += "   AC ";
        if (_camp.Selected_Character.ArmorClass < -9) _txt += "LO";
        if (_camp.Selected_Character.ArmorClass > -10 && _camp.Selected_Character.ArmorClass < 100) _txt += _camp.Selected_Character.ArmorClass;
        if (_camp.Selected_Character.ArmorClass > 99) _txt += "HI";
        _txt += "\n";

        //Luck Line
        if (_camp.Selected_Character.Luck < 10)
        { _txt += "        LUCK  " + _camp.Selected_Character.Luck + "  STATUS "; }
        else
        { _txt += "        LUCK " + _camp.Selected_Character.Luck + "  STATUS "; }
        if (_camp.Selected_Character.status == BlobberEngine.Enum._Status.OK && _camp.Selected_Character.Poison > 0)
        {
            _txt += "Poisoned \n\n";
        }
        else
        {
            _txt += _camp.Selected_Character.status.ToString() + "\n\n";
        }
            

        //Spells
        int[] _temp = new int[7];
        for (int i = 0; i < 7; i++) _temp[i] = _camp.Selected_Character.mageSpells[i] - _camp.Selected_Character.mageSpellsCast[i];
        _txt += "        MAGE " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n";
        for (int i = 0; i < 7; i++) _temp[i] = _camp.Selected_Character.priestSpells[i] - _camp.Selected_Character.priestSpellsCast[i];
        _txt += "      PRIEST " + _temp[0] + "/" + _temp[1] + "/" + _temp[2] + "/" + _temp[3] + "/" + _temp[4] + "/" + _temp[5] + "/" + _temp[6] + "\n\n";

        //Inventory            
        _txt += "*=EQUIP, -=CURSED, ?=UNKNOWN, #=UNUSABLE\n\n";
        string[] _18inv = new string[8];
        for (int i = 0; i < 8; i++)
        {
            if (_camp.Selected_Character.Inventory[i].index != -1)
            {
                //String displayed at start of string
                string _addChar = " ";

                //ChatGPT suggested this code:
                string charClass = _camp.Selected_Character.character_class.ToString().Substring(0, 1);
                string itemClassUse = GameManager.ITEM[_camp.Selected_Character.Inventory[i].index].class_use;
                if (itemClassUse.IndexOf(charClass, System.StringComparison.OrdinalIgnoreCase) == -1)
                {
                    _addChar = "#";
                }
                // Replacing this code:
                //if (!GameManager.ITEM[_camp.Selected_Character.Inventory[i].index].class_use.Contains(_camp.Selected_Character.character_class.ToString().Substring(0, 1).ToLower())) _addChar = "#";

                if (!_camp.Selected_Character.Inventory[i].identified) _addChar = "?";
                if (_camp.Selected_Character.Inventory[i].equipped) _addChar = "*";
                if (_camp.Selected_Character.Inventory[i].curse_active) _addChar = "-";
                _18inv[i] = (i + 1) + ")" + _addChar + _camp.Selected_Character.Inventory[i].ItemName();

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

        //Equip_Button_GO.SetActive(true);
        //ViewItem_Button_GO.SetActive(true);
        //ReadMagic_Button_GO.SetActive(true);
    }

    public void ButtonPressed(int _button)
    {
        if (_button == 0) 
        { 
            Equip_Flow_Panel.SetActive(true);
        }
        if (_button == 1)
        {
            View_Item_Panel.GetComponent<Camp_Item_View_Manager>().ChooseItem();
        }
        if (_button == 2)
        {
            View_Item_Panel.GetComponent<Camp_Item_View_Manager>().ShowMagic();
        }
        if (_button == 3)
        {
            Cast_Spell_Panel.SetActive(true);
            input.text = "";
        }
        if (_button == 4)
        {
            if(_camp.Selected_Character.character_class != BlobberEngine.Enum._Class.bishop)
            {
                _camp.PopUP.Show_Message("Only bishops can identify items this way!");
                return;
            }

            bool _itemsToIdentify = false;
            for (int i = 0; 8 < _camp.Selected_Character.Inventory.Length; i++)
            {
                if (_camp.Selected_Character.Inventory[i].index > -1 &&
                    !_camp.Selected_Character.Inventory[i].identified)
                    _itemsToIdentify = true;                    
            }

            if (!_itemsToIdentify)
            {
                _camp.PopUP.Show_Message("Nothing to identify");
                return;
            }

            bool _didItWork = false;
            bool _didItCrit = false;
            for (int i = 0; i < _camp.Selected_Character.Inventory.Length; i++)
            {
                if (_camp.Selected_Character.Inventory[i].index > -1 && !_camp.Selected_Character.Inventory[i].identified)
                {
                    int chance = (_camp.Selected_Character.level * 5) + 10;
                    int roll = Random.Range(0, 100) + 1;
                    int crit_fail_chance = 35 - (_camp.Selected_Character.level * 3);
                    int crit_fail_roll = Random.Range(0, 100) + 1;
                    if (roll <= chance)
                    {
                        _camp.Selected_Character.Inventory[i].identified = true;
                        _didItWork = true;
                    }
                    if(crit_fail_roll <= crit_fail_chance)
                    {
                        _camp.Selected_Character.Inventory[i].curse_active = GameManager.ITEM[_camp.Selected_Character.Inventory[i].index].cursed;
                        _didItCrit |= true;
                    }
                }
            }
            if (_didItWork && _didItCrit) _camp.PopUP.Show_Message("You Identified items, but encountered a curse!");
            if (_didItWork && !_didItCrit) _camp.PopUP.Show_Message("You succeded in Identifying items");
            if (!_didItWork && _didItCrit) _camp.PopUP.Show_Message("You failed to Identify items, but encountered a curse!");
            if (!_didItWork && !_didItCrit) _camp.PopUP.Show_Message("You failed to Identify items");
            return;
        }
        if (_button == 99) // Leave
        {
            _camp.UpdateScreen();
            this.gameObject.SetActive(false);
        }
    }
    public void TextReceived(string _text)
    {
        _text = _text.ToLower();
        if(_text != "")
        {
            _selected_spell = _magic.CanCastSpell(_camp.Selected_Character, _text);
        }
        if (_selected_spell != null && _selected_spell.target == "Character")
        {
            Cast_on_Who_panel.gameObject.SetActive(true);
        }
        else if (_selected_spell != null && _selected_spell.target == "Party")
        {
            _magic.Cast_Spell(_camp.Selected_Character, _selected_spell);
            _camp.RefreshCharacterSheet();
            Cast_Spell_Panel.SetActive(false);            
        }
        else
        {
            _camp.PopUP.Show_Message("You are not able to cast that spell");
            Cast_Spell_Panel.SetActive(false);            
        }
        return;
    }

    public void Cast_Spell_on(int _num)
    {
        Debug.Log("Caster = " + _camp.Selected_Character.name);
        Debug.Log("Spell = " + _selected_spell.name);
        Debug.Log("Target = " + GameManager.PARTY.LookUp_PartyMember(_num).name);
        _magic.Cast_Spell(_camp.Selected_Character, _selected_spell, GameManager.PARTY.LookUp_PartyMember(_num));
        _camp.RefreshCharacterSheet();
        Cast_on_Who_panel.SetActive(false);
        Cast_Spell_Panel.SetActive(false);        
        return;
    }
}
