using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeDifficulty : MenuButtons
{
    public override void Handle()
    {
        base.Handle();
        helpDifficulties.SetActive(true);
//        if (cam.hud != null)
//        {
//            halfHudEvent.Invoke(false);
//            hudEvent.Invoke(false);
//        }
    }

}
