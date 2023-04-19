using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name_Button_Controller : MonoBehaviour
{
    public string String;
    public TMPro.TextMeshProUGUI ButtonTitle;
    public bool inDungeon;
    private Input_Screen_Controller _input;

    private void OnEnable()
    {
        if(!inDungeon) _input = FindObjectOfType<Input_Screen_Controller>();
    }

    public void Button_Clicked()
    {
        if(!inDungeon) _input.Button_Clicked(String);
    }
}
