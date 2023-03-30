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

        
        //Debug Character
        Character_Class test = new Character_Class(), another_test = new Character_Class(), third_test = new Character_Class();
        test.name = "Ethan";
        another_test.name = "Evan";
        third_test.name = "Roberts";
        //string save = test.Save_Character();
        //Debug.Log(save);
        //test.name = "FUCKER";
        //test.Load_Character(save);
        //Debug.Log(test.name);

        ROSTER.Add(test);
        ROSTER.Add(another_test);
        ROSTER.Add(third_test);
        PARTY.AddMember(0);        
        PARTY.AddMember(1);        
        PARTY.AddMember(2);        
    }
}
