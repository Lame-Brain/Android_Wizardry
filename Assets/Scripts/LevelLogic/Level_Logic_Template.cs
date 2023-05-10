using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Logic_Template : MonoBehaviour
{
    public Level_Builder _builder;
    public Tile_Class[,] Map;

    /*
    private void Start()
    {
        _builder = FindObjectOfType<Level_Builder>();
        Map = new Tile_Class[22, 22];
        for (int y = 0; y < 22; y++)
            for (int x = 0; x < 22; x++)
                Map[x, y] = _builder.transform.GetChild(x * 22 + y).GetComponent<Tile_Class>();
    }
    */

}
