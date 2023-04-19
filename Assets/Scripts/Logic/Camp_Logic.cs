using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Logic : MonoBehaviour
{
    public enum _States { Camp, View_Character, View_Item, Cast_Spell, Reorder, Disband }
    public _States Camp_State;

    public Character_Class _selected_Character;
    public int _selected_Party_Slot,
               _selected_Inventory_Slot;
    public Item_Class _selected_Item_Class;
    public Item _selected_instanced_item;


    private Camp_Message_Controller _display;
    private Camp_Input_Controller _input;

    private void OnEnable()
    {
        _display = FindObjectOfType<Camp_Message_Controller>();
        _input = FindObjectOfType<Camp_Input_Controller>();
    }

    private void Start()
    {
        Camp_State = _States.Camp;
        Update_Screen();
    }

    public void Update_Screen()
    {
        if (Camp_State == _States.Camp)
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(i);

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

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK) _stat = _hp;

                    _partyText[i] = " " + (i+1) + " " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }

            string _txt = "+-------------------------------------+\n" +
                                            "| Dungeon                        camp |\n" +
                                            "+----------- Current party: ----------+\n" +
                                            "                                        \n" +
                                            " # character name  class ac hits status\n" +
                                            _partyText[0] +
                                            _partyText[1] +
                                            _partyText[2] +
                                            _partyText[3] +
                                            _partyText[4] +
                                            _partyText[5] +
                                            "+-------------------------------------+\n" +
                                            "                                       \n" +
                                            "You May Reorder your party, \n" +
                                            "        Veiw a party member, \n" +
                                            "        Disband your party, or \n" +
                                            "        Leave Camp.";
            _display.Update_Text_Screen(_txt);
        }
    }
}
