using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI myName;
    private string command;
    
    public void UpdateButton(string name, string _command)
    {
        myName.fontSize = GameManager.FONT;
        myName.text = name.ToUpper();
        command = _command;
    }

    public void PushButton()
    {
        FindObjectOfType<Castle_Button_Manager>().Button_Press_Received(command);
    }
}
