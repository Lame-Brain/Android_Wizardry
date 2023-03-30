using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Castle_Logic : MonoBehaviour
{
    public string townStatus;

    private Display_Screen_Controller _display;
    //private Input_Panel _input;

    private void Start()
    {
        _display = FindObjectOfType<Display_Screen_Controller>();
        townStatus = "Market";
    }

    public void Update_Screen()
    {
        if(townStatus == "Market")
        {
            _display.Update_Text_Screen("+--------------------------------------+\n" +
                                        "| Castle                        Market |\n" +
                                        "+----------- Current party: -----------+\n" +
                                        "                                        \n" +
                                        " # character name  class ac hits status \n");
        }
    }
}
