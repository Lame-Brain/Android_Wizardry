using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Input_Screen_Controller : MonoBehaviour
{
    //General buttons
    public GameObject Leave_button, Name_button;

    //Market buttons
    public GameObject Advntr_Inn_button, Glgmsh_Tavern_button, Bltc_TP_button, Tmpl_CANT_button, Edge_of_Town_button;


    private Castle_Logic _castle;
    private void Start()
    {
        _castle = FindObjectOfType<Castle_Logic>();
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
            }

        }
    }
}
