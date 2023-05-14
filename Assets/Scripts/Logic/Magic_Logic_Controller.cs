using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Magic_Logic_Controller : MonoBehaviour
{
    private Castle_Pop_Up_Manager _popUp;
    private Dungeon_Logic_Manager _dungeon;
    private void Start()
    {
        _dungeon = FindObjectOfType<Dungeon_Logic_Manager>();
        _popUp = _dungeon.PopUp;
    }

    public Spell_Class CanCastSpell(Character_Class _caster, string _spell)
    {
        Debug.Log(_caster.name + " casts " + _spell + "...");
        Spell_Class result = null;
        int _spellCircle = -1;
        string _spellBook = "nil";
        bool _spellKnown = false;
        Spell_Class _thisSpell = null;
        for (int i = 0; i < GameManager.SPELL.Count; i++)
        {
            if(_spell.ToLower() == GameManager.SPELL[i].name.ToLower())
            {
                Debug.Log("found " + _spell);
                _spellCircle = GameManager.SPELL[i].circle;
                _spellBook = GameManager.SPELL[i].book;
                if (_caster.SpellKnown[i]) _spellKnown = true;
                if (_caster.SpellKnown[i]) Debug.Log(_caster.name + " knows " + _spell);                
                if (GameManager.PARTY.inBattle && !GameManager.SPELL[i].combat) _spellBook = "nil";
                if (!GameManager.PARTY.inBattle && !GameManager.SPELL[i].camp) _spellBook = "nil";
                if (GameManager.PARTY.inBattle && !GameManager.SPELL[i].combat) Debug.Log("cant cast because not in combat");
                if (!GameManager.PARTY.inBattle && !GameManager.SPELL[i].camp) Debug.Log("cant cast because not in camp");
                _thisSpell = GameManager.SPELL[i];
            }
        }
        if(_spellBook == "Priest" && _spellKnown && _caster.priestSpells[_spellCircle-1] - _caster.priestSpellsCast[_spellCircle - 1] > 0) result = _thisSpell;
        if(_spellBook == "Mage" && _spellKnown && _caster.mageSpells[_spellCircle-1] - _caster.mageSpellsCast[_spellCircle - 1] > 0) result = _thisSpell;

        Debug.Log("returning result = " + (result == null ? "false" : "true"));
        return result;
    }

    public void Cast_Spell(Character_Class _caster, Spell_Class _spell, Character_Class _target = null)
    {
        //Debug.Log("Casting " + _spell.name);
        if(CanCastSpell(_caster, _spell.name) != null)
        {
            if (_spell.book == "Priest") _caster.priestSpellsCast[_spell.circle - 1]++;
            if (_spell.book == "Mage") _caster.mageSpellsCast[_spell.circle - 1]++;
            if (GameManager.PARTY.antiMagic) //Anti-Magic squares fizzle spells, but AFTER taking a spell point
            {
                _popUp.Show_Message("The Spell Fizzles!");
                return;
            }            
            if(_target != null)
            {
                Apply_Spell(_target, _spell.name);
            }
            else
            {
                Apply_Spell(_spell.name);
            }
        }
    }

    public void Apply_Spell(Character_Class _target, string _spell)
    {
        Vector2Int _myLoc = _dungeon._player.WhatRoomAmIin().Map_Coordinates;
        _spell = _spell.ToLower();
        //Debug.Log("Casting on target: " + _spell + ", " + _target.name);
        if (_spell == "dios")
        {
            int _healAmount = new Dice(1, 8).Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
            _popUp.Show_Message(_target.name + " heals " + _healAmount + " points.");
            _target.HP += _healAmount;
        }
        if (_spell == "dial")
        {
            int _healAmount = new Dice(2, 8).Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
            _popUp.Show_Message(_target.name + " heals " + _healAmount + " points.");
            _target.HP += _healAmount;
        }
        if (_spell == "dialma")
        {
            int _healAmount = new Dice(3, 8).Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
            _popUp.Show_Message(_target.name + " heals " + _healAmount + " points.");
            _target.HP += _healAmount;
        }
        if (_spell == "madi")
        {
            _target.HP = _target.HP_MAX;
            _target.status = Enum._Status.OK;
            _target.Poison = 0;
            _popUp.Show_Message(_target.name + " is healed!!!");
        }
        if (_spell == "di")
        {
            if (_target.status == Enum._Status.ashes)
            {
                _popUp.Show_Message("Not enoough of " + _target.name + "'s left! the spell fizzles!");
                return;
            }
            if (_target.status != Enum._Status.dead)
            {
                _popUp.Show_Message(_target.name + " is still alive! the spell fizzles!");
                return;
            }
            int chance = _target.Vitality * 4;
            int roll = Random.Range(0, 100) + 1;
            Debug.Log("Kadorto Roll: " + roll + " vs " + chance + (roll >= chance ? "... success!" : "... fail!"));
            if(roll >= chance) 
            {
                _target.HP = _target.HP_MAX;
                _target.status = Enum._Status.OK;
                _target.Poison = 0;
                _target.Vitality--;
                _popUp.Show_Message(_target.name + " is fully restored!!!");
                return;
            }
            else            
            {
                _target.status = Enum._Status.ashes;
                _target.Vitality--;
                _popUp.Show_Message(_target.name + " is reduced to ashes... OH NO!");
                return;
            }
        }
        if (_spell == "kadorto")
        {            
            if(_target.status != Enum._Status.dead || _target.status != Enum._Status.ashes)
            {
                _popUp.Show_Message(_target.name + " is still alive! the spell fizzles!");
                return;
            }
            int chance = _target.Vitality * 4;
            int roll = Random.Range(0, 100) + 1;
            Debug.Log("Kadorto Roll: " + roll + " vs " + chance + (roll >= chance ? "... success!" : "... fail!"));
            if(roll >= chance) 
            {
                _target.HP = _target.HP_MAX;
                _target.status = Enum._Status.OK;
                _target.Poison = 0;
                _target.Vitality--;
                _popUp.Show_Message(_target.name + " is fully restored!!!");
                return;
            }
            else 
            {
                int _partyIndexNum = GameManager.PARTY.Get_Party_Index(_target);
                GameManager.PARTY.RemoveMember(_partyIndexNum);
                _target.status = Enum._Status.lost;
                _target.lostXYL = new Vector3Int(_myLoc.x, _myLoc.y, GameManager.PARTY._PartyXYL.z);
                _popUp.Show_Message(_target.name + " blasted to oblivion! OH NO!");                
                return;            
            }
        }
        if (_spell == "latumofis")
        {
            _target.Poison = 0;
            _popUp.Show_Message(_target.name + " is cured of poison!");
        }
    }

    public void Apply_Spell(string _spell)
    {
        Vector2Int _myLoc = _dungeon._player.WhatRoomAmIin().Map_Coordinates;        
        _spell = _spell.ToLower();
        //Debug.Log("casting on party: " + _spell);
        if (_spell == "maporfic")
        {
            GameManager.PARTY.Party_Shield_Bonus = true;
            _popUp.Show_Message("The party is shielded in magic energy!");
        }
        if (_spell == "milwa")
        {
            GameManager.PARTY.Party_Light_Timer += UnityEngine.Random.Range(0, 15) + 15;
            _popUp.Show_Message("The dungeon brightens around you!");
        }
        if (_spell == "lomilwa")
        {
            GameManager.PARTY.Party_Light_Timer = -1;
            _popUp.Show_Message("The dungeon brightens around you!");
        }
        if (_spell == "dialko")
        {
            string _bgn = "Glowing energy seeps over the party...", _result = "\n but nothing happens.";
            for (int i = 0; i < 6; i++)
            {
                if (!GameManager.PARTY.EmptySlot(i))
                    if (GameManager.PARTY.LookUp_PartyMember(i).status == BlobberEngine.Enum._Status.plyze ||
                        GameManager.PARTY.LookUp_PartyMember(i).status == BlobberEngine.Enum._Status.stoned)
                    {
                        if(_result == "\n but nothing happens.")
                        { _result = "\n" + GameManager.PARTY.LookUp_PartyMember(i).name + " is healed!"; }
                        else
                        { _result += "\n" + GameManager.PARTY.LookUp_PartyMember(i).name + " is healed!"; }
                        GameManager.PARTY.LookUp_PartyMember(i).status = BlobberEngine.Enum._Status.OK;
                    }
            }
            _popUp.Show_Message(_bgn + _result);
        }
        if ( _spell == "kandi")
        {
            _dungeon.ButtonPressReceived("kandi");
            return;
        }
        if (_spell == "loktofeit")
        {
            //clear equipment and 75% of geld
            for (int i = 0; i < 6; i++)
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    for (int j = 0; j < 8; j++) 
                        GameManager.PARTY.LookUp_PartyMember(i).Inventory[j] = new Item();
                    GameManager.PARTY.LookUp_PartyMember(i).Geld = (int)(GameManager.PARTY.LookUp_PartyMember(i).Geld * .25f);
                    GameManager.PARTY.LookUp_PartyMember(i).HP = GameManager.PARTY.LookUp_PartyMember(i).HP_MAX;
                }
            GameManager.instance.SaveGame();
            GameManager.instance._Persistent_Message = "The souls of the party are ripped from their bodies, and placed in new, naked bodies, in town!";
            UnityEngine.SceneManagement.SceneManager.LoadScene("Castle");
        }
        if( _spell == "dumapic")
        {
            _dungeon.ButtonPressReceived("dumapic");
            return;
        }
        if(_spell == "malor")
        {
            _dungeon.ButtonPressReceived("malor");
            return;
        }
    }
}
