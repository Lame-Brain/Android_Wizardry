using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using BlobberEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float SetFloatDefault;
    public static float FONT;
    public static Party_Class PARTY;
    public static List<Character_Class> ROSTER = new List<Character_Class>();
    public static List<Spell_Class> SPELL = new List<Spell_Class>();
    public static List<Item_Class> ITEM = new List<Item_Class>();
    [SerializeField]
    private TextAsset ItemListCSV, SpellListCSV;


    public GameObject _initCanvas, _initPanel;
    public TMPro.TextMeshProUGUI Screen_Sizing_String;


    private Castle_Display_Manager _display;

    private void Awake()
    {
        //Set Singleton
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
            //Initialize screen
            FONT = SetFloatDefault;
            if (FONT == 0)
            {
                _initCanvas.SetActive(true); _initPanel.SetActive(true);
                RectTransform _myScreen = _initPanel.GetComponent<RectTransform>();
                float _screenW = Screen.width;
                float _screenH = 0.73f * _screenW;

                _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _screenW);
                _myScreen.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _screenH);

                Screen_Sizing_String.ForceMeshUpdate();
                FONT = Screen_Sizing_String.fontSize;
                _initPanel.SetActive(false); _initCanvas.SetActive(false);
            }
            return;
        }
        else
        {
            Destroy(this.gameObject);
            return;
        }
    }

    private void Start()
    {
        _display = FindObjectOfType<Castle_Display_Manager>();
        PARTY = FindObjectOfType<Party_Class>();

        PARTY.InitParty();
        LoadCSVs();
        LoadGame();

        FindObjectOfType<Castle_Logic_Manager>().StartCastle();
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
        //Load up saved stock as default
        PARTY.BoltacStock = new int[ITEM.Count];
        for (int i = 0; i < ITEM.Count; i++) PARTY.BoltacStock[i] = ITEM[i].store_stock;

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

    public void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/Roster.wiz")) return;

        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Roster.wiz", FileMode.Open);
        Save_Game_Class sd = (Save_Game_Class)bf.Deserialize(file);
        file.Close();

        ROSTER.Clear();
        for (int i = 0; i < sd.SG_ROSTER.Count; i++)
        {
            Character_Class _newToon = new Character_Class();
            _newToon.Load_Character(sd.SG_ROSTER[i]);
            ROSTER.Add(_newToon);
        }
        for (int z = 0; z < 10; z++) for (int y = 0; y < 20; y++) for (int x = 0; x < 20; x++)
                    PARTY.tile_visited[x, y, z] = sd.SG_map[x, y, z];
        PARTY.Temple_Favor = sd.SG_TempleFavor;
        for (int i = 0; i < sd.SG_BoltacStock.Length; i++)
            PARTY.BoltacStock[i] = sd.SG_BoltacStock[i];
        PARTY.mem = sd.SG_mem;

        //DEBUG
        for (int i = 0; i < 6; i++)
        {
            PARTY.AddMember(i);
        }
        PARTY.LookUp_PartyMember(0).xp = 1500;
        //PARTY.LookUp_PartyMember(0).level = 1;
        //PARTY.LookUp_PartyMember(0).HP = 100;
        //PARTY.LookUp_PartyMember(0).HP_MAX = 205;
        PARTY.LookUp_PartyMember(0).Inventory[0] = new Item(1, false, false, true);
        PARTY.LookUp_PartyMember(0).Inventory[1] = new Item(1, false, false, false);
        PARTY.LookUp_PartyMember(0).Inventory[2] = new Item(7, false, false, false);
        PARTY.LookUp_PartyMember(0).Inventory[3] = new Item(10, false, false, true);
        PARTY.LookUp_PartyMember(0).Inventory[4] = new Item(14, false, false, true);
        PARTY.LookUp_PartyMember(0).Inventory[5] = new Item(39, false, false, false);
        PARTY.LookUp_PartyMember(0).Inventory[6] = new Item(35, false, false, false);
        PARTY.LookUp_PartyMember(0).mageSpells[0] = 9;
        PARTY.LookUp_PartyMember(0).priestSpells[0] = 9;
        for (int i = 0; i < SPELL.Count; i++)
            PARTY.LookUp_PartyMember(0).SpellKnown[i] = true;


        Debug.Log(PARTY.LookUp_PartyMember(0).name + " has " + PARTY.LookUp_PartyMember(0).HP + " / " + PARTY.LookUp_PartyMember(0).HP_MAX);
        PARTY.BoltacStock[17] = 2;
        PARTY.BoltacStock[18] = 1;
        PARTY.BoltacStock[24] = 1;


        //DEBUG
    }

}
