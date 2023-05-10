using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Magic_Logic_Controller : MonoBehaviour
{

    public Spell_Class CanCastSpell(Character_Class _caster, string _spell)
    {
        Spell_Class result = null;
        int _spellCircle = -1;
        string _spellBook = "nil";
        bool _spellKnown = false;
        Spell_Class _thisSpell = null;
        for (int i = 0; i < GameManager.SPELL.Count; i++)
        {
            if(_spell.ToLower() == GameManager.SPELL[i].name.ToLower())
            {
                _spellCircle = GameManager.SPELL[i].circle;
                _spellBook = GameManager.SPELL[i].book;
                if (_caster.SpellKnown[i]) _spellKnown = true;
                if (GameManager.PARTY.inBattle && !GameManager.SPELL[i].combat) _spellBook = "nil";
                if (!GameManager.PARTY.inBattle && !GameManager.SPELL[i].camp) _spellBook = "nil";
                _thisSpell = GameManager.SPELL[i];
            }
        }
        if(_spellBook == "Priest" && _spellKnown && _caster.priestSpells[_spellCircle-1] - _caster.priestSpellsCast[_spellCircle - 1] > 0) result = _thisSpell;
        if(_spellBook == "Mage" && _spellKnown && _caster.mageSpells[_spellCircle-1] - _caster.mageSpellsCast[_spellCircle - 1] > 0) result = _thisSpell;

        return result;
    }

    public void Cast_Spell(Character_Class _caster, Spell_Class _spell, Character_Class _target = null)
    {
        //Debug.Log("Casting " + _spell.name);
        if(CanCastSpell(_caster, _spell.name) != null)
        {
            if (_spell.book == "Priest") _caster.priestSpellsCast[_spell.circle - 1]++;
            if (_spell.book == "Mage") _caster.mageSpellsCast[_spell.circle - 1]++;
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
        _spell = _spell.ToLower();
        //Debug.Log("Casting on target: " + _spell + ", " + _target.name);
        if (_spell == "dios")
        {
            Dice _healAmount = new Dice(1, 8);
            _target.HP += _healAmount.Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
        }
        if (_spell == "dial")
        {
            Dice _healAmount = new Dice(2, 8);
            _target.HP += _healAmount.Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
        }
        if (_spell == "dialma")
        {
            Dice _healAmount = new Dice(3, 8);
            _target.HP += _healAmount.Roll();
            if (_target.HP > _target.HP_MAX) _target.HP = _target.HP_MAX;
        }
        if (_spell == "Madi")
        {
            _target.HP = _target.HP_MAX;
            _target.status = Enum._Status.OK;
            _target.Poison = 0;
        }
        if (_spell == "kadorto")
        {
            int chance = _target.Vitality * 4;
            int roll = Random.Range(0, 100) + 1;
            Debug.Log("Kadorto Roll: " + roll + " vs " + chance + (roll >= chance ? "... success!" : "... fail!"));
            if(roll >= chance) 
            {
                _target.HP = _target.HP_MAX;
                _target.status = Enum._Status.OK;
                _target.Poison = 0;
                _target.Vitality--;
                return;
            }

            if (_target.status != Enum._Status.ashes)
            {
                _target.status = Enum._Status.ashes;
                _target.Vitality--;
                return;
            }
            else 
            {
                int _partyIndexNum = GameManager.PARTY.Get_Party_Index(_target);
                GameManager.PARTY.RemoveMember(_partyIndexNum);
                _target.status = Enum._Status.lost;
                //UPDATE THIS CHARACTER'S LOSTXYL!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                return;            
            }
        }
        if (_spell == "latumofis")
        {
            _target.Poison = 0;
        }
    }

    public void Apply_Spell(string _spell)
    {
        _spell = _spell.ToLower();
        //Debug.Log("casting on party: " + _spell);
        if (_spell == "maporfic")
        {
            GameManager.PARTY.Party_Shield_Bonus = true;
        }
        if (_spell == "milwa")
        {
            GameManager.PARTY.Party_Light_Timer += Random.Range(0, 15) + 15;
        }
        if (_spell == "lomilwa")
        {
            GameManager.PARTY.Party_Light_Timer = -1;
        }
        if(_spell == "dialko")
        {
            for (int i = 0; i < 6; i++)
            {
                if (!GameManager.PARTY.EmptySlot(i))
                    if (GameManager.PARTY.LookUp_PartyMember(i).status == BlobberEngine.Enum._Status.plyze ||
                        GameManager.PARTY.LookUp_PartyMember(i).status == BlobberEngine.Enum._Status.stoned)
                        GameManager.PARTY.LookUp_PartyMember(i).status = BlobberEngine.Enum._Status.OK;
            }
        }
    }
}
