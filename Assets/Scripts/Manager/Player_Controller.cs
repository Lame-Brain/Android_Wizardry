using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;
using UnityEditor.Experimental.GraphView;

public class Player_Controller : MonoBehaviour
{
    public Enum._Direction facing;
    private bool _canAcceptmovement;
    private string _currentState;
    private Vector2Int _pos;
    [SerializeField] private float Move_Length, Turn_Length, Move_Delay, Turn_Delay, Action_Delay;

    private Dungeon_Logic_Manager _dungeon;
    private Level_Logic_Template _level;

    private void Start()
    {
        _dungeon = FindObjectOfType<Dungeon_Logic_Manager>();
        _level = FindObjectOfType<Level_Logic_Template>();        
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

    public void ReceiveCommand(string _command)
    {
        if(_command == "move_forward" && _canAcceptmovement && _currentState == "moving")
        {
            //Debug.Log("Move Forward");
            _pos = new Vector2Int((int)this.transform.position.x, (int)this.transform.position.z * -1);
            _dungeon.UpdateScreen();

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

        while (_timeElapsed < Move_Length)
        {
            this.transform.position = Vector3.Lerp(_startPos, _endPos, _timeElapsed / Move_Length);
            yield return null;
            _timeElapsed += Time.deltaTime;
        }
        this.transform.position = _endPos;
        yield return new WaitForSeconds(Move_Delay);
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
            if (facing == Enum._Direction.north) _endROT = Quaternion.Euler(0, 90, 0);
            if (facing == Enum._Direction.east) _endROT = Quaternion.Euler(0, 180, 0);
            if (facing == Enum._Direction.south) _endROT = Quaternion.Euler(0, 270, 0);
            if (facing == Enum._Direction.west) _endROT = Quaternion.Euler(0, 0, 0);
        }
        if (d == -1)
        {
            if (facing == Enum._Direction.north) _endROT = Quaternion.Euler(0, 270, 0);
            if (facing == Enum._Direction.east) _endROT = Quaternion.Euler(0, 0, 0);
            if (facing == Enum._Direction.south) _endROT = Quaternion.Euler(0, 90, 0);
            if (facing == Enum._Direction.west) _endROT = Quaternion.Euler(0, 180, 0);
        }
        if (d == 2)
        {
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
        _canAcceptmovement = true;
    }

}
