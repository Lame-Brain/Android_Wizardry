using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MALOR_Controller_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI title, destination, x_axis, y_axis, z_axis, cast_btn, cancel_btn, west_btn, east_btn, north_btn, south_btn, up_btn, down_btn;
    private Vector3Int _target;

    private void OnEnable()
    {
        _target = new Vector3Int(0, 0, 0);
        title.fontSize = GameManager.FONT;
        destination.fontSize = GameManager.FONT;
        x_axis.fontSize = GameManager.FONT;
        y_axis.fontSize = GameManager.FONT;
        z_axis.fontSize = GameManager.FONT;
        cast_btn.fontSize = GameManager.FONT;
        cancel_btn.fontSize = GameManager.FONT;
        west_btn.fontSize = GameManager.FONT;
        east_btn.fontSize = GameManager.FONT;
        north_btn.fontSize = GameManager.FONT;
        south_btn.fontSize = GameManager.FONT;
        up_btn.fontSize = GameManager.FONT;
        down_btn.fontSize = GameManager.FONT;
        Update_Screen();
    }

    private void Update_Screen()
    {
        string x_axis = "east", y_axis = "north", z_axis = "down";
        if (_target.x < 0) x_axis = "west";
        if (_target.y < 0) y_axis = "south";
        if (_target.z < 0) z_axis = "up";
        destination.text = "from the castle steps...\n" +
                   Mathf.Abs(_target.x) + " steps " + x_axis+"\n" +
                   Mathf.Abs(_target.y) + " steps "+y_axis+"\n" +
                   Mathf.Abs(_target.z) + " floors " + z_axis;
    }

    public void Change_Target_Button(string _input)
    {
        if (_input == "west") _target = new Vector3Int(_target.x + 1, _target.y, _target.z);
        if (_input == "east") _target = new Vector3Int(_target.x - 1, _target.y, _target.z);
        if (_input == "north") _target = new Vector3Int(_target.x, _target.y - 1, _target.z);
        if (_input == "south") _target = new Vector3Int(_target.x, _target.y + 1, _target.z);
        if (_input == "up") _target = new Vector3Int(_target.x, _target.y, _target.z - 1);
        if (_input == "down") _target = new Vector3Int(_target.x, _target.y, _target.z + 1);
        Update_Screen();
    }
    public void Cancel_Button()
    {
        this.gameObject.SetActive(false);
    }
    public void Cast_Button()
    {
        bool _valid = true;
        int x = GameManager.PARTY._PartyXYL.x + _target.x;
        Debug.Log(GameManager.PARTY._PartyXYL.x + " + " + _target.x + " = " + x);
        if (x < 0 || x > 20) _valid = false;
        int y = GameManager.PARTY._PartyXYL.y + _target.y;
        Debug.Log(GameManager.PARTY._PartyXYL.y + " + " + _target.y + " = " + y);
        if (y < 0 || y > 20) _valid = false;
        int z = GameManager.PARTY._PartyXYL.z + _target.z;
        Debug.Log(GameManager.PARTY._PartyXYL.z + " + " + _target.z + " = " + z);
        if (z < 1 || z > 9) _valid = false;

        if (!_valid)         
        {
            FindObjectOfType<Dungeon_Logic_Manager>().PopUp.Show_Message("That is not a valid destination...\n" +
                                                                         "The spell fizzles!");
            FindObjectOfType<Camp_Character_Sheet_Manager>().UpdateScreen();
            this.gameObject.SetActive(false);
            return;
        }
        this.gameObject.SetActive(false);
        GameManager.instance.SaveGame();
        GameManager.PARTY._PartyXYL = new Vector3Int(x, y, z);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Dungeon" + z);
        return;
    }
}
