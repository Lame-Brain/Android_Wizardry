using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Display_Screen_Controller : MonoBehaviour
{
    public TextMeshProUGUI Screen_Sizing_String, Output, Message, instructions;
    public RectTransform Input_Panel;
    public GameObject Button_Block_Panel, Message_Pop_Up, Text_Input_Controller;

    public float FONT_SIZE;

    private int pp_NumOfPages, pp_currentPage;
    private string[] pp_message;
    public int LINES_PER_PAGE = 22;

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
        instructions.fontSize = FONT_SIZE * 0.5f;        
    }

    private void Start()
    {
        pp_message = new string[10 * LINES_PER_PAGE];
    }

    public void Update_Text_Screen(string _txt)
    {
        _txt = _txt.ToUpper();
        Output.text = _txt;
    }

    public void Block_Buttons()
    {
        Button_Block_Panel.SetActive(true);
        Button_Block_Panel.transform.SetAsLastSibling();
    }

    public void PopUpMessage(string _message)
    {
        _message = _message.ToUpper();
        for (int i = 0; i < 10 + LINES_PER_PAGE; i++) pp_message[i] = "";
        string[] _lines = _message.Split("\n");
        for (int i = 0; i < _lines.Length; i++) pp_message[i] = _lines[i];
        pp_NumOfPages = Mathf.FloorToInt(_lines.Length / LINES_PER_PAGE);
        pp_currentPage = 0;

        Message_Pop_Up.SetActive(true);
        Block_Buttons();
        Show_Pop_Up_Message();
    }
    public void ClosePopUpMessage()
    {
        Message_Pop_Up.SetActive(false);
        Button_Block_Panel.SetActive(false);        
    }

    private void Show_Pop_Up_Message()
    {
        Message.fontSize = FONT_SIZE;
        int _last = 0; string _t = "";
        _last = pp_message.Length - (LINES_PER_PAGE * pp_currentPage);
        if (_last > LINES_PER_PAGE) _last = LINES_PER_PAGE;
        for (int i = 0; i < _last; i++)
        {
            _t += pp_message[(LINES_PER_PAGE * pp_currentPage) + i] + "\n";
        }
        Message.text = _t;
    }

    public void PopUp_Tapped()
    {
        if (pp_currentPage >= pp_NumOfPages)
        {
            ClosePopUpMessage();
            return;
        }
        else
        {
            pp_currentPage++;
            Show_Pop_Up_Message();
        }
    }
}
