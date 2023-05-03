using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle_Trade_Panel_Manager : MonoBehaviour
{
    public TextMeshProUGUI Title_Message, Left_btn, Right_btn, View_btn, Buy_btn, Cancel_btn;
    public TextMeshProUGUI[] Slot_btn;
    public Transform Slot_Root;
    public GameObject Cursor;

    private int _selected_int = -1, _page = 0;
    private Castle_Logic_Manager _castle;

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
        for (int i = 0; i < Slot_btn.Length; i++)
        {
            Slot_btn[i].fontSize = _font;
            Slot_btn[i].text = "";
        }
        _selected_int = -1;
        _page = 0;
        Cursor.SetActive(false);
        UpdateScreen();
    }

    public void UpdateScreen()
    {
        //Display the number of resources
        Title_Message.text = _castle.Selected_Character.name + " has " + _castle.Selected_Character.Geld + "g.";
        
        //Stock the _STOCK_
        List<Item_Class> _STOCK_ = new List<Item_Class>();
        List<string> _SLOT_ = new List<string>();
        for (int i = 0; i < GameManager.ITEM.Count; i++)
            if (GameManager.PARTY.BoltacStock[i] == -1 || GameManager.PARTY.BoltacStock[i] > 0)
            {
                string _str = "",
                       _alpha =  " " + GameManager.ITEM[i].name + " ",
                       _delta = "",
                       _omega = " " + GameManager.ITEM[i].price + " G";
                while (_alpha.Length + _delta.Length + _omega.Length < 35)
                    _delta += "-";
                _STOCK_.Add(GameManager.ITEM[i]);
                _SLOT_.Add(_str);
            }

        //Choose which _STOCK_ to show on this page
        for (int i = 0; i < Slot_btn.Length; i++)
            if ((_page * Slot_btn.Length) + i < _STOCK_.Count)
                Slot_btn[i].text = " " + _STOCK_[i].name + " ----- " + _STOCK_[i].price + " G";
    }

    public void ButtonPressed(int _n)
    {

    }
}
