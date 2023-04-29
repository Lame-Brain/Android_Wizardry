using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Logic_Manager : MonoBehaviour
{
    public enum Screen { Street, Tavern, Inn, Trader, Temple, Trainer, Maze }
    public Screen CurrentScreen;
    [HideInInspector] public Character_Class Selected_Character;
    [HideInInspector] public Item Selected_Item;
    [HideInInspector] public Item_Class Selected_Item_Class;
    [HideInInspector] public int Selected_Party_Slot, Selected_Roster_Slot, Selected_Inventory_Slot, Selected_Item_Index;

    private Castle_Button_Manager _input;
    private Castle_Display_Manager _display;

    private void Start()
    {
        _input = FindObjectOfType<Castle_Button_Manager>();
        _display = FindObjectOfType<Castle_Display_Manager>();
    }
}
