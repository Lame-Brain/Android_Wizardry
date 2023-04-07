using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Game_Logic : MonoBehaviour
{
    public static Party_Class PARTY;
    public static List<Character_Class> ROSTER = new List<Character_Class>();
    public static List<Spell_Class> SPELL = new List<Spell_Class>();
    public static List<Item_Class> ITEM = new List<Item_Class>();

    [SerializeField]
    private TextAsset ItemListCSV, SpellListCSV;

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

        LoadCSVs();

        //Debug
        Character_Class test = new Character_Class(), another_test = new Character_Class(), third_test = new Character_Class();
        test.name = "Ethan"; test.race = Enum._Race.human;  test.alignment = Enum._Alignment.good;
        test.Strength = 14; test.Vitality = 16; test.Luck = 18;
        test.ageInWeeks = 52 * 45; test.level = 12; test.ArmorClass = -58;
        test.Inventory[0] = new Item(29, false, false, false);
        test.Inventory[1] = new Item(7,false,false,true);
        test.Inventory[2] = new Item(1,false,false,true);
        test.mageSpells[0] = 1; test.priestSpells[0] = 1;
        Debug.Log("spelk = " + SPELL.Count);
        for (int i = 0; i < SPELL.Count; i++) test.SpellKnown[i] = true;
        test.SpellKnown[2] = false; test.SpellKnown[6] = false;
        test.HP = 5; test.HP_MAX = 5;
        another_test.name = "Evan";
        third_test.name = "Roberts"; third_test.location = Enum._Locaton.Dungeon;

        test.Geld = 10000;
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
        //PARTY.AddMember(2);
    }

    private void LoadCSVs()
    {
        //Item List
        string[] All_Items = ItemListCSV.text.Split("\n");
        for (int i = 0; i < All_Items.Length; i++)
        {
            string[] _itmData = All_Items[i].Split(",");
            int _n; Item_Class _itm = new Item_Class();
            _n = 0; _itm.index = int.Parse(_itmData[_n]);
            _n = 1; _itm.name = _itmData[_n];
            _n = 2; _itm.name_unk = _itmData[_n];
            //3 is item type
            //4 is item align
            _n = 5; _itm.cursed = _itmData[_n] == "TRUE" ? true : false;
            _n = 6; _itm.change_to = int.Parse(_itmData[_n]);
            _n = 7; _itm.change_chance = int.Parse(_itmData[_n]);
            _n = 8; _itm.price = int.Parse(_itmData[_n]);
            _n = 9; _itm.store_stock = int.Parse(_itmData[_n]);
            _n = 10; _itm.spell = _itmData[_n];
            _n = 11; _itm.class_use = _itmData[_n];
            _n = 12; _itm.heal_pts = int.Parse(_itmData[_n]);
            _n = 13; _itm.damage.num = int.Parse(_itmData[_n]);
            _n = 14; _itm.damage.sides = int.Parse(_itmData[_n]);
            _n = 15; _itm.damage.bonus = int.Parse(_itmData[_n]);
            _n = 16; _itm.armor_mod = int.Parse(_itmData[_n]);
            _n = 17; _itm.hit_mod = int.Parse(_itmData[_n]);
            _n = 18; _itm.xtra_swings = int.Parse(_itmData[_n]);            
            _n = 19; _itm.crit_hit = _itmData[_n] == "TRUE" ? true : false;
            _n = 20; _itm.wep_vs_type = _itmData[_n];
            _n = 21; _itm.ac_vs_type = _itmData[_n];
            string swtch = _itmData[3].ToLower();
            switch (swtch) //Weapon, Armor, Helmet, Gauntlets, Special, Misc, Consumable
            {                
                case "weapon":
                    _itm.item_type = Enum._Item_Type.Weapon;
                    break;
                case "armor":
                    _itm.item_type = Enum._Item_Type.Armor;
                    break;
                case "shield":
                    _itm.item_type = Enum._Item_Type.Shield;
                    break;
                case "helmet":
                    _itm.item_type = Enum._Item_Type.Helmet;
                    break;
                case "gauntlet":
                    _itm.item_type = Enum._Item_Type.Gauntlets;
                    break;
                case "special":
                    _itm.item_type = Enum._Item_Type.Special;
                    break;
                case "consumable":
                    _itm.item_type = Enum._Item_Type.Consumable;
                    break;
                default:
                    _itm.item_type = Enum._Item_Type.Misc;
                    break;
            }
            swtch = _itmData[4].ToLower();
            switch (swtch) // none, good, neutral, evil
            {                
                case "good":
                    _itm.item_align = Enum._Alignment.good;
                    break;
                case "neutral":
                    _itm.item_align = Enum._Alignment.neutral;
                    break;
                case "evil":
                    _itm.item_align = Enum._Alignment.evil;
                    break;
                default:
                    _itm.item_align = Enum._Alignment.none;
                    break;
            }

            ITEM.Add(_itm);
        }

        //Spell List
        string[] All_Spells = SpellListCSV.text.Split("\n");
        for (int i = 0; i < All_Spells.Length; i++)
        {
            string[] _splData = All_Spells[i].Split(",");
            int _n; Spell_Class _spl = new Spell_Class();
            _n = 0; _spl.index = int.Parse(_splData[_n]);
            _n = 1; _spl.circle = int.Parse(_splData[_n]);
            _n = 2; _spl.book = _splData[_n];
            _n = 3; _spl.name = _splData[_n];
            _n = 4; _spl.word = _splData[_n];
            _n = 5; _spl.camp = _splData[_n] == "TRUE" ? true : false;
            _n = 6; _spl.combat = _splData[_n] == "TRUE" ? true : false;
            _n = 7; _spl.learn_bonus = int.Parse(_splData[_n]);

            SPELL.Add(_spl);
        }
    }



    #region LEVEL_UP
    public void LevelUpCharacter(int _n)
    {
        Character_Class _me = ROSTER[_n];
        Display_Screen_Controller _display = FindObjectOfType<Display_Screen_Controller>();
        bool _newSpells = false;
        int _deltaStr = 0, _deltaIQ = 0, _deltaPi = 0, _deltaVit = 0, _deltaAgi = 0, _deltaLk = 0;

        //Increase level and assign new nnl        
        _me.level++; _me.xp_nnl = new XPTable().LookupNNL(_me.level, _me.character_class);

        //Each attrib has a 75% chance to change
        int VAL = 65;
        bool _strChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _IQChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _PiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _VitChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _AgiChange = Random.Range(0, 101) <= VAL ? true : false;
        bool _LkChange = Random.Range(0, 101) <= VAL ? true : false;

        //if attrib changes, there is a chance it decreases, otherwise it increases
        float _downOdds = (_me.ageInWeeks / 52) / 130f; Debug.Log("Odds of decreasing " + _downOdds * 100);
        if (_strChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaStr = -1; }
            else { _deltaStr = 1; }
        if (_IQChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaIQ = -1; }
            else { _deltaIQ = 1; }
        if (_PiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaPi = -1; }
            else { _deltaPi = 1; }
        if (_VitChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaVit = -1; }
            else { _deltaVit = 1; }
        if (_AgiChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaAgi = -1; }
            else { _deltaAgi = 1; }
        if (_LkChange)
            if (Random.Range(0f, 1f) < _downOdds)
            { _deltaLk = -1; }
            else { _deltaLk = 1; }
        // If Attribs are 18 and delta is -1, 5 in 6 chance that they do not change
        if (_me.Strength == 18 && _deltaStr == -1 && Random.Range(0, 6) > 0) _deltaStr = 0;
        if (_me.IQ == 18 && _deltaIQ == -1 && Random.Range(0, 6) > 0) _deltaIQ = 0;
        if (_me.Piety == 18 && _deltaPi == -1 && Random.Range(0, 6) > 0) _deltaPi = 0;
        if (_me.Vitality == 18 && _deltaVit == -1 && Random.Range(0, 6) > 0) _deltaVit = 0;
        if (_me.Agility == 18 && _deltaAgi == -1 && Random.Range(0, 6) > 0) _deltaAgi = 0;
        if (_me.Luck == 18 && _deltaLk == -1 && Random.Range(0, 6) > 0) _deltaLk = 0;
        //Apply attrib changes
        _me.Strength += _deltaStr;
        _me.IQ += _deltaIQ;
        _me.Piety += _deltaPi;
        _me.Vitality += _deltaVit;
        _me.Agility += _deltaAgi;
        _me.Luck += _deltaLk;
        //Bound Attribs
        if (_me.Strength > 18) { _me.Strength = 18; _deltaStr = 0; }
        if (_me.Strength < 1) { _me.Strength = 1; _deltaStr = 0; }
        if (_me.IQ > 18) { _me.IQ = 18; _deltaIQ = 0; }
        if (_me.IQ < 1) { _me.IQ = 1; _deltaIQ = 0; }
        if (_me.Piety > 18) { _me.Piety = 18; _deltaPi = 0; }
        if (_me.Piety < 1) { _me.Piety = 1; _deltaPi = 0; }
        if (_me.Vitality > 18) { _me.Vitality = 18; _deltaVit = 0; }        
        if (_me.Agility > 18) { _me.Agility = 18; _deltaAgi = 0; }
        if (_me.Agility < 1) { _me.Agility = 1; _deltaAgi = 0; }
        if (_me.Luck > 18) { _me.Luck = 18; _deltaLk = 0; }
        if (_me.Luck < 1) { _me.Luck = 1; _deltaLk = 0; }
        //Vitality below 3 is a special case
        if (_me.Vitality < 3) 
        {
            Debug.Log("RIP");
            Castle_Logic _cl = FindObjectOfType<Castle_Logic>();
            _me.Vitality = 1; _deltaVit = 0;
            _me.status = BlobberEngine.Enum._Status.dead;
            _me.location = BlobberEngine.Enum._Locaton.Temple;
            _me.inParty = false;
            PARTY.RemoveMember(_cl._selectedRoster);
            string _RIP = _me.name + " has died of old age.".ToUpper();
            _display.PopUpMessage(_RIP);
            _cl._selected_character = null;
            _cl._selectedRoster = -1;
            _cl.townStatus = Castle_Logic.ts.Inn_Intro;
            _cl.Update_Screen();
            return;
        }
        //Swing Count
        _me.CalculateBaseSwings();
        if (_me.eqWeapon > -1 && ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqWeapon].index].xtra_swings;
        if (_me.eqArmor > -1 && ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqArmor].index].xtra_swings;
        if (_me.eqShield > -1 && ITEM[_me.Inventory[_me.eqShield].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqShield].index].xtra_swings;
        if (_me.eqHelmet > -1 && ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqHelmet].index].xtra_swings;
        if (_me.eqGauntlet > -1 && ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqGauntlet].index].xtra_swings;
        if (_me.eqMisc > -1 && ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings > _me.swing_count) _me.swing_count = ITEM[_me.Inventory[_me.eqMisc].index].xtra_swings;

        //ReRoll HP_MAX
        int _bonus = 0; //Bonus hitpoints based on vitality
        if (_me.Vitality == 3) _bonus = -3;
        if (_me.Vitality == 5 || _me.Vitality == 4) _bonus = -1;
        if (_me.Vitality == 16) _bonus = 1;
        if (_me.Vitality == 17) _bonus = 2;
        if (_me.Vitality == 18) _bonus = 3;
        int _hpDelta = _me.HP_MAX - _me.HP;
        _me.HP_MAX++; 
        int _newHP = 0; for (int i = 0; i < _me.level; i++) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //roll once per level
        if (_me.character_class == Enum._Class.samurai) _newHP += Random.Range(1, _me.hitDiceSides + 1) + _bonus; //Samurai get 1 extra roll
        if (_newHP > _me.HP_MAX) _me.HP_MAX = _newHP; //assign newHP if it is greater than HP_MAX + 1
        _me.HP = _me.HP_MAX - _hpDelta; //Gaining new HP_Max should not widen the gap between HP and HP_Max

        //Wizardry
        int a = 0, b = 0;
        //Mage Spells
        if (_me.character_class == Enum._Class.mage || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.samurai)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
            //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.mage) { a = 0; b = 2; }
            if(_me.character_class == Enum._Class.bishop) { a = 0; b = 4; }
            if(_me.character_class == Enum._Class.samurai) { a = 3; b = 3; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.mageSpells[i])
                {
                    _newSpells = true;
                    _me.mageSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.IQ / 30);
            for (int i = 29; i < 50; i++)
                if (_me.mageSpells[SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[29]) _SpC[0]++;
            if (_me.SpellKnown[30]) _SpC[0]++;
            if (_me.SpellKnown[31]) _SpC[0]++;
            if (_me.SpellKnown[32]) _SpC[0]++;
            if (_me.SpellKnown[33]) _SpC[1]++;
            if (_me.SpellKnown[34]) _SpC[1]++;
            if (_me.SpellKnown[35]) _SpC[2]++;
            if (_me.SpellKnown[36]) _SpC[2]++;
            if (_me.SpellKnown[37]) _SpC[3]++;
            if (_me.SpellKnown[38]) _SpC[3]++;
            if (_me.SpellKnown[39]) _SpC[3]++;
            if (_me.SpellKnown[40]) _SpC[4]++;
            if (_me.SpellKnown[41]) _SpC[4]++;
            if (_me.SpellKnown[42]) _SpC[4]++;
            if (_me.SpellKnown[43]) _SpC[5]++;
            if (_me.SpellKnown[44]) _SpC[5]++;
            if (_me.SpellKnown[45]) _SpC[5]++;
            if (_me.SpellKnown[46]) _SpC[5]++;
            if (_me.SpellKnown[47]) _SpC[6]++;
            if (_me.SpellKnown[48]) _SpC[6]++;
            if (_me.SpellKnown[49]) _SpC[6]++;
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.mageSpells[i] < _SpC[i]) _me.mageSpells[i] = _SpC[i];
        }
        //Priest Spells
        if (_me.character_class == Enum._Class.priest || _me.character_class == Enum._Class.bishop || _me.character_class == Enum._Class.lord)
        {
            int[] _newSP = new int[7]; for (int i = 0; i < 7; i++) _newSP[i] = 0; // New spell points
            int[] _SpC = new int[7]; for (int i = 0; i < 7; i++) _SpC[i] = 0; // spells per Circle
            //Calculate Spell Points per Circle
            if (_me.character_class == Enum._Class.priest) { a = 0; b = 2; }
            if(_me.character_class == Enum._Class.bishop) { a = 3; b = 4; }
            if(_me.character_class == Enum._Class.lord) { a = 3; b = 2; }
            for (int i = 0; i < 7; i++) //Calculate new spell points
            {
                _newSP[i] = _me.level - a + b - (b * i);
                if (_newSP[i] < 0) _newSP[i] = 0;
                if (_newSP[i] > 9) _newSP[i] = 9;

                if (_newSP[i] > _me.priestSpells[i])
                {
                    _newSpells = true;
                    _me.priestSpells[i] = _newSP[i]; //assign spell points if it is higher than current;
                }
            }
            //Learn new Spells
            float _chanceToLearn = (float)(_me.Piety / 30);
            for (int i = 0; i < 30; i++)
                if (_me.priestSpells[SPELL[i].circle - 1] > 0 && !_me.SpellKnown[i] && Random.Range(0f, 1f) + SPELL[i].learn_bonus <= _chanceToLearn)
                {
                    _newSpells = true;
                    _me.SpellKnown[i] = true;
                }
            //Count Spells Known by circle
            if (_me.SpellKnown[0]) _SpC[0]++;
            if (_me.SpellKnown[1]) _SpC[0]++;
            if (_me.SpellKnown[2]) _SpC[0]++;
            if (_me.SpellKnown[3]) _SpC[0]++;
            if (_me.SpellKnown[4]) _SpC[0]++;
            if (_me.SpellKnown[5]) _SpC[1]++;
            if (_me.SpellKnown[6]) _SpC[1]++;
            if (_me.SpellKnown[7]) _SpC[1]++;
            if (_me.SpellKnown[8]) _SpC[1]++;
            if (_me.SpellKnown[9]) _SpC[2]++;
            if (_me.SpellKnown[10]) _SpC[2]++;
            if (_me.SpellKnown[11]) _SpC[2]++;
            if (_me.SpellKnown[12]) _SpC[2]++;
            if (_me.SpellKnown[13]) _SpC[3]++;
            if (_me.SpellKnown[14]) _SpC[3]++;
            if (_me.SpellKnown[15]) _SpC[3]++;
            if (_me.SpellKnown[16]) _SpC[3]++;
            if (_me.SpellKnown[17]) _SpC[4]++;
            if (_me.SpellKnown[18]) _SpC[4]++;
            if (_me.SpellKnown[19]) _SpC[4]++;
            if (_me.SpellKnown[20]) _SpC[4]++;
            if (_me.SpellKnown[21]) _SpC[4]++;
            if (_me.SpellKnown[22]) _SpC[4]++;
            if (_me.SpellKnown[23]) _SpC[5]++;
            if (_me.SpellKnown[24]) _SpC[5]++;
            if (_me.SpellKnown[25]) _SpC[5]++;
            if (_me.SpellKnown[26]) _SpC[5]++;
            if (_me.SpellKnown[27]) _SpC[6]++;
            if (_me.SpellKnown[28]) _SpC[6]++;      
            //Make sure that there is at least 1 spell point for each known spell
            for (int i = 0; i < 7; i++) if (_me.priestSpells[i] < _SpC[i]) _me.priestSpells[i] = _SpC[i];
        }

        string _levelUpMessage = _me.name + " has leveled up!";
        if (_newSpells) _levelUpMessage += "\n"+ _me.name + " learned new spells!!!";
        if (_deltaStr < 0) _levelUpMessage += "\n" + _me.name + " has lost Strength.";
        if (_deltaStr > 0) _levelUpMessage += "\n" + _me.name + " has gained Strength.";
        if (_deltaIQ < 0) _levelUpMessage += "\n" + _me.name + " has lost I.Q.";
        if (_deltaIQ > 0) _levelUpMessage += "\n" + _me.name + " has gained I.Q.";
        if (_deltaPi < 0) _levelUpMessage += "\n" + _me.name + " has lost Piety.";
        if (_deltaPi > 0) _levelUpMessage += "\n" + _me.name + " has gained Piety.";
        if (_deltaVit < 0) _levelUpMessage += "\n" + _me.name + " has lost Vitality.";
        if (_deltaVit > 0) _levelUpMessage += "\n" + _me.name + " has gained Vitality.";
        if (_deltaAgi < 0) _levelUpMessage += "\n" + _me.name + " has lost Agility.";
        if (_deltaAgi > 0) _levelUpMessage += "\n" + _me.name + " has gained Agility.";
        if (_deltaLk < 0) _levelUpMessage += "\n" + _me.name + " has lost Luck.";
        if (_deltaLk > 0) _levelUpMessage += "\n" + _me.name + " has gained Luck.";
        _levelUpMessage = _levelUpMessage.ToUpper();
        _display.PopUpMessage(_levelUpMessage);
    }
    #endregion
}
