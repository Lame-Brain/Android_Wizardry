using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using BlobberEngine;

public class Level1_Logic : Level_Logic_Template
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
        if(_string == "go_up")
        {
            GameManager.instance.SaveGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(12, 7, 2);
            GameManager.PARTY.facing = Enum._Direction.west;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon2");
        }
        if (_string == "Sign")
        {
            Debug.Log("It worked?");                                  //0123456789012345678901234567890123456789
                                             _dungeon.UpdateMessge("\n\n   A LARGE SIGN ON THE WALL READS:\n" +
                                                                       "     ** AREA OUT OF BOUNDS! **");
        }
        if(_string == "Town_Portal")
        {                                                         //0123456789012345678901234567890123456789
                                             _dungeon.UpdateMessge("     \"MAPIRO MAHAMA DIROMAT!!\"\n" +
                                                                   "A STRANGE GLOW SEEMS TO EMANATE FROM\n" +
                                                                   "THIS ROOM.\n" +
                                                                   "IN THE CENTER, A SMALLISH MAN IN A\n" +
                                                                   "LONG ROBE TURNS TOWARDS THE PARTY\n" +
                                                                   "AND SHOUTS: \"BEGONE, STRANGERS!\"\n\n" +
                                                                   "HE SLOWLY WAVES HIS HANDS, AND CHANTS\n" +
                                                                   "      \"MAPIRO MAHAMA DIROMAT!!\"");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "OK", "Special:Town_Portal2");
            _dungeon.SetButtonText("2", "", "");
        }
        if (_string == "Town_Portal2")
        {
            GameManager.instance.SaveGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
        }
        if(_string == "Murphys_Ghosts")
        {                        //0123456789012345678901234567890123456789
            _dungeon.UpdateMessge("THERE IS A GEM ENCRUSTED STATUE OF A \n" +
                                  "HOODED MAN HERE.\n" +
                                  "A GOLDEN LIGHT EMANATES FROM THE HOOD.\n\n" +
                                  "IN FRONT OF THE STATUE IS AN ALTAR, IN \n" +
                                  "WHICH PUNGENT INCENSE IS BURNING.\n\n" +
                                  "SEARCH THE ALTAR? <yes/No>");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "YES", "Special:Murphys_Ghosts2");
            _dungeon.SetButtonText("2", "NO", "cancel");
        }
        if (_string == "Murphys_Ghosts2")
        {

        }
        if (_string == "Bronze_Key")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("IN THIS ROOM IS A STATUE OF A MONSTER \n" +
                                  "WITH THE BODY OF A CHICKEN AND THE    \n" +
                                  "Head OF A CAT.                        \n" +
                                  "THE STATUE IS MADE OF BRONZE, AND LIES\n" +
                                  "ON AN ONYX PEDESTAL.                  \n" +
                                  "THERE ARE UNUSUAL RUNES ON A PLAQUE   \n" +
                                  "THEREON.                              \n\n" +
                                  "Do you want to look around? <yes/No>");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "YES", "Special:Bronze_Key2");
            _dungeon.SetButtonText("2", "NO", "cancel");
        }
        if (_string == "Bronze_Key2")
        {
            if (GameManager.PARTY.PartyHasItemCheck(97))
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
                _dungeon.UpdateMessge("\n\n        You find a Bronze Key!       \n" +
                                           _me.name + " takes it.");
                _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(97);
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
                _dungeon.UpdateMessge("\n\n     There is a Bronze Key here       \n" +
                                          "   But no one has any room for it!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "cancel");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if (_string == "Silver_Key")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("IN THIS ROOM IS A SILVER STATUE OF A  \n" +
                                  "BOAR. WITH HORNS AND LONG FANGS.      \n" +
                                  "ON THE WALL BY THE STATUE IS A Partia-\n" +
                                  "ALLY OBSCURED Message THAT APPEARS TO \n" +
                                  "HAVE BEEN LEFT BY PASSING ELVES.      \n" +
                                  "IT IS HARDLY LEGIBLE, BUT seems to be \n" +
                                  "a WARNING ABOUT GHOSTS AND DEMONS.    \n\n"+
                                  "Do you want to look around? <yes/No>");

            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "YES", "Special:Silver_Key2");
            _dungeon.SetButtonText("2", "NO", "cancel");
        }
        if (_string == "Silver_Key2")
        {
            if (GameManager.PARTY.PartyHasItemCheck(98))
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
                _dungeon.UpdateMessge("\n\n        You find a Silver Key!       \n" +
                                           _me.name + " takes it.");
                _me.Inventory[_me.GetEmptyInventorySlot()] = new Item(98);
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
                _dungeon.UpdateMessge("\n\n     There is a Silver Key here       \n" +
                                          "   But no one has any room for it!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "OK", "cancel");
                _dungeon.SetButtonText("2", "", "");
            }
        }
        if (_string == "PoR_Reference")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("There is a sign on the wall, it says: \n" +
                                  "What were you expecting? A bow and    \n" +
                                  "20 magic arrows?\n\n");
        }       
    }
}
