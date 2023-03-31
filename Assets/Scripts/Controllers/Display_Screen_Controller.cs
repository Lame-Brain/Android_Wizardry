using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Display_Screen_Controller : MonoBehaviour
{
    public TextMeshProUGUI Screen_Sizing_String, Output, Message;
    public RectTransform Input_Panel;
    public GameObject Button_Block_Panel, Message_Pop_Up;

    private float FONT_SIZE;
    private void OnEnable()
    {
        RectTransform _myScreen = this.GetComponent<RectTransform>();
        float _screenW = Screen.width;
        float _screenH = 0.73f * _screenW;
        if (_screenH + 550 > Screen.height)
        {
            _screenW = (_screenH + 550) * (35 / 48);
            _screenH = Screen.height + 550;
        }

        _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _screenW);
        _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical,_screenH);
        Screen_Sizing_String.ForceMeshUpdate();
        FONT_SIZE = Screen_Sizing_String.fontSize;
        Screen_Sizing_String.gameObject.SetActive(false);
        Output.fontSize = FONT_SIZE;
    }

    public void Update_Text_Screen(string _txt)
    {
        _txt = _txt.ToUpper();
        Output.text = _txt;
    }

    public void PopUpMessage(string _message)
    {
        _message = _message.ToUpper();
        Message_Pop_Up.SetActive(true);
        Button_Block_Panel.transform.SetAsLastSibling();
        Button_Block_Panel.SetActive(true);
        Message.fontSize = FONT_SIZE;
        Message.text = _message;
    }
    public void ClosePopUpMessage()
    {
        Message_Pop_Up.SetActive(false);
        Button_Block_Panel.SetActive(false);
    }
}
