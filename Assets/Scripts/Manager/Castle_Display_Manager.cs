using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Display_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Display;

    public void Update_Display(string location_string, string flavor_string)
    {
        Display.fontSize = GameManager.FONT;
        int _spaces = 30 - location_string.Length;
        string _output = "+--------------------------------------+\n" +
                         "| castle";
        for (int i = 0; i < _spaces; i++) _output += " ";
        _output += location_string + " |\n+--------------------------------------+\n" +
                                     " # character name  class ac hits status \n";

        _output = _output.ToUpper();
        Display.text = _output;
    }
}
