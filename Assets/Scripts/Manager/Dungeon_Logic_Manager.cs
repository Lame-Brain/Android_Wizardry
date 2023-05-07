using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dungeon_Logic_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Message, up_btn_txt, left_btn_txt, down_btn_txt, right_btn_txt, one_btn_txt, two_btn_txt;

    public GameObject Camp_Screen, Light_Icon, Shield_Icon;

    public void StartDungeon()
    {
        Message.fontSize = GameManager.FONT;
        up_btn_txt.fontSize = GameManager.FONT;
        left_btn_txt.fontSize = GameManager.FONT;
        down_btn_txt.fontSize = GameManager.FONT;
        right_btn_txt.fontSize = GameManager.FONT;
        one_btn_txt.fontSize = GameManager.FONT;
        two_btn_txt.fontSize = GameManager.FONT;
    }

    public void ButtonPressReceived(string _command)
    {
        if(_command == "make_camp")
        {
            Camp_Screen.SetActive(true);
        }
    }
}
