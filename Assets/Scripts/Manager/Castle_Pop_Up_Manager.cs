using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Pop_Up_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI top, bottom;

    private void OnEnable()
    {
        top.fontSize = GameManager.FONT;
        bottom.fontSize = GameManager.FONT;
    }

    public void Close_Pop_up()
    {
            this.gameObject.SetActive(false);
    }

    public void Show_Message(string _text)
    {
        this.gameObject.SetActive(true);
        _text = _text.ToUpper();
        _text = _text.Replace("[D]", "d");
        top.text = _text;
    }
}
