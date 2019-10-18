using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MenuButtons
{
    public void InventoryHandle()
    {
        if (cam.mainMenu != null)
        {
            cam.mainMenu.SetActive(true);
        }
        else if (cam.pauseMenu != null)
        {
            cam.pauseMenu.SetActive(true);
        }
        Instantiate(Resources.Load("prefabs/Menus/HelpInventory"));
        if (cam.hud != null)
        {
            halfHudEvent.Invoke(false);
            hudEvent.Invoke(false);
        }
    }

}
