using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BlobberEngine
{
    public class Enum
    {
        public enum _Class { none, fighter, mage, priest, thief, bishop, samurai, lord, ninja, animal, demon, dragon, enchanted, giant, insect, myth, undead, were }
        public enum _Race { none, human, elf, dwarf, gnome, halfling }
        public enum _Alignment { none, good, neutral, evil }
        public enum _Direction { none, north, east, south, west, forward, backward, left, right }
        public enum _Damage_type { physical, fire, cold }
        public enum _Status {  OK, afraid, asleep, plyze, stoned, dead, ashes, lost }
        public enum _Item_Type { Weapon, Armor, Shield, Helmet, Gauntlets, Special, Misc, Consumable }
        public enum _Locaton { Roster, Dungeon, Temple }
    }
    public class Dice
    {
        public int num, sides, bonus;

        public Dice(int _n = 1, int _s = 1, int _b = 0)
        {
            this.num = _n; this.sides = _s; this.bonus = _b;
        }

        public int Roll()
        {
            int _result = 0;
            for (int i = 0; i < num; i++)
                _result += Random.Range(1, sides + 1);
            _result += bonus;
            return _result;
        }
    }

    public class XPTable
    {
        public int[,] xpTable = new int[13, 8] { { 1000, 1100, 1050, 900, 1000, 1250, 1300, 1450 },
                                                 { 1724, 1896, 1810, 1551, 2105, 2192, 2280, 2543 },
                                                 { 2972, 3268, 3120, 2674, 3692, 3845, 4000, 4461 },
                                                 { 5124, 5634, 5379, 4610, 6477, 6745, 7017, 7826 },
                                                 { 8834, 9713, 9274, 7948, 11363, 11833, 12310, 13729 },
                                                 { 15231, 16746, 15989, 13703, 19935, 20759, 21596, 24085 },
                                                 { 26260, 28872, 27567, 23625, 34973, 36419, 37887, 42254 },
                                                 { 45275, 49779, 47529, 40732, 61136, 63892, 66468, 74129 },
                                                 { 78060, 85825, 81946, 70187, 107642, 112091, 116610, 130050 },
                                                 { 134586, 147974, 141286, 121081, 188845, 196650, 204578, 228157 },
                                                 { 232044, 255127, 243596, 208750, 331370, 345000, 358908, 400275 },
                                                 { 400075, 439874, 419993, 359931, 581240, 605263, 629663, 702236 },
                                                 { 289709, 318529, 304132, 260639, 438479, 456601, 475008, 529756 } };
        
        public int LookupNNL(int _level, Enum._Class _class)
        {
            //Set
            int _result = 0;
            int _col = 0;
            int _extra = 0;

            //Row and Col  (NOTE: for a reason I do not fully understand, Rows and Columns are backwards... just roll with it)
            int _row = _level - 1;
            if (_class == Enum._Class.fighter) _col = 0;
            if (_class == Enum._Class.mage) _col = 1;
            if (_class == Enum._Class.priest) _col = 2;
            if (_class == Enum._Class.thief) _col = 3;
            if (_class == Enum._Class.bishop) _col = 4;
            if (_class == Enum._Class.samurai) _col = 5;
            if (_class == Enum._Class.lord) _col = 6;
            if (_class == Enum._Class.ninja) _col = 7;

            //Bounds
            if (_row < 0) _row = 0;
            if(_row > 11)
            {
                int _over = _row - 11;
                _row = 11;
                _extra = xpTable[_row, _col] * _over;
            }            

            //APPLY
            _result = xpTable[_row, _col] + _extra;
            return _result;
        }
    }
}
