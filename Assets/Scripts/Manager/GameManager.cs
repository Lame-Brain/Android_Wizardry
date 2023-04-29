using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public static float FONT;
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
    }

    private void Start()
    {
        _display = FindObjectOfType<Castle_Display_Manager>();
        _display.Update_Display("inn", "");
    }
}
