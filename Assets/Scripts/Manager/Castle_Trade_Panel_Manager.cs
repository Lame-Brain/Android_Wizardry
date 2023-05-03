using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle_Trade_Panel_Manager : MonoBehaviour
{
    public TextMeshProUGUI Title_Message, Left_btn, Right_btn, View_btn, Buy_btn, Cancel_btn, View_Item, vi_Done_btn;
    public TextMeshProUGUI[] Slot_btn;
    public Transform Slot_Root;
    public GameObject Cursor, View_Item_Panel;
    public Castle_Pop_Up_Manager PopUp_Panel;

    private int _selected_int = -1, _page = 0, _stock_count = 0;
    private string[] _slot;
    private Castle_Logic_Manager _castle;
    private List<Item_Class> _STOCK_ = new List<Item_Class>();
    private List<string> _SLOT_ = new List<string>();

    private void OnEnable()
    {
        _castle = FindObjectOfType<Castle_Logic_Manager>();
        float _font = GameManager.FONT;
        Title_Message.fontSize = _font;
        Left_btn.fontSize = _font;
        Right_btn.fontSize = _font;
        View_btn.fontSize = _font;
        Buy_btn.fontSize = _font;
        Cancel_btn.fontSize = _font;
        View_Item.fontSize = _font;
        vi_Done_btn.fontSize = _font;
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].fontSize = _font;
        _selected_int = -1;
        _page = 0;
        Cursor.SetActive(false);
    }

    public void BuyScreen()
    {
        this.gameObject.SetActive(true);
        UpdateBuyScreen();
    }

    public void SellScreen()
    {
        this.gameObject.SetActive(true);
        UpdateSellScreen();
    }

    private void UpdateBuyScreen()
    {
        //Display the number of resources
        Title_Message.text = _castle.Selected_Character.name + " has " + _castle.Selected_Character.Geld + "g.";

        //clear screen
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].text = "";

        //Stock the _STOCK_
        _STOCK_.Clear();
        _SLOT_.Clear();
        for (int i = 0; i < GameManager.ITEM.Count; i++)
            if (GameManager.PARTY.BoltacStock[i] == -1 || GameManager.PARTY.BoltacStock[i] > 0)
            {
                string _str = "",
                       _alpha =  " " + GameManager.ITEM[i].name + " ",
                       _delta = "",
                       _omega = " " + GameManager.ITEM[i].price + " G",
                       _classBit = _castle.Selected_Character.character_class.ToString().Substring(0,1).ToLower();                
                while (_alpha.Length + _delta.Length + _omega.Length < 26) 
                    _delta += ".";
                _str = _alpha + _delta + _omega;

                //Modify item name based on class restriction
                if (!GameManager.ITEM[i].class_use.Contains(_classBit)) _str += " UNUSABLE";

                _STOCK_.Add(GameManager.ITEM[i]);
                _SLOT_.Add(_str);
            }
        _stock_count = _STOCK_.Count;

        //Choose which _STOCK_ to show on this page
        for (int i = 0; i < Slot_btn.Length; i++) 
        {
            int c = (_page * Slot_btn.Length) + i;
            if (c < _stock_count)
                Slot_btn[i].text = _SLOT_[c]; 
        }
    }

    private void UpdateSellScreen()
    {

    }

    public void ButtonPressed(int _n)
    {
        for (int i = 0; i < Slot_btn.Length; i++)
        {
            if(_n == i)
            {
                Cursor.SetActive(true);
                Cursor.transform.SetParent(Slot_btn[i].transform);
                Cursor.transform.localPosition = Vector3.zero;
                Cursor.transform.localScale = Vector3.one;
                _selected_int = (_page * Slot_btn.Length) + _n;
                //Debug.Log(_STOCK_[_selected_int].name + ", NOT " + GameManager.ITEM[_selected_int].name);
                return;
            }
        }
        if (_n == 100)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            if (_page > 0) _page--;
            UpdateBuyScreen();
            return;
        }
        if (_n == 200)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            if ((_page + 1) * Slot_btn.Length <= _stock_count) _page++;
            UpdateBuyScreen();
            return;
        }
        if (_n == 300)
        {
            if (_selected_int == -1)
            {
                PopUp_Panel.Show_Message("Examine what? You gotta pick something first!");
                Cursor.SetActive(false);
                UpdateBuyScreen();
                return;
            }

            View_Item_Panel.SetActive(true);
            string _txt = _STOCK_[_selected_int].name + "    \n";
            switch (_STOCK_[_selected_int].item_type)
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
            _txt += "\n Value: " + _STOCK_[_selected_int].price + "g\n";
            if (_STOCK_[_selected_int].damage.num > 0)
            {
                _txt += "\n";
                if (_STOCK_[_selected_int].hit_mod < 0) _txt += "(ToHit " + _STOCK_[_selected_int].hit_mod + ") ";
                if (_STOCK_[_selected_int].hit_mod >= 0) _txt += "(ToHit +" + _STOCK_[_selected_int].hit_mod + ") ";
                _txt += "Damage = " + _STOCK_[_selected_int].damage.num + "d" + _STOCK_[_selected_int].damage.sides;
                if (_STOCK_[_selected_int].damage.bonus < 0) _txt += _STOCK_[_selected_int].damage.bonus;
                if (_STOCK_[_selected_int].damage.bonus > 0) _txt += "+" + _STOCK_[_selected_int].damage.bonus;
                if (_STOCK_[_selected_int].xtra_swings > 1) _txt += "x" + _STOCK_[_selected_int].xtra_swings;
                _txt += "\n";
            }
            if (_STOCK_[_selected_int].armor_mod > 0) _txt += "\n Armor: " + _STOCK_[_selected_int].armor_mod + "\n";
            if (_STOCK_[_selected_int].spell != "") _txt += "\n When invoked, casts: " + _STOCK_[_selected_int].spell + ".\n";
            if (_STOCK_[_selected_int].item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
            if (_STOCK_[_selected_int].item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
            if (_STOCK_[_selected_int].item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
            if (_STOCK_[_selected_int].item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
            if (_STOCK_[_selected_int].class_use.Contains("z")) _txt += "<None>\n";
            if (_STOCK_[_selected_int].class_use.Contains("f")) _txt += "Fighter\n";
            if (_STOCK_[_selected_int].class_use.Contains("m")) _txt += "Mage\n";
            if (_STOCK_[_selected_int].class_use.Contains("p")) _txt += "Priest\n";
            if (_STOCK_[_selected_int].class_use.Contains("t")) _txt += "Thief\n";
            if (_STOCK_[_selected_int].class_use.Contains("b")) _txt += "Bishop\n";
            if (_STOCK_[_selected_int].class_use.Contains("s")) _txt += "Samurai\n";
            if (_STOCK_[_selected_int].class_use.Contains("l")) _txt += "Lord\n";
            if (_STOCK_[_selected_int].class_use.Contains("n")) _txt += "Ninja\n";

            View_Item.text = _txt;
            return;
        }
        if (_n == 400)
        {
            if(_selected_int == -1)
            {
                PopUp_Panel.Show_Message("Buy what? You gotta pick something first!");
                Cursor.SetActive(false);
                UpdateBuyScreen();
                return;
            }
            if (_castle.Selected_Character.Geld < _STOCK_[_selected_int].price)
            {
                PopUp_Panel.Show_Message("You do not have enough money!");
                Cursor.SetActive(false);
                _selected_int = -1;
                UpdateBuyScreen();
                return;
            }
            else
            {
                _castle.Selected_Character.Geld -= (int)_STOCK_[_selected_int].price;
                _castle.Selected_Character.Inventory[_castle.Selected_Character.GetEmptyInventorySlot()] = new Item(_STOCK_[_selected_int].index,false, false, true);
                Cursor.SetActive(false);
                _selected_int = -1;
                this.gameObject.SetActive(false);
                _castle.UpdateScreen();
                return;
            }
        }
        if (_n == 500)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            this.gameObject.SetActive(false);
            _castle.UpdateScreen();
            return;
        }
        if (_n == 999)
        {
            View_Item_Panel.SetActive(false);
            return;
        }
    }
}
