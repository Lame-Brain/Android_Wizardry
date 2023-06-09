using System.Collections;
using UnityEngine;
using BlobberEngine;

public class Player_Controller : MonoBehaviour
{
    public GameObject Light;
    public Enum._Direction facing;
    private bool _canAcceptmovement;
    private string _currentState;
    private Vector2Int _pos;
    [SerializeField] private float Move_Length, Turn_Length, Move_Delay, Turn_Delay, Action_Delay;

    private Dungeon_Logic_Manager _dungeon;
    private Level_Logic_Template _level;

    public void initPlayer()
    {
        _dungeon = FindObjectOfType<Dungeon_Logic_Manager>();
        _level = FindObjectOfType<Level_Logic_Template>();
        transform.position = new Vector3Int(GameManager.PARTY._PartyXYL.x + 1, 0, (20 - GameManager.PARTY._PartyXYL.y) * -1);
        facing = GameManager.PARTY.facing;
        _canAcceptmovement = true;
        _currentState = "moving";
        SetPlayerFacing();
    }
    private void SetPlayerFacing()
    {
        if (facing == Enum._Direction.north) this.transform.eulerAngles = new Vector3(0, 0, 0);
        if (facing == Enum._Direction.east) this.transform.eulerAngles = new Vector3(0, 90, 0);
        if (facing == Enum._Direction.south) this.transform.eulerAngles = new Vector3(0, 180, 0);
        if (facing == Enum._Direction.west) this.transform.eulerAngles = new Vector3(0, 270, 0);
    }

    public int WhatIsInFrontOfPlayer()
    {
        int _result = 0;
        _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);

        if (facing == Enum._Direction.north) _result = _level.Map[_pos.x, _pos.y].Wall[0];
        if (facing == Enum._Direction.east) _result = _level.Map[_pos.x, _pos.y].Wall[1];
        if (facing == Enum._Direction.south) _result = _level.Map[_pos.x, _pos.y].Wall[2];
        if (facing == Enum._Direction.west) _result = _level.Map[_pos.x, _pos.y].Wall[3];

