using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Item_View_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI main, drop_btn_title, trade_btn_title, done1_btn_title, done2_btn_title, done3_btn_title, confirm_btn_title;
    public TMPro.TextMeshProUGUI[] charSlot_btn_title;
    public TMPro.TextMeshProUGUI[] invn_slot_btn_title;
    public GameObject trade_button, drop_button, done1_button, done2_button, done3_button, confirm_button;
    public GameObject[] charSlot_button;
    public GameObject[] invnSlot_button;

    public TMPro.TMP_InputField _InputField;
    public TMPro.TextMeshProUGUI _InputPlaceHolder;
    public TMPro.TextMeshProUGUI _InputText;


    private Castle_Logic_Manager _castle;
    public bool tradeGeld = false; 

    private void OnEnable()
    {
        _castle = FindObjectOfType<Castle_Logic_Manager>();
        main.fontSize = GameManager.FONT;
        trade_btn_title.fontSize = GameManager.FONT;
        drop_btn_title.fontSize = GameManager.FONT;
        done1_btn_title.fontSize = GameManager.FONT;
        done2_btn_title.fontSize = GameManager.FONT;
        done3_btn_title.fontSize = GameManager.FONT;
        confirm_btn_title.fontSize = GameManager.FONT;
        for (int i = 0; i < charSlot_btn_title.Length; i++)
            charSlot_btn_title[i].fontSize = GameManager.FONT;
        for (int i = 0; i < invn_slot_btn_title.Length; i++)
            invn_slot_btn_title[i].fontSize = GameManager.FONT;
        _InputPlaceHolder.fontSize = GameManager.FONT;
        _InputText.fontSize = GameManager.FONT;
        _InputField.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
    }

    public void ClearButtons()
    {
        drop_button.SetActive(false);
        trade_button.SetActive(false);
        done1_button.SetActive(false);
        done2_button.SetActive(false);
        done3_button.SetActive(false);
        confirm_button.SetActive(false);
        for (int i = 0; i < charSlot_button.Length; i++)
            charSlot_button[i].SetActive(false);
        for (int i = 0; i < invnSlot_button.Length; i++)
            invnSlot_button[i].SetActive(false);
        _InputField.gameObject.SetActive(false);
    }

    public void Button(string _text)
    {
        if(_text == "Done")
        {
            this.gameObject.SetActive(false);
        }
        if (_text.Contains("ChooseItem#"))
        {
            _text = _text.Replace("ChooseItem#", "");
            int _n = int.Parse(_text);
            _castle.Selected_Inventory_Slot = _n;
            _castle.Selected_Item = _castle.Selected_Character.Inventory[_n];
            _castle.Selected_Item_Index = _castle.Selected_Item.index;
            _castle.Selected_Item_Class = GameManager.ITEM[_castle.Selected_Item.index];
            ShowItem(_castle.Selected_Item);
        }
        if (_text.Contains("Character#"))
        {
            _text = _text.Replace("Character#", "");
            int _n = int.Parse(_text);
            _castle.Other_Party_Slot = _n;
            _castle.Other_Character = GameManager.PARTY.LookUp_PartyMember(_castle.Other_Party_Slot);
            _castle.Other_Roster_Slot = GameManager.PARTY.Get_Roster_Index(_castle.Other_Party_Slot);
            if (tradeGeld)
            {
                HowMuchGeld();
            }
            else
            {
                if (!_castle.Other_Character.HasEmptyInventorySlot())
                {
                    ClearButtons();
                    main.text = _castle.Other_Character + " Does not have any empty slots.\nTrade is cancelled.";
                    done1_button.SetActive(true);
                }
                else if (_castle.Selected_Item.curse_active)
                {
                    ClearButtons();
                    main.text = "You cannot trade cursed objects!\nTrade is cancelled.";
                    done1_button.SetActive(true);
                }
                else
                {
                    if (_castle.Selected_Item.equipped) _castle.Selected_Character.UnequipItem(_castle.Selected_Inventory_Slot);
                    int _slot = _castle.Other_Character.GetEmptyInventorySlot();
                    _castle.Other_Character.Inventory[_slot] = new Item(_castle.Selected_Item.index, false, false, _castle.Selected_Item.identified);
                    _castle.Selected_Character.Inventory[_castle.Selected_Inventory_Slot] = new Item();
                    _castle.RefreshCharacterSheet();
                    this.gameObject.SetActive(false);
                }
            }

        }
        if(_text == "Trade_Item")
        {
            ChoosePartyMember();
        }
        if(_text == "Drop_Item")
        {
            ClearButtons();
            done1_button.SetActive(true);
            if (_castle.Selected_Item.curse_active)
            {
                main.text = "You cannot discard cursed objects!";
            }
            else
            {
                main.text = "Really discard " + _castle.Selected_Item.ItemName() + " forever?";
                confirm_button.SetActive(true);
            }
        }
        if(_text == "Confirm_Drop")
        {
            _castle.Selected_Character.Inventory[_castle.Selected_Inventory_Slot] = new Item();
            this.gameObject.SetActive(false);
            _castle.RefreshCharacterSheet();
        }
    }

    public void ChooseItem()
    {
        this.gameObject.SetActive(true);
        ClearButtons();
        main.text = "Which item will you view?";
        done3_button.SetActive(true);
        for (int i = 0; i < 8; i++)
            if(_castle.Selected_Character.Inventory[i].index > -1)
            {
                invnSlot_button[i].SetActive(true);
                invn_slot_btn_title[i].text = _castle.Selected_Character.Inventory[i].ItemName();
            }
    }
    public void ShowItem(Item _item)
    {
        string _txt = _castle.Selected_Item.ItemName() + "    ";
        switch (_castle.Selected_Item_Class.item_type)
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
        if (_castle.Selected_Item.identified)
        {
            _txt += "\n Value: " + _castle.Selected_Item_Class.price + "g\n";
            if (_castle.Selected_Item_Class.damage.num > 0)
            {
                _txt += "\n";
                if (_castle.Selected_Item_Class.hit_mod < 0) _txt += "(ToHit " + _castle.Selected_Item_Class.hit_mod + ") ";
                if (_castle.Selected_Item_Class.hit_mod >= 0) _txt += "(ToHit +" + _castle.Selected_Item_Class.hit_mod + ") ";
                _txt += "Damage = " + _castle.Selected_Item_Class.damage.num + "d" + _castle.Selected_Item_Class.damage.sides;
                if (_castle.Selected_Item_Class.damage.bonus < 0) _txt += _castle.Selected_Item_Class.damage.bonus;
                if (_castle.Selected_Item_Class.damage.bonus > 0) _txt += "+" + _castle.Selected_Item_Class.damage.bonus;
                if (_castle.Selected_Item_Class.xtra_swings > 1) _txt += "x" + _castle.Selected_Item_Class.xtra_swings;
                _txt += "\n";
            }
            if (_castle.Selected_Item_Class.armor_mod > 0) _txt += "\n Armor: " + _castle.Selected_Item_Class.armor_mod + "\n";
            if (_castle.Selected_Item_Class.spell != "") _txt += "\n When invoked, casts: " + _castle.Selected_Item_Class.spell + ".\n";
            if (_castle.Selected_Item_Class.item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
            if (_castle.Selected_Item_Class.item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
            if (_castle.Selected_Item_Class.item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
            if (_castle.Selected_Item_Class.item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
            if (_castle.Selected_Item_Class.class_use.Contains("z")) _txt += "<None>\n";
            if (_castle.Selected_Item_Class.class_use.Contains("f")) _txt += "Fighter\n";
            if (_castle.Selected_Item_Class.class_use.Contains("m")) _txt += "Mage\n";
            if (_castle.Selected_Item_Class.class_use.Contains("p")) _txt += "Priest\n";
            if (_castle.Selected_Item_Class.class_use.Contains("t")) _txt += "Thief\n";
            if (_castle.Selected_Item_Class.class_use.Contains("b")) _txt += "Bishop\n";
            if (_castle.Selected_Item_Class.class_use.Contains("s")) _txt += "Samurai\n";
            if (_castle.Selected_Item_Class.class_use.Contains("l")) _txt += "Lord\n";
            if (_castle.Selected_Item_Class.class_use.Contains("n")) _txt += "Ninja\n";

            if (_castle.Selected_Item.equipped) _txt += "\nThis item is equipped.\n";
            if (!_castle.Selected_Item.equipped)
            {
                string _s = "";
                if (_castle.Selected_Item_Class.item_type == BlobberEngine.Enum._Item_Type.Weapon && _castle.Selected_Character.eqWeapon > -1) _s = _castle.Selected_Character.Inventory[_castle.Selected_Character.eqWeapon].ItemName();
                if (_castle.Selected_Item_Class.item_type == BlobberEngine.Enum._Item_Type.Armor && _castle.Selected_Character.eqArmor > -1) _s = _castle.Selected_Character.Inventory[_castle.Selected_Character.eqArmor].ItemName();
                if (_castle.Selected_Item_Class.item_type == BlobberEngine.Enum._Item_Type.Shield && _castle.Selected_Character.eqShield > -1) _s = _castle.Selected_Character.Inventory[_castle.Selected_Character.eqShield].ItemName();
                if (_castle.Selected_Item_Class.item_type == BlobberEngine.Enum._Item_Type.Helmet && _castle.Selected_Character.eqHelmet > -1) _s = _castle.Selected_Character.Inventory[_castle.Selected_Character.eqHelmet].ItemName();
                if (_castle.Selected_Item_Class.item_type == BlobberEngine.Enum._Item_Type.Gauntlets && _castle.Selected_Character.eqGauntlet > -1) _s = _castle.Selected_Character.Inventory[_castle.Selected_Character.eqGauntlet].ItemName();
                if (_s != "") _txt += "\n" + _s + " is equipped to this slot.";
            }
        }
        else
        {
            _txt += "\n Identify this item to learn more about it.\n\n\n";
        }
        main.text = _txt;
        ClearButtons();
        trade_button.SetActive(true);
        drop_button.SetActive(true);
        done1_button.SetActive(true);
    }
    public void ShowMagic()
    {
        this.gameObject.SetActive(true);
        ClearButtons();
        string _txt = _castle.Selected_Character.name + " Knows thse spells:\n----------------------------------------\n";

        _txt += "\n[[[Mage Spells]]]\n";
        if (_castle.Selected_Character.mageSpells[0] > 0)
        {
            for (int i = 29; i < 50; i++)
            {
                if (_castle.Selected_Character.SpellKnown[i]) _txt += GameManager.SPELL[i].name + " (" + GameManager.SPELL[i].word + "), ";
            }
        }
        else
        {
            _txt += "--< None >--\n";
        }
        _txt += "\n\n[[[Priest Spells]]]\n";
        if (_castle.Selected_Character.priestSpells[0] > 0)
        {
            
            for (int i = 0; i < 29; i++)
            {
                if (_castle.Selected_Character.SpellKnown[i]) _txt += GameManager.SPELL[i].name + " (" + GameManager.SPELL[i].word + "), ";
            }
        }
        else
        {
            _txt += "--< None >--\n";
        }


        main.text = _txt;
        done3_button.SetActive(true);
    }
    public void ChoosePartyMember()
    {
        this.gameObject.SetActive(true);
        ClearButtons();        
        main.text = "Trade to which character?";
        done2_button.SetActive(true);
        for (int i = 0; i < 6; i++)
            if(!GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i) != _castle.Selected_Character)
            {
                charSlot_btn_title[i].text = GameManager.PARTY.LookUp_PartyMember(i).name;
                charSlot_button[i].SetActive(true);
            }
    }
    public void HowMuchGeld()
    {
        ClearButtons();
        main.text = _castle.Selected_Character.name + " has " + _castle.Selected_Character.Geld + "g.\n\nHow much to trade?";
        _InputField.gameObject.SetActive(true);
        _InputField.text = "";
    }
    public void ThisMuchGeld(string _s)
    {
        int _n = 0;  int.TryParse(_s, out _n);
        if (_n < 0) _n = 0;
        if (_n > _castle.Selected_Character.Geld) _n = _castle.Selected_Character.Geld;
        _castle.Selected_Character.Geld -= _n;
        _castle.Other_Character.Geld += _n;
        this.gameObject.SetActive(false);
        _castle.RefreshCharacterSheet();
        return;
    }
}



