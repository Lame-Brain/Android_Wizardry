using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Message_Controller : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Message;

    private float _FontSize;

    private void OnEnable()
    {
        _FontSize = Game_Logic.TEXT_FONT;
        if (_FontSize == 0) _FontSize = 40.1f;
    }

    public void Update_Text_Screen(string _txt)
    {
        Message.fontSize = _FontSize;        
        _txt = _txt.ToUpper();
        Message.text = _txt;
    }
}
