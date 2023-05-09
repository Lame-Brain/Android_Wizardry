using Codice.Client.BaseCommands;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Level_Builder))]

public class Level_Builder_Editor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        Level_Builder host = (Level_Builder)target;

        if (GUILayout.Button("Build Map"))
        {
            //init
            host.Map = new Tile_Class[22, 22];

            //delete old map data
            foreach (GameObject _go in GameObject.FindGameObjectsWithTag("Dungeon_Tile")) DestroyImmediate(_go);

            //Make new map data
            for (int y = 0; y < 22; y++)
                for (int x = 0; x < 22; x++)
                {
                    GameObject _go = Instantiate(host.tilePrefab, new Vector3(x, 0, -y), Quaternion.identity, host.transform);
                    _go.name = "Tile [" + x + ", " + y + "]";
                    host.Map[x, y] = _go.GetComponent<Tile_Class>();
                    host.Map[x, y].coordinates = new Vector2Int(x, y);
                    host.Map[x, y].Wall = new int[4];
                    for (int i = 0; i < 4; i++) host.Map[x, y].Wall[i] = 0;
                }


            int[,,] _rawData = new int[25, 25, 10];
            //parse the text data
            string[] blocks = host.leveldata.text.Split(";");
            for (int b = 0; b < blocks.Length; b++)
            {
                string[] lines = blocks[b].Split("\n");
                for (int y = 0; y < lines.Length; y++)
                {
                    string[] rows = lines[y].Split(",");
                    for (int x = 0; x < rows.Length; x++)
                    {
                        int _int = -1;
                        if (int.TryParse(rows[x], out _int))
                        {
                            _rawData[x, y, b] = _int;
                        }
                    }
                }
            }
            //Update walls and such
            for (int y = 0; y < 22; y++)
                for (int x = 0; x < 22; x++)

                    for (int z = 0; z < 5; z++)
                    {

                        if (_rawData[x, y, z] == 0)
                        {
                            host.Map[x, y].north_wall.SetActive(true);
                            host.Map[x, y].Wall[0] = 4;
                        }
                        if (_rawData[x, y, z] == 1)
                        {
                            host.Map[x, y].east_wall.SetActive(true);
                            host.Map[x, y].Wall[1] = 4;
                        }
                        if (_rawData[x, y, z] == 2)
                        {
                            host.Map[x, y].south_wall.SetActive(true);
                            host.Map[x, y].Wall[2] = 4;
                        }
                        if (_rawData[x, y, z] == 3)
                        {
                            host.Map[x, y].west_wall.SetActive(true);
                            host.Map[x, y].Wall[3] = 4;
                        }
                        if (_rawData[x, y, z] == 4) // darkness tile
                        {
                            host.Map[x, y].feature = 1;
                        }
                        if (_rawData[x, y, z] == 5)
                        {
                            host.Map[x, y].north_wall.SetActive(true);
                            host.Map[x, y].Wall[0] = 1;
                        }
                        if (_rawData[x, y, z] == 6)
                        {
                            host.Map[x, y].east_wall.SetActive(true);
                            host.Map[x, y].Wall[1] = 1;
                        }
                        if (_rawData[x, y, z] == 7)
                        {
                            host.Map[x, y].south_wall.SetActive(true);
                            host.Map[x, y].Wall[2] = 1;
                        }
                        if (_rawData[x, y, z] == 8)
                        {
                            host.Map[x, y].west_wall.SetActive(true);
                            host.Map[x, y].Wall[3] = 1;
                        }
                    }
        }


    }
}