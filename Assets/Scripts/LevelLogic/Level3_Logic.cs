using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level3_Logic : Level_Logic_Template
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
            GameManager.PARTY._PartyXYL = new Vector3Int(16, 15, 2);
            GameManager.PARTY.facing = Enum._Direction.south;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon2");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(10, 18, 4);
            GameManager.PARTY.facing = Enum._Direction.north;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon3");
        }

        if ( _string == "Message1")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("THE FLOOR HAS A MOSAIC READING:       \n" +
                                  "                      \"TURN AROUND.\"");
        }
        if ( _string == "Message2")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("THE FLOOR HAS A MOSAIC READING:       \n" +
                                                          "\"TURN LEFT.\"");
        }
        if ( _string == "Message3")
        {
            //                     01234567890123456789012345678901234567
            _dungeon.UpdateMessge("THE FLOOR HAS A MOSAIC READING:       \n" +
                                  "                       \"TURN RIGHT.\"");
        }

    }
}
