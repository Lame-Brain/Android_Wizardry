using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Level10_Logic : Level_Logic_Template
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
        
        if(_string == "Message1")
        {
                                 //01234567890123456789012345678901234567
            _dungeon.UpdateMessge("INSCRIBED ON AN ORNATE, GOLD PLAQUE IS \n" +
                                  "A MESSAGE. THE SCRIPT IS ORNATE, AND   \n" +
                                  "BLINKS IN VARIOUS COLORS:              \n" +
                                  "\"YOU ARE TRESPASSING ON THE DOMAIN OF\n" +
                                  "            ** WERDNA **.             \n" +
                                  "THERE IS NO POSSIBILITY THAT YOU CAN  \n" +
                                  "GET PAST MY GUARDIANS!                \n" +
                                  "SO SURE AM I OF MY DEFENSES THAT I GIVE\n " +
                                  "YOU THIS CLUE: \"                     \n" +
                                  "\"CONTRA-DEXTRA AVENUE\"\n\n\nPS - TREBOR SUX!");
        }
        if(_string == "Message2")
        {
                                 //01234567890123456789012345678901234567
            _dungeon.UpdateMessge("You hear a whispering voice coming from\n" +
                                  "the walls themselves:                 \n" +
                                  "\"ONE GROUP OF GUARDIANS HAVE BEEN    \n" +
                                  "DEFEATED, BUT THERE ARE MANY MORE.    \n" +
                                  "TURN BACK WHILE YOU CAN, FOOLS!\"");
        }
        if(_string == "Message3")
        {
                                 //01234567890123456789012345678901234567
            _dungeon.UpdateMessge("A DISCREET SIGN ON THE WALL READS:    \n" +
                                  "LAIR OF THE EVIL WIZARD WERADNA! OFFICE\n" +
                                  "HOURS 9AM TO 3PM BY APPOINTMENT ONLY  \n" +
                                  "THE WIZARD IS * IN *!");
        }
        if (_string == "Werdnas_Town_Portal")
        {                        //01234567890123456789012345678901234567
            _dungeon.UpdateMessge("Strange energies swirl for a moment...\n\n" +
                                  "Suddenly, you feel yourself yanked into\n" + 
                                  "the Sky!\n" + 
                                  "After a terrifying moment, you find \n" + 
                                  "back in the castle.");
            _dungeon.SetButtonText("u", "", "");
            _dungeon.SetButtonText("r", "", "");
            _dungeon.SetButtonText("d", "", "");
            _dungeon.SetButtonText("l", "", "");
            _dungeon.SetButtonText("1", "OK", "Special:Werdnas_Town_Portal2");
            _dungeon.SetButtonText("2", "", "");
        }
        if (_string == "Werdnas_Town_Portal2")
        {
            GameManager.instance.SaveGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
        }
        if(_string == "backtostart")
        {
            _dungeon._player.WarpPlayer(new Vector3(1,0,-20), Enum._Direction.north);
            _dungeon.SetButtonText("u", "Up", "move_forward");
            _dungeon.SetButtonText("d", "down", "turn_around");
            _dungeon.SetButtonText("l", "left", "turn_left");
            _dungeon.SetButtonText("r", "right", "turn_right");
            _dungeon.SetButtonText("2", "Camp", "make_camp");
            _dungeon.SetContextButton();
        }
    }
}
