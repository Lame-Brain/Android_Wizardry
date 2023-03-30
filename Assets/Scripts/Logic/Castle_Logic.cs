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
        townStatus = "Market"; // Inn, Tavern, Shop, Temple, Exit
        Update_Screen();
    }

    public void Update_Screen()
    {
        if(townStatus == "Market")
        {
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!Game_Logic.PARTY.EmptySlot(i))
                {
                    Character_Class me = Game_Logic.PARTY.LookUp_PartyMember(0);
                    
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

                    _partyText[i] = " 1 " + _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
            }
            
            _display.Update_Text_Screen("+--------------------------------------+\n" +
                                        "| Castle                        Market |\n" +
                                        "+----------- Current party: -----------+\n" +
                                        "                                        \n" +
                                        " # character name  class ac hits status \n" +
                                        _partyText[0] +
                                        _partyText[1] +
                                        _partyText[2] +
                                        _partyText[3] +
                                        _partyText[4] +
                                        _partyText[5] +
                                        "+--------------------------------------+\n");
            //Buttons: Adventurers Inn
            //         Gilgamesh's tavern
            //         Boltacs' Trading Post
            //         Temple of CANT
            //         Edge of Town
        }
    }
}
