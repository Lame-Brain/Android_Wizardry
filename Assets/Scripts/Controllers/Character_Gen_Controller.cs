using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BlobberEngine;

public class Character_Gen_Controller : MonoBehaviour
{
    public GameObject Name_Panel, Race_Panel, Stats_Panel, Class_Panel, Final_Panel, Align_Panel, Next_Button, Select_Marker;

    public TMPro.TextMeshProUGUI Name_Text, Name_Text_Input, Name_Text_Placeholder,
        Race_Text, Race_Text_Human, Race_Text_Elf, Race_Text_Dwarf, Race_Text_Gnome, Race_Text_Hobbit,
        Stat_Text, Stat_Text_Strength, Stat_Text_IQ, Stat_Text_Piety, Stat_Text_Vitality, Stat_Text_Agility, Stat_Text_Luck, Stat_Text_Bonus,
        Str_plus, str_minus, IQ_plus, IQ_minus, Pie_plus, Pie_minus, Vit_plus, Vit_minus, Agi_plus, Agi_minus, Lk_plus, Lk_minus,
        Class_Text, Class_Text_Fighter, Class_Text_Mage, Class_Text_Priest, Class_Text_Thief, Class_Text_Bishop, Class_Text_Samurai, Class_Text_Lord, Class_Text_Ninja, 
        Final_Text, Final_Yes, Final_No, Next_Button_Text, 
        Align_Text, Align_Text_Good, Align_Text_Neutral, Align_Text_Evil;


    private Enum._Race _race;
    private int _str, _iq, _pie, _vit, _agi, _lk, _bonus;
    private int _str_base, _iq_base, _pie_base, _vit_base, _agi_base, _lk_base;
    private int _str_mod, _iq_mod, _pie_mod, _vit_mod, _agi_mod, _lk_mod;
    private Enum._Class _class;
    private Enum._Alignment _align;
    private string _name;
    private int _page;

    private void OnEnable()
    {
        FindObjectOfType<Display_Screen_Controller>().Block_Buttons();
        float _FS = FindObjectOfType<Display_Screen_Controller>().FONT_SIZE;
        Name_Text.fontSize = _FS;
        Name_Text_Input.fontSize = _FS;
        Name_Text_Placeholder.fontSize = _FS;
        Race_Text.fontSize = _FS;
        Race_Text_Human.fontSize = _FS;
        Race_Text_Dwarf.fontSize = _FS;
        Race_Text_Elf.fontSize = _FS;
        Race_Text_Gnome.fontSize = _FS;
        Race_Text_Hobbit.fontSize = _FS;
        Stat_Text.fontSize = _FS;
        Stat_Text_Strength.fontSize = _FS;
        Stat_Text_IQ.fontSize = _FS;
        Stat_Text_Piety.fontSize = _FS;
        Stat_Text_Vitality.fontSize = _FS;
        Stat_Text_Agility.fontSize = _FS;
        Stat_Text_Luck.fontSize = _FS;
        Stat_Text_Bonus.fontSize = _FS;
        Str_plus.fontSize = _FS;
        str_minus.fontSize = _FS;
        IQ_plus.fontSize = _FS;
        IQ_minus.fontSize = _FS;
        Vit_plus.fontSize = _FS;
        Vit_minus.fontSize = _FS;
        Agi_plus.fontSize = _FS;
        Agi_minus.fontSize = _FS;
        Lk_plus.fontSize = _FS;
        Lk_minus.fontSize = _FS;
        Final_Text.fontSize = _FS;
        Final_Yes.fontSize = _FS;
        Final_No.fontSize = _FS;
        Next_Button_Text.fontSize = _FS;
        Align_Text.fontSize = _FS;
        Align_Text_Good.fontSize = _FS;
        Align_Text_Neutral.fontSize = _FS;
        Align_Text_Evil.fontSize = _FS;
        _page = 0;
        Hide_Cursor();
        Update_Screen();
    }
    private void OnDisable()
    {
        FindObjectOfType<Display_Screen_Controller>().Button_Block_Panel.SetActive(false);
    }

