using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level5_Logic : Level_Logic_Template
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
            GameManager.PARTY._PartyXYL = new Vector3Int(17, 12, 4);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon4");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(8, 8, 6);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon3");
        }
    }
}
