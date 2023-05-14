using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inspect_Controller_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI main, btn1, btn2;
    public GameObject button_1, button_2;

    private Level_Logic_Template _level;
    private string _button1_Command, _button2_Command;
    private List<Character_Class> the_lost = new List<Character_Class>();
    private Character_Class _lost;

    private void OnEnable()
    {
        main.fontSize = GameManager.FONT;
        btn1.fontSize = GameManager.FONT;
        btn2.fontSize = GameManager.FONT;
        button_1.SetActive(true); button_2.SetActive(true);
        main.text = "You Inspect the area for the remains of fallen adventurers...";
        btn1.text = "Proceed"; _button1_Command = "inspect1";
        btn2.text = "Cancel"; _button2_Command = "cancel";
    }

    public void Button_One_Pushed()
    {
        Command_Received(_button1_Command);
    }
    public void Button_Two_Pushed()
    {
        Command_Received(_button2_Command);
    }
    public void Command_Received(string _command)
    {
        if(_command == "cancel")
        {
            this.gameObject.SetActive(false);
            return;
        }
        if(_command == "inspect1")
        {
            //clear the list
            the_lost.Clear();
            //build new list of lost characters
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
            {
                if (GameManager.ROSTER[i].status == BlobberEngine.Enum._Status.lost)
                    the_lost.Add(GameManager.ROSTER[i]);
            }
            Command_Received("inspect2");
            return;
        }
        if ( _command == "inspect2")
        {
            //search list for nearby characters...
            for (int range = 0; range <= 2; range++) // ...at range 0, then 1, then 2...
                for (int i = 0; i < the_lost.Count; i++)
                {
                    int lost_z = the_lost[i].lostXYL.z;
                    int my_z = GameManager.PARTY._PartyXYL.z;
                    if (my_z == lost_z) //skip any lost who are not on the same level as the party
                    {
                        int lost_x = the_lost[i].lostXYL.x;
                        int lost_y = the_lost[i].lostXYL.y;
                        int my_x = GameManager.PARTY._PartyXYL.x;
                        int my_y = GameManager.PARTY._PartyXYL.y;

                        if (my_x >= lost_x - range && my_x <= lost_x + range 
                            && my_y >= lost_y - range && my_y <= lost_y + range)
                        {
                            PresentLostCharacter(the_lost[i]);
                            return;
                        }
                    }
                }
            Command_Received("no_one_found");
        }
        if(_command == "reject_character")
        {
            the_lost.Remove(_lost);
            Command_Received("inspect2");
            return;
        }
        if(_command == "add_character")
        {
            if (!GameManager.PARTY.EmptySlot(5))
            {
                main.text = "There is no room! You will need to return with fewer party members!";
                button_1.SetActive(false); button_2.SetActive(true);
                btn2.text = "Ok"; _button2_Command = "cancel";
                return;
            }
            //determine roster index
            int _index = 0;
            for (int i = 0; i < GameManager.ROSTER.Count; i++)
                if (GameManager.ROSTER[i] == _lost) _index = i;
            GameManager.PARTY.AddMember(_index);
            Command_Received("inspect2");
        }
        if(_command == "no_one_found")
        {
            main.text = "You do not find any remains nearby.";
            button_1.SetActive(false); button_2.SetActive(true);
            btn2.text = "Ok"; _button2_Command = "cancel";
            return;
        }
    }
    private void PresentLostCharacter(Character_Class _toon)
    {
        _lost = _toon;
        main.text = "You have found someone!\n" + "     " + _lost.name + ", " + _lost.character_class +
            "\n\n\n would you like to add this character to your party?";
        button_1.SetActive(true); button_2.SetActive(true);
        btn1.text = "Yes"; _button1_Command = "add_character";
        btn2.text = "No"; _button2_Command = "reject_character";
        return;
    }
}
