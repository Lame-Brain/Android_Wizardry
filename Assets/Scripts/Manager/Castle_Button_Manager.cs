using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Button_Manager : MonoBehaviour
{
    public Button_Manager[] button;

    public void ClearButtons()
    {        
        //Debug.Log("Called ClearButtons()");
        for (int i = 0; i < button.Length; i++) button[i].UpdateButton("", "");
    }

    public void SetButton(int _num, string _name, string _command)
    {
        //Debug.Log("Called SetButton: " + _num + ", " + _name + ", " + _command);
        if (_num < 0) _num = 0;
        if (_num > button.Length - 1) _num = button.Length - 1;
        button[_num].UpdateButton(_name, _command);
    }

    public void Button_Press_Received(string _text)
    {
        //Debug.Log("Button Press!");
        FindObjectOfType<Castle_Logic_Manager>().ReceiveButtonPress(_text);
    }
}
