using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level2_Logic : Level_Logic_Template
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
            GameManager.PARTY._PartyXYL = new Vector3Int(0, 10, 1);
            GameManager.PARTY.facing = Enum._Direction.east;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon1");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(0, 0, 3);
            GameManager.PARTY.facing = Enum._Direction.west;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon3");
        }

        if(_string == "Silver_Lock")
        {
            if (!GameManager.PARTY.PartyHasItemCheck(98))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("AS YOU ENTER THIS ROOM, A SILVERY FOG \n" +
                                      "ENVELOPS YOU.                         \n" +
                                      "TERRIBLE DEMONS FORM ALL AROUND YOU.  \n" +
                                      "IN TERROR, YOU FLEE FROM THE ROOM!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "Continue", "Special:Silver_Lock2");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if(_string == "Silver_Lock2")
        {
            _dungeon._player.WarpPlayer(new Vector3(9, 0, -9), Enum._Direction.north);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
        if(_string == "Bronze_Lock")
        {
            if (!GameManager.PARTY.PartyHasItemCheck(97))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("AS YOU ENTER THIS ROOM, A BRONZE,     \n" +
                                      "SMOKE FILLS IT.\r\nYOU FEEL COMPELLED \n" +
                                      "TO LEAVE IMMEDIATELY!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "Continue", "Special:Bronze_Lock2");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if(_string == "Bronze_Lock2")
        {
            _dungeon._player.WarpPlayer(new Vector3(9, 0, -12), Enum._Direction.south);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
        if(_string == "Bear_Statue")
        {
            Character_Class _me = null; string _char = "";
            for (int i = 0; i < 6; i++)
                if (_me == null && !GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).HasEmptyInventorySlot())
                    _me = GameManager.PARTY.LookUp_PartyMember(i);
            if (_me != null)
            {
                _char = _me.name + " picks up the statue.";
            }
            else
            {
                _char = "NO One has room to carry the statue.";
            }

            if (!GameManager.PARTY.PartyHasItemCheck(95))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("YOU SEE A STATUE OF A BEAR ON A       \n" +
                                      "PEDESTAL.                             \n" +
                                      "ON THE WALL IS A SIGN READING:        \n" +
                                      "      \"I'VE GOT A MILLION OF'EM\".   \n" +
                                      "\n" + _char);
                if (_me != null) _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(95);
            }
            else
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("YOU SEE An empty PEDESTAL.            \n" +
                                      "ON THE WALL IS A SIGN READING:        \n" +
                                      "      \"I'VE GOT A MILLION OF'EM\".");

            }
        }
        if (_string == "Frog_Statue")
        {
            Character_Class _me = null; string _char = "";
            for (int i = 0; i < 6; i++)
                if (_me == null && !GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).HasEmptyInventorySlot())
                    _me = GameManager.PARTY.LookUp_PartyMember(i);
            if (_me != null)
            {
                _char = _me.name + " picks up the statue.";
            }
            else
            {
                _char = "NO One has room to carry the statue.";
            }

            if (!GameManager.PARTY.PartyHasItemCheck(96))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("ON A SILVERY DISK STANDS A STATUE OF  \n" +
                                      "A FROG WEARING A RED AND BLUE CAPE.   \n" +
                                      "THE STATUE ANIMATES AND SHAKES ITS    \n" +
                                      "LEGS WHILE IT YELLS \"YEAH .. YEAH ..\"\n" +
                                      "\n" + _char);
                if (_me != null) _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(96);
            }
            else
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("YOU SEE An empty Silvery Disk.       \n" +
                                      "You think you hear a faint echo...   \n" +
                                      "                     \"...Yeah!...\"");
            }
        }
        if(_string == "Bear_Lock")
        {
            if (!GameManager.PARTY.PartyHasItemCheck(95))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("The door will not open, there is a    \n" +
                                      "symbol on it that looks like a strange\n" +
                                      "bear...");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "Special:Bear_Lock2");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if(_string == "Bear_Lock2")
        {
            _dungeon._player.WarpPlayer(new Vector3(5, 0, -10), Enum._Direction.north);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }

        if (_string == "Frog_Lock")
        {
            if (!GameManager.PARTY.PartyHasItemCheck(96))
            {
                //                     01234567890123456789012345678901234567
                _dungeon.UpdateMessge("The door will not open, there is a    \n" +
                                  "symbol on it that looks like a strange\n" +
                                  "frog...");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "Special:Frog_Lock2");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if (_string == "Frog_Lock2")
        {
            _dungeon._player.WarpPlayer(new Vector3(5, 0, -9), Enum._Direction.north);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }

        if (_string == "Gold_Key")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("IN THIS ROOM IS A STATUE OF A MONSTER \n" +
                                  "WITH THE BODY OF A CHICKEN AND THE    \n" +
                                  "HEAD OF A CAT.                        \n" +
                                  "THE STATUE IS MADE OF BRONZE, AND LIES \n" +
                                  "ON AN ONYX PEDESTAL.\r\nTHERE ARE UNUS-\n" +
                                  "UAL RUNES ON A PLAQUE THEREON.         \n\n" +
                                  "Would you like to look around?");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "YES", "Special:Gold_Key2");
            _dungeon.SetButtonText("2", "NO", "cancel");
        }
        if (_string == "Gold_Key2")
        {
            if (GameManager.PARTY.PartyHasItemCheck(99))
            {   //                         01234567890123456789012345678901234567
                _dungeon.UpdateMessge("\n\n      You find nothing of value       ");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "cancel");
                _dungeon.SetButtonText("2", "", "");
                return;
            }
            Character_Class _me = null;
            for (int i = 0; i < 6; i++)
                if (_me == null && !GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).HasEmptyInventorySlot())
                    _me = GameManager.PARTY.LookUp_PartyMember(i);
            if (_me != null)
            {
                //                         01234567890123456789012345678901234567
                _dungeon.UpdateMessge("\n\n        You find a Gold Key!       \n" +
                                           _me.name + " takes it.");
                _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(99);
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "cancel");
                _dungeon.SetButtonText("2", "", "");
            }
            else
            {
                //                         01234567890123456789012345678901234567
                _dungeon.UpdateMessge("\n\n     There is a Gold Key here       \n" +
                                          "   But no one has any room for it!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "cancel");
                _dungeon.SetButtonText("2", "", "");
            }
        }

        if(_string == "Message1")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A PLACARD NEAR THE GROUND READ:       \n" +
                                  "              \"A DUNGEON'S DARK ...\"");
        }
        if(_string == "Message2")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A PLACARD NEAR THE GROUND READ:       \n" +
                                  "              \"WHEN IT's NOT LIT...\"");
        }
        if(_string == "Message3")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A PLACARD NEAR THE GROUND READ:       \n" +
                                  "           \"WATCH OUT, OR YOU'LL...\"");
        }
    }
}
