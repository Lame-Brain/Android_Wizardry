using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float SetFloatDefault;
    public static float FONT;
    public static Party_Class PARTY;
    public static List<Character_Class> ROSTER = new List<Character_Class>();
    public static List<Spell_Class> SPELL = new List<Spell_Class>();
    public static List<Item_Class> ITEM = new List<Item_Class>();


    public GameObject _initCanvas, _initPanel;
    public TMPro.TextMeshProUGUI Screen_Sizing_String;


    private Castle_Display_Manager _display;

    private void Awake()
    {
        //Set Singleton
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }

        //Initialize screen
        FONT = SetFloatDefault;
        if(FONT == 0)
        {
            _initCanvas.SetActive(true); _initPanel.SetActive(true);
            RectTransform _myScreen = _initPanel.GetComponent<RectTransform>();
            float _screenW = Screen.width;
            float _screenH = 0.73f * _screenW;

            _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _screenW);
            _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _screenH);

            Screen_Sizing_String.ForceMeshUpdate();
            FONT = Screen_Sizing_String.fontSize;
            _initPanel.SetActive(false); _initCanvas.SetActive(false);
        }

        //initialize party
        PARTY = FindObjectOfType<Party_Class>();
        PARTY.InitParty();
    }

    private void Start()
    {
        _display = FindObjectOfType<Castle_Display_Manager>();


        //DEBUG
        _display.Update_Display("inn", "");
    }
}
