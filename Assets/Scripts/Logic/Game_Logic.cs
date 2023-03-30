using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Game_Logic : MonoBehaviour
{
    public static int[] PARTY;
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
        PARTY = new int[6];
        for (int i = 0; i < 6; i++) PARTY[i] = -1;
    }
}
