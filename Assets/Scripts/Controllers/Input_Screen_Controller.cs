using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Screen_Controller : MonoBehaviour
{
    public GameObject button_prefab;    

    public Character_Class _selected_character;
    public int _selectedRoster;

    private Display_Screen_Controller _display;
    private Castle_Logic _castle;

    private void Start()
    {
        _display = FindObjectOfType<Display_Screen_Controller>();
        _castle = FindObjectOfType<Castle_Logic>();
        _selected_character = null;
        _selectedRoster = -1;
    }

    public void Clear_Buttons()
    {
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Button")) Destroy(_go);
    }

    public void Create_Button (string name, string command)
    {
        GameObject _go = Instantiate(button_prefab, this.transform);
        _go.tag = "Button";
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name.ToUpper();
        _go.GetComponent<Name_Button_Controller>().String = command;
    }
    public void Create_Button_First (string name, string command)
    {
        GameObject _go = Instantiate(button_prefab, this.transform);
        _go.tag = "Button";
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name;
        _go.GetComponent<Name_Button_Controller>().String = command;
        _go.transform.SetAsFirstSibling();
    }
    public void Create_Button_Last (string name, string command)
    {
        GameObject _go = Instantiate(button_prefab, this.transform);
        _go.tag = "Button";
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name;
        _go.GetComponent<Name_Button_Controller>().String = command;
        _go.transform.SetAsLastSibling();
    }




    public void Button_Clicked(string _button)
    {
        Debug.Log("Button Clicked! Received input: " + _button);

        //<<<<<<<<<<   MARKET   >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.Market)
        {
            if (_button == "Inn_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Inn_Intro;
                _castle.Update_Screen();
                return;
            }
            if (_button == "Tavern_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Tavern;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   INN  INTRO >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.Inn_Intro)
        {
            if(_button == "Leave_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }
            else
            {
                _selectedRoster = int.Parse(_button);
                _selected_character = Game_Logic.ROSTER[_selectedRoster];
                _castle._selected_character = _selected_character;
                _castle._selectedRoster = _selectedRoster;
                _castle.townStatus = Castle_Logic.ts.Inn;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   INN   >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Inn)
        {
            if (_button == "Leave_Button")
            {                
                _selected_character = null; _castle._selected_character = null;
                _selectedRoster = -1; _castle._selectedRoster = -1;
                _castle.townStatus = Castle_Logic.ts.Inn_Intro;                
                _castle.Update_Screen();
                return;
            }
            else 
            {
                Display_Screen_Controller _display = FindObjectOfType<Display_Screen_Controller>();
                int _delta = _selected_character.xp_nnl - _selected_character.xp;
                int _gpCost = 0, _hpAdd = 0;

                if(_button == "Stables") { _gpCost = 0; _hpAdd = 0; }
                if(_button == "Cots") { _gpCost = 10; _hpAdd = Random.Range(1,6); }
                if(_button == "Economy") { _gpCost = 50; _hpAdd = Random.Range(3, 11); }
                if(_button == "Merchant") { _gpCost = 200; _hpAdd = Random.Range(5, 16); }
                if(_button == "Royal") { _gpCost = 500; _hpAdd = Random.Range(10, 21); }

                if (_selected_character.Geld < _gpCost)
                {
                    _display.PopUpMessage(_selected_character.name + " doesn't have enough geld!");
                    return;
                }
                else 
                {
                    _selected_character.Geld -= _gpCost;
                    _selected_character.HP += _hpAdd;
                    if (_delta > 0)
                    {
                        string _txt = _selected_character.name + " needs " + _delta + " more XP to make the next level.";
                        if(_hpAdd > 0) _txt = _selected_character.name + " heals " + _hpAdd + "hp.\n" + _txt;
                        _display.PopUpMessage(_txt);
                    }
                    else
                    {
                        Game_Logic.instance.LevelUpCharacter(_selectedRoster);
                    }
                    return; 
                }
            }
        }
        //<<<<<<<<<<   Tavern   >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Tavern)
        {
            if (_button == "Add_Member")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("WHO WOULD YOU LIKE TO ADD?");
            }

            if (_button.Length > 9 && _button.Substring(0, 10) == "TextInput:")
            {
                string _name = _button.Replace("TextInput:", "");
                bool _found = false;                
                for (int i = 0; i < Game_Logic.ROSTER.Count; i++)
                {
                    if (_name.ToUpper() == Game_Logic.ROSTER[i].name.ToUpper())
                    {
                        _found = true;
                        _selectedRoster = i;
                        _selected_character = Game_Logic.ROSTER[i];
                    }
                }
                if (_found && _selected_character.inParty) 
                    _display.PopUpMessage("That character is already in the party...".ToUpper());
                if (_found && _selected_character.location == BlobberEngine.Enum._Locaton.Dungeon) 
                    _display.PopUpMessage("That character is out of town...".ToUpper());
                if (_found && !_selected_character.inParty && _selected_character.location != BlobberEngine.Enum._Locaton.Dungeon) 
                    Game_Logic.PARTY.AddMember(_selectedRoster);
                if (!_found) _display.PopUpMessage("I don't recognize that name...".ToUpper());

                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                _castle.Update_Screen();
                return;
            }

            if (_button == "Rem_Member")
            {
                _castle.townStatus = Castle_Logic.ts.Tavern_Remove;
                _castle.Update_Screen();
                return;
            }

            if (_button.Substring(0, 5) == "View:")
            {
                string _name = _button.Replace("View:", "");
                int _num = int.Parse(_name);
                _selected_character = Game_Logic.PARTY.LookUp_PartyMember(_num);
                _castle._selected_character = _selected_character;
                _selectedRoster = _num;
                _castle._selectedRoster = _selectedRoster;
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }

            if (_button == "Leave_Button")
            {
                _selected_character = null; _castle._selected_character = null;
                _selectedRoster = -1; _castle._selectedRoster = -1;
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   Tavern REMOVE   >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Tavern_Remove)
        {

            if (_button.Substring(0, 5) == "Char:")
            {
                string _name = _button.Replace("Char:", "");
                int _num = int.Parse(_name);
                Game_Logic.PARTY.RemoveMember(_num);
                _castle.Update_Screen();
                return;
            }

            if (_button == "Leave_Button")
            {
                _selected_character = null; _castle._selected_character = null;
                _selectedRoster = -1; _castle._selectedRoster = -1;
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   View Character  >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.View_Char)
        {
            if(_button == "View_Item")
            {
                Clear_Buttons();
                for (int i = 0; i < 8; i++) 
                    if (_selected_character.Inventory[i] != null)
                        if(_selected_character.Inventory[i].index != -1) 
                            Create_Button("Item #" + i, "ViewI" + i);
                Create_Button_Last("Cancel", "Cancel");
                return;
            }
            if(_button.Substring(0,5) == "ViewI")
            {
                string _choice = _button.Replace("ViewI", "");
                int _num = int.Parse(_choice);
                _castle._selectedInventorySlot = _num;
                _castle._selectedItemIndex = _selected_character.Inventory[_num].index;
                _castle.townStatus = Castle_Logic.ts.View_Item;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Trade_Geld")
            {
                //_display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("WHO WOULD YOU LIKE TO ADD?");
                _display.Update_Text_Screen("Trade Geld to whom?");
                Clear_Buttons();
                for (int i = 0; i < 6; i++)
                    if (!Game_Logic.PARTY.EmptySlot(i))                        
                        Create_Button( Game_Logic.PARTY.LookUp_PartyMember(i).name, "Trad2" + i);
                Create_Button_Last("Cancel", "CancelTradeGeld");
                return;
            }
            if (_button.Substring(0, 5) == "Trad2")
            {
                int _n = int.Parse(_button.Replace("Trad2", ""));
                if (_n < 0 && _n > 5) _n = 0;
                _selectedRoster = Game_Logic.PARTY.Get_Roster_Index(_n);
                _selected_character = Game_Logic.PARTY.LookUp_PartyMember(_n);
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("HOW MUCH WOULD YOU LIKE TO TRADE?");
                _display.Block_Buttons();
                return;
            }
            if (_button.Length > 9 && _button.Substring(0, 10) == "TextInput:")
            {
                int _G = 0;
                if(int.TryParse(_button.Replace("TextInput:", ""), out _G))
                {
                    if (_G < 0) _G = 0;
                    if (_G > _castle._selected_character.Geld) _G = _castle._selected_character.Geld;
                    Debug.Log("Logic check. Character sending Geld is " + _castle._selected_character.name + " and the Character receiving Geld is " + _selected_character.name);
                    _castle._selected_character.Geld -= _G;
                    _selected_character.Geld += _G;
                    _castle.townStatus = Castle_Logic.ts.Tavern;
                    _display.Button_Block_Panel.SetActive(false);
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _castle.Update_Screen();
                    return;
                }
                else
                {
                    _display.PopUpMessage("That is not a valid number. Please try again.");
                    Button_Clicked("Trade_Geld");                    
                    _display.Button_Block_Panel.SetActive(false);
                    return;
                }
            }
            if(_button == "CancelTradeGeld")
            {
                _display.Button_Block_Panel.SetActive(false);
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Leave_Button")
            {
                _display.Button_Block_Panel.SetActive(false);
                _castle.townStatus = Castle_Logic.ts.Tavern;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Read_Magic")
            {
                string _t = "";
                //Check selected character for priest spells
                //if there are some, start with label "Priest Spells" then divide them out by circle
                if(_castle._selected_character.priestSpells[0] > 0)
                {
                    _t = "--<Priest Spells>--\n\n";
                    if(_castle._selected_character.SpellKnown[0] || 
                       _castle._selected_character.SpellKnown[1] ||
                       _castle._selected_character.SpellKnown[2] ||
                       _castle._selected_character.SpellKnown[3])
                    {
                        if (_castle._selected_character.SpellKnown[0]) _t += "";
                        if (_castle._selected_character.SpellKnown[1]) _t += "";
                        if (_castle._selected_character.SpellKnown[2]) _t += "";
                        if (_castle._selected_character.SpellKnown[3]) _t += "";
                        _t += "\n";
                    }
                }


                //Check selected character for mage spells
                //if there are some, start with label "Mage Spells" then divide them out by circle


                for (int i = 0; i < 45; i++) _t += (i + 1) + ") SpellName" + (i + 1) + "\n";
                _display.PopUpMessage(_t);
            }
        }
    }
}
