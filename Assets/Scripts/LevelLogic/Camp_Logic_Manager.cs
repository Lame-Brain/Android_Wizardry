using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Logic_Manager : MonoBehaviour
{
    public enum Camp_Logic_States { main = 0, reorder_party = 1, choose_member = 3 }
    public Camp_Logic_States state;
    public Button_Manager[] button;
    public TMPro.TextMeshProUGUI Message;

    public Camp_Character_Sheet_Manager CharacterSheet;
    public Castle_Pop_Up_Manager PopUP;
    public Magic_Logic_Controller Magic;

    private List<int> _toons = new List<int>();
    private List<int> _newParty = new List<int>();

    public Character_Class Selected_Character, Other_Character;
    public int Selected_Party_Slot, Selected_Roster_Slot, Selected_Inventory_Slot, Selected_Item_Index, Other_Party_Slot, Other_Roster_Slot, Other_Inventory_Slot;
    public Item_Class Selected_Item_Class;
    public Item Selected_Item;

    

    private void OnEnable()
    {        
        Message.fontSize = GameManager.FONT;
        for (int i = 0; i < button.Length; i++)
            button[i].GetComponentInChildren<TMPro.TextMeshProUGUI>().fontSize = GameManager.FONT;
        ClearButtons();
        state = Camp_Logic_States.main;
        UpdateScreen();
    }

    private void OnDisable()
    {
        FindObjectOfType<Dungeon_Logic_Manager>().ButtonPressReceived("update_screen");
    }

    public void ClearButtons()
    {
        for (int i = 0; i < button.Length; i++) button[i].UpdateButton("", "");
    }
    public void SetButton(int _num, string _name, string _command)
    {
        if (_num < 0) _num = 0;
        if (_num > button.Length - 1) _num = button.Length - 1;
        button[_num].UpdateButton(_name, _command);
    }

    public void RefreshCharacterSheet()
    {
        CharacterSheet.UpdateScreen();
    }

    public void UpdateScreen()
    {
        ClearButtons();
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

                _partyText[i] = " " + (i+1) + " " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                    me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                    _ac + " " + _hp + " " + _stat + "\n";
            }
            else
            {
                _partyText[i] = "\n";
            }
        }

        //build static info
        string _output = "+--------------------------------------+\n" +
                         "| labyrinth                       camp |\n" +
                         "+--------------------------------------+\n" +
                         "# character name  class ac hits status \n";
        _output += _partyText[0];
        _output += _partyText[1];
        _output += _partyText[2];
        _output += _partyText[3];
        _output += _partyText[4];
        _output += _partyText[5];
        _output += "+--------------------------------------+\n";

        if(state == Camp_Logic_States.main)
        {
            //              1234567890123456789012345678901234567890
            _output += "\n\nYour party takes a moment to stop and   \n" +
                           "rest.";
            SetButton(0, "Reorder Party", "reorder_party:start");
            SetButton(2, "Inspect Party Member", "inspect_member");
            SetButton(6, "Disband Party", "disband_party");
            SetButton(10, "Break Camp", "break_camp");
        }

        if(state == Camp_Logic_States.reorder_party)
        {
            _output = "\n\n";
            for (int i = 0; i < 6; i++)
            {
                _output += i + ". ";
                if (i < _newParty.Count) _output += GameManager.ROSTER[_newParty[i]].name + ", " + GameManager.ROSTER[_newParty[i]].character_class;
                _output += "\n";
            }
            //              1234567890123456789012345678901234567890
            _output += "\n\nSelect the new party order.";

            ClearButtons();
            for (int i = 0; i < _toons.Count; i++)
                if (_toons[i] > -1)
                    SetButton(i, GameManager.ROSTER[_toons[i]].name + ", " + GameManager.ROSTER[_toons[i]].character_class, "reorder_party:" + i);
            SetButton(10, "Cancel", "cancel");
        }

        if(state == Camp_Logic_States.choose_member)
        {
            _output += "Which Party Member?";
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i)) SetButton(i, GameManager.PARTY.LookUp_PartyMember(i).name, "choose_member:" + i);
            SetButton(10, "Cancel", "cancel");
        }

        //output the string
        _output = _output.ToUpper();
        _output = _output.Replace("[D]", "d");
        Message.text = _output;
    }



    public void Button_Press_Received(string _command)
    {

        if (_command == "cancel")
        {
            state = Camp_Logic_States.main;
            UpdateScreen();
            return;
        }

        if (_command.Contains("reorder_party:"))
        {
            _command = _command.Replace("reorder_party:", "");

            if (_command == "start")
            {
                state = Camp_Logic_States.reorder_party;
                ClearButtons();
                _toons.Clear();
                _newParty.Clear();
                for (int i = 0; i < 6; i++)
                    if (!GameManager.PARTY.EmptySlot(i))
                    { _toons.Add(GameManager.PARTY.Get_Roster_Index(i)); }
                    else
                    { _toons.Add(-1); }
                UpdateScreen();
                return;
            }

            int _int = -1;
            int.TryParse(_command, out _int);
            if (_int > -1)
            {
                //determine target party size
                int _fullParty = 0;
                for (int i = 0; i < 6; i++) if (!GameManager.PARTY.EmptySlot(i)) _fullParty++;
                //fill up _newParty
                _newParty.Add(_int);
                //remove from available list.
                _toons[_int] = -1;
                //check if the party is all moved
                if (_newParty.Count == _fullParty)
                {
                    int[] _slot = new int[6]; for (int i = 0; i < 6; i++) { _slot[i] = -1; if (i < _newParty.Count) _slot[i] = _newParty[i]; }
                    GameManager.PARTY.OverRide_PartySlots(_slot[0], _slot[1], _slot[2], _slot[3], _slot[4], _slot[5]);
                    state = Camp_Logic_States.main;
                }
                UpdateScreen();
                return;
            }
        }

        if (_command == "inspect_member")
        {
            state = Camp_Logic_States.choose_member;
            UpdateScreen();
            return;
        }

        if (_command.Contains("choose_member:"))
        {
            _command = _command.Replace("choose_member:", "");
            int _chosen1 = -1;
            int.TryParse(_command, out _chosen1);
            if (_chosen1 > -1)
            {
                Selected_Party_Slot = _chosen1;
                Selected_Character = GameManager.PARTY.LookUp_PartyMember(_chosen1);
                Selected_Roster_Slot = GameManager.PARTY.Get_Roster_Index(_chosen1);
                state = Camp_Logic_States.main;
                CharacterSheet.UpdateScreen();
            }
        }

        if ( _command == "break_camp")
        {
            this.gameObject.SetActive(false);
        }
    }
}
