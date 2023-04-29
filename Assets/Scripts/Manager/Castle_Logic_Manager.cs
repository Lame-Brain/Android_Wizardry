using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Logic_Manager : MonoBehaviour
{
    public enum Screen { Street, Tavern, Inn, Trader, Temple, Trainer, Maze }
    public Screen CurrentScreen;
    public int CurrentPage;
    [HideInInspector] public Character_Class Selected_Character;
    [HideInInspector] public Item Selected_Item;
    [HideInInspector] public Item_Class Selected_Item_Class;
    [HideInInspector] public int Selected_Party_Slot, Selected_Roster_Slot, Selected_Inventory_Slot, Selected_Item_Index;

    private Castle_Button_Manager _input;
    private Castle_Display_Manager _display;
    private Party_Class _party;

    private void Start()
    {
    }
    private void OnEnable()
    {
        _input = FindObjectOfType<Castle_Button_Manager>();
        _display = FindObjectOfType<Castle_Display_Manager>();
        _party = FindObjectOfType<Party_Class>();
        CurrentPage = 0;
        CurrentScreen = Screen.Street;
        UpdateScreen();
    }

    public void UpdateScreen()
    {
        //Check for Party
        if (_party.Get_Roster_Index(0) == -1) CurrentScreen = Screen.Tavern;
        
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
                   //1234567890123456789012345678901234567890
            _desc = "Gilgamesh's Tavern noisy and crowded.   \n" +
                    "Gilgamesh himself is happily busy behind\n" +
                    "the bar, simultaneuosly pouring drinks  \n" +
                    "while maintaining several conversations \n" +
                    "at once.\n\n" +
                    "You find a seat and wait to be noticed. \n";
            _input.SetButton(0, "Add Party Member", "add_party_member");
            _input.SetButton(2, "Remove Party member", "remove_party_member");
            _input.SetButton(4, "Divvy Geld", "divvy_geld");
            _input.SetButton(9, "Leave Tavern", "goto_street");
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
                _input.SetButton(9, "Cancel", "goto_street");
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
                        "Royal Suite (500g/week, 7[d]8 HP.)      \n";
                _input.SetButton(0, "Stables, Free", "sleep, 0, 0");
                _input.SetButton(2, "Cot, 10g", "sleep, 10, 1");
                _input.SetButton(4, "Economy, 50g", "sleep, 50, 3");
                _input.SetButton(6, "Merchant, 200g", "sleep, 200, 5");
                _input.SetButton(8, "Royal, 500g", "sleep, 500, 7");
                _input.SetButton(9, "Leave the Inn", "goto_street");                     
            }
        }

        if(CurrentScreen == Screen.Trader)
        {
            _name = "Trade Post";
            if(CurrentPage == 0)
            {
                       //1234567890123456789012345678901234567890
                _desc = "A stout dwarf, apparently Boltac,     \n" +
                        "  glares at you. He bellows at you:   \n" +
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
                        "   cursed or unidentified gear you got.\"\n\n";
                _input.SetButton(0, "Buy Something", "goto_buy_menu");
                _input.SetButton(2, "Sell Something", "goto_sell_menu");
                _input.SetButton(4, "Uncurse Something", "goto_uncurse_menu");
                _input.SetButton(6, "Identify Something", "goto_identify_menu");
                _input.SetButton(9, "Leave Shop", "goto_street");
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
            _input.SetButton(4, "View a Roster Character", "view_roster_character");
            _input.SetButton(9, "Leave Training Ground", "goto_street");
        }

        _display.Update_Display(_name, _desc);
    }

    public void ReceiveButtonPress(string _text)
    {
        if(_text == "goto_street")
        {
            Selected_Character = null;
            Selected_Item = null;
            Selected_Item_Class = null;
            Selected_Party_Slot = -1;
            Selected_Roster_Slot = -1;
            Selected_Inventory_Slot = -1;
            Selected_Item_Index = -1;
            CurrentPage = 0;
            CurrentScreen = Screen.Street;
            UpdateScreen();
        }                
    }
}
