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
        for (int i = 0; i < Slot_btn.Length; i++)
            Slot_btn[i].fontSize = _font;
        _selected_int = -1;
        _page = 0;
        Cursor.SetActive(false);
        UpdateScreen();
    }

    public void UpdateScreen()
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
                if (GameManager.ITEM[i].class_use.Contains(_classBit)) _str += " UNUSABLE";

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
                int c = (_page * Slot_btn.Length) + _n;
                
            }
        }

        if(_n == 100)
        {
            Cursor.SetActive(false);
            if (_page > 0) _page--;
            UpdateScreen();
        }
        if(_n == 200)
        {
            Cursor.SetActive(false);
            if ((_page + 1) * Slot_btn.Length <= _stock_count) _page++;
            UpdateScreen();
        }
    }
}
