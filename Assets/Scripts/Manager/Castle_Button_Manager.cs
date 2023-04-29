using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Button_Manager : MonoBehaviour
{
    public Button_Manager[] button;

    private void Start()
    {
        for (int i = 0; i < button.Length; i++) button[i].UpdateButton("", "");
    }

    public void Button_Press_Received(string _text)
    {

    }
}
