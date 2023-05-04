using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle_Trade_Panel_Manager : MonoBehaviour
{
    public TextMeshProUGUI Title_Message, Left_btn, Right_btn, View_btn, Buy_btn, Cancel_btn, View_Item, vi_Done_btn;
    public TextMeshProUGUI[] Slot_btn;
    public Transform Slot_Root;
    public GameObject Cursor, View_Item_Panel, Left_Button, Right_Button, View_button;
    public Castle_Pop_Up_Manager PopUp_Panel;

    private int _selected_int = -1, _page = 0, _stock_count = 0;
    private string[] _slot;
    private Castle_Logic_Manager _castle;
    private List<Item_Class> _STOCK_ = new List<Item_Class>();
    private List<int> SickRoster_index = new List<int>();
    private List<int> SickRoster_price = new List<int>();
    private List<string> _SLOT_ = new List<string>();
    private string _mode;

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
        _mode = "buy";        
        this.gameObject.SetActive(true);
        UpdateBuyScreen();
    }

    public void SellScreen()
    {
        _mode = "sell";
        this.gameObject.SetActive(true);
        UpdateSellScreen();
    }

    public void UncurseScreen()
    {
        _mode = "uncurse";
        this.gameObject.SetActive(true);
        UpdateUncurseScreen();
    }

    public void IdentifyScreen()
    {
        _mode = "identify";
        this.gameObject.SetActive(true);
        UpdateIdentifyScreen();
    }

    public void HealScreen()
    {
        _mode = "heal";
        this.gameObject.SetActive(true);
        UpdateHealScreen();
    }

    private void UpdateBuyScreen()
    {
        Buy_btn.text = "Buy";
        Left_Button.SetActive(true);
        Right_Button.SetActive(true);
        View_button.SetActive(true);
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
        Buy_btn.text = "Sell";
        Left_Button.SetActive(false);
        Right_Button.SetActive(false);
        View_button.SetActive(true);
        Title_Message.text = "Boltac nods. \"What would ya like to sell?\"";
        
        //clear screen
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].text = "";

        //Sell stock
        for (int i = 0; i < 8; i++)
        {
            if (_castle.Selected_Character.Inventory[i].index > -1)
            {
                int _price = (int)GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].price / 2;
                string _str = "",
                       _alpha = " " + _castle.Selected_Character.Inventory[i].ItemName() + " ",
                       _delta = "",
                       _omega = " " + _price + " G",
                       _classBit = _castle.Selected_Character.character_class.ToString().Substring(0, 1).ToLower();
                while (_alpha.Length + _delta.Length + _omega.Length < 26)
                    _delta += ".";
                _str = _alpha + _delta + _omega;

                //Modify item name based on class restriction
                if (!GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].class_use.Contains(_classBit)) _str += " UNUSABLE";
                Slot_btn[i].text = _str;
            }
            else
            {
                Slot_btn[i].text = "";
            }
        }
    }

    private void UpdateUncurseScreen()
    {
        Buy_btn.text = "Uncurse";
        Left_Button.SetActive(false);
        Right_Button.SetActive(false);
        View_button.SetActive(false);
        Title_Message.text = "Boltac grimaces. \"That is a nasty curse alright.\"";

        //clear screen
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].text = "";

        //Uncurse stock
        for (int i = 0; i < 8; i++)
        {
            if (_castle.Selected_Character.Inventory[i].index > -1 && _castle.Selected_Character.Inventory[i].curse_active)
            {
                int _price = (int)GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].price / 2;
                string _str = "",
                       _alpha = " " + _castle.Selected_Character.Inventory[i].ItemName() + " ",
                       _delta = "",
                       _omega = " " + _price + " G",
                       _classBit = _castle.Selected_Character.character_class.ToString().Substring(0, 1).ToLower();
                while (_alpha.Length + _delta.Length + _omega.Length < 26)
                    _delta += ".";
                _str = _alpha + _delta + _omega;

                //Modify item name based on class restriction
                if (!GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].class_use.Contains(_classBit)) _str += " UNUSABLE";
                Slot_btn[i].text = _str;
            }
            else
            {
                Slot_btn[i].text = "";
            }
        }
    }

    private void UpdateIdentifyScreen()
    {
        Buy_btn.text = "Identify";
        Left_Button.SetActive(false);
        Right_Button.SetActive(false);
        View_button.SetActive(false);
        Title_Message.text = "Boltac rubs his hands. \"What do you want to know more about?\"";

        //clear screen
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].text = "";

        //Unidentified stock
        for (int i = 0; i < 8; i++)
        {
            if (_castle.Selected_Character.Inventory[i].index > -1 && !_castle.Selected_Character.Inventory[i].identified)
            {
                int _price = (int)GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].price / 2;
                string _str = "",
                       _alpha = " " + _castle.Selected_Character.Inventory[i].ItemName() + " ",
                       _delta = "",
                       _omega = " " + _price + " G",
                       _classBit = _castle.Selected_Character.character_class.ToString().Substring(0, 1).ToLower();
                while (_alpha.Length + _delta.Length + _omega.Length < 26)
                    _delta += ".";
                _str = _alpha + _delta + _omega;

                //Modify item name based on class restriction
                if (!GameManager.ITEM[_castle.Selected_Character.Inventory[i].index].class_use.Contains(_classBit)) _str += " UNUSABLE";
                Slot_btn[i].text = _str;
            }
            else
            {
                Slot_btn[i].text = "";
            }
        }

    }

    private void UpdateHealScreen()
    {
        Buy_btn.text = "Heal";
        Left_Button.SetActive(true);
        Right_Button.SetActive(true);
        View_button.SetActive(false);
        //Display the number of resources
        Title_Message.text = "The party has " + GameManager.PARTY.Temple_Favor + " favor.";

        //clear screen
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].text = "";

        //Stock the SickRoster_index
        SickRoster_index.Clear();
        SickRoster_price.Clear();
        _SLOT_.Clear();
        for (int i = 0; i < GameManager.ROSTER.Count; i++)
            if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.plyze || GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.stoned ||
                GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.dead || GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.ashes)
            {                
                //determine price of treatment
                int price = 0;
                if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.plyze) 
                    price = 100 * GameManager.ROSTER[i].level;
                if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.stoned) 
                    price = 200 * GameManager.ROSTER[i].level;
                if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.dead) 
                    price = 250 * GameManager.ROSTER[i].level;
                if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.ashes) 
                    price = 500 * GameManager.ROSTER[i].level;

                //build string
                string _str = "",
                       _alpha = " " + GameManager.ROSTER[i].name + " ",
                       _delta = "",
                       _omega = " " + price + " G";
                while (_alpha.Length + _delta.Length + _omega.Length < 35)
                    _delta += ".";
                _str = _alpha + _delta + _omega;

                SickRoster_index.Add(i);
                SickRoster_price.Add(price);
                _SLOT_.Add(_str);
            }
        _stock_count = SickRoster_index.Count;

        //Choose which _STOCK_ to show on this page
        for (int i = 0; i < Slot_btn.Length; i++)
        {
            int c = (_page * Slot_btn.Length) + i;
            if (c < _stock_count)
                Slot_btn[i].text = _SLOT_[c];
        }

    }

    public void ButtonPressed(int _n)
    {
        for (int i = 0; i < Slot_btn.Length; i++)
        {
            if(_n == i)
            {
                if (_mode == "buy" && (_page * Slot_btn.Length) + _n < _stock_count)
                {
                    Cursor.SetActive(true);
                    Cursor.transform.SetParent(Slot_btn[i].transform);
                    Cursor.transform.localPosition = Vector3.zero;
                    Cursor.transform.localScale = Vector3.one;
                    _selected_int = (_page * Slot_btn.Length) + _n;
                    //Debug.Log(_thisItem.name + ", NOT " + GameManager.ITEM[_selected_int].name);
                    return;
                }
                if (_mode == "sell" || _mode == "uncurse" || _mode == "identify")
                    if(_n < 8 && _castle.Selected_Character.Inventory[_n].index > -1)
                    {
                        Cursor.SetActive(true);
                        Cursor.transform.SetParent(Slot_btn[i].transform);
                        Cursor.transform.localPosition = Vector3.zero;
                        Cursor.transform.localScale = Vector3.one;
                        _selected_int = _n;
                        Debug.Log("This item is " + _castle.Selected_Character.Inventory[_n].ItemName());
                        return;
                    }
                if (_mode == "heal" && (_page * Slot_btn.Length) + _n < _stock_count)
                {
                    Cursor.SetActive(true);
                    Cursor.transform.SetParent(Slot_btn[i].transform);
                    Cursor.transform.localPosition = Vector3.zero;
                    Cursor.transform.localScale = Vector3.one;
                    _selected_int = (_page * Slot_btn.Length) + _n;                    
                    return;
                }
            }
        }
        //Left Button
        if (_n == 100)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            if (_page > 0) _page--;
            if (_mode == "buy") UpdateBuyScreen();
            if (_mode == "heal") UpdateHealScreen();
            return;
        }
        //Right Button
        if (_n == 200)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            if ((_page + 1) * Slot_btn.Length <= _stock_count) _page++;
            if (_mode == "buy") UpdateBuyScreen();
            if (_mode == "heal") UpdateHealScreen();
            return;
        }
        //View Button
        if (_n == 300)
        {
            if (_selected_int == -1)
            {
                PopUp_Panel.Show_Message("Boltac looks confused. \"Huh? Examine what? You gotta pick something first!\"");
                Cursor.SetActive(false);
                if(_mode == "buy") UpdateBuyScreen();
                if(_mode == "sell") UpdateSellScreen();
                return;
            }

            bool _id = false;
            Item_Class _thisItem = null;
            if (_mode == "buy")
            {
                _id = true;
                _thisItem = _STOCK_[_selected_int];
            }
            else
            {
                _thisItem = GameManager.ITEM[_castle.Selected_Character.Inventory[_selected_int].index];
                if (_castle.Selected_Character.Inventory[_selected_int].identified) _id = true;
            }

            View_Item_Panel.SetActive(true);
            string _txt = "";
            if(_id) _txt = _thisItem.name + "    \n";
            if(!_id) _txt = _thisItem.name_unk + "    \n";

            switch (_thisItem.item_type)
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
            if (_id)
            {
                _txt += "\n Value: " + _thisItem.price + "g\n";
                if (_thisItem.damage.num > 0)
                {
                    _txt += "\n";
                    if (_thisItem.hit_mod < 0) _txt += "(ToHit " + _thisItem.hit_mod + ") ";
                    if (_thisItem.hit_mod >= 0) _txt += "(ToHit +" + _thisItem.hit_mod + ") ";
                    _txt += "Damage = " + _thisItem.damage.num + "d" + _thisItem.damage.sides;
                    if (_thisItem.damage.bonus < 0) _txt += _thisItem.damage.bonus;
                    if (_thisItem.damage.bonus > 0) _txt += "+" + _thisItem.damage.bonus;
                    if (_thisItem.xtra_swings > 1) _txt += "x" + _thisItem.xtra_swings;
                    _txt += "\n";
                }
                if (_thisItem.armor_mod > 0) _txt += "\n Armor: " + _thisItem.armor_mod + "\n";
                if (_thisItem.spell != "") _txt += "\n When invoked, casts: " + _thisItem.spell + ".\n";
                if (_thisItem.item_align == BlobberEngine.Enum._Alignment.none) _txt += "\nUsable by: \n";
                if (_thisItem.item_align == BlobberEngine.Enum._Alignment.good) _txt += "\nUsable by: (Good Only)\n";
                if (_thisItem.item_align == BlobberEngine.Enum._Alignment.neutral) _txt += "\nUsable by: (Neutral Only)\n";
                if (_thisItem.item_align == BlobberEngine.Enum._Alignment.evil) _txt += "\nUsable by: (Evil Only)\n";
                if (_thisItem.class_use.Contains("z")) _txt += "<None>\n";
                if (_thisItem.class_use.Contains("f")) _txt += "Fighter\n";
                if (_thisItem.class_use.Contains("m")) _txt += "Mage\n";
                if (_thisItem.class_use.Contains("p")) _txt += "Priest\n";
                if (_thisItem.class_use.Contains("t")) _txt += "Thief\n";
                if (_thisItem.class_use.Contains("b")) _txt += "Bishop\n";
                if (_thisItem.class_use.Contains("s")) _txt += "Samurai\n";
                if (_thisItem.class_use.Contains("l")) _txt += "Lord\n";
                if (_thisItem.class_use.Contains("n")) _txt += "Ninja\n";
            }
            else 
            {
                _txt += "\nThis item is not identified. Identify it to learn more about it.";
            }

            View_Item.text = _txt;
            return;
        }
        //Action Button
        if (_n == 400)
        {
            if (_selected_int == -1)
            {
                PopUp_Panel.Show_Message("Boltac looks confused. \"Buy what? You gotta pick something first!\"");
                Cursor.SetActive(false);
                if (_mode == "buy") UpdateBuyScreen();
                if (_mode == "sell") UpdateSellScreen();
                return;
            }
            if (_mode == "buy")
            {
                if (_castle.Selected_Character.Geld < _STOCK_[_selected_int].price)
                {
                    PopUp_Panel.Show_Message("Boltac reproaches you. \"You don't have enough money!\"");
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    UpdateBuyScreen();
                    return;
                }
                else
                {
                    _castle.Selected_Character.Geld -= (int)_STOCK_[_selected_int].price;
                    _castle.Selected_Character.Inventory[_castle.Selected_Character.GetEmptyInventorySlot()] = new Item(_STOCK_[_selected_int].index, false, false, true);
                    if (GameManager.PARTY.BoltacStock[_STOCK_[_selected_int].index] > 0) GameManager.PARTY.BoltacStock[_STOCK_[_selected_int].index]--;
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    this.gameObject.SetActive(false);
                    _castle.UpdateScreen();
                    return;
                }
            }
            if (_mode == "sell")
            {
                if (_castle.Selected_Character.Inventory[_selected_int].curse_active)
                {
                    PopUp_Panel.Show_Message("Boltac looks angry. \"Are you trying sell me a cursed item!?\"");
                    Cursor.SetActive(false);
                    UpdateSellScreen();
                    _selected_int = -1;
                    Cursor.SetActive(false);
                    return;
                }
                if (_castle.Selected_Character.Inventory[_selected_int].equipped)
                    _castle.Selected_Character.UnequipItem(_selected_int);
                _castle.Selected_Character.Geld += (int)GameManager.ITEM[_castle.Selected_Character.Inventory[_selected_int].index].price / 2;
                int _index = _castle.Selected_Character.Inventory[_selected_int].index;
                if (GameManager.PARTY.BoltacStock[_index] > -1) GameManager.PARTY.BoltacStock[_index]++;
                _castle.Selected_Character.Inventory[_selected_int] = new Item();
                Cursor.SetActive(false);
                _selected_int = -1;
                this.gameObject.SetActive(false);
                _castle.UpdateScreen();
                return;
            }
            if (_mode == "uncurse")
            {
                int _price = (int)GameManager.ITEM[_castle.Selected_Character.Inventory[_selected_int].index].price / 2;
                if (_castle.Selected_Character.Geld < _price)
                {
                    PopUp_Panel.Show_Message("Boltac reproaches you. \"You don't have enough money!\"");
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    UpdateUncurseScreen();
                    return;
                }
                else
                {
                    if (_castle.Selected_Character.Inventory[_selected_int].equipped)
                        _castle.Selected_Character.UnequipItem(_selected_int);
                    _castle.Selected_Character.Geld -= _price;
                    _castle.Selected_Character.Inventory[_selected_int] = new Item();
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    this.gameObject.SetActive(false);
                    _castle.UpdateScreen();
                    return;
                }
            }
            if (_mode == "identify")
            {
                int _price = (int)GameManager.ITEM[_castle.Selected_Character.Inventory[_selected_int].index].price / 2;
                if (_castle.Selected_Character.Geld < _price)
                {
                    PopUp_Panel.Show_Message("Boltac reproaches you. \"You don't have enough money!\"");
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    UpdateUncurseScreen();
                    return;
                }
                else
                {
                    _castle.Selected_Character.Geld -= _price;
                    _castle.Selected_Character.Inventory[_selected_int].identified = true;
                    ButtonPressed(300);
                    return;
                }
            }
            if (_mode == "heal")
            {
                //Not enough favor
                if(GameManager.PARTY.Temple_Favor < SickRoster_price[_selected_int])
                {
                    PopUp_Panel.Show_Message("The priest is apologetic, but firm.\n\"I am sorry, you do not have enough favor to aid this adventurer. " +
                        "However, our god rewards those who are diligent in their devotions! return later!\"");
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    UpdateHealScreen();
                    return;
                }
                GameManager.PARTY.Temple_Favor -= SickRoster_price[_selected_int];
                if (GameManager.ROSTER[SickRoster_index[_selected_int]].status == BlobberEngine.Enum._Status.plyze ||
                    GameManager.ROSTER[SickRoster_index[_selected_int]].status == BlobberEngine.Enum._Status.stoned)
                {
                    GameManager.ROSTER[SickRoster_index[_selected_int]].status = BlobberEngine.Enum._Status.OK;
                    PopUp_Panel.Show_Message(GameManager.ROSTER[SickRoster_index[_selected_int]].name + " is healed!!!");
                    Cursor.SetActive(false);
                    _selected_int = -1;
                    this.gameObject.SetActive(false);
                    _castle.UpdateScreen();
                    return;
                }
                if (GameManager.ROSTER[SickRoster_index[_selected_int]].status == BlobberEngine.Enum._Status.dead)
                {
                    float _chance = (GameManager.ROSTER[SickRoster_index[_selected_int]].Vitality * 3) + 50;
                    float _roll = Random.Range(0f, 100f) + 1;
                    Debug.Log("Chance is " + _chance + ", Roll is " + _roll);
                    if (_roll <= _chance)
                    {
                        GameManager.ROSTER[SickRoster_index[_selected_int]].status = BlobberEngine.Enum._Status.OK;
                        PopUp_Panel.Show_Message(GameManager.ROSTER[SickRoster_index[_selected_int]].name + " is restored to life!!!");
                        Cursor.SetActive(false);
                        _selected_int = -1;
                        this.gameObject.SetActive(false);
                        _castle.UpdateScreen();
                        return;
                    }
                    else
                    {
                        GameManager.ROSTER[SickRoster_index[_selected_int]].status = BlobberEngine.Enum._Status.ashes;
                        PopUp_Panel.Show_Message("OH NO!" + GameManager.ROSTER[SickRoster_index[_selected_int]].name + " is reduced to ashes!!!");
                        Cursor.SetActive(false);
                        _selected_int = -1;
                        UpdateHealScreen();
                        return;
                    }
                }
                if (GameManager.ROSTER[SickRoster_index[_selected_int]].status == BlobberEngine.Enum._Status.ashes)
                {
                    float _chance = (GameManager.ROSTER[SickRoster_index[_selected_int]].Vitality * 3) + 40;
                    float _roll = Random.Range(0f, 100f) + 1;
                    Debug.Log("Chance is " + _chance + ", Roll is " + _roll);

                    if (_roll <= _chance)
                    {
                        GameManager.ROSTER[SickRoster_index[_selected_int]].status = BlobberEngine.Enum._Status.OK;
                        PopUp_Panel.Show_Message(GameManager.ROSTER[SickRoster_index[_selected_int]].name + " is restored from ashes!!!");
                        Cursor.SetActive(false);
                        _selected_int = -1;
                        this.gameObject.SetActive(false);
                        _castle.UpdateScreen();
                        return;
                    }
                    else
                    {
                        GameManager.ROSTER[SickRoster_index[_selected_int]].status = BlobberEngine.Enum._Status.ashes;
                        PopUp_Panel.Show_Message("OH NO!" + GameManager.ROSTER[SickRoster_index[_selected_int]].name + " failed to be restored!!!");
                        Cursor.SetActive(false);
                        _selected_int = -1;
                        UpdateHealScreen();
                        return;
                    }
                }
            }
        }
        //Done button
        if (_n == 500)
        {
            Cursor.SetActive(false);
            _selected_int = -1;
            this.gameObject.SetActive(false);
            _castle.UpdateScreen();
            return;
        }
        //View_panel Done button
        if (_n == 999)
        {
            View_Item_Panel.SetActive(false);
            if(_mode == "identify")
            {
                Cursor.SetActive(false);
                _selected_int = -1;
                this.gameObject.SetActive(false);
                _castle.UpdateScreen();
            }
            return;
        }
    }
}
