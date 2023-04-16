using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Party_Class : MonoBehaviour
{
    public bool[,,] tile_visited;
    public int Temple_Favor;
    public int[] BoltacStock;

    private int[] Party;
    private Enum._Alignment PartyAlign;

    public void InitParty()
    {
        Party = new int[6];
        for (int i = 0; i < 6; i++) Party[i] = -1;
        tile_visited = new bool[20, 20, 10];
        for (int z = 0; z < 10; z++)
            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 20; x++)
                    tile_visited[x, y, z] = false;
    }

    public bool EmptySlot(int _n)
    {
        bool _result = true;
        if (_n < 0) _n = 0;
        if (_n > 5) _n = 5;
        if (Party[_n] > -1) _result = false;
        return _result;
    }

    public int Get_Roster_Index (int _partySlot)
    {
        int _result = -1;
        if (!EmptySlot(_partySlot)) _result = Party[_partySlot];        
        return _result;
    }
    public Character_Class LookUp_PartyMember(int _n)
    {
        Character_Class _result = Game_Logic.ROSTER[Party[_n]];
        return _result;
    }
    
    public void AddMember(int RosterIndex)
    {
        for (int i = 0; i < 6; i++)
            if(Party[i] == -1)
            {
                Party[i] = RosterIndex;
                Game_Logic.ROSTER[RosterIndex].inParty = true;
                return;
            }
    }
    public void RemoveMember(int PartyIndex)
    {
        Game_Logic.ROSTER[Party[PartyIndex]].inParty = false;
        Party[PartyIndex] = -1;
        for (int i = PartyIndex; i < 5; i++)
        {
            Party[i] = Party[i + 1];
            Party[i + 1] = -1;
        }
    }
}
