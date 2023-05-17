using System.Collections;
using System.Collections.Generic;
using BlobberEngine;

public class Monster_Class
{
    public int index;
    public int pic;
    public string name_unk, names_unk, name, names;
    public Dice group_size, HitDice;
    public Enum._Class monster_class;
    public int ArmorClass;
    public List<Dice> attack = new List<Dice>();
    public int reward1, reward2;
    public int partner_chance, partner_index;
    public int mage_spells, priest_spells;
    public int spell_resist;
    public string elem_resist;
    public string abilities;
    public int xp;
    public int unique;
    public string special;
    public string weapon_style; //swings, thrusts, stabs, slashes, chops [or] tears, rips, gnaws, bites, claws [or both]
    public bool resist_friendly;
    public int morale_level;
    public int level;
    public bool identified;
    public string groupName;
    public List<Monster> monster = new List<Monster>();

    public void MakeCopy(int _MONSTER_INDEX)
    {
        morale_level = GameManager.MONSTER[_MONSTER_INDEX].morale_level;
        level = GameManager.MONSTER[_MONSTER_INDEX].level;
        identified = GameManager.MONSTER[_MONSTER_INDEX].identified;
        groupName = GameManager.MONSTER[_MONSTER_INDEX].groupName;
        if (GameManager.MONSTER[_MONSTER_INDEX].monster.Count > 0)
        { //I am doing this, in case a Monster_Class is ever copied to itself.
            List<Monster> _newlist = new List<Monster>(); 
            for (int i = 0; i < GameManager.MONSTER[_MONSTER_INDEX].monster.Count; i++)
                _newlist.Add(GameManager.MONSTER[_MONSTER_INDEX].monster[i]);
            GameManager.MONSTER[_MONSTER_INDEX].monster.Clear();
            for (int i = 0; i < _newlist.Count; i++)
                monster.Add(_newlist[i]);
        }
    }
}
