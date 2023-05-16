using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleScreen_Logic : MonoBehaviour
{
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
    
    //mnake these private after debugging
    public List<string> player_command = new List<string>();
    public List<string> monster_command = new List<string>();
    public List<string> turn = new List<string>();

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

        BeginBattle();
    }

    public void ShowFeedback(string _input)
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

    public void ShowMonsterStatus()
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
    public void ShowPartyStatus(string _updateText = "")
    {
        if (_updateText == "")
        {
            //build party string
            string[] _partyText = new string[6];
            for (int i = 0; i < 6; i++)
            {
                _partyText[i] = "";
                if (!GameManager.PARTY.EmptySlot(i))
                {
                    Character_Class me = GameManager.PARTY.LookUp_PartyMember(i);

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

    public void BeginBattle()
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
            Debug.Log(monsterGroup[i].group_size.num + "d" + monsterGroup[i].group_size.sides + "+" + monsterGroup[i].group_size.bonus);
            num[i] = monsterGroup[i].group_size.Roll();
            Debug.Log("roll = " + num[i]);
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
                monsterGroup[i].monster[j].myStatus = BlobberEngine.Enum._Status.OK; // set the status to OK
            }
        }
        #endregion

        Battle_Main_Loop();
    }

    public void Battle_Main_Loop()
    {
        //Get player actions
        //Monsters advance
        //determine monster actions
        //determine initiative
            ///Each round, each character has an initiative roll of 1d10. Initiative is further modified by agility.
                ///3 = +2 
                ///4 & 5 = +1
                ///6 & 7 = 0
                ///8 thru 14 = -1
                ///15 = -2
                ///16 = -3
                ///17 = -4
                ///18 = -5
            ///Monsters’ initiatives are each set to 1d8+1. Everyone acts in order of initiative, from lowest to highest.
        //run through initiative order, applying actions

        ShowMonsterStatus();
        ShowPartyStatus();
    }

    public void MonsterGroupButton_pushed(int _monstergroup)
    {

    }

    /* When monsters advance, each monster has a “strength” value determined by this formula:
     * [Remaining HP] - 3 * (MageLevel + PriestLevel)
     * 
     * The group strength is the sum of all monsters’ strength in the group, only counting monsters with OK status.
     * 
     * After group strength is calculated for all groups, monster group 4 compares its strength to monster group 3, and performs this calculation:
     * 20% * [Group 4 Strength] / [Group 3 Strength] + 31%
     * This result is the probability that each group moves up a rank.
     * 
     * The process repeats for group 3 and group 2.
     */
}


