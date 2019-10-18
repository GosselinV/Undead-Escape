using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuButtons : GameManager {

	protected BoolEvent halfHudEvent = new BoolEvent();
	protected BoolEvent hudEvent = new BoolEvent();
	protected Event closeInventoryEvent = new Event ();

	protected override void Awake(){

        cam = Camera.main.GetComponent<UndeadEscape>();
        if (cam == null)
        {
            cam = Camera.main.GetComponent<MainMenus>();
        }
        base.Awake();
		EventManager.AddMenuButtonInvoker (this);
	}

	public void AddHalfHudEventListener(UnityAction<bool> listener){
		halfHudEvent.AddListener (listener);
	}

	public void AddHudEventListener(UnityAction<bool> listener){
		hudEvent.AddListener (listener);
	}

	public void AddCloseInventoryEventListener(UnityAction listener){
		closeInventoryEvent.AddListener (listener);
	}

    public virtual void Handle()
    {
        if (pauseMenu != null) {
        	pauseMenu.SetActive (false);
        }
        if (helpAbout != null) {
        	helpAbout.SetActive (false);
        }
        if (helpUI != null) {
        	helpUI.SetActive (false);
        }
        if (helpControls != null) {
        	helpControls.SetActive (false);
        }
        if (helpDifficulties != null) {
        	helpDifficulties.SetActive (false);
        }
        if (difficultiesMenu != null)
        {
            difficultiesMenu.SetActive(false);
        }
        if (inventory != null) {
        	inventory.SetActive (false);
        }
    }
}
