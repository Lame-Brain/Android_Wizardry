using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI myName;
    public string command;
    public bool isCamp = false;
    
    public void UpdateButton(string name, string _command)
    {
        //Debug.Log("Called Update Button: " + name + ", " + _command);
        myName.fontSize = GameManager.FONT;
        myName.text = name.ToUpper();
        command = _command;
    }

    public void PushButton()
    {
        if(!isCamp) FindObjectOfType<Castle_Button_Manager>().Button_Press_Received(command);
        if(isCamp) FindObjectOfType<Camp_Logic_Manager>().Button_Press_Received(command);
    }
}
