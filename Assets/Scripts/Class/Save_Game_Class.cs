using System.Collections;
using System.Collections.Generic;

[System.Serializable]
public class Save_Game_Class 
{
    public List<string> SG_ROSTER;
    public bool[,,] SG_map;
    public int SG_TempleFavor;
    public int[] SG_BoltacStock;
    public string SG_mem;

    public Save_Game_Class(int stock_length)
    {
        SG_ROSTER = new List<string>();
        SG_ROSTER.Clear();
        SG_map = new bool[20, 20, 10];
        for (int z = 0; z < 10; z++)
            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 20; x++)
                    SG_map[x, y, z] = false;
        SG_BoltacStock = new int[stock_length];
        for (int i = 0; i < stock_length; i++)
            SG_BoltacStock[i] = -10;
    }
}
