using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Castle_Logic_Manager : MonoBehaviour
{
    public enum Screen { Street, Tavern, Inn, Trader, Temple, Trainer, Maze }
    public Screen CurrentScreen;
    public int CurrentPage;
    [HideInInspector] public Character_Class Selected_Character, Other_Character;
    [HideInInspector] public Item Selected_Item;
    [HideInInspector] public Item_Class Selected_Item_Class;
    [HideInInspector] public int Selected_Party_Slot, Other_Party_Slot, Selected_Roster_Slot, Other_Roster_Slot, Selected_Inventory_Slot, Selected_Item_Index;
    public GameObject Character_Generator_Flow;

    private Castle_Button_Manager _input;
    private Castle_Display_Manager _display;
    private Party_Class _party;

    public void Start()
    {
        _input = FindObjectOfType<Castle_Button_Manager>();
        _display = FindObjectOfType<Castle_Display_Manager>();
        _party = FindObjectOfType<Party_Class>();        
        CurrentPage = 0;
        CurrentScreen = Screen.Street;
        //reset spells
        GameManager.PARTY.Party_Light_Timer = 0;
        GameManager.PARTY.Party_Shield_Bonus = false;
        //reset characters
        for (int i = 0; i < 6; i++)        
            if (!GameManager.PARTY.EmptySlot(i))
            {
                GameManager.PARTY.LookUp_PartyMember(i).Poison = 0; //Cure Poison
                if (GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.afraid || GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.asleep)
                    GameManager.PARTY.LookUp_PartyMember(i).status = Enum._Status.OK; //Cures Asleep and Afraid
                if (GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.lost) GameManager.PARTY.LookUp_PartyMember(i).status = Enum._Status.dead; //Lost characers are now dead
                if (GameManager.PARTY.LookUp_PartyMember(i).location == Enum._Locaton.Dungeon)
                    GameManager.PARTY.LookUp_PartyMember(i).location = Enum._Locaton.Party; //Dungeon characters return to party
            }
        
        for (int i = 5; i > 0; i--) //remove dead, ashes, stoned, and paralyzed members.
            if (!GameManager.PARTY.EmptySlot(i))
            {
                if (GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.plyze ||
                    GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.stoned ||
                    GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.dead ||
                    GameManager.PARTY.LookUp_PartyMember(i).status == Enum._Status.ashes)
                    GameManager.PARTY.RemoveMember(i);
            }        
        
        UpdateScreen();
        if (GameManager.instance._Persistent_Message != "") _display.PopUp_Panel.Show_Message(GameManager.instance._Persistent_Message);
    }

    public void RefreshCharacterSheet()
    {
        _display.Character_Sheet.gameObject.SetActive(false);
        _display.Character_Sheet.gameObject.SetActive(true);
    }

    public void UpdateScreen()
    {
        //Check for Party
        //if (_party.Get_Roster_Index(0) == -1) CurrentScreen = Screen.Tavern;
        
        //DEBUG
        //{ 
        //    CurrentScreen = Screen.Temple; CurrentPage = 1; 
        //    Selected_Character = new Character_Class(); 
        //    Selected_Character.name = "BOONE";
        //    GameManager.ROSTER.Add(Selected_Character);
        //    _party.AddMember(0);
        //}

        _input.ClearButtons();

        string _name = "", _desc = "";
        if (CurrentScreen == Screen.Street)
        {
            _name = "Street";
                   //1234567890123456789012345678901234567890
            _desc = "A bustling community thrives within the \n" +
                    " castle walls. Looking around, you see: \n\n" +
                    "   A tavern full of adventurers         \n" +
                    "   A quiet Inn where you can rest       \n" +
                    "   A Trade shop, filled with wares      \n" +
                    "   A temple thrumming with Holy power   \n" +
                    "   A small training camp where Adven-   \n" +
                    "         turers are testing themselves  \n" +
                    "   ...and just beyond the wall, the     \n" +
                    "                     Labyrinth awaits...";
            _input.SetButton(0, "Visit the Tavern", "goto_tavern");
            _input.SetButton(1, "Visit the Inn", "goto_inn");
            _input.SetButton(2, "Visit Boltac's Trade Goods", "goto_trader");
            _input.SetButton(3, "Visit Temple of CANT", "goto_temple");
            _input.SetButton(4, "Visit Training Camp", "goto_trainer");
            _input.SetButton(9, "Enter the Labyrinth", "goto_maze");
        }

        if (CurrentScreen == Screen.Tavern)
        {
            _name = "tavern";
            if (CurrentPage == 0)
            {
                       //1234567890123456789012345678901234567890
                _desc = "Gilgamesh's Tavern noisy and crowded.   \n" +
                        "Gilgamesh himself is happily busy behind\n" +
                        "the bar, simultaneuosly pouring drinks  \n" +
                        "while maintaining several conversations \n" +
                        "at once.\n\n" +
                        "You find a table in the corner. \n";
                _input.SetButton(0, "Add Party Member", "add_party_member");
                _input.SetButton(1, "Review Roster of Characters", "review_roster");
                _input.SetButton(2, "Remove Party member", "remove_party_member");
                _input.SetButton(3, "View Character Sheet", "view_party_member");
                _input.SetButton(4, "Divvy Geld", "divvy_geld");
                _input.SetButton(9, "Leave Tavern", "goto_street");
            }
            if(CurrentPage == 1)
            {
                _desc = "Which Character would you like to View?";
                for (int i = 0; i < 6; i++)
                    if (_party.Get_Roster_Index(i) > -1) _input.SetButton(i, _party.LookUp_PartyMember(i).name, ("select_roster:" + i));
                _input.SetButton(9, "Back", "goto_tavern");
            }
            if (CurrentPage == 2)
            {
                _display.Character_Sheet.ShowCharacterSheet();
            }
        }

        if(CurrentScreen == Screen.Inn)
        {
            _name = "Inn";
            if (CurrentPage == 0)
            {
                       //1234567890123456789012345678901234567890
                _desc = "The Clerk calls \"NEXT!!!\"...          \n" +
                        "   who steps forward?";
                for (int i = 0; i < 6; i++)
                    if (_party.Get_Roster_Index(i) > -1) _input.SetButton(i, _party.LookUp_PartyMember(i).name, ("select_roster:"+i));
                _input.SetButton(9, "Back", "goto_street");
            }

            if (CurrentPage == 1)
            {
                       //1234567890123456789012345678901234567890
                _desc = "The clerk speaks in a bored voice       \n" +
                        "   without looking up. \"Welcome to the \n" +
                        "   Adventure Inn. Are you looking for a \n" +
                        "   room?\"                              \n\n" +
                        "Stables (free, no HP gain.)             \n" +
                        "A simple Cot (10g/week, 1[d]8 HP.)      \n" +
                        "Economy Room (50g/week, 3[d]8 HP.)      \n" +
                        "Merchant Suite (200g/week, 5[d]8 HP.)   \n" +
                        "Royal Suite (500g/week, 7[d]8 HP.)      \n\n" +
                        Selected_Character.name + " has " + Selected_Character.Geld + "g.";

                _input.SetButton(0, "Stables, Free", "sleep,0,0");
                _input.SetButton(1, "Pool Geld", "pool_geld");
                _input.SetButton(2, "Cot, 10g", "sleep,10,1");
                _input.SetButton(4, "Economy, 50g", "sleep,50,3");
                _input.SetButton(6, "Merchant, 200g", "sleep,200,5");
                _input.SetButton(8, "Royal, 500g", "sleep,500,7");
                _input.SetButton(9, "Back", "goto_inn");                     
            }
        }

        if(CurrentScreen == Screen.Trader)
        {
            _name = "Trade Post";
            if(CurrentPage == 0)
            {
                       //1234567890123456789012345678901234567890
                _desc = "A stout dwarf, apparently Boltac,     \n" +
                        "  glares at you. He bellows:          \n" +
                        "  \"it's a small shop! one at a time!\"\n" +
                        "   He then spits and waves you in.    \n\n" +
                        "Who will enter?";
                for (int i = 0; i < 6; i++)
                    if (_party.Get_Roster_Index(i) > -1) _input.SetButton(i, _party.LookUp_PartyMember(i).name, ("select_roster:" + i));
                _input.SetButton(9, "Cancel", "goto_street");
            }
            if(CurrentPage == 1)
            {
                       //1234567890123456789012345678901234567890
                _desc = "Boltac sizes you up. \"Well? you lookin'\n" +
                        "   to buy, or you got loot to sell?\"   \n" +
                        "   He grunts. \"I can also look at any  \n" +
                        "   cursed or unidentified gear you got.\"\n\n\n" +
                        Selected_Character.name + " has " + Selected_Character.Geld + "g.";
                _input.SetButton(0, "Buy Something", "buy_menu");
                _input.SetButton(1, "Pool Geld", "pool_geld");
                _input.SetButton(2, "Sell Something", "sell_menu");
                _input.SetButton(4, "Uncurse Something", "uncurse_menu");
                _input.SetButton(6, "Identify Something", "identify_menu");
                _input.SetButton(9, "Leave Shop", "goto_trader");
            }
        }

        if(CurrentScreen == Screen.Temple)
        {
            _desc = "Current Favor: " + _party.Temple_Favor + "\n\n" +
                   //1234567890123456789012345678901234567890
                    "As you enter, the head priest smoothly  \n" +
                    "finishes his chant and smiles at you.   \n" +
                    "  \"I greet you in Cant's name. How may \n" +
                    "   I be of assistance?\"";
            _input.SetButton(0, "Offer tithe to gain favor.", "pay_tithe");
            _input.SetButton(2, "Say a prayer to Cant", "say_prayer");
            _input.SetButton(4, "Request Cant's Aid for a fallen character", "cant_heal");
            _input.SetButton(9, "Leave Temple", "goto_street");
        }

        if(CurrentScreen == Screen.Trainer)
        {
            //1234567890123456789012345678901234567890
            _desc = "Around you, adventurers train in small  \n" +
                    "   groups or on their own. At first, it \n" +
                    "   seems like utter chaos, but after a  \n" +
                    "   a moment, you can see the order with \n" +
                    "   which these adventurers conduct      \n" +
                    "      themselves.";
            _input.SetButton(0, "Create New Character", "create_character");
            _input.SetButton(2, "Review Roster of Characters", "review_roster");
            _input.SetButton(4, "View Character Details", "view_roster_character");
            _input.SetButton(7, "Restore Roster", "debug_button_restore");
            _input.SetButton(7, "Kill Roster and quit", "debug_button_delete");
            _input.SetButton(9, "Leave Training Ground", "goto_street");
        }

        _display.Update_Display(_name, _desc);
    }

    public void ReceiveButtonPress(string _text)
    {
        //NAVIGATION
        if (_text.Contains("goto"))
        {
            CurrentScreen = Screen.Street;
            if (_text == "goto_tavern") CurrentScreen = Screen.Tavern;
            if (_text == "goto_inn") CurrentScreen = Screen.Inn;
            if (_text == "goto_trader") CurrentScreen = Screen.Trader;
            if (_text == "goto_temple") CurrentScreen = Screen.Temple;
            if (_text == "goto_trainer") CurrentScreen = Screen.Trainer;
            if (_text == "goto_maze")
            {
                if (GameManager.PARTY.EmptySlot(0))
                {
                    _display.PopUp_Panel.Show_Message("You must gather a party before venturing forth!");
                    return;
                }
                GameManager.PARTY._MakeCampOnLoad = true;
                GameManager.PARTY._PartyXYL = new Vector3Int(0, 0, 1);
                GameManager.PARTY.facing = Enum._Direction.north;
                for (int i = 0; i < 6; i++)
                    if (!GameManager.PARTY.EmptySlot(i))
                        GameManager.PARTY.LookUp_PartyMember(i).location = Enum._Locaton.Dungeon;
                GameManager.instance.SaveGame();
                UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon1");
                return;
            }

            Selected_Character = null;
            Selected_Item = null;
            Selected_Item_Class = null;
            Selected_Party_Slot = -1;
            Selected_Roster_Slot = -1;
            Selected_Inventory_Slot = -1;
            Selected_Item_Index = -1;
            CurrentPage = 0;
            
            UpdateScreen();
            return;
        }

        //ROSTER
        if(_text == "review_roster")
        {
            string _roster = "Roster\n------------\n";
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
            {
                _roster += GameManager.ROSTER[i].name + " (" + GameManager.ROSTER[i].status + ") ";
                if (GameManager.ROSTER[i].location == BlobberEngine.Enum._Locaton.Dungeon ||
                    GameManager.ROSTER[i].location == BlobberEngine.Enum._Locaton.Party) _roster += "out";
                _roster += "\n";
            }
            _display.PopUp_Panel.Show_Message(_roster);
            return;
        }

        //Add member to party
        if(_text == "add_party_member")
        {
            //Room for one more?
            bool _room = false;
            for (int i = 0; i < 6; i++)
                if (GameManager.PARTY.EmptySlot(i))
                    _room = true;

            if (_room)
            {
                _display.TextInput_Panel.Show_Text_Input_Panel("ENTER NAME", "add_party_input:");
            }
            else
            {
                _display.PopUp_Panel.Show_Message("Party is full already");
            }
        }
        if (_text.Contains("ADD_PARTY_INPUT:"))
        {
            //Clean up _text
            _text = _text.Replace("ADD_PARTY_INPUT:", "");
            
            //determine party alignment
            Enum._Alignment _partyAlignment = Enum._Alignment.neutral;
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i))
                    if (_partyAlignment == Enum._Alignment.neutral)
                        _partyAlignment = GameManager.PARTY.LookUp_PartyMember(i).alignment;

            //search the roster for a match
            string _output = "";
            bool _found = false;
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
            {
                string _RosterName = GameManager.ROSTER[i].name.ToUpper();
                if(_text == _RosterName)
                {
                    _found = true;
                    Selected_Character = GameManager.ROSTER[i];
                    Selected_Roster_Slot = i;
                }
            }
            
            if (_found)
            {
                if(Selected_Character.location == BlobberEngine.Enum._Locaton.Dungeon)
                {
                    _output = "That character is already out!";
                }
                else if(Selected_Character.status == BlobberEngine.Enum._Status.lost)
                {
                    _output = "That character is Lost!";
                }
                else if(Selected_Character.location == BlobberEngine.Enum._Locaton.Party)
                {
                    _output = "That character is already in a party!";
                }
                else if (_partyAlignment != Enum._Alignment.neutral && 
                        Selected_Character.alignment != Enum._Alignment.neutral && 
                        Selected_Character.alignment != _partyAlignment)
                {
                    _output = "That character is the wrong alignment for this party!";
                }
                else
                {
                    _output = Selected_Character.name + " has been added to the party";
                    GameManager.PARTY.AddMember(Selected_Roster_Slot);
                    _display.Refresh_Display();
                }
            }
            else
            {
                _output = "NO CHARACTER BY THAT NAME";
            }

            _display.PopUp_Panel.Show_Message(_output);
            return;
        }

        //Remove member from party
        if(_text == "remove_party_member")
        {
            _display.Update_Display("Tavern", "Which Party Member would you like to remove?");
            
            _input.ClearButtons();
            
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i)) _input.SetButton(i, "Remove " + GameManager.PARTY.LookUp_PartyMember(i).name, "remove_party_member#" + i);
            
            _input.SetButton(9, "Cancel", "goto_tavern");
            
            return;
        }
        if (_text.Contains("remove_party_member#"))
        {
            _text = _text.Replace("remove_party_member#", "");
            int _input = int.Parse(_text);
            if (_input >= 0 && _input <= 5) GameManager.PARTY.RemoveMember(_input);
            UpdateScreen();
            return;
        }

        if(_text == "view_party_member")
        {
            CurrentPage++;
            UpdateScreen();
        }

        //Geld Stuff
        if (_text == "divvy_geld")
        {
            int _pool = 0, _num = 0, _share = 0, _remainder = 0;
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    _num++;
                    _pool += GameManager.PARTY.LookUp_PartyMember(i).Geld;
                    GameManager.PARTY.LookUp_PartyMember(i).Geld = 0;
                }
            if (_num > 0)
            {
                _share = (int)(_pool / _num);
                _remainder = _pool % _num;
                for (int i = 0; i < 6; i++)
                    if (!GameManager.PARTY.EmptySlot(i))
                    {
                        GameManager.PARTY.LookUp_PartyMember(i).Geld = _share;
                    }
                GameManager.PARTY.LookUp_PartyMember(0).Geld += _remainder;
                string _txt = "Each party memeber receives " + _share + "g.";
                if (_remainder > 0) _txt += "\n" + GameManager.PARTY.LookUp_PartyMember(0).name + " receives the extra " + _remainder + "g.";
                _display.PopUp_Panel.Show_Message(_txt);
            }
            UpdateScreen();
            return;
        }
        if (_text == "pool_geld")
        {
            int _pool = 0;
            for (int i = 0; i < 6; i++)
                if(!GameManager.PARTY.EmptySlot(i))
                {
                    _pool += GameManager.PARTY.LookUp_PartyMember(i).Geld;
                    GameManager.PARTY.LookUp_PartyMember(i).Geld = 0;
                }
            Selected_Character.Geld = _pool;
            _display.PopUp_Panel.Show_Message(Selected_Character.name + " collects " + _pool + " g from the party.");
            UpdateScreen();
            return;
        }

        //Enter Anything
        if (_text.Contains("select_roster:"))
        {
            _text = _text.Replace("select_roster:", "");
            int _num = int.Parse(_text);
            if(_num >= 0 && _num <= 5)
            {
                Selected_Party_Slot = _num;
                Selected_Character = GameManager.PARTY.LookUp_PartyMember(_num);
                Selected_Roster_Slot = GameManager.PARTY.Get_Roster_Index(_num);
                CurrentPage++;
            }
            UpdateScreen();
            return;
        }

        //Inn
        if (_text.Contains("sleep"))
        {
            _text = _text.Replace("sleep,", "");
            string[] data = _text.Split(",");
            int _stay_cost = int.Parse(data[0]);
            int _dice = int.Parse(data[1]);
            int _delta = Selected_Character.xp_nnl - Selected_Character.xp;

            if (Selected_Character.Geld < _stay_cost)
            {
                _display.PopUp_Panel.Show_Message(Selected_Character.name + " does not have enough Geld!");
                UpdateScreen();
                return;
            }
            else 
            {
                //subtract Geld
                Selected_Character.Geld = -_stay_cost;

                //Heal HP
                if (_dice > 0) 
                    for (int i = 0; i < _dice; i++) 
                        Selected_Character.HP += Random.Range(0, 8) + 1;
                if (Selected_Character.HP > Selected_Character.HP_MAX) Selected_Character.HP = Selected_Character.HP_MAX;

                //Restore spells
                for (int i = 0; i < 7; i++)
                {
                    Selected_Character.mageSpellsCast[i] = 0;
                    Selected_Character.priestSpellsCast[i] = 0;
                }

                //check for levelup
                if (_delta > 0)
                { //no
                    _display.PopUp_Panel.Show_Message(Selected_Character.name + " needs " + _delta + " XP to Level Up.");
                    CurrentPage = 0;
                    UpdateScreen();
                    return;
                }
                else
                { //yes
                    LevelUp(Selected_Character);
                    CurrentPage = 0;
                    UpdateScreen();
                    return;
                }
            }
        }

        //Boltac's
        if(_text == "buy_menu")
        {
            if (Selected_Character.HasEmptyInventorySlot())
            {
                _display.Trade_Panel.BuyScreen();
                return;
            }
            else
            {
                _display.PopUp_Panel.Show_Message("Boltac sighs angrily.\n\"You don't have any space to buy anything!\nMaybe sell something first?\"");
            }
        }
        if(_text == "sell_menu")
        {
            _display.Trade_Panel.SellScreen();
            return;
        }
        if(_text == "uncurse_menu")
        {
            bool curses = false;
            for (int i = 0; i < 8; i++)
                if (Selected_Character.Inventory[i].curse_active) curses = true;
            if (!curses)
            {
                _display.PopUp_Panel.Show_Message("Boltac eyes you carefully.\n\"Nothing seems to be cursed, good.\"");
                return;
            }
            else
            {
                _display.Trade_Panel.UncurseScreen();
                return;
            }
        }
        if (_text == "identify_menu")
        {
            _display.Trade_Panel.IdentifyScreen();
            return;
        }

        //Temple
        if (_text == "pay_tithe")
        {
            string _txt = "Each character pays 10% of their\n" +
                          "geld as a tithe.\n" +
                          "------------------------------------\n";
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    int _tithe = (int)(GameManager.PARTY.LookUp_PartyMember(i).Geld * 0.1f);
                    _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " tithes " + _tithe + "g\n";
                    GameManager.PARTY.LookUp_PartyMember(i).Geld -= _tithe;
                    GameManager.PARTY.Temple_Favor += _tithe;
                }

            _display.PopUp_Panel.Show_Message(_txt);
            UpdateScreen();
            return;
        }
        if (_text == "say_prayer")
        {
            string _txt = "Each character says a prayer to CANT\n" +
                          "------------------------------------\n";
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    if (GameManager.PARTY.LookUp_PartyMember(i).Piety < 5) 
                        _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " has angered CANT.\n";
                    if (GameManager.PARTY.LookUp_PartyMember(i).Piety > 4 && GameManager.PARTY.LookUp_PartyMember(i).Piety < 10) 
                        _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " feels nothing.\n";
                    if (GameManager.PARTY.LookUp_PartyMember(i).Piety > 9 && GameManager.PARTY.LookUp_PartyMember(i).Piety < 16) 
                        _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " feels warm.\n";
                    if (GameManager.PARTY.LookUp_PartyMember(i).Piety > 15 && GameManager.PARTY.LookUp_PartyMember(i).Piety < 18) 
                        _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " feels welcome.\n";
                    if (GameManager.PARTY.LookUp_PartyMember(i).Piety == 18)
                    {
                        _txt += GameManager.PARTY.LookUp_PartyMember(i).name + " feels bliss.\n";
                        GameManager.PARTY.LookUp_PartyMember(i).HP++;
                        if (GameManager.PARTY.LookUp_PartyMember(i).HP > GameManager.PARTY.LookUp_PartyMember(i).HP_MAX) 
                            GameManager.PARTY.LookUp_PartyMember(i).HP = GameManager.PARTY.LookUp_PartyMember(i).HP_MAX;
                        for (int c = 0; c < 7; c++)
                        {
                            Selected_Character.mageSpellsCast[c] = 0;
                            Selected_Character.priestSpellsCast[c] = 0;
                        }
                    }
                }
            _display.PopUp_Panel.Show_Message(_txt);
            UpdateScreen();
            return;
        }
        if (_text == "cant_heal")
        {
            _display.Trade_Panel.HealScreen();
            return;
        }

        //Trainer
        if(_text == "view_roster_character")
        {
            _display.TextInput_Panel.Show_Text_Input_Panel("ENTER NAME", "trainer_view_input:");
            
        }
        if (_text.Contains("TRAINER_VIEW_INPUT:"))
        {
            _text = _text.Replace("TRAINER_VIEW_INPUT:", "");
            _text = _text.ToUpper();

            //search the roster for a match
            bool _found = false;
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
            {
                string _RosterName = GameManager.ROSTER[i].name.ToUpper();
                if (_text == _RosterName)
                {
                    _found = true;
                    Selected_Character = GameManager.ROSTER[i];
                    Selected_Roster_Slot = i;
                }
            }
            if (_found)
            {
                _display.Character_Sheet.ShowCharacterSheet(true);
                return;
            }
            else
            {
                _display.PopUp_Panel.Show_Message("No Character by that name.");
                return;
            }
        }
        if (_text.Contains("RENAME_CHARACTER:"))
        {
            _text = _text.Replace("RENAME_CHARACTER:", "");
            _text = _text.ToUpper();
            //search the roster for a match
            bool _found = false;
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
            {
                string _RosterName = GameManager.ROSTER[i].name.ToUpper();
                if (_text == _RosterName)
                    _found = true;
            }
            if (_found)
            {
                _display.PopUp_Panel.Show_Message("Another character already has that name!");
            }
            else
            {
                Selected_Character.name = _text;
            }
            _display.Character_Sheet.ShowCharacterSheet(true);
            return;
        }
        if(_text == "create_character")
        {
            Character_Generator_Flow.SetActive(true);
        }

        if(_text == "debug_button_restore")
        {
            GameManager.instance.RestoreRoster();
        }
        if(_text == "debug_button_delete")
        {
            GameManager.instance.KillRoster();
        }
    }

    #region LEVEL_UP

    public void LevelUp(Character_Class _me)
    {
        bool _newSpells = false;
        int _deltaStr = 0, _deltaIQ = 0, _deltaPi = 0, _deltaVit = 0, _deltaAgi = 0, _deltaLk = 0;

        //Increase level and assign new nnl        
        _me.level++; _me.xp_nnl = new XPTable().LookupNNL(_me.level, _me.character_class);

        //Each attrib has a 75% chance to change
        int VAL = 65;
        bool _strChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _IQChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _PiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _VitChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _AgiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _LkChange = Random.Range(0, 101) <= VAL ? true : false;

        //if attrib changes, there is a chance it decreases, otherwise it increases
        float _downOdds = (_me.ageInWeeks / 52) / 130f; //Debug.Log("Odds of decreasing " + _downOdds * 100);
        if (_strChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaStr = -1; }
            else { _deltaStr = 1; }
        if (_IQChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaIQ = -1; }
            else { _deltaIQ = 1; }
        if (_PiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaPi = -1; }
            else { _deltaPi = 1; }
        if (_VitChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaVit = -1; }
            else { _deltaVit = 1; }
        if (_AgiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaAgi = -1; }
            else { _deltaAgi = 1; }
        if (_LkChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaLk = -1; }
            else { _deltaLk = 1; }
        // If Attribs are 18 and delta is -1, 5 in 6 chance that they do not change
        if (_me.Strength == 18 && _deltaStr == -1 && Random.Range(0, 6) > 0) _deltaStr = 0;
        if (_me.IQ == 18 && _deltaIQ == -1 && Random.Range(0, 6) > 0) _deltaIQ = 0;
        if (_me.Piety == 18 && _deltaPi == -1 && Random.Range(0, 6) > 0) _deltaPi = 0;
        if (_me.Vitality == 18 && _deltaVit == -1 && Random.Range(0, 6) > 0) _deltaVit = 0;
        if (_me.Agility == 18 && _deltaAgi == -1 && Random.Range(0, 6) > 0) _deltaAgi = 0;
        if (_me.Luck == 18 && _deltaLk == -1 && Random.Range(0, 6) > 0) _deltaLk = 0;
        //Apply attrib changes
        _me.Strength += _deltaStr;
        _me.IQ += _deltaIQ;
        _me.Piety += _deltaPi;
        _me.Vitality += _deltaVit;
        _me.Agility += _deltaAgi;
        _me.Luck += _deltaLk;
        //Bound Attribs
        if (_me.Strength > 18) { _me.Strength = 18; _deltaStr = 0; }
        if (_me.Strength < 1) { _me.Strength = 1; _deltaStr = 0; }
        if (_me.IQ > 18) { _me.IQ = 18; _deltaIQ = 0; }
        if (_me.IQ < 1) { _me.IQ = 1; _deltaIQ = 0; }
        if (_me.Piety > 18) { _me.Piety = 18; _deltaPi = 0; }
        if (_me.Piety < 1) { _me.Piety = 1; _deltaPi = 0; }
        if (_me.Vitality > 18) { _me.Vitality = 18; _deltaVit = 0; }
        if (_me.Agility > 18) { _me.Agility = 18; _deltaAgi = 0; }
        if (_me.Agility < 1) { _me.Agility = 1; _deltaAgi = 0; }
        if (_me.Luck > 18) { _me.Luck = 18; _deltaLk = 0; }
        if (_me.Luck < 1) { _me.Luck = 1; _deltaLk = 0; }
        //Vitality below 3 is a special case
        if (_me.Vitality < 3)
        {
            _me.Vitality = 1; _deltaVit = 0;
            _me.status = BlobberEngine.Enum._Status.dead;
            _me.inParty = false;
            GameManager.PARTY.RemoveMember(Selected_Party_Slot);
            _display.PopUp_Panel.Show_Message(_me.name + " has died of old age.");
            CurrentPage = 0;
            CurrentScreen = Screen.Street;
            UpdateScreen();
            return;
        }

        //Swing Count
        _me.CalculateBaseSwings();
        if (_me.eqWeapon > -1 && GameManager.ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings;
        if (_me.eqArmor > -1 && GameManager.ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings;
        if (_me.eqShield > -1 && GameManager.ITEM[_me.Inventory[_me.eqShield].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqShield].index].xtra_swings;
        if (_me.eqHelmet > -1 && GameManager.ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings;
        if (_me.eqGauntlet > -1 && GameManager.ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings;
        if (_me.eqMisc > -1 && GameManager.ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings > _me.swing_count) _me.swing_count = GameManager.ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings;

        //ReRoll HP_MAX
        int _bonus = 0; //Bonus hitpoints based on vitality
        if (_me.Vitality == 3) _bonus = -3;
        if (_me.Vitality == 5 || _me.Vitality == 4) _bonus = -1;
        if (_me.Vitality == 16) _bonus = 1;
        if (_me.Vitality == 17) _bonus = 2;
        if (_me.Vitality == 18) _bonus = 3;
        int _hpDelta = _me.HP_MAX - _me.HP;
        _me.HP_MAX++;
        int _newHP = 0; for (int i = 0; i < _me.level; i++) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //roll once per level
        if (_me.character_class == Enum._Class.samurai) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //Samurai get 1 extra roll
        if (_newHP > _me.HP_MAX) _me.HP_MAX = _newHP; //assign newHP if it is greater than HP_MAX + 1
        _me.HP = _me.HP_MAX - _hpDelta; //Gaining new HP_Max should not widen the gap between HP and HP_Max

        //Wizardry
        int a = 0, b = 0;
        //Mage Spells
        if (_me.character_class == Enum._Class.mage || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.samurai)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
                                                                              //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.mage) { a = 0; b = 2; }
            if (_me.character_class == Enum._Class.bishop) { a = 0; b = 4; }
            if (_me.character_class == Enum._Class.samurai) { a = 3; b = 3; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.mageSpells[i])
                {
                    _newSpells = true;
                    _me.mageSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.IQ / 30);
            for (int i = 29; i < 50; i++)
                if (_me.mageSpells[GameManager.SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + GameManager.SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[29]) _SpC[0]++;
            if (_me.SpellKnown[30]) _SpC[0]++;
            if (_me.SpellKnown[31]) _SpC[0]++;
            if (_me.SpellKnown[32]) _SpC[0]++;
            if (_me.SpellKnown[33]) _SpC[1]++;
            if (_me.SpellKnown[34]) _SpC[1]++;
            if (_me.SpellKnown[35]) _SpC[2]++;
            if (_me.SpellKnown[36]) _SpC[2]++;
            if (_me.SpellKnown[37]) _SpC[3]++;
            if (_me.SpellKnown[38]) _SpC[3]++;
            if (_me.SpellKnown[39]) _SpC[3]++;
            if (_me.SpellKnown[40]) _SpC[4]++;
            if (_me.SpellKnown[41]) _SpC[4]++;
            if (_me.SpellKnown[42]) _SpC[4]++;
            if (_me.SpellKnown[43]) _SpC[5]++;
            if (_me.SpellKnown[44]) _SpC[5]++;
            if (_me.SpellKnown[45]) _SpC[5]++;
            if (_me.SpellKnown[46]) _SpC[5]++;
            if (_me.SpellKnown[47]) _SpC[6]++;
            if (_me.SpellKnown[48]) _SpC[6]++;
            if (_me.SpellKnown[49]) _SpC[6]++;
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.mageSpells[i] < _SpC[i]) _me.mageSpells[i] = _SpC[i];
        }
        //Priest Spells
        if (_me.character_class == Enum._Class.priest || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.lord)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
                                                                              //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.priest) { a = 0; b = 2; }
            if (_me.character_class == Enum._Class.bishop) { a = 3; b = 4; }
            if (_me.character_class == Enum._Class.lord) { a = 3; b = 2; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.priestSpells[i])
                {
                    _newSpells = true;
                    _me.priestSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.Piety / 30);
            for (int i = 0; i < 30; i++)
                if (_me.priestSpells[GameManager.SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + GameManager.SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[0]) _SpC[0]++;
            if (_me.SpellKnown[1]) _SpC[0]++;
            if (_me.SpellKnown[2]) _SpC[0]++;
            if (_me.SpellKnown[3]) _SpC[0]++;
            if (_me.SpellKnown[4]) _SpC[0]++;
            if (_me.SpellKnown[5]) _SpC[1]++;
            if (_me.SpellKnown[6]) _SpC[1]++;
            if (_me.SpellKnown[7]) _SpC[1]++;
            if (_me.SpellKnown[8]) _SpC[1]++;
            if (_me.SpellKnown[9]) _SpC[2]++;
            if (_me.SpellKnown[10]) _SpC[2]++;
            if (_me.SpellKnown[11]) _SpC[2]++;
            if (_me.SpellKnown[12]) _SpC[2]++;
            if (_me.SpellKnown[13]) _SpC[3]++;
            if (_me.SpellKnown[14]) _SpC[3]++;
            if (_me.SpellKnown[15]) _SpC[3]++;
            if (_me.SpellKnown[16]) _SpC[3]++;
            if (_me.SpellKnown[17]) _SpC[4]++;
            if (_me.SpellKnown[18]) _SpC[4]++;
            if (_me.SpellKnown[19]) _SpC[4]++;
            if (_me.SpellKnown[20]) _SpC[4]++;
            if (_me.SpellKnown[21]) _SpC[4]++;
            if (_me.SpellKnown[22]) _SpC[4]++;
            if (_me.SpellKnown[23]) _SpC[5]++;
            if (_me.SpellKnown[24]) _SpC[5]++;
            if (_me.SpellKnown[25]) _SpC[5]++;
            if (_me.SpellKnown[26]) _SpC[5]++;
            if (_me.SpellKnown[27]) _SpC[6]++;
            if (_me.SpellKnown[28]) _SpC[6]++;
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.priestSpells[i] < _SpC[i]) _me.priestSpells[i] = _SpC[i];
        }

        string _levelUpMessage = _me.name + " has leveled up!";
        if (_newSpells) _levelUpMessage += "\n" + _me.name + " learned new spells!!!";
        if (_deltaStr < 0) _levelUpMessage += "\n" + _me.name + " has lost Strength.";
        if (_deltaStr > 0) _levelUpMessage += "\n" + _me.name + " has gained Strength.";
        if (_deltaIQ < 0) _levelUpMessage += "\n" + _me.name + " has lost I.Q.";
        if (_deltaIQ > 0) _levelUpMessage += "\n" + _me.name + " has gained I.Q.";
        if (_deltaPi < 0) _levelUpMessage += "\n" + _me.name + " has lost Piety.";
        if (_deltaPi > 0) _levelUpMessage += "\n" + _me.name + " has gained Piety.";
        if (_deltaVit < 0) _levelUpMessage += "\n" + _me.name + " has lost Vitality.";
        if (_deltaVit > 0) _levelUpMessage += "\n" + _me.name + " has gained Vitality.";
        if (_deltaAgi < 0) _levelUpMessage += "\n" + _me.name + " has lost Agility.";
        if (_deltaAgi > 0) _levelUpMessage += "\n" + _me.name + " has gained Agility.";
        if (_deltaLk < 0) _levelUpMessage += "\n" + _me.name + " has lost Luck.";
        if (_deltaLk > 0) _levelUpMessage += "\n" + _me.name + " has gained Luck.";
        _levelUpMessage = _levelUpMessage.ToUpper();
        _display.PopUp_Panel.Show_Message(_levelUpMessage);
    }

    #endregion
}
