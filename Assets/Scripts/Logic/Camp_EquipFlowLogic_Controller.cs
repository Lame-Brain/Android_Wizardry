using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using BlobberEngine;

public class Camp_EquipFlowLogic_Controller : MonoBehaviour
{
    //0 Weapon
    //1 Armor
    //2 Shield
    //3 Helm
    //4 Gloves
    //5 Misc


    public TextMeshProUGUI top, mid;
    public TextMeshProUGUI[] button_message;
    public GameObject[] button;

    private int phase;
    private Camp_Logic_Manager _castle;
    private List<Item> _list = new List<Item>();
    private List<int> _slot = new List<int>();


    private void OnEnable()
    {
        _castle = FindObjectOfType<Camp_Logic_Manager>();
        top.fontSize = GameManager.FONT;
        mid.fontSize = GameManager.FONT;
        for (int i = 0; i < button_message.Length; i++) button_message[i].fontSize = GameManager.FONT;
        for (int i = 0; i < button.Length; i++) button[i].SetActive(false);
        button[button.Length - 1].SetActive(true);
        phase = 0;
        //Unequip equipped items. (even cursed ones, they will be equipped later)
        for (int c = 0; c < 8; c++)
            if (_castle.Selected_Character.Inventory[c].index > -1 && _castle.Selected_Character.Inventory[c].equipped)
                _castle.Selected_Character.UnequipItem(c);

        ShowScreen();
    }

    public void ShowScreen()
    {
        for (int i = 0; i < button.Length - 1; i++) button[i].SetActive(false);
        string _myClass = _castle.Selected_Character.character_class.ToString().Substring(0, 1).ToLower();
        //build list of eligible items
        _list.Clear();
        _slot.Clear();

        Item hasCurse = null;
        Enum._Item_Type _type = Enum._Item_Type.Weapon;
        if (phase == 1) _type = Enum._Item_Type.Armor;
        if (phase == 2) _type = Enum._Item_Type.Shield;
        if (phase == 3) _type = Enum._Item_Type.Helmet;
        if (phase == 4) _type = Enum._Item_Type.Gauntlets;
        if (phase == 5) _type = Enum._Item_Type.Misc;

        for (int c = 0; c < 8; c++)
        {
            if (_castle.Selected_Character.Inventory[c].index > -1)
                if (GameManager.ITEM[_castle.Selected_Character.Inventory[c].index].item_type == _type)
                    if (GameManager.ITEM[_castle.Selected_Character.Inventory[c].index].class_use.Contains(_myClass))
                    {
                        //If a slot has an item, it is of the appropriate type for this phase, and it is usable, it goes on the list
                        _list.Add(_castle.Selected_Character.Inventory[c]);
                        _slot.Add(c);
                        //if an item that is cursed is in the this phase's slot, that item is equipped.
                        if (GameManager.ITEM[_castle.Selected_Character.Inventory[c].index].cursed)
                        {
                            hasCurse = _castle.Selected_Character.Inventory[c];
                            if (_type == Enum._Item_Type.Weapon && _castle.Selected_Character.eqWeapon == -1) _castle.Selected_Character.EquipItem(c);
                            if (_type == Enum._Item_Type.Armor && _castle.Selected_Character.eqArmor == -1) _castle.Selected_Character.EquipItem(c);
                            if (_type == Enum._Item_Type.Shield && _castle.Selected_Character.eqShield == -1) _castle.Selected_Character.EquipItem(c);
                            if (_type == Enum._Item_Type.Helmet && _castle.Selected_Character.eqHelmet == -1) _castle.Selected_Character.EquipItem(c);
                            if (_type == Enum._Item_Type.Gauntlets && _castle.Selected_Character.eqGauntlet == -1) _castle.Selected_Character.EquipItem(c);
                            if (_type == Enum._Item_Type.Misc && _castle.Selected_Character.eqMisc == -1) _castle.Selected_Character.EquipItem(c);
                        }
                    }
        }

        if (_list.Count > 0) //skip this phase if there is no eligible items
        {

            if (phase == 0) top.text = "Select a Weapon\n(or select Done to equip nothing.)";
            if (phase == 1) top.text = "Select a Armor\n(or select Done to equip nothing.)";
            if (phase == 2) top.text = "Select a Shield\n(or select Done to equip nothing.)";
            if (phase == 3) top.text = "Select a Helmet\n(or select Done to equip nothing.)";
            if (phase == 4) top.text = "Select Gauntlets\n(or select Done to equip nothing.)";
            if (phase == 5) top.text = "Select Misc. Item\n(or select Done to equip nothing.)";

            if (hasCurse != null)
            {
                mid.text = "A cursed item has forced you to skip this slot.";
            }
            else
            {
                mid.text = "You can equip:\n--------------------\n";
                for (int c = 0; c < _list.Count; c++)
                {
                    mid.text += _list[c].ItemName() + "\n";
                    button[c].SetActive(true);
                    button_message[c].text = _list[c].ItemName();
                }
            }
        }
        else
        {
            phase++;
            if (phase < 6) ShowScreen();
            if (phase == 6) FinishFlow();
        }
    }

    public void Button(int _input)
    {
        if (_input >= 0 && _input <= 7)
            _castle.Selected_Character.EquipItem(_slot[_input]);

        phase++;
        if (phase < 6) ShowScreen();
        if (phase == 6) FinishFlow();
    }

    private void FinishFlow()
    {
        this.gameObject.SetActive(false);
        _castle.RefreshCharacterSheet();
        _castle.UpdateScreen();

    }
}
