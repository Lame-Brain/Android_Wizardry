using System.Collections;
using System.Collections.Generic;
using BlobberEngine;
public class Item
{
    public int index;
    public bool equipped;
    public bool curse_active;
    public bool identified;

    public Item(int _indx = -1, bool _eqpd = false, bool _curse = false, bool _id = false)
    {
        this.index = _indx;
        this.equipped = _eqpd;
        this.curse_active = _curse;
        this.identified = _id;
    }
}
