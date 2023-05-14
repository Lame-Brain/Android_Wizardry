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
    [HideInInspector] public string _Persistent_Message = "";

    public GameObject _initCanvas, _initPanel;
    public TMPro.TextMeshProUGUI Screen_Sizing_String;
    public Material _doormat; //weird place to put this, i know.

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

        if(FindObjectOfType<Castle_Logic_Manager>()) FindObjectOfType<Castle_Logic_Manager>().Start();
        if(FindObjectOfType<Dungeon_Logic_Manager>()) FindObjectOfType<Dungeon_Logic_Manager>().Start();
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
            _n = 5; _spl.target = _splData[_n];
            _n = 6; _spl.camp = _splData[_n] == "TRUE" ? true : false;
            _n = 7; _spl.combat = _splData[_n] == "TRUE" ? true : false;
            _n = 8; _spl.learn_bonus = int.Parse(_splData[_n]);

            SPELL.Add(_spl);
        }
    }

    private void MakeDefaultRoster()
    {
        Character_Class fighter1 = new Character_Class();
        fighter1.Load_Character("19,5,18,27,18,15,20,8,0,0,0,0,0,0,0,0,0,1,1,1,1216,0,-4,0,0,0,0,133,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,1000,10,10,10,10,0,0,1,2,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,11,8,5,15,8,9,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        Character_Class fighter2 = new Character_Class();
        fighter2.Load_Character("2,1,19,8,0,0,0,0,0,0,0,0,0,0,0,0,0,3,1,2,1072,0,-3,0,-4,0,0,176,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,1000,13,13,10,10,0,0,1,2,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,11,7,10,18,5,6,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        Character_Class priest = new Character_Class();
        priest.Load_Character("6,1,20,8,5,18,27,7,9,12,0,0,0,0,0,0,0,1,3,1,1020,0,-1,0,0,-3,0,135,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,1050,8,8,8,10,0,0,1,2,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,8,8,11,10,8,9,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,2;0;0;0;0;0;0;,0;0;0;0;0;0;0;,1;1;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        Character_Class thief = new Character_Class();
        thief.Load_Character("16,5,16,16,19,0,0,0,0,0,0,0,0,0,0,0,0,5,4,2,1168,0,0,0,-3,0,-3,131,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,900,6,6,6,10,0,0,1,2,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,7,7,7,10,11,18,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        Character_Class mage = new Character_Class();
        mage.Load_Character("20,1,20,20,5,18,19,0,0,0,0,0,0,0,0,0,0,2,2,2,1090,0,0,-2,0,0,-3,121,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,1100,4,4,4,10,0,0,2,1,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,7,18,10,6,9,17,2;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;1;1;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        Character_Class bishop = new Character_Class();
        bishop.Load_Character("9,19,5,14,0,0,0,0,0,0,0,0,0,0,0,0,0,4,5,1,1000,0,0,-2,0,-4,-2,95,-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;-1||0||0||0||;,1,0,1000,6,6,6,10,0,0,1,2,0,0,0,0,-1,-1,-1,-1,-1,-1,0,0,0,0,0,7,12,12,8,10,9,2;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;,0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;1;1;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;0;,");
        ROSTER.Add(fighter1);
        ROSTER.Add(fighter2); 
        ROSTER.Add(priest);
        ROSTER.Add(thief); 
        ROSTER.Add(mage);
        ROSTER.Add(bishop);

        
        //DEBUG
        ROSTER[0].Inventory[0] = new Item(1, false, false, true);
        ROSTER[0].Inventory[1] = new Item(11, false, false, true);
        ROSTER[0].Inventory[2] = new Item(8, false, false, true);
        ROSTER[0].Inventory[3] = new Item(14, false, false, true);
        ROSTER[1].Inventory[0] = new Item(1, false, false, true);
        ROSTER[1].Inventory[1] = new Item(11, false, false, true);
        ROSTER[1].Inventory[2] = new Item(8, false, false, true);
        ROSTER[1].Inventory[3] = new Item(14, false, false, true);
        ROSTER[2].Inventory[0] = new Item(3, false, false, true);
        ROSTER[2].Inventory[1] = new Item(11, false, false, true);
        ROSTER[2].Inventory[2] = new Item(8, false, false, true);
        PARTY.AddMember(0);
        PARTY.AddMember(1);
        PARTY.AddMember(2);
        PARTY.AddMember(3);
        PARTY.AddMember(4);
        PARTY.AddMember(5);
        for (int i = 0; i < SPELL.Count; i++)
            ROSTER[5].SpellKnown[i] = true;
        for (int i = 0; i < 7; i++)
        {
            ROSTER[5].mageSpells[i] = 9;
            ROSTER[5].priestSpells[i] = 9;
        }

        ROSTER.Add(new Character_Class());
        ROSTER[ROSTER.Count - 1].name = "BRUTUS";
        ROSTER[ROSTER.Count - 1].status = Enum._Status.lost;
        ROSTER[ROSTER.Count - 1].lostXYL = new Vector3Int(1, 0, 1);

        //DEBUG

    }

    public void LoadGame()
    {
        if (!File.Exists(Application.persistentDataPath + "/Roster.wiz"))
        {
            MakeDefaultRoster();
            return;
        }

        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        FileStream file = File.Open(Application.persistentDataPath + "/Roster.wiz", FileMode.Open);
        Save_Game_Class sd = (Save_Game_Class)bf.Deserialize(file);
        file.Close();

        ROSTER.Clear();
        for (int i = 0; i < sd.SG_ROSTER.Count; i++)
        {
            Character_Class _newToon = new Character_Class();
            _newToon.Load_Character(sd.SG_ROSTER[i]);

            //Debug.Log("This is " + _newToon.name);
            //Debug.Log(_newToon.Save_Character());

            ROSTER.Add(_newToon);
        }
        for (int z = 0; z < 10; z++) for (int y = 0; y < 20; y++) for (int x = 0; x < 20; x++)
                    PARTY.tile_visited[x, y, z] = sd.SG_map[x, y, z];
        PARTY.Temple_Favor = sd.SG_TempleFavor;
        for (int i = 0; i < sd.SG_BoltacStock.Length; i++)
            PARTY.BoltacStock[i] = sd.SG_BoltacStock[i];
        PARTY.mem = sd.SG_mem;

        //DEBUG
        //PARTY.AddMember(0);
        //PARTY.AddMember(1);
        //PARTY.AddMember(2);
        //PARTY.AddMember(3);
        //PARTY.AddMember(4);
        //PARTY.AddMember(5);
        //DEBUG
    }

    public void SaveGame()
    {
        System.Runtime.Serialization.Formatters.Binary.BinaryFormatter bf = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
        if (File.Exists(Application.persistentDataPath + "/Roster.wiz")) File.Delete(Application.persistentDataPath + "/Roster.wiz");
        FileStream file = File.Create(Application.persistentDataPath + "/Roster.wiz");

        Save_Game_Class SaveGame = new Save_Game_Class(ITEM.Count);
        for (int i = 0; i < ROSTER.Count; i++) SaveGame.SG_ROSTER.Add(ROSTER[i].Save_Character());
        for (int z = 0; z < 10; z++)
            for (int y = 0; y < 20; y++)
                for (int x = 0; x < 20; x++)
                    SaveGame.SG_map[x, y, z] = PARTY.tile_visited[x, y, z];
        SaveGame.SG_TempleFavor = PARTY.Temple_Favor;
        for (int i = 0; i < ITEM.Count; i++) SaveGame.SG_BoltacStock[i] = PARTY.BoltacStock[i];
        SaveGame.SG_mem = PARTY.mem;

        bf.Serialize(file, SaveGame);
        file.Close();
    }

    public void RestoreRoster()
    {
        for (int i = 5; i > 0; i--)
            if (!PARTY.EmptySlot(i))
                PARTY.RemoveMember(i); 
        for (int i = 0; i < ROSTER.Count; i++)
        {
            ROSTER[i].status = Enum._Status.OK;
            ROSTER[i].location = Enum._Locaton.Roster;
        }
    }
    public void KillRoster()
    {
        if (File.Exists(Application.persistentDataPath + "/Roster.wiz")) File.Delete(Application.persistentDataPath + "/Roster.wiz");
        Application.Quit();
    }

}
