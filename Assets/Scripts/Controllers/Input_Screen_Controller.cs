using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Screen_Controller : MonoBehaviour
{
    //General buttons
    public GameObject Leave_button, Name_button;

    //Market buttons
    public GameObject Advntr_Inn_button, Glgmsh_Tavern_button, Bltc_TP_button, Tmpl_CANT_button, Edge_of_Town_button;

    //Inn buttons
    public GameObject Stables_button, Cot_button, EconR_button, MerchS_button, RoyalS_button;


    public Character_Class _selected_character;
    public int _selectedRoster;


    private Castle_Logic _castle;

    private void Start()
    {
        _castle = FindObjectOfType<Castle_Logic>();
        _selected_character = null;
        _selectedRoster = -1;
    }

    public void Clear_Buttons()
    {
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Button")) _go.SetActive(false);
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Temp_Button")) Destroy(_go);        
    }

    public void Enable_Button (GameObject _go)
    {
        _go.SetActive(true);
    }
    public void Enable_Button_First (GameObject _go)
    {
        _go.SetActive(true);
        _go.transform.SetAsFirstSibling();
    }
    public void Enable_Button_Last (GameObject _go)
    {
        _go.SetActive(true);
        _go.transform.SetAsLastSibling();
    }




    public void Button_Clicked(string _button)
    {
        Debug.Log("Button Clicked! Received input: " + _button);

        //<<<<<<<<<<   MARKET   >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.Market)
        {
            if (_button == "Inn_Button")
            {
                _castle.townStatus = Castle_Logic.ts.Inn_Intro;
                _castle.Update_Screen();
                return;
            }
        }
        //<<<<<<<<<<   INN   >>>>>>>>>>>>>>>>>>>>>>>
        if(_castle.townStatus == Castle_Logic.ts.Inn_Intro)
        {
            if(_button == "Leave_Button")
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
                if (_delta > 0)
                {
                    _display.PopUpMessage(_selected_character.name + " needs " + _delta + " more XP to make the next level.");
                }
                else
                {
                    Game_Logic.instance.LevelUpCharacter(_selectedRoster);
                }
            }
        }
    }
}
