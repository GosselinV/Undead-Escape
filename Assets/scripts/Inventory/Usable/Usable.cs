using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Usable : Item
{
    protected int healthReturn = 0;
    protected int ammoAmount = 0;

    public int HealthReturn
    {
        get { return healthReturn; }
    }

    public int AmmoAmount
    {
        get { return ammoAmount; }
    }
}
