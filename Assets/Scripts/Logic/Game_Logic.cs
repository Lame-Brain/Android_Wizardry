using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Logic : MonoBehaviour
{
    public static Party_Class PARTY;
    public static List<Character_Class> ROSTER = new List<Character_Class>();


    //singleton
    public static Game_Logic instance;
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance.gameObject);
            return;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        PARTY = FindObjectOfType<Party_Class>();
        PARTY.InitParty();

        Character_Class test = new Character_Class();
        test.name = "Ethan";
        Debug.Log(test.Save_Character());
    }
}
