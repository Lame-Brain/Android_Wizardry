using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camp_Cast_Spell_Manager : MonoBehaviour
{
    public TMPro.TextMeshProUGUI[] Script; //Buttons first, THEN title!

    private void OnEnable()
    {
        for (int i = 0; i < Script.Length; i++) Script[i].fontSize = GameManager.FONT;
        for (int i = 0; i < 6; i++)
        {
            if (!GameManager.PARTY.EmptySlot(i)) Script[i].text = GameManager.PARTY.LookUp_PartyMember(i).name;
        }
    }

    public void Button(int _num)
    {
        if (!GameManager.PARTY.EmptySlot(_num))
        {
            FindObjectOfType<Camp_Character_Sheet_Manager>().Cast_Spell_on(_num);
        }
    }
}
