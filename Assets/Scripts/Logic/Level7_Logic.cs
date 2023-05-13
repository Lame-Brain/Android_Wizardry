using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level7_Logic : Level_Logic_Template
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
            GameManager.PARTY._PartyXYL = new Vector3Int(9, 18, 6);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon6");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(0, 0, 8);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon8");
        }
        if (_string == "Warp_Zone")
        {
            //teleports to 20,0,-8 and rotates clockwise
            Enum._Direction _newDir = Enum._Direction.none;
            switch (_dungeon._player.facing) 
            { 
                case Enum._Direction.north:
                    _newDir = Enum._Direction.east; break;
                case Enum._Direction.east:
                    _newDir = Enum._Direction.south; break;
                case Enum._Direction.south: 
                    _newDir = Enum._Direction.west; break;
                case Enum._Direction.west:
                    _newDir = Enum._Direction.north; break;
            }
            _dungeon._player.WarpPlayer(new Vector3(20, 0, -8), _newDir);
        }
        if( _string == "Message1")
        {
            _dungeon.UpdateMessge("ITS GETTING WARM AROUND HERE!");
        }
        if( _string == "Message2")
        {
            _dungeon.UpdateMessge("ITS GETTING REALLY HOT!");
        }
        if( _string == "Message3")
        {
            _dungeon.UpdateMessge("ITS ALMOST TOO HOT TO BEAR!");
        }
        if(_string == "Dragon")
        {
            //TO DO Fire Dragon Encounter
        }
    }
}
