using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DUMAPIC_Controller_Manager : MonoBehaviour
{
    public Sprite[] DUMAPIC_tile;
    public TMPro.TextMeshProUGUI title, done_btn;
    public Transform[] vert_panel;
    private Transform[] horz_panel;
    private Transform[,] tile;

    private Level_Logic_Template _level;
    private Player_Controller _player;
    private int full_scale, half_scale;

    private void OnEnable()
    {
        title.fontSize = GameManager.FONT;
        done_btn.fontSize = GameManager.FONT;
        _level = FindObjectOfType<Level_Logic_Template>();
        _player = FindObjectOfType<Player_Controller>();
        full_scale = 11;
        half_scale = 5;
        tile = new Transform[full_scale,full_scale];
        horz_panel = new Transform[full_scale];
        for (int y = 0; y < full_scale; y++)
        {            
            for (int i = 0; i < full_scale; i++)
                horz_panel[i] = vert_panel[y].GetChild(i);
            for (int x = 0; x < full_scale; x++)
            {
                tile[x,y] = horz_panel[x].transform;
            }
        }

        ClearMap();
        DrawMap();
        ReportPosiiton();
    }
    private void ClearMap()
    {
        //clear tiles
        foreach (GameObject _go in GameObject.FindGameObjectsWithTag("DUMAPIC_tile")) Destroy(_go);
    }
    private void DrawMap()
    {
        //Draw the map
        Vector2Int _loc = _player.WhatRoomAmIin().Game_Coordinates; //Debug.Log("my coords " + _player.WhatRoomAmIin().Map_Coordinates);
        Debug.Log("locX = " + _loc.x + ", locY = " + _loc.y);
        for (int y = -half_scale; y <= half_scale; y++)//for (int y = 3; y >= -3; y--)
            for (int x = -half_scale; x <= half_scale; x++)
            {
                if (!tile[x + half_scale, y + half_scale].GetComponent<Image>())
                    tile[x + half_scale, y + half_scale].gameObject.AddComponent<Image>().sprite = DUMAPIC_tile[0];
                tile[x + half_scale, y + half_scale].gameObject.name = "Tile [" + (x + half_scale) + ", " + (y + half_scale) + "]";

                //draw tiles
                int _newX = (_loc.x + x) + 1, _newY = -1 * (_loc.y + y) + 20;
                if (_newX >= 0 && _newX <= 20 && _newY >= 0 && _newY <= 20)
                {
                    #region walls
                    if (_level.Map[_newX, _newY].Wall[0] == 4 || _level.Map[_newX, _newY].Wall[0] == 2)
                    {
                        GameObject _go = new GameObject("nwall[" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 1, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[1] == 4 || _level.Map[_newX, _newY].Wall[1] == 2)
                    {
                        GameObject _go = new GameObject("ewall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 2, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[2] == 4 || _level.Map[_newX, _newY].Wall[2] == 2)
                    {
                        GameObject _go = new GameObject("swall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 3, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[3] == 4 || _level.Map[_newX, _newY].Wall[3] == 2)
                    {
                        GameObject _go = new GameObject("wwall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 4, x, y);
                    }
                    #endregion

                    if (_level.Map[_newX, _newY].isSpecial || _level.Map[_newX, _newY].isWarp)
                    {
                        GameObject _go = new GameObject("Special" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 5, x, y);
                    }

                    #region doors
                    if (_level.Map[_newX, _newY].Wall[0] == 1 || _level.Map[_newX, _newY].Wall[0] == 3)
                    {
                        GameObject _go = new GameObject("nwall[" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 6, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[1] == 1 || _level.Map[_newX, _newY].Wall[1] == 3)
                    {
                        GameObject _go = new GameObject("ewall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 7, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[2] == 1 || _level.Map[_newX, _newY].Wall[2] == 3)
                    {
                        GameObject _go = new GameObject("swall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 8, x, y);
                    }
                    if (_level.Map[_newX, _newY].Wall[3] == 1 || _level.Map[_newX, _newY].Wall[3] == 3)
                    {
                        GameObject _go = new GameObject("wwall" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        SetTile(_go, 9, x, y);
                    }
                    #endregion

                    if (x == 0 && y == 0)
                    {
                        GameObject _go = new GameObject("Center_Marker" + (x + half_scale) + ", " + (y + half_scale) + "]");
                        _go.tag = "DUMAPIC_tile";
                        _go.transform.SetParent(tile[x + half_scale, y + half_scale]);
                        _go.AddComponent<Image>().sprite = DUMAPIC_tile[10];
                        RectTransform _r = _go.GetComponent<RectTransform>();
                        if (_player.facing == BlobberEngine.Enum._Direction.north)
                            _r.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                        if (_player.facing == BlobberEngine.Enum._Direction.east)
                            _r.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 270));
                        if (_player.facing == BlobberEngine.Enum._Direction.south)
                            _r.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 180));
                        if (_player.facing == BlobberEngine.Enum._Direction.west)
                            _r.GetComponent<RectTransform>().SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 90));
                        _r.anchorMin = new Vector2(0, 0);
                        _r.anchorMax = new Vector2(1, 1);
                        _r.offsetMin = new Vector2(0, 0);
                        _r.offsetMax = new Vector2(0, 0);
                    }
                }
                // 0 value = no wall, 1 value = door, 2 value = secret door (hidden), 3 value = secret door (revealed), 4 value = wall
            }
    }
    private void SetTile(GameObject _go, int _tileNum, int x, int y)
    {
        //x += 3; y += 3;
        _go.tag = "DUMAPIC_tile";
        _go.transform.SetParent(tile[x + half_scale, y + half_scale]);// Debug.Log(" w> " + _go.name);
        _go.AddComponent<Image>().sprite = DUMAPIC_tile[_tileNum];
        RectTransform _r = _go.GetComponent<RectTransform>();
        _r.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
        _r.anchorMin = new Vector2(0, 0);
        _r.anchorMax = new Vector2(1, 1);
        _r.offsetMin = new Vector2(0, 0);
        _r.offsetMax = new Vector2(0, 0);
    }
    private void ReportPosiiton()
    {
        Vector2Int _myLoc = _player.WhatRoomAmIin().Map_Coordinates;
        string _result = ("The party's location is revealed!\n" +
                 "You are: " + (_myLoc.x -1) + " steps east, " + (20 - _myLoc.y) + " steps north, " + " and " + GameManager.PARTY._PartyXYL.z + " levels down from the castle stairs.");
        title.text = _result;
    }
    public void CloseButtonPushed()
    {
        ClearMap();
        this.gameObject.SetActive(false);
    }
}
