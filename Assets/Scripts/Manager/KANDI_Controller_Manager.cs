using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KANDI_Controller_Manager : MonoBehaviour
{
    public Sprite[] DUMAPIC_tile;
    public TMPro.TextMeshProUGUI title, done_btn;
    public Transform[] vert_panel;
    private Transform[] horz_panel;
    private Transform[,] tile;

    private Level_Logic_Template _level;
    private Player_Controller _player;

    private void OnEnable()
    {
        title.fontSize = GameManager.FONT;
        done_btn.fontSize = GameManager.FONT;
        _level = FindObjectOfType<Level_Logic_Template>();
        _player = FindObjectOfType<Player_Controller>();
        tile = new Transform[7,7];
        horz_panel = new Transform[7];
        for (int y = 0; y < 7; y++)//for (int y = 6; y >= 0; y--)
        {
            //Debug.Log("on -> " + vert_panel[y].name);
            for (int i = 0; i < 7; i++)
                horz_panel[i] = vert_panel[y].GetChild(i);
            for (int x = 0; x < 7; x++)
            {
                tile[x,y] = horz_panel[x].transform;
            }
        }
        

        //Draw the map
        Vector2Int _loc = _player.WhatRoomAmIin().Game_Coordinates; //Debug.Log("my coords " + _player.WhatRoomAmIin().Map_Coordinates);
        Debug.Log("locX = " + _loc.x + ", locY = " + _loc.y);
        for (int y = -3; y < 4; y++)//for (int y = 3; y >= -3; y--)
            for (int x = -3; x < 4; x++)
            {
                if (!tile[x + 3, y + 3].GetComponent<Image>()) 
                    tile[x + 3, y + 3].gameObject.AddComponent<Image>().sprite = DUMAPIC_tile[0];
                tile[x + 3, y + 3].gameObject.name = "Tile [" + (x + 3) + ", " + (y + 3) + "]";

                //draw tiles
                int _newX = (_loc.x + x) + 1, _newY = -1 * (_loc.y + y) + 20;
                if (_newX >= 0 && _newX <= 20 && _newY >= 0 && _newY <= 20)
                {
                    #region walls
                    if (_level.Map[_newX, _newY].Wall[0] == 4 || _level.Map[_newX, _newY].Wall[0] == 2)
                    {
                        Debug.Log("north wall " + _level.Map[_newX, _newY].name + " at " + (x + 3) + ", " + (y + 3));
                        GameObject _go = new GameObject("nwall[" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 1, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[1] == 4 || _level.Map[_newX, _newY].Wall[1] == 2)
                    {
                        Debug.Log("east wall " + _level.Map[_newX, _newY].name + " at " + (x + 3) + ", " + (y + 3));
                        GameObject _go = new GameObject("ewall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 2, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[2] == 4 || _level.Map[_newX, _newY].Wall[2] == 2)
                    {
                        Debug.Log("south wall " + _level.Map[_newX, _newY].name + " at " + (x + 3) + ", " + (y + 3));
                        GameObject _go = new GameObject("swall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 3, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[3] == 4 || _level.Map[_newX, _newY].Wall[3] == 2)
                    {
                        Debug.Log("west wall " + _level.Map[_newX, _newY].name + " at " + (x + 3) + ", " + (y + 3));
                        GameObject _go = new GameObject("wwall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 4, x, y);
                    }
                    #endregion
                    
                    if(_level.Map[_newX, _newY].isSpecial || _level.Map[_newX, _newY].isWarp)
                    {
                        GameObject _go = new GameObject("Special" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 5, x, y);
                    }

                    #region doors
                    if (_level.Map[_newX, _newY].Wall[0] == 1 || _level.Map[_newX, _newY].Wall[0] == 3)
                    {
                        GameObject _go = new GameObject("nwall[" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 6, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[1] == 1 || _level.Map[_newX, _newY].Wall[1] == 3)
                    {
                        GameObject _go = new GameObject("ewall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 7, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[2] == 1 || _level.Map[_newX, _newY].Wall[2] == 3)
                    {
                        GameObject _go = new GameObject("swall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 8, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[3] == 1 || _level.Map[_newX, _newY].Wall[3] == 3)
                    {
                        GameObject _go = new GameObject("wwall" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 9, x, y);
                    }
                    #endregion
                    #region corners
                    if ((_level.Map[_newX, _newY].Wall[0] != 0 && _newY - 1 > 0 && _level.Map[_newX, _newY - 1].Wall[3] != 0) ||
                        (_level.Map[_newX, _newY].Wall[3] != 0 && _newX - 1 > 0 && _level.Map[_newX - 1, _newY].Wall[0] != 0))
                    {
                        GameObject _go = new GameObject("nw_corner[" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 6, x, y);
                    }
                    #endregion

                    if (x == 0 && y == 0)
                    {
                        GameObject _go = new GameObject("Center_Marker" + (x + 3) + ", " + (y + 3) + "]");
                        SetTile(_go, 10, x, y);
                    }
                }
                // 0 value = no wall, 1 value = door, 2 value = secret door (hidden), 3 value = secret door (revealed), 4 value = wall
            }
    }
    private void OnDisable()
    {
        //clear tiles
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("DUMAPIC_tile")) Destroy(_go);
    }
    private void SetTile(GameObject _go, int _tileNum, int x, int y)
    {
        //x += 3; y += 3;
        _go.tag = "DUMAPIC_tile";
        _go.transform.SetParent(tile[x + 3, y + 3]);// Debug.Log(" w> " + _go.name);
        _go.AddComponent<Image>().sprite = DUMAPIC_tile[_tileNum];
        RectTransform _r = _go.GetComponent<RectTransform>();
        _r.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        _r.anchorMin = new Vector2(0, 0);
        _r.anchorMax = new Vector2(1, 1);
        _r.offsetMin = new Vector2(0, 0);
        _r.offsetMax = new Vector2(0, 0);
    }
}
