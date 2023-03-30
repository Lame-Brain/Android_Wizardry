using System.Collections;
using System.Collections.Generic;

public class Spell_Class
{
    public int index;
    public string name;
    public string book; //mage or priest spell?
    public int circle; //what level of spell?
    public int learn_bonus; //first spells of a circle are easier to learn
    public bool camp, combat; //where can you cast this spell?
    public string description;
}
