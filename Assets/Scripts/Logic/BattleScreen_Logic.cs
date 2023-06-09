using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScreen_Logic : MonoBehaviour
{
    private float WAIT_TIME = 0.5f;

    public TMPro.TextMeshProUGUI action_output, partyStats, feedback, fight_btn, spell_btn, parry_btn, run_btn, use_btn, dispel_btn;
    public TMPro.TextMeshProUGUI[] mg_btn;
    public GameObject[] MonsterGroupPortrait, ActionButton;
    public Sprite[] MonsterPortrait;
    public Castle_Pop_Up_Manager PopUp;

    private Monster_Class[] monsterGroup;
    private int[,] monster_HP;
    private string[,] monster_Status;
    private string _feedbackText = "";
    private string[] mg_button_command;
    private int isSurprise; //-1 = monsters surprise players, 0 = no surprise round, 1 = players surprise monsters
    private bool isFriendly;
    private string phase;
    
    //mnake these private after debugging
    public List<string> player_command = new List<string>();
    public List<string> monster_command = new List<string>();
    //public List<string> turn = new List<string>();
    [SerializeField]private List<InitElement> Initiative = new List<InitElement>();
    private int Current_Player_Slot;
    private int Current_Monster_Group;
    private int Total_Party_Level;
    private int Total_Monster_Morale;
    private bool monsters_demoralized;

    private int[] hero_slot;

    private struct InitElement
    {
        public string action;
        public int Init;

        public InitElement(string _txt, int _int)
        {
            action = _txt;
            Init = _int;
        }
    }


    public void Start()
    {
        action_output.fontSize = GameManager.FONT;
        partyStats.fontSize = GameManager.FONT;
        feedback.fontSize = GameManager.FONT;
        fight_btn.fontSize = GameManager.FONT;
        spell_btn.fontSize = GameManager.FONT;
        parry_btn.fontSize = GameManager.FONT;
        run_btn.fontSize= GameManager.FONT;
        use_btn.fontSize= GameManager.FONT;
        dispel_btn.fontSize= GameManager.FONT;
        for (int i = 0; i < mg_btn.Length; i++)
        {
            mg_btn[i].fontSize = GameManager.FONT;
            mg_btn[i].text = "";
        }
        for (int i = 0; i < ActionButton.Length; i++)
        {
            ActionButton[i].SetActive(false);
        }
        hero_slot = new int[6];
        for (int i = 0; i < 6; i++)
        {
            if (GameManager.PARTY.EmptySlot(i))
                hero_slot[i] = -1;
            if (!GameManager.PARTY.EmptySlot(i))
                hero_slot[i] = i;
        }

        BeginBattle();
    }

    private void ShowFeedback(string _input)
    {

        if (_feedbackText == "")
        {
            StartCoroutine(Start_Feedback_CR(_input));
        }
        else
        {            
            StartCoroutine(Change_Feedback_CR(_input));
        }

    }
    private IEnumerator Start_Feedback_CR(string _input)
    {
        _feedbackText = _input;
        float wait = .03f;
        feedback.text = "<color=#404040>" + _feedbackText + "</color>";
        yield return new WaitForSeconds(wait);
        feedback.text = "<color=#808080>" + _feedbackText + "</color>";
        yield return new WaitForSeconds(wait);
        feedback.text = "<color=#FFFFFF>" + _feedbackText + "</color>";
    }
    private IEnumerator Change_Feedback_CR(string _input)
    {
        float wait = .03f;
        feedback.text = "<color=#808080>" + _feedbackText + "</color>";
        yield return new WaitForSeconds(wait);
        feedback.text = "<color=#404040>" + _feedbackText + "</color>";
        yield return new WaitForSeconds(wait);
        feedback.text = "<color=#000000>" + _feedbackText + "</color>";
        yield return new WaitForSeconds(wait);
        StartCoroutine(Start_Feedback_CR(_input));
    }

    private void ShowMonsterStatus()
    {
        //update portaits
        for (int i = 0; i < monsterGroup.Length; i++)
        {
            if(monsterGroup[i] != null)
            {
                MonsterGroupPortrait[i].SetActive(true);
                MonsterGroupPortrait[i].GetComponent<Image>().sprite = MonsterPortrait[monsterGroup[i].pic];
            }
            else
            {
                MonsterGroupPortrait[i].SetActive(false);
            }
        }

        string enemyline = "";
        for (int i = 0; i < monsterGroup.Length; i++)
        {
            enemyline = "(";
            int _ok_count = 0;
            for(int j = 0; j < monsterGroup[i].monster.Count; j++)
                if (monsterGroup[i].monster[j].myStatus == BlobberEngine.Enum._Status.OK) _ok_count++;
            enemyline += _ok_count + ") " + monsterGroup[i].monster.Count + " " + monsterGroup[i].name;
            mg_btn[i].text = enemyline;
        }
        

    }
    private void ShowPartyStatus(string _updateText = "")
    {
        if (_updateText == "")
        {
            //build party string
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (hero_slot[i] > 0)
                {
                    Character_Class me = GameManager.PARTY.LookUp_PartyMember(hero_slot[i]);

                    //Name replacement for spacing
                    string _tmpNam = me.name; while (_tmpNam.Length < 15) _tmpNam += " ";

                    //armor class replacment for spacing
                    int _ac_bonus = GameManager.PARTY.Party_Shield_Bonus ? -2 : 0;
                    string _ac = (me.ArmorClass + _ac_bonus).ToString();
                    if (me.ArmorClass > 0 && me.ArmorClass < 10) _ac = " " + _ac;
                    if (me.ArmorClass < -9) _ac = "lo";
                    if (me.ArmorClass > 11) _ac = "hi";

                    //HP replacement, for spacing                    
                    string _hp = me.HP.ToString();
                    if (me.HP > 9999) _hp = "lots";
                    if (me.HP < 1000) _hp = " " + _hp;
                    if (me.HP < 100) _hp = " " + _hp;
                    if (me.HP < 10) _hp = " " + _hp;
                    string _hpMax = me.HP_MAX.ToString();
                    if (me.HP > 9999) _hpMax = "lots";
                    if (me.HP < 1000) _hpMax = " " + _hpMax;
                    if (me.HP < 100) _hpMax = " " + _hpMax;
                    if (me.HP < 10) _hpMax = " " + _hpMax;

                    //Status replacment, for spacing
                    string _stat = me.status.ToString();
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison != 0) _stat = "POISON";
                    if (me.status == BlobberEngine.Enum._Status.OK && me.Poison == 0) _stat = _hpMax;

                    _partyText[i] = _tmpNam + " " + me.alignment.ToString()[0] + "-" +
                        me.character_class.ToString()[0] + me.character_class.ToString()[1] + me.character_class.ToString()[2] + " " +
                        _ac + " " + _hp + " " + _stat + "\n";
                }
                else
                {
                    _partyText[i] = "\n";
                }
            }
            //build static info
            _updateText = "character name  class ac hits status\n";
            _updateText += _partyText[0];
            _updateText += _partyText[1];
            _updateText += _partyText[2];
            _updateText += _partyText[3];
            _updateText += _partyText[4];
            _updateText += _partyText[5];
        }

        _updateText = _updateText.ToUpper();
        _updateText = _updateText.Replace("[D]", "d");
        partyStats.text = _updateText;
    }

    private void BeginBattle()
    {
        #region Monster_Setup
        // Get the first monster        
        int _total = 1, _level = GameManager.PARTY._PartyXYL.z;
        Monster_Class firstMonster = GameManager._monster_that_starts_battle,
                      secondMonster = null,
                      thirdMonster = null,
                      fourthMonster = null;        

        //check for second monster group
        if(Random.Range(0,100)+1 <= firstMonster.partner_chance)
        {
            _total++;
            secondMonster = GameManager.MONSTER[firstMonster.partner_index];
        }

        //check for third monster group
        if(_level > 1 && _total == 2 && Random.Range(0,100) + 1 <= secondMonster.partner_chance) 
        {
            _total++;
            thirdMonster = GameManager.MONSTER[secondMonster.partner_index];
        }

        //check for fourth monster group
        if(_level > 2 && _total == 3 && Random.Range(0,100) + 1 <= thirdMonster.partner_chance) 
        {
            _total++;
            fourthMonster = GameManager.MONSTER[secondMonster.partner_index];
        }

        //clear spell casting
        firstMonster.mageSpellsCast = 0; firstMonster.priestSpellsCast = 0;
        if (_total > 1) { secondMonster.mageSpellsCast = 0; secondMonster.priestSpellsCast = 0; }
        if (_total > 2) { thirdMonster.mageSpellsCast = 0; thirdMonster.priestSpellsCast = 0; }
        if (_total > 3) { fourthMonster.mageSpellsCast = 0; fourthMonster.priestSpellsCast = 0; }

        //now that they are generated, assign to array
        monsterGroup = new Monster_Class[_total];
        monsterGroup[0] = firstMonster;
        if (_total > 1) monsterGroup[1] = secondMonster;
        if (_total > 2) monsterGroup[2] = thirdMonster;
        if (_total > 3) monsterGroup[3] = fourthMonster;

        //How many in each group?
        int[] num = new int[_total];
        for (int i = 0; i < _total; i++)
        {
            //Debug.Log(monsterGroup[i].group_size.num + "d" + monsterGroup[i].group_size.sides + "+" + monsterGroup[i].group_size.bonus);
            num[i] = monsterGroup[i].group_size.Roll();
            //Debug.Log("roll = " + num[i]);
            if(num[i] > GameManager.PARTY._PartyXYL.z + 4) num[i] = GameManager.PARTY._PartyXYL.z + 4;
        }

        //calculate hp and set status to ok
        for (int i = 0; i < _total; i++)
        {
            monsterGroup[i].groupName = monsterGroup[i].names_unk; //set group name to plural unknown
            if (num[i] == 1) monsterGroup[i].groupName = monsterGroup[i].name_unk; //set to single unknown if only one
            monsterGroup[i].identified = false; // default to false;
            monsterGroup[i].level = monsterGroup[i].HitDice.num; //set the level from the number of hitdice
            monsterGroup[i].monster.Clear(); //prepare the monster list for individuals
            for (int j = 0; j < num[i]; j++)
            {
                monsterGroup[i].monster.Add(new Monster()); //add a monster individual
                monsterGroup[i].monster[j].myName = monsterGroup[i].name_unk + "(" + j + ")"; //name it the unknown single name
                monsterGroup[i].monster[j].myHP = monsterGroup[i].HitDice.Roll(); // roll the hitdice for initial HP
                monsterGroup[i].monster[j].myWounds = 0;
                monsterGroup[i].monster[j].myStatus = BlobberEngine.Enum._Status.OK; // set the status to OK
            }
        }
        #endregion
        #region enconter setup
        // check if friendly, if there is only one group.
        if(monsterGroup.Length == 1)
        {
            int _roll = Random.Range(0, 100) + 1, chance = 0;
            switch (monsterGroup[0].monster_class)
            {
                case BlobberEngine.Enum._Class.fighter: chance = 11; break;
                case BlobberEngine.Enum._Class.mage: chance = 6; break;
                case BlobberEngine.Enum._Class.priest: chance = 16; break;
                case BlobberEngine.Enum._Class.thief: chance = 4; break;
                case BlobberEngine.Enum._Class.dragon: chance = 26; break;
                default: chance = 1; break;
            }
            isFriendly = _roll <= chance ?  true :  false;

            //override for evil party
            for (int i = 0; i < 6; i++)
                if(!GameManager.PARTY.EmptySlot(i) && GameManager.PARTY.LookUp_PartyMember(i).alignment != BlobberEngine.Enum._Alignment.neutral)
                {
                    if (GameManager.PARTY.LookUp_PartyMember(i).alignment == BlobberEngine.Enum._Alignment.evil) isFriendly = false;
                    break;
                }

            //check for surprise
            isSurprise = 0;
            if (Random.Range(0, 100)+1 <= 20)
            {
                isSurprise++;
            }
            else if (Random.Range(0, 100) + 1 <= 20)
            {
                isSurprise--;
            }
        }
        #endregion

        //Calulate Total_Party_level
        Total_Party_Level = 0;
        for (int i = 0; i < 6; i++)
            if (!GameManager.PARTY.EmptySlot(i)) 
                Total_Party_Level += GameManager.PARTY.LookUp_PartyMember(i).level;

        ShowMonsterStatus();
        ShowPartyStatus();
        if (isSurprise >= 0 && !isFriendly) GetPlayerCommands(0); 
        if (isSurprise < 0 && !isFriendly) DetermineMonsterAction(0);
    }
    
    //Get player actions
    private void GetPlayerCommands(int _Current_Player_Slot)
    {
        Current_Player_Slot = _Current_Player_Slot;
        phase = "Get Player Commands";
        
        if (Current_Player_Slot == 0) player_command.Clear(); //clear player_command list if this is the first player (again)

        if(Current_Player_Slot > 5)
        {
            Current_Player_Slot = 0;
            // --> monsters advance
            if (isSurprise == 0) Monsters_Advance();
            if (isSurprise > 0) Determine_Initiative();
            return;
        }

        if (GameManager.PARTY.EmptySlot(Current_Player_Slot))
        {
            GetPlayerCommands(Current_Player_Slot + 1);
            return;
        }

        for (int i = 0; i < ActionButton.Length; i++) ActionButton[i].SetActive(false);

        action_output.text = "What action will " + GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).name + " take?";

        if (Current_Player_Slot < 3) ActionButton[0].SetActive(true);

        if (isSurprise == 0) ActionButton[1].SetActive(true);

        ActionButton[2].SetActive(true);

        ActionButton[3].SetActive(true);

        ActionButton[4].SetActive(true);

        if (GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).character_class == BlobberEngine.Enum._Class.priest) ActionButton[5].SetActive(true);
        if (GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).character_class == BlobberEngine.Enum._Class.bishop &&
            GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).level > 3) ActionButton[5].SetActive(true);
        if (GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).character_class == BlobberEngine.Enum._Class.lord &&
            GameManager.PARTY.LookUp_PartyMember(Current_Player_Slot).level > 8) ActionButton[5].SetActive(true);
    }


    //Monsters advance
    private void Monsters_Advance()
    {
        phase = "Monsters Advance";
        if (monsterGroup.Length == 1)
        {
            DetermineMonsterAction(0);
            return;
        }
        StartCoroutine(Monster_Advance_CR());
    }

    IEnumerator Monster_Advance_CR()
    {

        /* When monsters advance, each monster has a �strength� value determined by this formula:
         * [Remaining HP] - 3 * (MageLevel + PriestLevel)
         * 
         * The group strength is the sum of all monsters� strength in the group, only counting monsters with OK status.
          * 
          * After group strength is calculated for all groups, monster group 4 compares its strength to monster group 3, and performs this calculation:
          * 20% * [Group 4 Strength] / [Group 3 Strength] + 31%
          * This result is the probability that each group moves up a rank.
          * 
          * The process repeats for group 3 and group 2.
          */        
        int[] _strength = new int[monsterGroup.Length];
        for (int y = 0; y < monsterGroup.Length; y++)
            for (int x = 0; x < monsterGroup[y].monster.Count; x++)
                if (monsterGroup[y].monster[x].myStatus == BlobberEngine.Enum._Status.OK) _strength[y] += monsterGroup[y].monster[x].myHP - 3;

        if (monsterGroup.Length == 4)
        {
            float _prob = 20 * _strength[3] / _strength[2] + 31;
            if (Random.Range(0f, 100f) <= _prob)
            {
                Monster_Class _temp = new Monster_Class();
                _temp = GameManager.MONSTER[monsterGroup[2].index];
                _temp.MakeCopy(monsterGroup[2].index);

                monsterGroup[2] = new Monster_Class();
                monsterGroup[2] = GameManager.MONSTER[monsterGroup[3].index];
                monsterGroup[2].MakeCopy(monsterGroup[3].index);

                monsterGroup[3] = new Monster_Class();
                monsterGroup[3] = GameManager.MONSTER[_temp.index];
                monsterGroup[3].MakeCopy(_temp.index);
                action_output.text = monsterGroup[3].groupName + " advances!";
                yield return new WaitForSeconds(WAIT_TIME);
                ShowMonsterStatus();
            }
        }
        if (monsterGroup.Length > 2)
        {
            float _prob = 20 * _strength[2] / _strength[1] + 31;
            if (Random.Range(0f, 100f) <= _prob)
            {
                Monster_Class _temp = new Monster_Class();
                _temp = GameManager.MONSTER[monsterGroup[1].index];
                _temp.MakeCopy(monsterGroup[1].index);

                monsterGroup[1] = new Monster_Class();
                monsterGroup[1] = GameManager.MONSTER[monsterGroup[2].index];
                monsterGroup[1].MakeCopy(monsterGroup[2].index);

                monsterGroup[2] = new Monster_Class();
                monsterGroup[2] = GameManager.MONSTER[_temp.index];
                monsterGroup[2].MakeCopy(_temp.index);
                action_output.text = monsterGroup[2].groupName + " advances!";
                yield return new WaitForSeconds(WAIT_TIME);
                ShowMonsterStatus();
            }
        }
        if (monsterGroup.Length > 1)
        {
            float _prob = 20 * _strength[1] / _strength[0] + 31;
            if (Random.Range(0f, 100f) <= _prob)
            {
                Monster_Class _temp = new Monster_Class();
                _temp = GameManager.MONSTER[monsterGroup[0].index];
                _temp.MakeCopy(monsterGroup[0].index);

                monsterGroup[0] = new Monster_Class();
                monsterGroup[0] = GameManager.MONSTER[monsterGroup[1].index];
                monsterGroup[0].MakeCopy(monsterGroup[1].index);

                monsterGroup[1] = new Monster_Class();
                monsterGroup[1] = GameManager.MONSTER[_temp.index];
                monsterGroup[1].MakeCopy(_temp.index);
                action_output.text = monsterGroup[1].groupName + " advances!";
                yield return new WaitForSeconds(WAIT_TIME);
                ShowMonsterStatus();
            }
        }
        DetermineMonsterAction(0);
    }

    //determine monster actions
    private void DetermineMonsterAction(int _current_monster_group)
    {
        Current_Monster_Group = _current_monster_group;
        phase = "Monster Actions";

        if(Current_Monster_Group == monsterGroup.Length)
        {
            // -> Determine Initiative ->
            Determine_Initiative();
            return;
        }

        //calculate morale level
        if(Current_Monster_Group == 0)
        {
            monster_command.Clear();

            for (int i = 0; i < monsterGroup.Length; i++)
            {
                int _counter = 0;
                for (int c = 0; c < monsterGroup[i].monster.Count; c++)
                    if (monsterGroup[i].monster[c].myStatus == BlobberEngine.Enum._Status.OK)
                        _counter++;
                Total_Monster_Morale += monsterGroup[i].level * _counter;
            }
            monsters_demoralized = false;
            if (Total_Party_Level > Total_Monster_Morale) monsters_demoralized = true;
        }

        for (int c = 0; c < monsterGroup[Current_Monster_Group].monster.Count; c++)
        {
            bool _done = false;
            //run?
            if (!_done && monsters_demoralized && monsterGroup[Current_Monster_Group].abilities.Contains("Run") && Random.Range(0, 100) + 1 <= 65)
            {
                monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:Run");
                _done = true;
            }
            //Call for help?
            if (!_done && monsterGroup[Current_Monster_Group].abilities.Contains("Call") && monsterGroup[Current_Monster_Group].monster.Count < 5 && Random.Range(0, 100) + 1 <= 75)
            {
                monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:Call");
                _done = true;
            }
            //breath attack
            if (!_done && isSurprise == 0 && monsterGroup[Current_Monster_Group].special.Contains("breath") && Random.Range(0,100)+1 <= 60)
            {
                monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:Breath");
                _done = true;
            }
            //cast mage spell
            if (!_done && monsterGroup[Current_Monster_Group].mage_spells - monsterGroup[Current_Monster_Group].mageSpellsCast > 0 && Random.Range(0, 100) + 1 <= 75)
            {
                monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:CastMageSpell");
                _done = true;
            }
            //cast priest spell
            if (!_done && monsterGroup[Current_Monster_Group].priest_spells - monsterGroup[Current_Monster_Group].priestSpellsCast > 0 && Random.Range(0, 100) + 1 <= 75)
            {
                monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:CastPriestSpell");
                _done = true;
            }
            //attack
            if(!_done) monster_command.Add("Monster:" + Current_Monster_Group + ":" + c + ":action:Attack");
        }
       
        DetermineMonsterAction(Current_Monster_Group + 1);
    }

    //determine initiative
    private void Determine_Initiative()
    {
        phase = "Determine Initiative";
        Initiative.Clear();
        if (isSurprise >= 0) //Assign player commands to initiative list
        {
            for (int i = 0; i < player_command.Count; i++)
            {
                string[] _all = player_command[i].Split(':');
                int _init = Random.Range(0, 9), _agi = GameManager.PARTY.LookUp_PartyMember(int.Parse(_all[1])).Agility;
                if (_agi <= 3) _init += 3;
                if (_agi == 4 || _agi == 5) _init += 2;
                if (_agi == 6 || _agi == 7) _init += 1;
                if (_agi == 15) _init += -1;
                if (_agi == 16) _init += -2;
                if (_agi == 17) _init += -3;
                if (_agi == 18) _init += -4;
                if (_init < 1) _init = 1;
                Initiative.Add(new InitElement(player_command[i], 7));
            }
        }
        if(isSurprise <= 0) //Assign monster commands to initiative list
        {
            for (int i = 0; i < monster_command.Count; i++)
            {
                Initiative.Add(new InitElement(monster_command[i], Random.Range(0, 7) + 2));
            }
        }
        
        isSurprise = 0; //Surprise round ends.

        //Sort the initiative list
        Initiative.Sort(SortByInitiative);
        for (int i = 0; i < Initiative.Count; i++)
            Debug.Log(Initiative[i].Init + ", " + Initiative[i].action);

        RunActions();
    }
    private int SortByInitiative(InitElement p1, InitElement p2)
    {
        return p1.Init.CompareTo(p2.Init);
    }
    
        
    //run through initiative order, applying actions
    private void RunActions()
    {
        phase = "Action!";
        StartCoroutine(RunActons_CR());
    }
    IEnumerator RunActons_CR()
    {
        for (int _turn = 0; _turn < Initiative.Count; _turn++)
        {
            string[] _allterms = Initiative[_turn].action.Split(":");

            yield return new WaitForSeconds(WAIT_TIME);
         
            ShowPartyStatus();
            ShowMonsterStatus();
        }
    }
    public void MonsterGroupButton_pushed(int _monstergroup)
    {

    }

    public void ActionButtonPushed(string _action)
    {
        player_command.Add("Player:" + Current_Player_Slot + ":action:" + _action);
        GetPlayerCommands(Current_Player_Slot+1);
    }
}


