using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MenuButtons
{

    public override void Handle()
    {
        Application.Quit();
    }
}