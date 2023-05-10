using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Builder : MonoBehaviour
{
    public GameObject tilePrefab;
    public TextAsset leveldata;
    public Tile_Class[,] Map;
    public Material[] mat;
    public Level_Logic_Template _level;
}