    public void Update_Screen()
    {
        Next_Button.SetActive(false);
        Next_Button.transform.SetAsLastSibling();
        if (_page == 0)
        {
            Name_Panel.SetActive(true);
            Align_Panel.SetActive(false);
            Race_Panel.SetActive(false);
            Stats_Panel.SetActive(false);
            Class_Panel.SetActive(false);
            Final_Panel.SetActive(false);
            return;
        }

        if (_page == 1)
        {
            Name_Panel.SetActive(false);
            Align_Panel.SetActive(true);
            Race_Panel.SetActive(false);
            Stats_Panel.SetActive(false);
            Class_Panel.SetActive(false);
            Final_Panel.SetActive(false);
            return;
        }

        if (_page == 2)
        {
            Name_Panel.SetActive(false);
            Align_Panel.SetActive(false);
            Race_Panel.SetActive(true);
            Stats_Panel.SetActive(false);
            Class_Panel.SetActive(false);
            Final_Panel.SetActive(false);
            return;
        }

        if (_page == 3) //Stats Panel
        {
            Name_Panel.SetActive(false);
            Align_Panel.SetActive(false);
            Race_Panel.SetActive(false);
            Stats_Panel.SetActive(true);
            Class_Panel.SetActive(false);
            Final_Panel.SetActive(false);
            Stat_Text_Bonus.text = "BONUS POINTS: " + _bonus;
            Stat_Text_Strength.text = "STRENGTH: " + _str_base + "+" + _str_mod + "=" + (_str_base + _str_mod);
            Stat_Text_IQ.text = " I.Q.: " + _iq_base + "+" + _iq_mod + "=" + (_iq_base + _iq_mod);
            Stat_Text_Piety.text = "PIETY: " + _pie_base + "+" + _pie_mod + "=" + (_pie_base + _pie_mod);
            Stat_Text_Vitality.text = "VITALITY: " + _vit_base + "+" + _vit_mod + "=" + (_vit_base + _vit_mod);
            Stat_Text_Agility.text = "AGILITY: " + _agi_base + "+" + _agi_mod + "=" + (_agi_base + _agi_mod);
            Stat_Text_Luck.text = "LUCK: " + _lk_base + "+" + _lk_mod + "=" + (_lk_base + _lk_mod);
            if (_bonus == 0)
            {
                if (_str_base + _str_mod > 10 || _iq_base + _iq_mod > 10)
                    Next_Button.SetActive(true);
                if (_agi_base + _agi_mod > 10 && _align != Enum._Alignment.good) //Thieves can't be good
                    Next_Button.SetActive(true);
                if (_pie_base + _pie_mod > 10 && _align != Enum._Alignment.neutral) //Priests can't be neutral
                    Next_Button.SetActive(true);
            }
            return;
        }

        if (_page == 4) //Classes
        {
            Name_Panel.SetActive(false);
            Align_Panel.SetActive(false);
            Race_Panel.SetActive(false);
            Stats_Panel.SetActive(false);
            Class_Panel.SetActive(true);
            Final_Panel.SetActive(false);
            Class_Text_Fighter.gameObject.SetActive(false);
            Class_Text_Mage.gameObject.SetActive(false);
            Class_Text_Priest.gameObject.SetActive(false);
            Class_Text_Thief.gameObject.SetActive(false);
            Class_Text_Bishop.gameObject.SetActive(false);
            Class_Text_Samurai.gameObject.SetActive(false);
            Class_Text_Lord.gameObject.SetActive(false);
            Class_Text_Ninja.gameObject.SetActive(false);
            _str = _str_base + _str_mod;
            _iq = _iq_base + _iq_mod;
            _pie = _pie_base + _pie_mod;
            _vit = _vit_base + _vit_mod;
            _agi = _agi_base + _agi_mod;
            _lk = _lk_base + _lk_mod;
            if (_str > 10) Class_Text_Fighter.gameObject.SetActive(true);
            if (_iq > 10) Class_Text_Mage.gameObject.SetActive(true);
            if (_pie > 10 && _align != Enum._Alignment.neutral) Class_Text_Priest.gameObject.SetActive(true);
            if (_agi > 10 && _align != Enum._Alignment.good) Class_Text_Thief.gameObject.SetActive(true);
            if (_iq > 11 && _pie > 11 && _align != Enum._Alignment.neutral) Class_Text_Bishop.gameObject.SetActive(true);
            if (_str > 14 && _iq > 10 && _pie > 9 && _vit > 13 && _agi > 9 && _align != Enum._Alignment.evil) Class_Text_Samurai.gameObject.SetActive(true);
            if (_str > 14 && _iq > 11 && _pie > 11 && _vit > 14 && _agi > 13 && _lk > 14 && _align == Enum._Alignment.good) Class_Text_Lord.gameObject.SetActive(true);
            if (_str > 14 && _iq > 14 && _pie > 14 && _vit > 14 && _agi > 14 && _lk > 14 && _align == Enum._Alignment.evil) Class_Text_Ninja.gameObject.SetActive(true);
            return;
        }

        if (_page == 5)
        {
            Name_Panel.SetActive(false);
            Align_Panel.SetActive(false);
            Race_Panel.SetActive(false);
            Stats_Panel.SetActive(false);
            Class_Panel.SetActive(false);
            Final_Panel.SetActive(true);            

            Final_Text.text = "NAME: " + _name.ToUpper() + "\n" +
                              "RACE: " + _race.ToString().ToUpper() + "\n" +
                              "CLASS: " + _class.ToString().ToUpper() + "\n" +
                              "ALIGN: " + _align.ToString().ToUpper() + "\n" +
                              "STRENGTH: " + _str + "\n" +
                              "I.Q.: " + _iq + "\n" +
                              "PIETY: " + _pie + "\n" +
                              "VITALITY: " + _vit + "\n" +
                              "AGILITY: " + _agi + "\n" +
                              "LUCK: " + _lk + "\n" +
                              "\n" +
                              "KEEP THIS CHARACTER?";
            return;
        }
    }

