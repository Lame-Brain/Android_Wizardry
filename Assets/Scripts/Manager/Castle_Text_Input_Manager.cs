using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Castle_Text_Input_Manager : MonoBehaviour
{
    public TextMeshProUGUI Message;
    public TMP_InputField input;
    public TextMeshProUGUI InputPlaceHolder;
    public TextMeshProUGUI InputText;

    private string _token;

    private void OnEnable()
    {
        Message.fontSize = GameManager.FONT;
        InputPlaceHolder.fontSize = GameManager.FONT;
        InputText.fontSize = GameManager.FONT;
        input.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
        input.text = "";
        //input.onFocusSelectAll = true;
    }

    public void Show_Text_Input_Panel(string _text, string _commandText)
    {
        this.gameObject.SetActive(true);
        
        _commandText = _commandText.ToUpper();
        _token = _commandText;

        _text = _text.ToUpper();
        _text = _text.Replace("[D]", "d");
        
        Message.text = _text;
    }

    public void Text_Input_Received(string _input)
    {
        if (_token == "") _token = "TEXT_INPUT:";
        _input = _token + _input;
        FindObjectOfType<Castle_Logic_Manager>().ReceiveButtonPress(_input);
        this.gameObject.SetActive(false);
    }

}
