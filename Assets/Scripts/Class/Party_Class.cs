using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Party_Class : MonoBehaviour
{
    public bool[,,] tile_visited;
    public int Temple_Favor;
    public int[] BoltacStock;
    public string mem;

    public bool _MakeCampOnLoad;
    public Vector3Int _PartyXYL; 
    public Enum._Direction facing;
    public bool Party_Shield_Bonus = false;
    public int Party_Light_Timer = 0;
    public bool inBattle = false;

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
    public int Get_Party_Index (Character_Class _me)
    {
        int _result = -1;
        for (int i = 0; i < 6; i++)
        {
            if(!EmptySlot(i) && GameManager.ROSTER[Party[i]] == _me)
                _result = i;
        }        
        return _result;
    }
    public int Get_Party_Index (int _rosterIndex)
    {
        int _result = -1;
        for (int i = 0; i < 6; i++)
        {
            if(!EmptySlot(i) && Party[i] == _rosterIndex)
                _result = i;
        }        
        return _result;
    }
    public Character_Class LookUp_PartyMember(int _n)
    {        
        Character_Class _result = GameManager.ROSTER[Party[_n]];
        return _result;
    }
    
    public void AddMember(int RosterIndex)
    {
        for (int i = 0; i < 6; i++)
            if(Party[i] == -1)
            {
                Party[i] = RosterIndex;
                GameManager.ROSTER[RosterIndex].inParty = true;
                return;
            }
    }
    public void RemoveMember(int PartyIndex)
    {
        GameManager.ROSTER[Party[PartyIndex]].inParty = false;
        Party[PartyIndex] = -1;
        for (int i = PartyIndex; i < 5; i++)
        {
            Party[i] = Party[i + 1];
            Party[i + 1] = -1;
        }
    }

    public void OverRide_PartySlots(int _PartySlot1, int _PartySlot2, int _PartySlot3, int _PartySlot4, int _PartySlot5, int _PartySlot6)
    {
        Party[0] = _PartySlot1;
        Party[1] = _PartySlot2;
        Party[2] = _PartySlot3;
        Party[3] = _PartySlot4;
        Party[4] = _PartySlot5;
        Party[5] = _PartySlot6;
    }
}
