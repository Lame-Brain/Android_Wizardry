using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level6_Logic : Level_Logic_Template
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
            GameManager.PARTY._PartyXYL = new Vector3Int(8, 8, 5);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon5");
        }
        if (_string == "go_down")
        {
            GameManager.PARTY._MakeCampOnLoad = false;
            GameManager.instance.SaveGame();
            GameManager.PARTY._PartyXYL = new Vector3Int(0, 19, 7);
            GameManager.PARTY.facing = Enum._Direction.none;
            UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon7");
        }
        if (_string == "Easter_Egg")
        {
            if (!GameManager.PARTY.mem.Contains("|Thundarr|") && Random.Range(0, 100) == 100)
            {
                GameManager.PARTY.mem += "|Thundarr|";
                _dungeon.UpdateMessge("SUDDENLY, YOU SEE A GROUP OF 3 HUMAN-\n" +
                                      "OIDS. ONE IS A BARBARIAN WITH A SWORD\n" +
                                      "THAT GLOWS, ONE IS A SEXY FEMALE MAGE,\n" +
                                      "AND THE THIRD LOOKS LIKE A HUGE OGRE!");
                _dungeon.SetButtonText("u", "", "");
                _dungeon.SetButtonText("r", "", "");
                _dungeon.SetButtonText("d", "", "");
                _dungeon.SetButtonText("l", "", "");
                _dungeon.SetButtonText("1", "", "");
                _dungeon.SetButtonText("2", "Continue", "Special:Easter_Egg2");
            }
            else
            {
                GameManager.PARTY.mem = GameManager.PARTY.mem.Replace("|Thundarr|", "");
            }
        }
        if (_string == "Easter_Egg2")
        {     
            _dungeon.UpdateMessge("THE BARBARIAN POINTS TO THE END OF THE\n" +
                                  "CORRIODOR AND YELLS \"ARIEL.. OOKLA..\n" +
                                  "THIS WAY!!\" AND THEY ALL RUN OFF!");
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
    }
}
