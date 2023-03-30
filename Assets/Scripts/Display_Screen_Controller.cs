using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Display_Screen_Controller : MonoBehaviour
{
    public TextMeshProUGUI Screen_Sizing_String, Output;
    public RectTransform Input_Panel;

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

        /*
        Update_Text_Screen("alven dwarf g-fighter                    \n" +
                           "                                         \n" +
                           "       Strength 15             gold 217  \n" +
                           "           I.Q.  9             exp 5170  \n" +
                           "          Piety 12                       \n" +
                           "       vitality 18   level  5   Age 23   \n" +
                           "        agility  5    hits 22/ 29 AC 1   \n" +
                           "           luck  9  status OK            \n" +
                           "                                         \n" +
                           "            mage 0/0/0/0/0/0/0           \n" +
                           "          priest 0/0/0/0/0/0/0           \n" +
                           "                                         \n" +
                           "*=equip, -=cursed, ?=unknown, #=unusuable\n" +
                           "1 )*long sword      2 )*Large shield     \n" +
                           "3 )*Plate Mail      4 )*Helm             \n" +
                           "                                         \n" +
                           "                                         \n" +
                           "you may e)quip, d)rop an item, t)rade,   \n" +
                           "        r)ead spell books. or l)eave.");
        */
    }

    public void Update_Text_Screen(string _txt)
    {
        _txt = _txt.ToUpper();
        Output.text = _txt;
    }
}