    public void Next_Button_Clicked()
    {
        _page++;
        if (_page > 5) _page = 5;
        Update_Screen();
    }
    public void Prev_Button_Clicked()
    {
        _page--;
        if (_page < 0) _page = 0;
        Update_Screen();
    }

    private void Set_Cursor(Transform _val)
    {
        Select_Marker.SetActive(true);
        Select_Marker.transform.SetParent(_val);
        Select_Marker.transform.SetLocalPositionAndRotation(new Vector3(0, 0, 0), Quaternion.identity);
        Select_Marker.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        Select_Marker.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);       
    }
    private void Hide_Cursor()
    {
        Select_Marker.SetActive(false);
    }

    public void Name_Input_Accepted(string _val)
    {
        _name = _val;
        _val = _val.Replace(",", " ");
        Next_Button.SetActive(true);
    }
    public void Choose_Alignment(int _val)
    {
        if (_val == 0)
        {
            _align = Enum._Alignment.good;
            Set_Cursor(Align_Text_Good.transform);
        }
        if (_val == 1)
        {
            _align = Enum._Alignment.neutral;
            Set_Cursor(Align_Text_Neutral.transform);
        }
        if (_val == 2)
        {
            _align = Enum._Alignment.evil;
            Set_Cursor(Align_Text_Evil.transform);
        }
        Next_Button.SetActive(true);
    }

    public void Choose_Race(int _val)
    {
        if(_val == 0) //Human
        {
            _race = Enum._Race.human;
            _str_base = 8;
            _iq_base = 8;
            _pie_base = 5;
            _vit_base = 8;
            _agi_base = 8;
            _lk_base = 9;
            Set_Cursor(Race_Text_Human.transform);
        }
        if(_val == 1) //Elf
        {
            _race = Enum._Race.elf;
            _str_base = 7;
            _iq_base = 10;
            _pie_base = 10;
            _vit_base = 6;
            _agi_base = 9;
            _lk_base = 6;
            Set_Cursor(Race_Text_Elf.transform);
        }
        if(_val == 2) //Dwarf
        {
            _race = Enum._Race.dwarf;
            _str_base = 10;
            _iq_base = 7;
            _pie_base = 10;
            _vit_base = 10;
            _agi_base = 5;
            _lk_base = 6;
            Set_Cursor(Race_Text_Dwarf.transform);
        }
        if(_val == 3) //Gnome
        {
            _race = Enum._Race.gnome;
            _str_base = 7;
            _iq_base = 7;
            _pie_base = 10;
            _vit_base = 8;
            _agi_base = 10;
            _lk_base = 7;
            Set_Cursor(Race_Text_Gnome.transform);
        }
        if(_val == 4) //Halfling
        {
            _race = Enum._Race.hobbit;
            _str_base = 5;
            _iq_base = 7;
            _pie_base = 7;
            _vit_base = 6;
            _agi_base = 10;
            _lk_base = 15;
            Set_Cursor(Race_Text_Hobbit.transform);
        }
        //Assign bonus points
        _bonus = Random.Range(0, 4) + 7;
        if (Random.Range(0, 11) == 0) _bonus += 10;
        if (_bonus < 20 && Random.Range(0, 11) == 0) _bonus += 10;

        Next_Button.SetActive(true);
    }

    public void Modify_Attrib(int _val)
    {
        if(_val == 1) // Strength Up
        {
            if(_str_base + _str_mod < 18 && _bonus > 0)
            {
                _str_mod++;
                _bonus--;
            }
        }
        if(_val == -1) // Strength Down
        {
            if(_str_mod > 0)
            {
                _str_mod--;
                _bonus++;
            }
        }
        if(_val == 2) // IQ up
        {
            if(_iq_base + _iq_mod < 18 && _bonus > 0)
            {
                _iq_mod++;
                _bonus--;
            }
        }
        if(_val == -2)// IQ down
        {
            if(_iq_mod > 0)
            {
                _iq_mod--;
                _bonus++;
            }
        }
        if(_val == 3)// Piety Up
        {
            if(_pie_base + _pie_mod < 18 && _bonus > 0)
            {
                _pie_mod++;
                _bonus--;
            }
        }
        if(_val == -3)// Piety down
        {
            if(_pie_mod > 0)
            {
                _pie_mod--;
                _bonus++;
            }
        }
        if(_val == 4)// Vitality up
        {
            if(_vit_base + _vit_mod < 18 && _bonus > 0)
            {
                _vit_mod++;
                _bonus--;
            }
        }
        if(_val == -4)// Vitality down
        {
            if(_vit_mod > 0)
            {
                _vit_mod--;
                _bonus++;
            }
        }
        if(_val == 5)// Agility Up
        {
            if(_agi_base + _agi_mod < 18 && _bonus > 0)
            {
                _agi_mod++;
                _bonus--;
            }
        }
        if(_val == -5)// agility down
        {
            if(_agi_mod > 0)
            {
                _agi_mod--;
                _bonus++;
            }
        }
        if(_val == 6)// Luck up
        {
            if(_lk_base + _lk_mod < 18 && _bonus > 0)
            {
                _lk_mod++;
                _bonus--;
            }
        }
        if(_val == -6)// Luck down
        {
            if(_lk_mod > 0)
            {
                _lk_mod--;
                _bonus++;
            }
        }

        Update_Screen();
    }

    public void Choose_Class(int _val)
    {
        if(_val == 0)
        {
            _class = Enum._Class.fighter;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 1)
        {
            _class = Enum._Class.mage;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 2)
        {
            _class = Enum._Class.priest;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 3)
        {
            _class = Enum._Class.thief;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 4)
        {
            _class = Enum._Class.bishop;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 5)
        {
            _class = Enum._Class.samurai;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 6)
        {
            _class = Enum._Class.lord;
            Set_Cursor(Class_Text_Fighter.transform);
        }
        if(_val == 7)
        {
            _class = Enum._Class.ninja;
            Set_Cursor(Class_Text_Fighter.transform);
        }

        Next_Button.SetActive(true);
    }

    public void FINAL_NO()
    {
        this.gameObject.SetActive(false);
    }
    public void FINAL_YES()
    {
        //FINISH CHARACTER GENERATION
        Character_Class New_Character = new Character_Class();
        New_Character.name = _name;
        New_Character.character_class = _class;
        New_Character.race = _race;
        New_Character.alignment = _align;
        New_Character.Strength = _str;
        New_Character.IQ = _iq;
        New_Character.Piety = _pie;
        New_Character.Vitality = _vit;
        New_Character.Agility = _agi;
        New_Character.Luck = _lk;
        switch (_class)
        {
            case Enum._Class.fighter:
                New_Character.hitDiceSides = 10;
                New_Character.Save_vs_Death = -3;
                break;
            case Enum._Class.mage:
                New_Character.hitDiceSides = 4;
                New_Character.Save_vs_Spell -= 3;
                New_Character.mageSpells[0] = 2;
                New_Character.SpellKnown[30] = true;
                New_Character.SpellKnown[31] = true;
                break;
            case Enum._Class.priest:
                New_Character.hitDiceSides = 8;
                New_Character.Save_vs_Petrify -= 3;
                New_Character.priestSpells[0] = 2;
                New_Character.SpellKnown[0] = true;
                New_Character.SpellKnown[1] = true;
                break;
            case Enum._Class.thief:
                New_Character.hitDiceSides = 6;
                New_Character.Save_vs_Breath -= 3;
                break;
            case Enum._Class.bishop:
                New_Character.hitDiceSides = 6;
                New_Character.Save_vs_Petrify -= 2;
                New_Character.Save_vs_Wand -= 2;
                New_Character.Save_vs_Spell -= 2;
                New_Character.mageSpells[0] = 2;
                New_Character.SpellKnown[30] = true;
                New_Character.SpellKnown[31] = true;
                break;
            case Enum._Class.samurai:
                New_Character.hitDiceSides = 8;
                New_Character.Save_vs_Death -= 2;
                New_Character.Save_vs_Spell -= 2;
                break;
            case Enum._Class.lord:
                New_Character.hitDiceSides = 10;
                New_Character.Save_vs_Death -= 2;
                New_Character.Save_vs_Petrify -= 2;
                break;
            case Enum._Class.ninja:
                New_Character.hitDiceSides = 6;
                New_Character.Save_vs_Death -= 3;
                New_Character.Save_vs_Breath -= 3;
                New_Character.Save_vs_Petrify -= 2;
                New_Character.Save_vs_Spell -= 2;
                New_Character.Save_vs_Wand -= 4;
                New_Character.hit_dam = new Dice(2, 4, 0);
                break;
            default:
                New_Character.hitDiceSides = 8;
                break;
        }
        switch (_race)
        {
            case Enum._Race.human:
                New_Character.Save_vs_Death--;
                break;
            case Enum._Race.elf:
                New_Character.Save_vs_Wand -= 2;
                break;
            case Enum._Race.dwarf:
                New_Character.Save_vs_Breath -= 4;
                break;
            case Enum._Race.gnome:
                New_Character.Save_vs_Petrify -= 2;
                break;
            case Enum._Race.hobbit:
                New_Character.Save_vs_Spell -= 3;
                break;
        }
        New_Character.HP_MAX = Random.Range(0, New_Character.hitDiceSides) + 1; //roll max hp
        if (New_Character.Vitality == 3) New_Character.HP_MAX -= 2; //Adjust for vitality
        if (New_Character.Vitality == 4 || New_Character.Vitality == 5) New_Character.HP_MAX --;
        if (New_Character.Vitality == 16) New_Character.HP_MAX ++;
        if (New_Character.Vitality == 17) New_Character.HP_MAX += 2;
        if (New_Character.Vitality == 18) New_Character.HP_MAX += 3;
        if (New_Character.HP_MAX < 2) New_Character.HP_MAX = 2; //Hp max cannot be less than 2.
        New_Character.HP = New_Character.HP_MAX; //set Hp to Hp Max
        New_Character.Geld = Random.Range(0, 100) + 90; //Geld roll
        New_Character.xp_nnl = new XPTable().LookupNNL(1, New_Character.character_class);

        //WRITE THIS CHARACTER TO THE ROSTER
        Game_Logic.ROSTER.Add(New_Character);

        this.gameObject.SetActive(false);
    }
}
