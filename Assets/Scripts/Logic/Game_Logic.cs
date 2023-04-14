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
        test.Strength = 14; test.IQ = 13;  test.Vitality = 16; test.Luck = 18;
        test.ageInWeeks = 52 * 45; test.level = 12; test.ArmorClass = -58;
        test.Inventory[0] = new Item(1, false, false, true); test.EquipItem(0); test.hitDiceSides = 10;
        test.Inventory[1] = new Item(7,false,false,true);
        //test.Inventory[2] = new Item(1,false,false,true);
        //test.mageSpells[0] = 1; test.priestSpells[0] = 1;
        //for (int i = 0; i < SPELL.Count; i++) test.SpellKnown[i] = true;
        //test.SpellKnown[2] = false; test.SpellKnown[6] = false;
        test.HP = 5; test.HP_MAX = 5;
        another_test.name = "Evan";
        third_test.name = "Roberts"; third_test.location = Enum._Locaton.Dungeon;

        test.Geld = 1001;
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
}
