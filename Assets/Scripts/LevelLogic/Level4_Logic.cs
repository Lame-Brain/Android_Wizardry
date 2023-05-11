using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level4_Logic : Level_Logic_Template
{
    private void Start()
    {
        base._builder = FindObjectOfType<Level_Builder>();
        base.Map = new Tile_Class[22, 22];
        for (int y = 0; y < 22; y++)
            for (int x = 0; x < 22; x++)
                Map[x, y] = _builder.transform.GetChild(y * 22 + x).GetComponent<Tile_Class>();

    }
    public override void Special_Stuff(string _string)
    {
        Dungeon_Logic_Manager _dungeon = FindObjectOfType<Dungeon_Logic_Manager>();
        if (_string == "go_up")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(1, 8, 3);
            GameManager.PARTY.facing = Enum._Direction.north;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon2");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(0, 0, 5);
            GameManager.PARTY.facing = Enum._Direction.north;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon3");
        }
        
        if(_string == "Message2")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A SIGN ON THE DOOR READS:             \n" +
                                  "*** TESTING GROUNDS CONTROL CENTER ***\n" +
                                  "    AUTHORIZED PERSONNEL ONLY!      \n" +
                                  "    *** DO NOT ENTER ***");
        }
        if(_string == "Message4")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A SIGN ON THE DOOR READS: \n" +
                                  "            \"TREASURE REPOSITORY.\"");
        }
        if(_string == "Message5")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A SIGN ON THE DOOR READS: \n" +
                                  "        \"Monster Allocation Center.\"");
        }
        if(_string == "Message6")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("IN THIS ROOM, THERE IS A LARGE DESK.  \n" +
                                  "ON IT ARE THE REMAINS OF CRYSTAL BALLS\n" +
                                  "AND OTHER MAGICAL ARTIGACTS ALL NOW   \n" +
                                  "BROKEN.                               \n" +
                                  "AS YOU ENTERD THE ROOM, A PANEL ON THE\n" +
                                  "DOOR SLAMMED SHUT.\r\nTHEN IT GLOWED A\n" +
                                  "PALE BLUE. NO ONE CAN PRY IT OPEN.    \n" +
                                  "NEXT, THE DOOR ON THE OTHER SIDE OF   \n" +
                                  "THE ROOM STARTS TO GLOW BRIGHT ORANGE,\n" +
                                  "INVITING THE PARTY TO USE IT.");
        }
        if (_string == "Message8")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A SIGN OF THE DOOR READS:             \n" +
                                  "PRIVATE EXPRESS SERVICE ELEVATOR!     \n" +
                                  "AUTHORIZED USERS ONLY!");
        }
        if (_string == "Alarm_Trap")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("SUDDENLY, A LOUD ALARM BELL CAN BE    \n" +
                                  "HEARD.                                \n" +
                                  "THEN THE BELLS STOP, AND ARE REPLACED \n" +
                                  "BY THE SOUNDS OF GUARDIAN MONSTERS!");
            if (!GameManager.PARTY.mem.Contains("|Alarm_Encounter|")) GameManager.PARTY.mem += "|Alarm_Encounter|";
        }
        if (_string == "Alarm_Encounter")
        {
            if (GameManager.PARTY.mem.Contains("|Alarm_Encounter|"))
                GameManager.PARTY.mem = GameManager.PARTY.mem.Replace("|Alarm_Encounter|", "");
            //TO-DO - Forced encounter
        }
        if (_string == "Bear_Lock")
        {
            if (GameManager.PARTY.PartyHasItemCheck(95)) return;
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A LARGE SLIDING WALL WITH THE IMAGE OF\n" +
                                  "A BEAR UPON IT BLOCKS YOUR PATH AND   \n" +
                                  "FORCES YOU OUT OF THE ROOM.");
            if (_dungeon._player.facing == Enum._Direction.north) _dungeon._player.WarpPlayer(new Vector3(18, 0, -6), Enum._Direction.north);
            if (_dungeon._player.facing == Enum._Direction.south) _dungeon._player.WarpPlayer(new Vector3(18, 0, -4), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();

        }
        if (_string == "Frog_Lock")
        {
            if (GameManager.PARTY.PartyHasItemCheck(96)) return;
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A LARGE SLIDING WALL WITH THE IMAGE OF\n" +
                                  "A frog UPON IT BLOCKS YOUR PATH AND   \n" +
                                  "FORCES YOU OUT OF THE ROOM.");
            if (_dungeon._player.facing == Enum._Direction.north) _dungeon._player.WarpPlayer(new Vector3(18, 0, -9), Enum._Direction.north);
            if (_dungeon._player.facing == Enum._Direction.south) _dungeon._player.WarpPlayer(new Vector3(18, 0, -7), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
        if (_string == "BlueRibbon_Lock")
        {
            if (GameManager.PARTY.PartyHasItemCheck(100)) return;
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("YOU ALL FALL ASLEEP! LATER, EVERYONE  \n" +
                                  "WAKES UP, FEELING SICK AND WEAK.      \n" +
                                  "YOU ARE BACK OUTSIDE THE DOOR YOU     \n" +
                                  "TRIED TO ENTER.");            
            _dungeon._player.WarpPlayer(new Vector3(11, 0, -17), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
        if (_string == "Gold_Lock")
        {
            if (GameManager.PARTY.PartyHasItemCheck(99)) return;
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("As you approach this door, a golden   \n" +
                                  "light spills from it, blinding you!   \n" +
                                  "You are forced to step back, away from\n" +
                                  "the light.");
            if (_dungeon._player.facing == Enum._Direction.north) _dungeon._player.WarpPlayer(new Vector3(18, 0, -11), Enum._Direction.north);
            if (_dungeon._player.facing == Enum._Direction.south) _dungeon._player.WarpPlayer(new Vector3(4, 0, -9), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
        if (_string == "BlueRibbon_Key")
        {
            if (!GameManager.PARTY.PartyHasItemCheck(100))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("AS THE PARTY ENTERS THE ROOM, THE DOOR\n" +
                                      "SLAMS SHUT, GLOWS ORANGE AND VANISHES!\n" +
                                      "A DOOR APPEARS TO THE RIGHT, A VOICE, \n" +
                                      "COMING FROM THE CEILING, ADDRESSES YOU\n" +
                                      "\"CONGRATULATIONS, BRAVE ADVENTURERS!\"");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "", "");
                _dungeon.SetButtonText("2", "Continue", "Special:BlueRibbon_Key2");
            }
            else
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("The room is the same as before, but  \n" +
                                      "it is empty now. It is also dusty and\n" +
                                      "smells rather musty.");
            }
        }
        if (_string == "BlueRibbon_Key2")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("\"TODAY YOU HAVE PROVED YOURSELF TO ME\n" +
                                  "AND ARE NOW READY TO START YOU QUEST!\n" +
                                  "YEARS AGO, MY AMULET WAS STOLEN BY   \n" +
                                  "THE EVIL WIZARD WERDNA.              \n" +
                                  "WERDNA HIDES IN THE MAZE SOMEWHERE   \n" +
                                  "BELOW US.                            \n" +
                                  "FIND HIM SLAY HIM AND RETURN TO ME   \n" +
                                  "THE AMULET!");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "", "");
            _dungeon.SetButtonText("2", "Continue", "Special:BlueRibbon_Key3");
        }
        if (_string == "BlueRibbon_Key3")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("TO AID YOU IN YOUR QUEST, I WILL GIVE \n" +
                                  "YOU A BLUE RIBBON, WHICH MAY BE USED  \n" +
                                  "TO ENTER THE ELEVATOR ON THIS FLOOR.  \n" +
                                  "GO NOW - AND MAY THE GODS GUIDE YOU!\"");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "", "");
            _dungeon.SetButtonText("2", "Continue", "Special:BlueRibbon_Key4");
        }
        if (_string == "BlueRibbon_Key4")
        {
            _dungeon._player.WarpPlayer(new Vector3(11, 0, -10), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();

            Character_Class _me = null;
            for (int i = 0; i < 6; i++)
                if (_me == null && !GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).HasEmptyInventorySlot())
                    _me = GameManager.PARTY.LookUp_PartyMember(i);
            if (_me == null)
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("You try to store the blue ribbon, but \n" +
                                      "you don't have any room and it is lost");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "", "");
                _dungeon.SetButtonText("2", "OK", "cancel");
                return;
            }
            _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(100);
        }
        if(_string == "GuardianBattle")
        {
            if (GameManager.PARTY.mem.Contains("|DefeatedLvl4Guardians|")) return;
            if (GameManager.PARTY.mem.Contains("|FoughtLvl4Guardians|"))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("AS THE PARTY ENTERS THE ROOM, They are/n" +
                                      "faced by the Dungeon Guardians again.");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "", "");
                _dungeon.SetButtonText("2", "Continue", "GuardianBattle2");
            }
            else
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("AS THE PARTY ENTERS THE ROOM, They are\n" +
                                      "greeted by a cadre of deadly-looking  \n" +
                                      "soldiers!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "", "");
                _dungeon.SetButtonText("2", "Continue", "GuardianBattle2");
            }
        }
        if(_string == "GuardianBattle2")
        {
            //TO DO - GUARDIAN BATTLE            
        }
    }
}