        return _result;
    }
    public Tile_Class WhatRoomAmIin()
    {
        _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);
        Tile_Class _result = _level.Map[_pos.x, _pos.y];
        return _result;
    }

    public void Check4SecretDoor()
    {
        if (WhatIsInFrontOfPlayer() == 2)
        {
            if (facing == Enum._Direction.north)
            {
                _level.Map[_pos.x, _pos.y].Wall[0] = 3;
                _level.Map[_pos.x, _pos.y].north_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            }
            if (facing == Enum._Direction.east)
            {
                _level.Map[_pos.x, _pos.y].Wall[1] = 3;
                _level.Map[_pos.x, _pos.y].east_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            }
            if (facing == Enum._Direction.south)
            {
                _level.Map[_pos.x, _pos.y].Wall[2] = 3;
                _level.Map[_pos.x, _pos.y].south_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            }
            if (facing == Enum._Direction.west)
            {
                _level.Map[_pos.x, _pos.y].Wall[3] = 3;
                _level.Map[_pos.x, _pos.y].west_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            }
            //1234567890123456789012345678901234567
            _dungeon.UpdateMessge("      You found a secret door!");

        }
        else
        {
            StartCoroutine(Show_Bonk_CR());
        }
        _dungeon.TimePass();
    }
    public void RevealSecretDoors()
    {
        _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);
        bool found = false;
        if (_level.Map[_pos.x, _pos.y].Wall[0] == 2)
        {
            _level.Map[_pos.x, _pos.y].Wall[0] = 3;
            _level.Map[_pos.x, _pos.y].north_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            found = true;
        }
        if (_level.Map[_pos.x, _pos.y].Wall[1] == 2)
        {
            _level.Map[_pos.x, _pos.y].Wall[1] = 3;
            _level.Map[_pos.x, _pos.y].east_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            found = true;   
        }
        if (_level.Map[_pos.x, _pos.y].Wall[2] == 2)
        {
            _level.Map[_pos.x, _pos.y].Wall[2] = 3;
            _level.Map[_pos.x, _pos.y].south_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            found = true;
        }
        if (_level.Map[_pos.x, _pos.y].Wall[3] == 2)
        {
            _level.Map[_pos.x, _pos.y].Wall[3] = 3;
            _level.Map[_pos.x, _pos.y].west_wall.GetComponent<MeshRenderer>().material = GameManager.instance._doormat;
            found = true;
        }
        if(found)
            _dungeon.UpdateMessge("      You found a secret door!");

    }
    public void OpenDoor()
    {
        StartCoroutine(Move_CR());
    }
    public void SpinPlayer()
    {
        Debug.Log("Sipn");
        int _random = Random.Range(0, 4);
        if (_random == 0 ) facing = Enum._Direction.north;
        if (_random == 1 ) facing = Enum._Direction.east;
        if (_random == 2 ) facing = Enum._Direction.south;
        if (_random == 3 ) facing = Enum._Direction.west;
        SetPlayerFacing();
    }
    public void WarpPlayer(Vector3 _newPos, Enum._Direction _newFacing)
    {
        if (_newFacing != Enum._Direction.none) facing = _newFacing;
        this.transform.position = _newPos;
        SetPlayerFacing();
    }
    public void TotalPartyKill()
    {
        _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);
        for (int i = 0; i < 6; i++) // set all characters to dead
            if (!GameManager.PARTY.EmptySlot(i))
            {
                Character_Class _me = GameManager.PARTY.LookUp_PartyMember(i);
                _me.HP = 0;
                _me.status = Enum._Status.lost;
                _me.lostXYL = new Vector3Int(_pos.x, _pos.y, GameManager.PARTY._PartyXYL.z);
            }
        for (int i = 0; i < 6; i++)
            if (!GameManager.PARTY.EmptySlot(0)) GameManager.PARTY.RemoveMember(0);
        GameManager.instance.SaveGame();
        UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
    }

    public void ReceiveCommand(string _command)
    {
        if(_command == "move_forward" && _canAcceptmovement && _currentState == "moving")
        {
            //Debug.Log("Move Forward");
            _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);

            bool _canMove = false;
            //Check if the next node is available to move to.
            if (facing == Enum._Direction.north && _level.Map[_pos.x, _pos.y].Wall[0] == 0) _canMove = true;
            if (facing == Enum._Direction.east && _level.Map[_pos.x, _pos.y].Wall[1] == 0) _canMove = true;
            if (facing == Enum._Direction.south && _level.Map[_pos.x, _pos.y].Wall[2] == 0) _canMove = true;
            if (facing == Enum._Direction.west && _level.Map[_pos.x, _pos.y].Wall[3] == 0) _canMove = true;
            if (_canMove)
            {
                StartCoroutine(Move_CR());
            }
            else
            {
                //StopCoroutine(Show_Bonk_CR());
                //_dungeon.Block_Icon.SetActive(false);
                StartCoroutine(Show_Bonk_CR());
                _dungeon.TimePass();
            }            
        }

        if(_command == "turn_right" && _canAcceptmovement && _currentState == "moving")
        {
            StartCoroutine(Turn_CR(1));
        }
        if(_command == "turn_around" && _canAcceptmovement && _currentState == "moving")
        {
            StartCoroutine(Turn_CR(2));
        }
        if(_command == "turn_left" && _canAcceptmovement && _currentState == "moving")
        {
            StartCoroutine(Turn_CR(-1));
        }
    }

    IEnumerator Move_CR()
    {
        _canAcceptmovement = false;

        float _timeElapsed = 0;
        Vector3 pos_delta = Vector3.zero;
        if(facing == Enum._Direction.north) pos_delta = Vector3.forward;
        if(facing == Enum._Direction.east) pos_delta = Vector3.right;
        if(facing == Enum._Direction.south) pos_delta = Vector3.back;
        if(facing == Enum._Direction.west) pos_delta = Vector3.left;
        Vector3 _startPos = this.transform.position, _endPos = _startPos + pos_delta;
        _dungeon.ShowFeedback("Move Forward");
        while (_timeElapsed < Move_Length)
        {
            this.transform.position = Vector3.Lerp(_startPos, _endPos, _timeElapsed / Move_Length);
            yield return null;
            _timeElapsed += Time.deltaTime;
        }
        int x = (int)_endPos.x, y = (int)_endPos.y, z = (int)_endPos.z;
        if (x < 1) x = 20; if (x > 20) x = 1;
        if (z > -1) z = -20; if (z < -20) z = -1;
        this.transform.position = new Vector3Int(x, y, z);
        yield return new WaitForSeconds(Move_Delay);
        _dungeon.TimePass();
        _canAcceptmovement = true;
    }

    IEnumerator Show_Bonk_CR()
    {
        _canAcceptmovement = false;
        _dungeon.Block_Icon.SetActive(true);
        yield return new WaitForSeconds(Action_Delay);
        _dungeon.Block_Icon.SetActive(false);
        _canAcceptmovement = true;
    }

    IEnumerator Turn_CR(int d = 1)
    {
        _canAcceptmovement = false;
        float _timeElapsed = 0;
        Quaternion _startROT = this.transform.rotation;
        Quaternion _endROT = this.transform.rotation;
        //var _startROT = this.transform.eulerAngles;
        //var _endROT = this.transform.eulerAngles;
        if (d == 1)
        {
            _dungeon.ShowFeedback("Turn Right");
            if (facing == Enum._Direction.north) _endROT = Quaternion.Euler(0, 90, 0);
            if (facing == Enum._Direction.east) _endROT = Quaternion.Euler(0, 180, 0);
            if (facing == Enum._Direction.south) _endROT = Quaternion.Euler(0, 270, 0);
            if (facing == Enum._Direction.west) _endROT = Quaternion.Euler(0, 0, 0);
        }
        if (d == -1)
        {
            _dungeon.ShowFeedback("Turn Left");
            if (facing == Enum._Direction.north) _endROT = Quaternion.Euler(0, 270, 0);
            if (facing == Enum._Direction.east) _endROT = Quaternion.Euler(0, 0, 0);
            if (facing == Enum._Direction.south) _endROT = Quaternion.Euler(0, 90, 0);
            if (facing == Enum._Direction.west) _endROT = Quaternion.Euler(0, 180, 0);
        }
        if (d == 2)
        {
            _dungeon.ShowFeedback("Turn Around");
            if (facing == Enum._Direction.north) _endROT = Quaternion.Euler(0, 180, 0);
            if (facing == Enum._Direction.east) _endROT = Quaternion.Euler(0, 270, 0);
            if (facing == Enum._Direction.south) _endROT = Quaternion.Euler(0, 0, 0);
            if (facing == Enum._Direction.west) _endROT = Quaternion.Euler(0, 90, 0);
        }
        while (_timeElapsed < Turn_Length)
        {
            this.transform.rotation = Quaternion.Slerp(_startROT, _endROT, _timeElapsed / Turn_Length);
            yield return null;
            _timeElapsed += Time.deltaTime;
        }
        if(d == 1)
        {
            switch (facing)
            {
                case Enum._Direction.north:
                    facing = Enum._Direction.east; break;
                case Enum._Direction.east:
                    facing = Enum._Direction.south; break;
                case Enum._Direction.south:
                    facing = Enum._Direction.west; break;
                case Enum._Direction.west:
                    facing = Enum._Direction.north; break;  
            }
        }
        if(d == -1)
        {
            switch (facing)
            {
                case Enum._Direction.north:
                    facing = Enum._Direction.west; break;
                case Enum._Direction.west:
                    facing = Enum._Direction.south; break;
                case Enum._Direction.south:
                    facing = Enum._Direction.east; break;
                case Enum._Direction.east:
                    facing = Enum._Direction.north; break;  
            }
        }
        if(d == 2)
        {
            switch (facing)
            {
                case Enum._Direction.north:
                    facing = Enum._Direction.south; break;
                case Enum._Direction.west:
                    facing = Enum._Direction.east; break;
                case Enum._Direction.south:
                    facing = Enum._Direction.north; break;
                case Enum._Direction.east:
                    facing = Enum._Direction.west; break;  
            }
        }
        SetPlayerFacing();
        yield return new WaitForSeconds(Turn_Delay);
        _dungeon.SetContextButton();
        _canAcceptmovement = true;
    }

    public string GetDebugData()
    {
        _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);
        int x2 = _level.Map[_pos.x, _pos.y].Game_Coordinates.x;
        int y2 = _level.Map[_pos.x, _pos.y].Game_Coordinates.y;
        int x3 = (int)_level.Map[_pos.x, _pos.y].transform.position.x;
        int y3 = (int)_level.Map[_pos.x, _pos.y].transform.position.z;
        string _result = "(" + x2 + ", " + y2 + ") (" + x3 + ", " + y3 + ") - " + facing;
        return _result;
    }
}
