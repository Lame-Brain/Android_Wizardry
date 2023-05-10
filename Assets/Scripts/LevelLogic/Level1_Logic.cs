using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEditor.ObjectChangeEventStream;

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
        if(_string == "Sign")
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
    }
}
