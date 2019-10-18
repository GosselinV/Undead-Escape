using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitInventory : MenuButtons
{
    GameManager undeadEscape;

    public override void Handle()
    {
        base.Handle();
        halfHudEvent.Invoke(false);
        hudEvent.Invoke(true);
        GameConstants.GamePause = false;
        //SetHalfHudActive(false);
        //SetHudActive(true);

    }
}
