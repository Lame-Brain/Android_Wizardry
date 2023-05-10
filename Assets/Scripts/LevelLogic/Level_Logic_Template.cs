using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level_Logic_Template : MonoBehaviour
{
    public Level_Builder _builder;
    public Tile_Class[,] Map;

    public virtual void Special_Stuff(string _string) { }
}
