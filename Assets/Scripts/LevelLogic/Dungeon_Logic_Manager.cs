using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Dungeon_Logic_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI Message, up_btn_txt, left_btn_txt, down_btn_txt, right_btn_txt, one_btn_txt, two_btn_txt;
    public TextAsset[] Level_data;

    public GameObject Camp_Screen, Light_Icon, Shield_Icon, Block_Icon;

    public Castle_Pop_Up_Manager PopUp;
    public Magic_Logic_Controller Magic;
    public Player_Controller _player;
    private Level_Logic_Template _level;


    public void Start()
    {
        _level = FindObjectOfType<Level_Logic_Template>();
        _player = FindObjectOfType<Player_Controller>();
        _player.initPlayer();
        Message.fontSize = GameManager.FONT;
        up_btn_txt.fontSize = GameManager.FONT;
        left_btn_txt.fontSize = GameManager.FONT;
        down_btn_txt.fontSize = GameManager.FONT;
        right_btn_txt.fontSize = GameManager.FONT;
        one_btn_txt.fontSize = GameManager.FONT;
        two_btn_txt.fontSize = GameManager.FONT;
        SetButtonText("u", "Up", "move_forward");
        SetButtonText("d", "down", "turn_around");
        SetButtonText("l", "left", "turn_left");
        SetButtonText("r", "right", "turn_right");
        SetButtonText("1", "Examine", "examine");
        SetButtonText("2", "Camp", "make_camp");
        TimePass();
        if (GameManager.PARTY._MakeCampOnLoad) ButtonPressReceived("make_camp");
    }

    private void UpdateScreen()
    {
        //Icons
        if(GameManager.PARTY.Party_Light_Timer > 0)
        {
            Light_Icon.SetActive(true);
            RenderSettings.fogDensity = 0.25f;
        }
        else
        {
            Light_Icon.SetActive(false);
            RenderSettings.fogDensity = 0.95f;
        }

        if (GameManager.PARTY.Party_Shield_Bonus)
        {
            Shield_Icon.SetActive(true);
        }
        else
        {
            Shield_Icon.SetActive(false);
        }

        //Button context
        SetContextButton();
    }

    private void SetContextButton()
    {
        int _faced = _player.WhatIsInFrontOfPlayer();
        if (_faced == 0) SetButtonText("1", "Inspect", "inspect");
        if (_faced == 1) SetButtonText("1", "Open Door", "open_door");
        if (_faced == 2) SetButtonText("1", "Check Wall", "check_wall");
        if (_faced == 3) SetButtonText("1", "Open Door", "open_door");
        if (_faced == 4) SetButtonText("1", "Check Wall", "check_wall");
    }

    public void SetButtonText(string _buttonNam, string _newText, string _command)
    {
        if (_buttonNam == "u")
        {
            up_btn_txt.text = _newText.ToUpper();
            up_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
        if (_buttonNam == "d")
        {
            down_btn_txt.text = _newText.ToUpper();
            down_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
        if (_buttonNam == "l")
        {
            left_btn_txt.text = _newText.ToUpper();
            left_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
        if (_buttonNam == "r")
        {
            right_btn_txt.text = _newText.ToUpper();
            right_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
        if (_buttonNam == "1")
        {
            one_btn_txt.text = _newText.ToUpper();
            one_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
        if (_buttonNam == "2")
        {
            two_btn_txt.text = _newText.ToUpper();
            two_btn_txt.gameObject.GetComponentInParent<Dungeon_Button_Controller>().button_command = _command;
        }
    }
    public void UpdateMessge(string _newText = "")
    {
        string _output = "";
        //Debug.Log("Update Message: " + _newText);
        if(_newText == "")
        {
            //build party string
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    Character_Class me = GameManager.PARTY.LookUp_PartyMember(i);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    string _ac = me.ArmorClass.ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;
                    string _hpMax = me.HP_MAX.ToString();
                    if (me.HP > 9999) _hpMax = "lots";
                    if (me.HP < 1000) _hpMax = " " + _hpMax;
                    if (me.HP < 100) _hpMax = " " + _hpMax;
                    if (me.HP < 10) _hpMax = " " + _hpMax;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison != 0) _stat = "POISON";
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison == 0) _stat = _hpMax;

                    _partyText[i] = _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
                else
                {
                    _partyText[i] = "\n";
                }
            }
            //build static info
            _output = "character name  class ac hits status\n";
            _output += _partyText[0];
            _output += _partyText[1];
            _output += _partyText[2];
            _output += _partyText[3];
            _output += _partyText[4];
            _output += _partyText[5];
        }
        else
        {
            _output = _newText;
        }

        Message.text = _output;
    }

    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<
    public void TimePass()
    {
        //Timers
        if (GameManager.PARTY.Party_Light_Timer > 0) GameManager.PARTY.Party_Light_Timer--;
        for (int i = 0; i < 6; i++) 
        {
            if (!GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).Poison != 0)
            {
                GameManager.PARTY.LookUp_PartyMember(i).TakeDamage(Random.Range(0, 8) + 1);
                if (GameManager.PARTY.LookUp_PartyMember(i).Poison > 0) 
                    GameManager.PARTY.LookUp_PartyMember(i).Poison--;
            }
        }


        //Update message screen
        UpdateMessge();

        //Check for features. 
        // 0 = none, 1 = darkness, 2 = rock (dead), 3 = spinner, 4 = anti-magic, 5 = stairs going up, 6 = stairs going down, 7 = elevator, 8 = chute, 9 = pit
        Tile_Class _thisRoom = _player.WhatRoomAmIin();
        if(_thisRoom.feature == 1) //Darkness
        {
            GameManager.PARTY.Party_Light_Timer = 0;
            if (_player.Light.activeSelf) _player.Light.SetActive(false);
        }
        else
        {
            if(!_player.Light.activeSelf) _player.Light.SetActive(true);
        }
        if(_thisRoom.feature == 2) //Rock, kills party
        {
            GameManager.instance._Persistent_Message = "Oh no! Solid Rock! Everyone in the Party is Lost!";
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i)) GameManager.PARTY.LookUp_PartyMember(i).status = Enum._Status.lost;
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(0)) GameManager.PARTY.RemoveMember(0);
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");            
        }
        if(_thisRoom.feature == 3) // spinner, random direction
        {
            _player.SpinPlayer();
        }
        if(_thisRoom.feature == 4)
        {
            GameManager.PARTY.Party_Light_Timer = 0;
            GameManager.PARTY.Party_Shield_Bonus = false;
        }
        if(_thisRoom.feature == 9)
        {
            Dice _dam = new Dice(GameManager.PARTY._PartyXYL.z, 8,0);
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i)) GameManager.PARTY.LookUp_PartyMember(i).TakeDamage(_dam.Roll());
        }

        //Putting this here because none of the other features invole updating the screen, and I don't want the update to overwrite the message or buttons
        UpdateScreen();

        if (_thisRoom.feature == 5)
        {                   //1234567890123456789012345678901234567890
            UpdateMessge("\n\n          STAIRS LEADING UP!\n" +
                             "          TAKE THEM? <Yes/No>");
            SetButtonText("u", "", "");
            SetButtonText("d", "", "");
            SetButtonText("l", "", "");
            SetButtonText("r", "", "");
            SetButtonText("1", "Yes", "stairs_up");
            SetButtonText("2", "No", "cancel");
        }
        if (_thisRoom.feature == 6)
        {                   //1234567890123456789012345678901234567890
            UpdateMessge("\n\n        STAIRS LEADING DOWN...\n" +
                             "         TAKE THEM? <Yes/No>");
            SetButtonText("u", "", "");
            SetButtonText("d", "", "");
            SetButtonText("l", "", "");
            SetButtonText("r", "", "");
            SetButtonText("1", "Yes", "stairs_up");
            SetButtonText("2", "No", "cancel");
        }

        //SPECIAL
        if (_thisRoom.isSpecial)
        {
            _level.Special_Stuff(_thisRoom.special_code);
        }

    }
    //<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<<

    public void ButtonPressReceived(string _command)
    {
        if(_command == "update_screen") UpdateScreen();
        if(_command == "move_forward" || _command == "turn_right" || _command == "turn_around" || _command == "turn_left")
        {
            _player.ReceiveCommand(_command);
        }

        if(_command == "make_camp")
        {
            Camp_Screen.SetActive(true);
        }
        //"inspect
        if(_command == "check_wall")
        {
            _player.Check4SecretDoor();
        }
        if (_command == "open_door")
        {
            _player.OpenDoor();
        }
        if(_command == "cancel")
        {
            SetButtonText("u", "Up", "move_forward");
            SetButtonText("d", "down", "turn_around");
            SetButtonText("l", "left", "turn_left");
            SetButtonText("r", "right", "turn_right");            
            SetButtonText("2", "Camp", "make_camp");
            SetContextButton();
            UpdateMessge();
        }
        if(_command == "stairs_up")
        {
            GameManager.instance.SaveGame();
            if(GameManager.PARTY._PartyXYL.z == 1)
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
            }
            else
            {
                Debug.Log("Go up a level");
            }
        }
        if (_command.Contains("Special:"))
        {
            _command = _command.Replace("Special:", "");
            _level.Special_Stuff(_command);
        }
    }
}
