using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator_Controller_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] _string;
    private bool _isLowerFloor;

    private void OnEnable()
    {
        for (int i = 0; i < _string.Length; i++)
            _string[i].fontSize = GameManager.FONT;
    }

    public void EnterElevator(bool isLowerFloors = false)
    {
        _isLowerFloor = isLowerFloors;
        gameObject.SetActive(true);
        if (isLowerFloors)
        {
            _string[1].text = "4";
            _string[2].text = "5";
            _string[3].text = "6";
            _string[4].text = "7";
            _string[5].transform.parent.gameObject.SetActive(true);
            _string[5].text = "8";
            _string[6].transform.parent.gameObject.SetActive(true);
            _string[6].text = "9";
        }
        else
        {
            _string[1].text = "1";
            _string[2].text = "2";
            _string[3].text = "3";
            _string[4].text = "4";
            _string[5].transform.parent.gameObject.SetActive(false);
            _string[6].transform.parent.gameObject.SetActive(false);
        }
    }

    public void ButtonPressed(int _n)
    {
        if (_n == 999)
        {
            this.gameObject.SetActive(false);
            return;
        }

        if (_isLowerFloor)
        {
            switch (_n)
            {
                case 1:
                    _n = 5; break;
                case 2:
                    _n = 6; break;
                case 3:
                    _n = 7; break;
                case 4:
                    _n = 8; break;
                default:
                    _n = 4; break;
            }
        }
        else
        {
            switch (_n)
            {
                case 1:
                    _n = 2; break;
                case 2:
                    _n = 3; break;
                case 3:
                    _n = 4; break;
                default:
                    _n = 1; break;
            }
        }

        string _output = "ElevatorA:" + _n;
        if (_isLowerFloor) _output = "ElevatorB:" + _n;
        FindObjectOfType<Dungeon_Logic_Manager>().ButtonPressReceived(_output);
    }
}
