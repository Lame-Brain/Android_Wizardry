using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KANDI_Controller_Manager : MonoBehaviour
{
    public Sprite[] KANDI_tile;
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
        Vector2Int _loc = _player.WhatRoomAmIin().Game_Coordinates; Debug.Log("0,0 = " + _level.Map[0, 0].Game_Coordinates);
        for (int y = -3; y < 4; y++)//for (int y = 3; y >= -3; y--)
            for (int x = -3; x < 4; x++)
            {
                tile[x + 3, y + 3].gameObject.AddComponent<Image>().sprite = KANDI_tile[0];
                tile[x + 3, y + 3].gameObject.name = "Tile [" + (x + 3) + ", " + (y + 3) + "]";

                int _newX = _loc.x - 1 + x, _newY = _loc.y - 2 - y;
                if (_newX+1 >= 0 && _newX+1 < 21 && _newY+20 >= 0 && _newY+20 < 21)
                {
                    if (_level.Map[_newX+1, _newY+20].Wall[0] == 4)
                    {
                        GameObject _go = new GameObject("nwall[" + (x+3) + ", " + (y+3)+"]");
                        _go.transform.SetParent(tile[x + 3, y + 3]); Debug.Log(" n> " + _go.name);
                        _go.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0, 0, 0));
                        _go.AddComponent<Image>().sprite = KANDI_tile[5];
                        StretchRectTransform(_go.GetComponent<RectTransform>());
                    }
                    if (_level.Map[_newX+1, _newY+20].Wall[1] == 4)
                    {
                        GameObject _go = new GameObject("ewall" + (x + 3) + ", " + (y + 3) + "]");
                        _go.transform.SetParent(tile[x + 3, y + 3]); Debug.Log(" e> " + _go.name);
                        _go.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0,0,270));
                        _go.AddComponent<Image>().sprite = KANDI_tile[5];
                        StretchRectTransform(_go.GetComponent<RectTransform>());
                    }
                    if (_level.Map[_newX+1, _newY+20].Wall[2] == 4)
                    {
                        GameObject _go = new GameObject("swall" + (x + 3) + ", " + (y + 3) + "]");
                        _go.transform.SetParent(tile[x + 3, y + 3]); Debug.Log(" s> " + _go.name);
                        _go.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0,0,180));
                        _go.AddComponent<Image>().sprite = KANDI_tile[5];
                        StretchRectTransform(_go.GetComponent<RectTransform>());
                    }
                    if (_level.Map[_newX+1, _newY+20].Wall[3] == 4)
                    {
                        GameObject _go = new GameObject("wwall" + (x + 3) + ", " + (y + 3) + "]");
                        _go.transform.SetParent(tile[x + 3, y + 3]); Debug.Log(" w> " + _go.name);
                        _go.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.Euler(0,0,90));
                        _go.AddComponent<Image>().sprite = KANDI_tile[5];
                        StretchRectTransform(_go.GetComponent<RectTransform>());
                    }

                }
                // 0 value = no wall, 1 value = door, 2 value = secret door (hidden), 3 value = secret door (revealed), 4 value = wall
            }
    }

    private void StretchRectTransform(RectTransform _r)
    {
        _r.anchorMin = new Vector2(0, 0);
        _r.anchorMax = new Vector2(1, 1);
        _r.offsetMin = new Vector2(0, 0);
        _r.offsetMax = new Vector2(0, 0);
        //_r.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Top, 0, 1);
        //_r.SetInsetAndSizeFromParentEdge(RectTransform.Edge.Left, 0, 1);
    }
}
