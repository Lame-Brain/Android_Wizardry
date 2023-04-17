using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Input_Screen_Controller : MonoBehaviour
{
    public GameObject button_prefab;    

    public Character_Class _selected_character;
    public int _selectedRoster;

    private Display_Screen_Controller _display;
    private Castle_Logic _castle;
    private int _bp_page;

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
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.fontSize = _display.FONT_SIZE;
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name.ToUpper();
        _go.GetComponent<Name_Button_Controller>().String = command;
    }
    public void Create_Button_First (string name, string command)
    {
        GameObject _go = Instantiate(button_prefab, this.transform);
        _go.tag = "Button";
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.fontSize = _display.FONT_SIZE;
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name;
        _go.GetComponent<Name_Button_Controller>().String = command;
        _go.transform.SetAsFirstSibling();
    }
    public void Create_Button_Last (string name, string command)
    {
        GameObject _go = Instantiate(button_prefab, this.transform);
        _go.tag = "Button";
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.fontSize = _display.FONT_SIZE;
        _go.GetComponent<Name_Button_Controller>().ButtonTitle.text = name;
        _go.GetComponent<Name_Button_Controller>().String = command;
        _go.transform.SetAsLastSibling();
    }




    public void Button_Clicked(string _button)
    {
        //<<<<<<<<<<   MARKET   >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Market)
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
            if (_button == "Guild_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Training;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Boltac_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Shop_Intro;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Temple_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Temple_Intro;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   INN  INTRO >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Inn_Intro)
        {
            if (_button == "Leave_Button")
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

                if (_button == "Stables") { _gpCost = 0; _hpAdd = 0; }
                if (_button == "Cots") { _gpCost = 10; _hpAdd = Random.Range(1, 6); }
                if (_button == "Economy") { _gpCost = 50; _hpAdd = Random.Range(3, 11); }
                if (_button == "Merchant") { _gpCost = 200; _hpAdd = Random.Range(5, 16); }
                if (_button == "Royal") { _gpCost = 500; _hpAdd = Random.Range(10, 21); }

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
                        if (_hpAdd > 0) _txt = _selected_character.name + " heals " + _hpAdd + "hp.\n" + _txt;
                        _display.PopUpMessage(_txt);
                    }
                    else
                    {
                        _castle.LevelUpCharacter(_selectedRoster);
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
                _display.Block_Buttons();
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

            if (_button == "Divvy_Geld")
            {
                int _pool = 0, _num = 0, _share = 0, _remainder = 0;
                for (int i = 0; i < 6; i++)
                    if (!Game_Logic.PARTY.EmptySlot(i))
                    {
                        _num++;
                        _pool += Game_Logic.PARTY.LookUp_PartyMember(i).Geld;
                        Game_Logic.PARTY.LookUp_PartyMember(i).Geld = 0;
                    }
                if (_num > 0)
                {
                    _share = (int)(_pool / _num);
                    _remainder = _pool % _num;
                    for (int i = 0; i < 6; i++)
                        if (!Game_Logic.PARTY.EmptySlot(i))
                        {
                            Game_Logic.PARTY.LookUp_PartyMember(i).Geld = _share;
                        }
                    Game_Logic.PARTY.LookUp_PartyMember(0).Geld += _remainder;
                    string _txt = "Each party memeber receives " + _share + "g.";
                    if (_remainder > 0) _txt += "\n" + Game_Logic.PARTY.LookUp_PartyMember(0).name + " receives the extra " + _remainder + "g.";
                    _display.PopUpMessage(_txt);
                }
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
        if (_castle.townStatus == Castle_Logic.ts.View_Char)
        {
            if (_button == "View_Item")
            {
                Clear_Buttons();
                for (int i = 0; i < 8; i++)
                    if (_selected_character.Inventory[i] != null)
                        if (_selected_character.Inventory[i].index != -1)
                            Create_Button("Item #" + (i + 1), "ViewI" + i);
                Create_Button_Last("Cancel", "Cancel");
                return;
            }
            if (_button == "Cancel")
            {
                _display.Button_Block_Panel.SetActive(false);
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }
            if (_button.Substring(0, 5) == "ViewI")
            {
                string _choice = _button.Replace("ViewI", "");
                int _num = int.Parse(_choice);
                _castle._selectedInventorySlot = _num;
                _castle._selectedItemIndex = _selected_character.Inventory[_num].index;
                _castle.townStatus = Castle_Logic.ts.View_Item;
                _castle.Update_Screen();
                return;
            }
            if (_button == "Trade_Geld")
            {
                _display.Update_Text_Screen("Trade Geld to whom?");
                Clear_Buttons();
                for (int i = 0; i < 6; i++)
                    if (!Game_Logic.PARTY.EmptySlot(i))
                        Create_Button(Game_Logic.PARTY.LookUp_PartyMember(i).name, "Trad2" + i);
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
                if (int.TryParse(_button.Replace("TextInput:", ""), out _G))
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
            if (_button == "CancelTradeGeld")
            {
                _display.Button_Block_Panel.SetActive(false);
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }
            if (_button == "Leave_Button")
            {
                _display.Button_Block_Panel.SetActive(false);
                _castle.townStatus = Castle_Logic.ts.Tavern;
                _castle.Update_Screen();
                return;
            }
            if (_button == "Read_Magic")
            {
                string _t = "";
                int _lastCircle = 0;
                //Check selected character for priest spells
                //if there are some, start with label "Priest Spells" then divide them out by circle
                if (_castle._selected_character.priestSpells[0] > 0)
                {
                    _t += "--<Priest Spells>--\n";
                    for (int i = 0; i < Game_Logic.SPELL.Count; i++)
                    {
                        if (_castle._selected_character.SpellKnown[i] && Game_Logic.SPELL[i].book == "Priest")
                        {
                            if (_lastCircle < Game_Logic.SPELL[i].circle)
                            {
                                _t += "\n";
                                _lastCircle = Game_Logic.SPELL[i].circle;
                            }
                            _t += Game_Logic.SPELL[i].name + " (" + Game_Logic.SPELL[i].word + ")\n";
                        }
                    }
                }

                //Add extra spaces if character has both mage and preist spells
                if (_castle._selected_character.priestSpells[0] > 0 && _castle._selected_character.mageSpells[0] > 0) _t += "\n\n";


                //Check selected character for mage spells
                //if there are some, start with label "Mage Spells" then divide them out by circle
                _lastCircle = 0;
                if (_castle._selected_character.mageSpells[0] > 0)
                {
                    _t += "--<Mage Spells>--\n";
                    for (int i = 0; i < Game_Logic.SPELL.Count; i++)
                    {
                        if (_castle._selected_character.SpellKnown[i] && Game_Logic.SPELL[i].book == "Mage")
                        {
                            if (_lastCircle < Game_Logic.SPELL[i].circle)
                            {
                                _t += "\n";
                                _lastCircle = Game_Logic.SPELL[i].circle;
                            }
                            _t += Game_Logic.SPELL[i].name + " (" + Game_Logic.SPELL[i].word + ")\n";
                        }
                    }
                }
                _display.PopUpMessage(_t);
            }
        }
        //<<<<<<<<<<   View Item  >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.View_Item)
        {
            if (_button == "Equip_Item")
            {
                if (!_castle._selected_character.Inventory[_castle._selectedInventorySlot].equipped)
                { // Equip the item if it is not equipped

                    //Check if class can equip item
                    string _class = _castle._selected_character.character_class.ToString();
                    _class = _class.ToLower();
                    _class = _class.Substring(0, 1);
                    if (!Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains(_class))
                    {
                        _display.PopUpMessage("Your class cannot equip this item.");
                        _castle.Update_Screen();
                        return;
                    }

                    //Check if the slot is in use
                    bool _slotFull = false;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Weapon &&
                        _castle._selected_character.eqWeapon > -1) _slotFull = true;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Armor &&
                        _castle._selected_character.eqArmor > -1) _slotFull = true;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Shield &&
                        _castle._selected_character.eqShield > -1) _slotFull = true;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Helmet &&
                        _castle._selected_character.eqHelmet > -1) _slotFull = true;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Gauntlets &&
                        _castle._selected_character.eqGauntlet > -1) _slotFull = true;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type == BlobberEngine.Enum._Item_Type.Misc &&
                        _castle._selected_character.eqMisc > -1) _slotFull = true;
                    if (_slotFull)
                    {
                        _display.PopUpMessage("This equipment slot is already in use.");
                        _castle.Update_Screen();
                        return;
                    }

                    //If equipment is identifed, and the alignment doesn't match, stop the process
                    if (_castle._selected_character.Inventory[_castle._selectedInventorySlot].identified)
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align != BlobberEngine.Enum._Alignment.none &&
                            Game_Logic.ITEM[_castle._selectedItemIndex].item_align == _castle._selected_character.alignment)
                        {
                            _display.PopUpMessage("This equipment's alignment does not match the character's");
                            _castle.Update_Screen();
                            return;
                        }


                    //Equip it
                    _castle._selected_character.EquipItem(_castle._selectedInventorySlot);
                    _castle.townStatus = Castle_Logic.ts.View_Char;
                    _castle.Update_Screen();

                    //Report result (cursed and equipped, or just equipped)
                    if (!_castle._selected_character.Inventory[_castle._selectedInventorySlot].curse_active)
                    {
                        _display.PopUpMessage("You equipped " + _castle._selected_character.Inventory[_castle._selectedInventorySlot].ItemName() + ".");
                        _display.Block_Buttons();
                    }
                    else
                    {
                        _display.PopUpMessage("You are cursed by " + _castle._selected_character.Inventory[_castle._selectedInventorySlot].ItemName() + "!");
                    }

                    return;
                }

                if (_castle._selected_character.Inventory[_castle._selectedInventorySlot].equipped)
                { //Unequip Item
                    if (_castle._selected_character.Inventory[_castle._selectedInventorySlot].curse_active)
                    { //Can't unequip, cursed
                        _display.PopUpMessage("You cannot unequip cursed items. You must have the curse lifted.");
                        return;
                    }

                    //Unequip it.
                    _castle._selected_character.UnequipItem(_castle._selectedInventorySlot);

                    _castle.townStatus = Castle_Logic.ts.View_Char;
                    _castle.Update_Screen();
                    return;
                }
            }
            if (_button == "Trade_Item")
            {
                if (_castle._selected_character.Inventory[_castle._selectedInventorySlot].curse_active)
                {
                    _display.PopUpMessage("You cannot trade items with an activated curse!");
                    _castle.Update_Screen();
                    return;
                }

                _display.Update_Text_Screen("Who Would you like to trade " + _castle._selected_character.Inventory[_castle._selectedInventorySlot].ItemName() + " to?");
                Clear_Buttons();
                for (int i = 0; i < 6; i++)
                    if (!Game_Logic.PARTY.EmptySlot(i))
                        Create_Button(Game_Logic.PARTY.LookUp_PartyMember(i).name, "Trad2" + i);
                Create_Button_Last("Cancel", "CancelTradeItem");
                return;
            }
            if (_button.Substring(0, 5) == "Trad2")
            {
                int _n = int.Parse(_button.Replace("Trad2", ""));
                if (_n < 0 && _n > 5) _n = 0;
                _selectedRoster = Game_Logic.PARTY.Get_Roster_Index(_n);
                _selected_character = Game_Logic.PARTY.LookUp_PartyMember(_n);

                if (!_selected_character.HasEmptyInventorySlot())
                {
                    _display.PopUpMessage(_selected_character.name + " does not have any space.");
                    _castle.Update_Screen();
                    return;
                }

                _castle._selected_character.UnequipItem(_castle._selectedInventorySlot);
                int _a = _selected_character.GetEmptyInventorySlot();
                _selected_character.Inventory[_selected_character.GetEmptyInventorySlot()] = new Item(_castle._selected_character.Inventory[_castle._selectedInventorySlot].index, false, false, _castle._selected_character.Inventory[_castle._selectedInventorySlot].identified);
                _castle._selected_character.Inventory[_castle._selectedInventorySlot].index = -1;



                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                _display.PopUpMessage("Done. Sent.");
                return;
            }
            if (_button == "Use_Item")
            {
                //TO DO: FILL THIS IN
            }
            if (_button == "Trash_Item")
            {
                if (_castle._selected_character.Inventory[_castle._selectedInventorySlot].curse_active)
                {
                    _display.Update_Text_Screen("You cannot trash cursed items.\nYou have to have the curse lifted.");
                    Clear_Buttons();
                    Create_Button("BACK", "Cancel");
                    return;
                }
                else
                {
                    _display.Update_Text_Screen("Are you sure?");
                    Clear_Buttons();
                    Create_Button("YES", "YesTrash");
                    Create_Button("NO", "Cancel");
                    return;
                }
            }
            if (_button == "YesTrash")
            {
                _castle._selected_character.Inventory[_castle._selectedInventorySlot].index = -1;
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }
            if (_button == "ID_Item")
            {
                string _message = "Failed";
                if (_castle._selected_character.character_class == BlobberEngine.Enum._Class.bishop)
                {
                    _message = "Failed";
                    int _succeed_chance = 10 + (5 * _castle._selected_character.level),
                        _crit_fail_chance = 35 - (3 * _castle._selected_character.level);

                    if (Random.Range(0, 100) + 1 <= _succeed_chance)
                    {
                        _message = "Succeeded";
                        _castle._selected_character.Inventory[_castle._selectedInventorySlot].identified = true;
                    }
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].cursed && Random.Range(0, 100) + 1 <= _crit_fail_chance)
                    {
                        _message += " and struck by curse!";
                        _castle._selected_character.EquipItem(_castle._selectedInventorySlot);
                    }
                }
                else
                {
                    _message = "Only Bishops can Identify items";
                }
                _castle.townStatus = Castle_Logic.ts.View_Item;
                _castle.Update_Screen();
                _display.PopUpMessage(_message);
                _display.Block_Buttons();
                return;
            }

            if (_button == "Cancel")
            {
                _castle.townStatus = Castle_Logic.ts.View_Item;
                _castle.Update_Screen();
                return;
            }
            if (_button == "Leave_Button")
            {
                _castle.townStatus = Castle_Logic.ts.View_Char;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   Training Hall  >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Training)
        {
            if (_button == "Make_Character")
            {
                _display.Character_Gen.SetActive(true);
                return;
            }

            if (_button == "Show_Roster")
            {
                string _t = "Names in Use:\n" +
                            " --------------------------------------\n";
                for (int i = 0; i < Game_Logic.ROSTER.Count; i++)
                {
                    _t += Game_Logic.ROSTER[i].name + " level " + Game_Logic.ROSTER[i].level + " " + Game_Logic.ROSTER[i].race + " "
                       + Game_Logic.ROSTER[i].character_class + "(" + Game_Logic.ROSTER[i].status + ") ";
                    if (Game_Logic.ROSTER[i].location == BlobberEngine.Enum._Locaton.Dungeon) _t += "OUT";
                    if (Game_Logic.ROSTER[i].location == BlobberEngine.Enum._Locaton.Temple) _t += "Temple";
                    _t += "\n";
                }
                _display.PopUpMessage(_t);
                return;
            }

            if(_button == "Inspect_Hero")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("WHO WOULD YOU LIKE TO INSPECT?");
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
                        _castle._selectedRoster = i;
                        _castle._selected_character = Game_Logic.ROSTER[i];
                        _selectedRoster = i;
                        _selected_character = Game_Logic.ROSTER[i];
                    }
                }
                if (_found)
                {
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _castle.townStatus = Castle_Logic.ts.Inspect;
                    _castle.Update_Screen();
                    return;
                }
                if (!_found) _display.PopUpMessage("I don't recognize that name...".ToUpper());

                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
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
        //<<<<<<<<<<   Inspect Character  >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Inspect)
        {
            if (_button == "Rename_Button")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("Enter New Name for this character:");
            }

            if (_button.Length > 9 && _button.Substring(0, 10) == "TextInput:")
            {
                bool good_name = true;               
                string _val = _button.Replace("TextInput:", "");
                if (_val == "") _val = _selected_character.name;
                _val = _val.Replace(",", " ");
                if (_val.Length > 15) _val = _val.Substring(0, 15);
                _val = _val.ToUpper();

                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                for (int i = 0; i < Game_Logic.ROSTER.Count; i++)
                    if (_selectedRoster != i && _val == Game_Logic.ROSTER[i].name.ToUpper())
                    {
                        good_name = false;
                        _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("That name is Taken, please enter another.");
                    }

                if (good_name) _selected_character.name = _val;
                _castle.Update_Screen();
                return;
            }

            if(_button == "ReClass_Button")
            {
                //_castle.ChangeCharacterClass();
                _display.Update_Text_Screen("What Class woud you like to change to?");
                Clear_Buttons();

                if(_selected_character.character_class != Enum._Class.fighter && _selected_character.Strength > 10 ) Create_Button("FIGHTER", "Change2Fighter");
                if(_selected_character.character_class != Enum._Class.mage && _selected_character.IQ > 10 ) Create_Button("MAGE", "Change2Mage");
                if(_selected_character.character_class != Enum._Class.priest && _selected_character.Piety > 10 ) Create_Button("PRIEST", "Change2Priest");
                if(_selected_character.character_class != Enum._Class.thief && _selected_character.Agility > 10 ) Create_Button("THIEF", "Change2Thief");
                if(_selected_character.character_class != Enum._Class.bishop && _selected_character.IQ > 10 
                                                                             && _selected_character.Piety > 10 ) Create_Button("BISHOP", "Change2Bishop");
                if(_selected_character.character_class != Enum._Class.samurai && _selected_character.Strength > 14 
                                                                              && _selected_character.IQ > 10
                                                                              && _selected_character.Piety > 9 
                                                                              && _selected_character.Vitality > 13
                                                                              && _selected_character.Agility > 9) 
                                                                                    Create_Button("BISHOP", "Change2Samurai");
                if(_selected_character.character_class != Enum._Class.lord && _selected_character.Strength > 14 
                                                                           && _selected_character.IQ > 11
                                                                           && _selected_character.Piety > 11 
                                                                           && _selected_character.Vitality > 14
                                                                           && _selected_character.Agility > 13
                                                                           && _selected_character.Luck > 14)
                                                                                Create_Button("BISHOP", "Change2Lord");
                if(_selected_character.character_class != Enum._Class.ninja && _selected_character.Strength > 14 
                                                                            && _selected_character.IQ > 14
                                                                            && _selected_character.Piety > 14
                                                                            && _selected_character.Vitality > 14
                                                                            && _selected_character.Agility > 14
                                                                            && _selected_character.Luck > 14)
                                                                                 Create_Button("BISHOP", "Change2Ninja");
                Create_Button_Last("CANCEL", "cancel_button");
                return;
            }

            if (_button.Contains("Change2"))
            {
                if (_button.Contains("Figher")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.fighter);
                if (_button.Contains("Mage")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.mage);
                if (_button.Contains("Priest")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.priest);
                if (_button.Contains("Thief")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.thief);
                if (_button.Contains("Bishop")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.bishop);
                if (_button.Contains("Samurai")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.samurai);
                if (_button.Contains("Lord")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.lord);
                if (_button.Contains("Ninja")) _castle.ChangeCharacterClass(_selected_character, Enum._Class.ninja);
                _castle.Update_Screen();
                _display.PopUpMessage("Be sure to re-equip any equipment!");
                return;
            }

            if (_button == "cancel_button")
            {
                _castle.townStatus = Castle_Logic.ts.Inspect;
                _castle.Update_Screen();
                return;
            }

            if (_button == "Retire_Button")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("Are you sure? This will delete the Character forever!\nEnter Character's Name to confirm:", "del_Input:");
                return;
            }

            if (_button.Length > 9 && _button.Substring(0, 10) == "del_Input:")
            {
                string _val = _button.Replace("del_Input:", "");
                if (_val.ToUpper() == _selected_character.name.ToUpper())
                {
                    Game_Logic.ROSTER.Remove(_selected_character);
                    _castle.townStatus = Castle_Logic.ts.Training;
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _castle.Update_Screen();
                    return;
                }
                _castle.townStatus = Castle_Logic.ts.Inspect;
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                _castle.Update_Screen();
                return;
            }

            if (_button == "Leave_Button")
            {
                _selected_character = null; _castle._selected_character = null;
                _selectedRoster = -1; _castle._selectedRoster = -1;
                _castle.townStatus = Castle_Logic.ts.Training;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   Boltac's Trade Post  >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Shop_Intro || _castle.townStatus == Castle_Logic.ts.Shop)
        {
            if (_button == "Leave_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }

            if (_button.Contains("Character:"))
            {
                _button = _button.Replace("Character:", "");
                _selectedRoster = int.Parse(_button);
                _selected_character = Game_Logic.ROSTER[_selectedRoster];
                _castle._selected_character = _selected_character;
                _castle._selectedRoster = _selectedRoster;
                _castle.townStatus = Castle_Logic.ts.Shop;
                _castle.Update_Screen();
                return;
            }

            if (_button == "Pool_Geld") // <------------------------------------------------------------------------------ POOL GELD CODE
            {
                int _pool = 0;
                for (int i = 0; i < 6; i++)
                    if (Game_Logic.PARTY.Get_Roster_Index(i) > -1)
                    {
                        _pool += Game_Logic.PARTY.LookUp_PartyMember(i).Geld;
                        Game_Logic.PARTY.LookUp_PartyMember(i).Geld = 0;
                    }
                _selected_character.Geld = _pool;
                _castle.Update_Screen();
                _display.PopUpMessage(_selected_character.name + " now has " + _selected_character.Geld + " G.");
            }

            if (_button == "Buy_item")
            {
                if (_selected_character.HasEmptyInventorySlot())
                {
                    _bp_page = 0;
                    _button = "View_Buy_item";
                }
                else
                {
                    _display.PopUpMessage("Inventory full! Sell something first!");
                }
            }

            if (_button == "View_Buy_item")
            {
                //Update list of items
                List<int> _roster_index_in_stock = new List<int>();
                List<string> _name_in_stock = new List<string>();
                for (int i = 0; i < Game_Logic.ITEM.Count; i++)
                    if (Game_Logic.PARTY.BoltacStock[i] == -1 || Game_Logic.PARTY.BoltacStock[i] > 0)
                    {
                        _roster_index_in_stock.Add(Game_Logic.ITEM[i].index);
                        _name_in_stock.Add(Game_Logic.ITEM[i].name);
                    }
                for (int i = 0; i < _name_in_stock.Count; i++)
                {
                    if (Game_Logic.ITEM[_roster_index_in_stock[i]].item_align != Enum._Alignment.none &&
                        Game_Logic.ITEM[_roster_index_in_stock[i]].item_align != _selected_character.alignment)
                        //Debug.Log("ALIGNMENT FAIL ON " + _name_in_stock + " item is " + Game_Logic.ITEM[_roster_index_in_stock[i]].item_align + " and the character is " + _selected_character.alignment);
                        _name_in_stock[i] = "#" + _name_in_stock[i];

                    if (Game_Logic.ITEM[_roster_index_in_stock[i]].item_type != Enum._Item_Type.Consumable &&
                        Game_Logic.ITEM[_roster_index_in_stock[i]].item_type != Enum._Item_Type.Special)
                    {
                        string _chk = _selected_character.character_class.ToString();
                        _chk = _chk.ToLower();
                        _chk = _chk.Substring(0, 1);
                        if (!Game_Logic.ITEM[_roster_index_in_stock[i]].class_use.Contains(_chk))
                            //Debug.Log("Class fail. item can be used by " + Game_Logic.ITEM[_castle._selectedItemIndex].class_use + " and the character is " + _chk);
                            _name_in_stock[i] = "#" + _name_in_stock[i];
                    }

                    while (_name_in_stock[i].Length < 20)
                        _name_in_stock[i] += " ";
                    _name_in_stock[i] += Game_Logic.ITEM[_roster_index_in_stock[i]].price + "G.\n";
                }
                // shop screen
                string _txt = "+--------------------------------------+\n" +
                                            "| Castle                          Shop |\n" +
                                            "+--------------------------------------+\n\n";
                //Item list
                for (int i = 0; i < 10; i++)
                {
                    if ((1 + i + _bp_page * 10) < 10) _txt += " ";
                    _txt += (1 + i + _bp_page * 10);
                    if (i + _bp_page * 10 < _name_in_stock.Count) _txt += ". " + _name_in_stock[i + _bp_page * 10];
                }

                // geld and info
                _txt += "\n" + _selected_character.name + " has " + _selected_character.Geld + " G.";

                //Display text to screen
                _display.Update_Text_Screen(_txt);

                //purchase, scroll forward or back, go to start, leave
                Clear_Buttons();
                for (int i = 0; i < 10; i++)
                    if (i + _bp_page * 10 < _roster_index_in_stock.Count)
                        Create_Button("Purchase:\n" + Game_Logic.ITEM[_roster_index_in_stock[i + _bp_page * 10]].name, "Purchase:" + _roster_index_in_stock[i + _bp_page * 10]);
                if (_bp_page > 0) Create_Button("Scroll Back", "BP_Scroll_Back");
                if (_bp_page * 10 + 10 < _roster_index_in_stock.Count) Create_Button("Scroll Forward", "BP_Scroll_Forward");
                if (_bp_page > 0) Create_Button("Scroll to Top", "BP_Scroll_Top");
                Create_Button_Last("Done", "Leave_Buy_Panel");
                return;
            }

            if (_button == "Leave_Buy_Panel")
            {
                _castle.townStatus = Castle_Logic.ts.Shop;
                _castle.Update_Screen();
                return;
            }

            if (_button == "BP_Scroll_Back")
            {
                _bp_page--;
                Button_Clicked("View_Buy_item");
                return;
            }
            if (_button == "BP_Scroll_Forward")
            {
                _bp_page++;
                Button_Clicked("View_Buy_item");
                return;
            }
            if (_button == "BP_Scroll_Top")
            {
                _bp_page = 0;
                Button_Clicked("View_Buy_item");
                return;
            }

            if (_button.Contains("Purchase:"))
            {
                _button = _button.Replace("Purchase:", "");
                int _i = int.Parse(_button);
                _castle._selectedItemIndex = _i;
                bool _use = true;
                Debug.Log("Character has " + _selected_character.Geld + " and the " + Game_Logic.ITEM[_castle._selectedItemIndex].name + " costs " + Game_Logic.ITEM[_castle._selectedItemIndex].price);
                if (_selected_character.Geld >= Game_Logic.ITEM[_castle._selectedItemIndex].price)
                {
                    string _txt = "";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_type != Enum._Item_Type.Consumable &&
                        Game_Logic.ITEM[_castle._selectedItemIndex].item_type != Enum._Item_Type.Special)
                    {
                        string _chk = _selected_character.character_class.ToString();
                        _chk = _chk.ToLower();
                        _chk = _chk.Substring(0, 1);
                        if (!Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains(_chk))
                            _use = false;
                    }

                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align != Enum._Alignment.none &&
                        Game_Logic.ITEM[_castle._selectedItemIndex].item_align != _selected_character.alignment)
                    {
                        _use = false;
                    }

                    _txt += Game_Logic.ITEM[_castle._selectedItemIndex].name + "\n";
                    switch (Game_Logic.ITEM[_castle._selectedItemIndex].item_type)
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
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.num > 0)
                    {
                        _txt += "\n";
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod < 0) _txt += "(ToHit " + Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod + ") ";
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod >= 0) _txt += "(ToHit +" + Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod + ") ";
                        _txt += "Damage = " + Game_Logic.ITEM[_castle._selectedItemIndex].damage.num + "d" + Game_Logic.ITEM[_castle._selectedItemIndex].damage.sides;
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus < 0) _txt += Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus;
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus > 0) _txt += "+" + Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus;
                        if (Game_Logic.ITEM[_castle._selectedItemIndex].xtra_swings > 1) _txt += "x" + Game_Logic.ITEM[_castle._selectedItemIndex].xtra_swings;
                        _txt += "\n";
                    }
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].armor_mod > 0) _txt += "\n Armor: " + Game_Logic.ITEM[_castle._selectedItemIndex].armor_mod + "\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].spell != "") _txt += "\n When invoked, casts: " + Game_Logic.ITEM[_castle._selectedItemIndex].spell + ".\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("z")) _txt += "<None>\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("f")) _txt += "Fighter\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("m")) _txt += "Mage\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("p")) _txt += "Priest\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("t")) _txt += "Thief\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("b")) _txt += "Bishop\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("s")) _txt += "Samurai\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("l")) _txt += "Lord\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("n")) _txt += "Ninja\n";

                    if (!_use) _txt += "\nWARNING:\nThis character cannot use this item!\n\n";
                    _txt += "\nDo you wish to buy\n\n" + Game_Logic.ITEM[_i].name + "\n\nfor " + Game_Logic.ITEM[_i].price + " G?";
                    _display.Update_Text_Screen(_txt);
                    Clear_Buttons();
                    Create_Button("Yes", "Yes_buy_item");
                    Create_Button("No", "No_buy_item");
                    return;
                }
                else
                {
                    Button_Clicked("View_Buy_item");
                    _display.PopUpMessage("Not enough Geld!");
                    return;
                }
            }

            if (_button == "Yes_buy_item")
            {
                _selected_character.Geld -= (int)Game_Logic.ITEM[_castle._selectedItemIndex].price;
                _selected_character.Inventory[_selected_character.GetEmptyInventorySlot()] = new Item(_castle._selectedItemIndex, false, false, true);
                if (Game_Logic.PARTY.BoltacStock[_castle._selectedItemIndex] != -1) Game_Logic.PARTY.BoltacStock[_castle._selectedItemIndex]--;

                _castle._selectedInventorySlot = -1;
                Button_Clicked("View_Buy_item");
                _display.PopUpMessage("Thank you for your business!");
                return;
            }

            if (_button == "No_buy_item")
            {
                _castle._selectedInventorySlot = -1;
                Button_Clicked("View_Buy_item");
                _display.PopUpMessage("Perhaps next time...");
                return;
            }


            if (_button == "Sell_item")
            {

                string _txt = "+--------------------------------------+\n" +
                              "| Castle                          Shop |\n" +
                              "+--------------------------------------+\n\n";
                //Item list
                List<int> _slot_in_inventory = new List<int>();
                string _this_line;
                for (int i = 0; i < 8; i++)
                {
                    _this_line = " " + (i + 1) + ". ";
                    _slot_in_inventory.Add(i);

                    if (_selected_character.Inventory[i].index != -1 && !_selected_character.Inventory[i].curse_active)
                    {
                        _this_line += _selected_character.Inventory[i].ItemName();
                        while (_this_line.Length < 20)
                            _this_line += " ";
                        int _p = (int)Game_Logic.ITEM[_selected_character.Inventory[i].index].price / 2;
                        if (_p > 1 && !_selected_character.Inventory[i].identified) _p = 1;
                        _this_line += _p + " G.";
                    }
                    _txt += _this_line + "\n";
                }

                // geld and info
                _txt += "\n" + _selected_character.name + " has " + _selected_character.Geld + " G.\n" +
                    "Which Item would you like to sell?";
                _display.Update_Text_Screen(_txt);
                Clear_Buttons();
                for (int i = 0; i < 8; i++)
                    if (_selected_character.Inventory[i].index != -1 && !_selected_character.Inventory[i].curse_active)
                        Create_Button("Sell item #" + (i + 1), "Sell_This_Item:" + i);
                Create_Button_Last("Done", "Leave_Sell_Panel");
                return;
            }

            if (_button == "Leave_Sell_Panel")
            {
                _castle.townStatus = Castle_Logic.ts.Shop;
                _castle.Update_Screen();
                return;
            }

            // <<<<<<<<<<<<<<<<<<<<<<<<<< VIEW SELL ITEM
            if (_button.Contains("Sell_This_Item:"))
            {
                _button = _button.Replace("Sell_This_Item:", "");
                int _i = int.Parse(_button);
                _castle._selectedItemIndex = _selected_character.Inventory[_i].index;
                string _txt = "";

                _txt += Game_Logic.ITEM[_castle._selectedItemIndex].name + "\n";
                switch (Game_Logic.ITEM[_castle._selectedItemIndex].item_type)
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

                if (_selected_character.Inventory[_i].equipped)
                {
                    _txt += "This item is currently equipped.\n";
                }

                if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.num > 0)
                {
                    _txt += "\n";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod < 0) _txt += "(ToHit " + Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod + ") ";
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod >= 0) _txt += "(ToHit +" + Game_Logic.ITEM[_castle._selectedItemIndex].hit_mod + ") ";
                    _txt += "Damage = " + Game_Logic.ITEM[_castle._selectedItemIndex].damage.num + "d" + Game_Logic.ITEM[_castle._selectedItemIndex].damage.sides;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus < 0) _txt += Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus > 0) _txt += "+" + Game_Logic.ITEM[_castle._selectedItemIndex].damage.bonus;
                    if (Game_Logic.ITEM[_castle._selectedItemIndex].xtra_swings > 1) _txt += "x" + Game_Logic.ITEM[_castle._selectedItemIndex].xtra_swings;
                    _txt += "\n";
                }
                if (Game_Logic.ITEM[_castle._selectedItemIndex].armor_mod > 0) _txt += "\n Armor: " + Game_Logic.ITEM[_castle._selectedItemIndex].armor_mod + "\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].spell != "") _txt += "\n When invoked, casts: " + Game_Logic.ITEM[_castle._selectedItemIndex].spell + ".\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("z")) _txt += "<None>\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("f")) _txt += "Fighter\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("m")) _txt += "Mage\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("p")) _txt += "Priest\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("t")) _txt += "Thief\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("b")) _txt += "Bishop\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("s")) _txt += "Samurai\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("l")) _txt += "Lord\n";
                if (Game_Logic.ITEM[_castle._selectedItemIndex].class_use.Contains("n")) _txt += "Ninja\n";

                int _p = (int)Game_Logic.ITEM[_castle._selectedItemIndex].price / 2;
                if (_p > 1 && !_selected_character.Inventory[_i].identified) _p = 1;
                _txt += "\nDo you wish to Sell\n\n" + Game_Logic.ITEM[_castle._selectedItemIndex].name + "\n\nfor " + (_p) + " G?";
                _display.Update_Text_Screen(_txt);
                Clear_Buttons();
                Create_Button("Yes", "Yes_Sell_Item:" + _i);
                Create_Button("No", "No_Sell_item");
                return;                
            }

            if (_button == "No_Sell_item")
            {
                _castle._selectedInventorySlot = -1;
                Button_Clicked("Sell_item");
                _display.PopUpMessage("Perhaps next time...");
                return;
            }

            if (_button.Contains("Yes_Sell_Item:"))
            {
                _button = _button.Replace("Yes_Sell_Item:", "");
                int _n = int.Parse(_button);
                if (_selected_character.Inventory[_n].equipped) _selected_character.UnequipItem(_n);
                int _p = (int)(Game_Logic.ITEM[_castle._selectedItemIndex].price / 2);
                if (_p > 1 && !_selected_character.Inventory[_n].identified) _p = 1;
                _selected_character.Geld += _p;
                _selected_character.Inventory[_n].index = -1;
                if (Game_Logic.PARTY.BoltacStock[_castle._selectedItemIndex] > -1) Game_Logic.PARTY.BoltacStock[_castle._selectedItemIndex]++;

                _castle._selectedInventorySlot = -1;
                Button_Clicked("Sell_item");
                _display.PopUpMessage("Thank you for your business!");
                return;
            }

            if (_button == "Uncurse_item")
            {

                string _txt = "+--------------------------------------+\n" +
                              "| Castle                          Shop |\n" +
                              "+--------------------------------------+\n\n";
                //Item list
                List<int> _slot_in_inventory = new List<int>();
                string _this_line;
                for (int i = 0; i < 8; i++)
                {
                    _this_line = " " + (i + 1) + ". ";
                    _slot_in_inventory.Add(i);

                    if (_selected_character.Inventory[i].index != -1 && _selected_character.Inventory[i].curse_active)
                    {
                        _this_line += _selected_character.Inventory[i].ItemName();
                        while (_this_line.Length < 20)
                            _this_line += " ";
                        int _p = (int)Game_Logic.ITEM[_selected_character.Inventory[i].index].price / 2;
                        _this_line += _p + " G.";
                    }
                    _txt += _this_line + "\n";
                }

                // geld and info
                _txt += "\n" + _selected_character.name + " has " + _selected_character.Geld + " G.\n" +
                    "Which Item is cursed?";
                _display.Update_Text_Screen(_txt);
                Clear_Buttons();
                for (int i = 0; i < 8; i++)
                    if (_selected_character.Inventory[i].index != -1 && _selected_character.Inventory[i].curse_active)
                        Create_Button("Uncurse item #" + (i + 1), "Uncurse_This_Item:" + i);
                Create_Button_Last("Done", "Cancel_Uncurse_Identify_Panel");
                return;
            }
            if (_button == "Cancel_Uncurse_Identify_Panel")
            {
                _castle.Update_Screen();
                return;
            }

            if (_button.Contains("Uncurse_This_Item:"))
            {
                _button = _button.Replace("Uncurse_This_Item:", "");
                int _n = int.Parse(_button);
                int _price = (int)Game_Logic.ITEM[_selected_character.Inventory[_n].index].price / 2;
                if(_selected_character.Geld < _price)
                {
                    _castle._selectedItemIndex = -1;
                    _castle.Update_Screen();
                    _display.PopUpMessage("Not enough Geld for that service.");
                    return;
                }
                else
                {
                    _selected_character.Geld -= _price;
                    if (_selected_character.Inventory[_n].equipped) _selected_character.UnequipItem(_n);
                    _selected_character.Inventory[_n].index = -1;
                    _castle._selectedItemIndex = -1;
                    _castle.Update_Screen();
                    _display.PopUpMessage("The curse has been lifted!");
                    return;
                }
            }

            if (_button == "Identify_item")
            {

                string _txt = "+--------------------------------------+\n" +
                              "| Castle                          Shop |\n" +
                              "+--------------------------------------+\n\n";
                //Item list
                List<int> _slot_in_inventory = new List<int>();
                string _this_line;
                for (int i = 0; i < 8; i++)
                {
                    _this_line = " " + (i + 1) + ". ";
                    _slot_in_inventory.Add(i);

                    if (_selected_character.Inventory[i].index != -1 && !_selected_character.Inventory[i].identified)
                    {
                        _this_line += _selected_character.Inventory[i].ItemName();
                        while (_this_line.Length < 20)
                            _this_line += " ";
                        int _p = (int)Game_Logic.ITEM[_selected_character.Inventory[i].index].price / 2;
                        _this_line += _p + " G.";
                    }
                    _txt += _this_line + "\n";
                }

                // geld and info
                _txt += "\n" + _selected_character.name + " has " + _selected_character.Geld + " G.\n" +
                    "Which Item needs to be identified?";
                _display.Update_Text_Screen(_txt);
                Clear_Buttons();
                for (int i = 0; i < 8; i++)
                    if (_selected_character.Inventory[i].index != -1 && !_selected_character.Inventory[i].identified)
                        Create_Button("Identify item #" + (i + 1), "Identify_This_Item:" + i);
                Create_Button_Last("Done", "Cancel_Uncurse_Identify_Panel");
                return;
            }

            if (_button.Contains("Identify_This_Item:"))
            {
                _button = _button.Replace("Identify_This_Item:", "");
                int _n = int.Parse(_button);
                int _price = (int)Game_Logic.ITEM[_selected_character.Inventory[_n].index].price / 2;
                if (_selected_character.Geld < _price)
                {
                    _castle._selectedItemIndex = -1;
                    _castle.Update_Screen();
                    _display.PopUpMessage("Not enough Geld for that service.");
                    return;
                }
                else
                {
                    _selected_character.Geld -= _price;
                    if (_selected_character.Inventory[_n].equipped) _selected_character.UnequipItem(_n);
                    _selected_character.Inventory[_n].identified = true;
                    _castle._selectedItemIndex = -1;
                    _castle.Update_Screen();
                    _display.PopUpMessage("The Item is now identified!");
                    return;
                }
            }
        }
        //<<<<<<<<<<   TEMPLE INTRO  >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.Temple_Intro)
        {
            if (_button.Contains("Character:"))
            {
                _button = _button.Replace("Character:", "");
                int _num = int.Parse(_button);
                _selectedRoster = _num;
                _selected_character = Game_Logic.PARTY.LookUp_PartyMember(_num); //_selected_character is the temple petitioner
                _castle.townStatus = Castle_Logic.ts.Temple;
                _castle.Update_Screen();
                return;
            }
            if(_button == "Leave_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   TEMPLE >>>>>>>>>>>>>>>>>>>>>>>
        if (_castle.townStatus == Castle_Logic.ts.Temple)
        {
            if (_button == "Leave_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Market;
                _castle.Update_Screen();
                return;
            }

            if (_button == "Pool_Geld") // <------------------------------------------------------------------------------ POOL GELD CODE
            {
                int _pool = 0;
                for (int i = 0; i < 6; i++)
                    if (Game_Logic.PARTY.Get_Roster_Index(i) > -1)
                    {
                        _pool += Game_Logic.PARTY.LookUp_PartyMember(i).Geld;
                        Game_Logic.PARTY.LookUp_PartyMember(i).Geld = 0;
                    }
                _selected_character.Geld = _pool; // <----- _selected_character is petitioner
                _castle.Update_Screen();
                _display.PopUpMessage(_selected_character.name + " now has " + _selected_character.Geld + " G.");
            }

            if (_button == "Donate_button")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("HOW MUCH WOULD YOU LIKE TO DONATE?");
                _display.Block_Buttons();
                return;
            }
            if (_button.Contains("TextInput:"))
            {
                int _G = 0;
                if (int.TryParse(_button.Replace("TextInput:", ""), out _G))
                {
                    if (_G < 0) _G = 0;
                    if (_G > _selected_character.Geld) _G = _selected_character.Geld;                    
                    if(_G == 0)
                    {
                        _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                        _castle.Update_Screen();
                        return;
                    }
                    _selected_character.Geld -= _G;
                    Game_Logic.PARTY.Temple_Favor += _G;
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _castle.Update_Screen();
                    _display.PopUpMessage("Bless you! Generosity earns the favor of CANT!");
                    return;
                }
                else
                {
                    Button_Clicked("Donate_button");
                    _display.PopUpMessage("That is not a valid number. Please try again.");
                    return;
                }
            }

            if (_button == "Petition_button")
            {
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Show_Text_Input_Panel("WHO WOULD YOU LIKE TO AID?", "Who_to_Help:");
            }

            if (_button.Contains("Who_to_Help:"))
            {
                _button = _button.Replace("Who_to_Help:", "").ToUpper();
                Debug.Log("THIS IS WHAT I GOT ==> " + _button);
                
                for (int i = 0; i < Game_Logic.ROSTER.Count; i++)
                    if (Game_Logic.ROSTER[i].name.ToUpper() == _button)
                    {
                        Debug.Log("Found 'em");
                        _castle._selected_character = Game_Logic.ROSTER[i]; // <------ _castle._selected_Character is the character to help.
                        break;
                    }

                int _price = 0;
                if (_castle._selected_character.status == Enum._Status.plyze)
                    _price = 100 * _castle._selected_character.level;

                if (_castle._selected_character.status == Enum._Status.stoned)
                    _price = 200 * _castle._selected_character.level;

                if (_castle._selected_character.status == Enum._Status.dead)
                    _price = 250 * _castle._selected_character.level;

                if (_castle._selected_character.status != Enum._Status.ashes)
                    _price = 500 * _castle._selected_character.level;

                // Is the Character Lost? If so, temple cannot help.
                if (_castle._selected_character.status == Enum._Status.lost)
                {
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _button = "";
                    _castle._selected_character = null;
                    _castle.Update_Screen();
                    _display.PopUpMessage("\n\nThe Priest aplogizes: 'This character is beyond all help.\n\nI'm sorry, there is nothing to be done.'");
                    return;
                }

                // Is the Character still in the dungeon? If so, temple cannot help.
                if (_castle._selected_character.location == Enum._Locaton.Dungeon)
                {
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _button = "";
                    _castle._selected_character = null;
                    _castle.Update_Screen();
                    _display.PopUpMessage("\n\nThe Priest aplogizes: 'This character is still in the dungeon.\n\nFind them to rescue them, and perhaps we can assist!'");
                    return;
                }

                // Is the Character not Paralyzed, Stone, Dead, or Ashes? If so, there is nothing to help
                if (_castle._selected_character.status != Enum._Status.plyze && _castle._selected_character.status != Enum._Status.stoned 
                   && _castle._selected_character.status != Enum._Status.dead && _castle._selected_character.status != Enum._Status.ashes)
                {
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _button = "";
                    _castle.Update_Screen();
                    _display.PopUpMessage("\n\nThe Priest smiles: 'Fortunately"+ _castle._selected_character +" is not Paralyzed, Cursed with stone form, or Dead!");
                    _castle._selected_character = null;
                    return;
                }

                // If the Party does not have enough favor to heal the character, then the Temple will not help.
                if (Game_Logic.PARTY.Temple_Favor < _price)
                {
                    _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                    _button = "";
                    _castle._selected_character = null;
                    _castle.Update_Screen();
                    _display.PopUpMessage("\n\nThe Priest frowns. 'Sadly, CANT chooses to not help at this time.\n\nPerhaps later, when you have more favor, Cant will reconsider.'\n\n" +
                        "\n(You need at least " + _price + " favor.)");
                    return;
                }

                // CANT will help, here are the results:
                string _txt = "\n\nThe Priest smiles: 'CANT!!! BLESS " + _castle._selected_character.name + " with your curative power!'\n\n\n" +
                    "A shining Light shines down, and burns " + _price + " favor!\n\n\n";
                if (_castle._selected_character.status == Enum._Status.plyze || _castle._selected_character.status == Enum._Status.stoned) 
                {
                    _castle._selected_character.status = Enum._Status.OK;
                    _castle._selected_character.location = Enum._Locaton.Roster;
                    _txt += "Success! Welcome back " + _castle._selected_character.name + "!!!";
                }

                if (_castle._selected_character.status == Enum._Status.dead)
                {
                    int _perc = (_castle._selected_character.Vitality * 3) + 50;
                    if (Random.Range(1,101) < _perc)
                    {
                        _castle._selected_character.status = Enum._Status.OK;
                        _castle._selected_character.location = Enum._Locaton.Roster;
                        _txt += "Success! Welcome back " + _castle._selected_character.name + "!!!";
                    }
                    else
                    {
                        _castle._selected_character.status = Enum._Status.ashes;
                        _castle._selected_character.location = Enum._Locaton.Temple;
                        _txt += "Oh no! The ritual has failed!" + _castle._selected_character.name + " is reduced to Ashes!";
                    }
                }

                if (_castle._selected_character.status == Enum._Status.ashes)
                {
                    int _perc = (_castle._selected_character.Vitality * 3) + 40;
                    if (Random.Range(1,101) < _perc)
                    {
                        _castle._selected_character.status = Enum._Status.OK;
                        _castle._selected_character.location = Enum._Locaton.Roster;
                        _txt += "Success! Welcome back " + _castle._selected_character.name + "!!!";
                    }
                    else
                    {
                        _castle._selected_character.location = Enum._Locaton.Temple;
                        _txt += "Oh no! The ritual has failed!" + _castle._selected_character.name + " is still to Ashes!";
                    }
                }

                Game_Logic.PARTY.Temple_Favor -= _price;
                _display.Text_Input_Controller.GetComponent<Text_Input_Panel_Controller>().Close_Text_Input_Panel();
                _button = "";
                _castle.Update_Screen();
                _display.PopUpMessage(_txt);
                _castle._selected_character = null;
                return;
            }
        }
    }
}
