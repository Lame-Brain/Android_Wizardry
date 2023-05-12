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
                    host.Map[x, y] = _go.GetComponent<Tile_Class>();
                    host.Map[x, y].Map_Coordinates = new Vector2Int(x, y);
                    host.Map[x, y].Game_Coordinates = new Vector2Int(x - 1, (y * -1) + 20);
                    _go.name = "Tile [" + host.Map[x, y].Game_Coordinates.x + ", " + host.Map[x, y].Game_Coordinates.y + "]";
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
            //parse Rooms
            int[,] _roomData = new int[25, 25];
            string[] _roomLines = host.roomdata.text.Split("\n");
            for (int y = 0; y < _roomLines.Length; y++)
            {
                string[] _roomRows = _roomLines[y].Split(",");
                for (int x = 0; x < _roomRows.Length; x++)
                {
                    int _int = -1;
                    int.TryParse(_roomRows[x], out _int);
                    _roomData[x, y] = _int;
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
                            host.Map[x, y].north_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 1)
                        {
                            host.Map[x, y].east_wall.SetActive(true);
                            host.Map[x, y].Wall[1] = 4;
                            host.Map[x, y].east_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 2)
                        {
                            host.Map[x, y].south_wall.SetActive(true);
                            host.Map[x, y].Wall[2] = 4;
                            host.Map[x, y].south_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 3)
                        {
                            host.Map[x, y].west_wall.SetActive(true);
                            host.Map[x, y].Wall[3] = 4;
                            host.Map[x, y].west_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 4) // darkness tile
                        {
                            host.Map[x, y].feature = 1;
                        }
                        if (_rawData[x, y, z] == 5)
                        {
                            host.Map[x, y].north_wall.SetActive(true);
                            host.Map[x, y].Wall[0] = 1;
                            host.Map[x, y].north_wall.GetComponent<MeshRenderer>().material = host.mat[1];
                        }
                        if (_rawData[x, y, z] == 6)
                        {
                            host.Map[x, y].east_wall.SetActive(true);
                            host.Map[x, y].Wall[1] = 1;
                            host.Map[x, y].east_wall.GetComponent<MeshRenderer>().material = host.mat[1];
                        }
                        if (_rawData[x, y, z] == 7)
                        {
                            host.Map[x, y].south_wall.SetActive(true);
                            host.Map[x, y].Wall[2] = 1;
                            host.Map[x, y].south_wall.GetComponent<MeshRenderer>().material = host.mat[1];
                        }
                        if (_rawData[x, y, z] == 8)
                        {
                            host.Map[x, y].west_wall.SetActive(true);
                            host.Map[x, y].Wall[3] = 1;
                            host.Map[x, y].west_wall.GetComponent<MeshRenderer>().material = host.mat[1];
                        }
                        if (_rawData[x, y, z] == 9) // Rock tile
                        {
                            host.Map[x, y].feature = 2;
                        }
                        if (_rawData[x, y, z] == 10)
                        {
                            host.Map[x, y].north_wall.SetActive(true);
                            host.Map[x, y].Wall[0] = 2;
                            host.Map[x, y].north_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 11)
                        {
                            host.Map[x, y].east_wall.SetActive(true);
                            host.Map[x, y].Wall[1] = 2;
                            host.Map[x, y].east_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 12)
                        {
                            host.Map[x, y].south_wall.SetActive(true);
                            host.Map[x, y].Wall[2] = 2;
                            host.Map[x, y].south_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 13)
                        {
                            host.Map[x, y].west_wall.SetActive(true);
                            host.Map[x, y].Wall[3] = 2;
                            host.Map[x, y].west_wall.GetComponent<MeshRenderer>().material = host.mat[0];
                        }
                        if (_rawData[x, y, z] == 15) // Anti-Magic tile
                        {
                            host.Map[x, y].feature = 4;
                        }
                        if (_rawData[x, y, z] == 16) // Teleport tile
                        {
                            host.Map[x, y].isWarp = true;
                        }
                        if (_rawData[x, y, z] == 17) // Special message or encounter
                        {
                            host.Map[x, y].isSpecial = true;
                            host.Map[x, y].floor.GetComponent<MeshRenderer>().material = host.mat[2];
                        }
                        if (_rawData[x, y, z] == 18) // Stairs Up
                        {
                            host.Map[x, y].feature = 5;
                            host.Map[x, y].ceilng.GetComponent<MeshRenderer>().material = host.mat[2];
                        }
                        if (_rawData[x, y, z] == 19) // Stairs Down
                        {
                            host.Map[x, y].feature = 6;
                            host.Map[x, y].floor.GetComponent<MeshRenderer>().material = host.mat[2];
                        }
                        if (_rawData[x, y, z] == 20) // Chute
                        {
                            host.Map[x, y].feature = 7;
                        }
                        if (_rawData[x, y, z] == 21) // Elevator
                        {
                            host.Map[x, y].feature = 8;
                            host.Map[x, y].ceilng.GetComponent<MeshRenderer>().material = host.mat[2];
                            host.Map[x, y].floor.GetComponent<MeshRenderer>().material = host.mat[2];
                        }
                        if (_rawData[x, y, z] == 22) // Spinner
                        {
                            host.Map[x, y].feature = 3;
                        }
                        if (_rawData[x, y, z] == 23) // Pit
                        {
                            host.Map[x, y].feature = 9;
                        }
                    }
            //Apply Darkness
            for (int y = 0; y < 22; y++)
                for (int x = 0; x < 22; x++)
                {
                    host.Map[x, y].Room_Number = _roomData[x, y];

                    if (host.Map[x, y].feature == 1)
                    {
                        if (!host.Map[x, y - 1].south_wall.activeSelf)
                        {
                            host.Map[x, y - 1].south_wall.SetActive(true);
                            host.Map[x, y - 1].south_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        }
                        if (!host.Map[x + 1, y].west_wall.activeSelf)
                        {
                            host.Map[x + 1, y].west_wall.SetActive(true);
                            host.Map[x + 1, y].west_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        }
                        if (!host.Map[x, y + 1].north_wall.activeSelf)
                        {
                            host.Map[x, y + 1].north_wall.SetActive(true);
                            host.Map[x, y + 1].north_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        }
                        if (!host.Map[x - 1, y].east_wall.activeSelf)
                        {
                            host.Map[x - 1, y].east_wall.SetActive(true);
                            host.Map[x - 1, y].east_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        }
                        host.Map[x, y].north_wall.SetActive(true); host.Map[x, y].north_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        host.Map[x, y].east_wall.SetActive(true); host.Map[x, y].east_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        host.Map[x, y].south_wall.SetActive(true); host.Map[x, y].south_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                        host.Map[x, y].west_wall.SetActive(true); host.Map[x, y].west_wall.GetComponent<MeshRenderer>().material = host.mat[3];
                    }
                }
        }


    }
}