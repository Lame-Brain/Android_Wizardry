using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using BlobberEngine;

public class Text_Input_Panel_Controller : MonoBehaviour
{
    public TextMeshProUGUI Message;
    public TextMeshProUGUI InputPlaceHolder;
    public TextMeshProUGUI InputText;

    private Display_Screen_Controller _display;


    private void OnEnable()
    {
        _display = FindObjectOfType<Display_Screen_Controller>();
        Message.fontSize = _display.FONT_SIZE;
        InputPlaceHolder.fontSize = _display.FONT_SIZE;
        InputText.fontSize = _display.FONT_SIZE;
    }

    public void Show_Text_Input_Panel(string _text)
    {
        this.gameObject.SetActive(true);
        _text = _text.ToUpper();
        Message.text = _text;
    }

    public void Accept_Input(string _S)
    {
        Debug.Log(_S);
    }
}
