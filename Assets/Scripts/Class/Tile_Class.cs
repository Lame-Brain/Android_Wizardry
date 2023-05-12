using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile_Class : MonoBehaviour
{
    public int[] Wall; // 0 index = north, 1 index = east, 2 index = south, 3 index = west,
                       // 0 value = no wall, 1 value = door, 2 value = secret door (hidden), 3 value = secret door (revealed), 4 value = wall
    public int feature; // 0 = none, 1 = darkness, 2 = rock (dead), 3 = spinner, 4 = anti-magic, 5 = stairs going up, 6 = stairs going down, 7 = chute, 8 = elevator, 9 = pit, 10 = elevator2 
    public bool isSpecial;
    public string special_code; //triggers special encounters
    public bool isWarp;
    public Vector3Int warp; //location this tile warps you to. (may change levels)
    public int warp_facing;
    public Vector2Int Map_Coordinates;
    public Vector2Int Game_Coordinates;
    public GameObject ceilng, floor, north_wall, east_wall, south_wall, west_wall;
    public int Room_Number;
}
