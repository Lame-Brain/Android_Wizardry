using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Magic_Logic_Controller : MonoBehaviour
{

    public bool CanCastSpell(Character_Class _caster, string _spell)
    {
        bool result = false;
        int _spellCircle = -1;
        string _spellBook = "nil";
        bool _spellKnown = false;
        for (int i = 0; i < GameManager.SPELL.Count; i++)
        {
            if(_spell.ToLower() == GameManager.SPELL[i].name.ToLower())
            {
                _spellCircle = GameManager.SPELL[i].circle;
                _spellBook = GameManager.SPELL[i].book;
                if (_caster.SpellKnown[i]) _spellKnown = true;
                if (GameManager.PARTY.inBattle && !GameManager.SPELL[i].combat) _spellBook = "nil";
                if (!GameManager.PARTY.inBattle && !GameManager.SPELL[i].camp) _spellBook = "nil";
            }
        }
        if(_spellBook == "Priest" && _spellKnown && _caster.priestSpells[_spellCircle-1] - _caster.priestSpellsCast[_spellCircle - 1] > 0) result = true;
        if(_spellBook == "Mage" && _spellKnown && _caster.mageSpells[_spellCircle-1] - _caster.mageSpellsCast[_spellCircle - 1] > 0) result = true;

        return result;
    }

    public void Cast_Spell(Character_Class _caster, string _spell, Character_Class _target = null)
    {
        Spell_Class _thisSpell = null;
        for (int i = 0; i < GameManager.SPELL.Count; i++)
            if (_spell.ToLower() == GameManager.SPELL[i].name.ToLower())
                _thisSpell = GameManager.SPELL[i];

        if(CanCastSpell(_caster, _spell))
        {
            if (_thisSpell.book == "Priest") _caster.priestSpellsCast[_thisSpell.circle - 1]++;
            if (_thisSpell.book == "Mage") _caster.mageSpellsCast[_thisSpell.circle - 1]++;
            if(_target != null)
            {
                Apply_Spell(_target, _spell);
            }
            else
            {
                Apply_Spell(_spell);
            }
        }
    }

    public void Apply_Spell(Character_Class _target, string _spell)
    {
        _spell = _spell.ToLower();
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
    }

    public void Apply_Spell(string _spell)
    {
        if (_spell == "kalki")
        {
            GameManager.PARTY.Party_Shield_Bonus = -1;
        }
        if (_spell == "kalki")
        {
            GameManager.PARTY.Party_Shield_Bonus = -1;
        }
        if (_spell == "matu")
        {
            GameManager.PARTY.Party_Shield_Bonus = -2;
        }
        if (_spell == "bammatu")
        {
            GameManager.PARTY.Party_Shield_Bonus = -4;
        }
        if (_spell == "masopic")
        {
            GameManager.PARTY.Party_Shield_Bonus = -4;
        }
        if (_spell == "maporfic")
        {
            GameManager.PARTY.MAPORFIC = true;
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

        if(_spell == "latumofis")
        {
            for (int i = 0; i < 6; i++)
            {
                if (!GameManager.PARTY.EmptySlot(i))
                    if (GameManager.PARTY.LookUp_PartyMember(i).Poison > 0)
                        GameManager.PARTY.LookUp_PartyMember(i).Poison = 0;
            }
        }
    }
}
