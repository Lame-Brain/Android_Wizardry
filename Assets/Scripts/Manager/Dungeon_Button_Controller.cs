using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Button_Controller : MonoBehaviour
{
    public TMPro.TextMeshProUGUI button_title;
    public string button_command;

    public void Button_Pressed()
    {
        FindObjectOfType<Dungeon_Logic_Manager>().ButtonPressReceived(button_command);
    }
}
