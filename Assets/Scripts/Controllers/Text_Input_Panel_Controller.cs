using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlobberEngine;

public class Text_Input_Panel_Controller : MonoBehaviour
{
    public TextMeshProUGUI Message;
    public TMP_InputField input;
    public TextMeshProUGUI InputPlaceHolder;
    public TextMeshProUGUI InputText;

    private Input_Screen_Controller _input;
    private Display_Screen_Controller _display;
    private string _token;


    private void OnEnable()
    {
        _input = FindObjectOfType<Input_Screen_Controller>();
        _display = FindObjectOfType<Display_Screen_Controller>();
        Message.fontSize = Game_Logic.TEXT_FONT;
        InputPlaceHolder.fontSize = Game_Logic.TEXT_FONT;
        InputText.fontSize = Game_Logic.TEXT_FONT;
        input.onValidateInput += delegate (string s, int i, char c) { return char.ToUpper(c); };
        input.onFocusSelectAll = true;
    }

    public void Show_Text_Input_Panel(string _text, string token = "TextInput:")
    {
        this.gameObject.SetActive(true);
        _text = _text.ToUpper();
        Message.text = _text;
        _token = token;
    }
    public void Close_Text_Input_Panel()
    {
        input.text = "";
        input.DeactivateInputField();
        input.ActivateInputField();
        this.gameObject.SetActive(false);
        Message.text = "";        
    }

    public void Accept_Input(string _S)
    {
        _input.Button_Clicked(_token + _S);
    }
}
