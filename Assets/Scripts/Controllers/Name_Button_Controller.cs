using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Name_Button_Controller : MonoBehaviour
{
    public string String;
    public TMPro.TextMeshProUGUI ButtonTitle;
    private Input_Screen_Controller _input;

    private void OnEnable()
    {
        _input = FindObjectOfType<Input_Screen_Controller>();
    }

    public void Button_Clicked()
    {
        _input.Button_Clicked(String);
    }
}
