using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public enum GameState{
		Paused,
		Running
	}

	protected GameObject hudEndTurnButton;
	protected GameObject hudActionsPanel;
	protected Text hudHealthText;
	protected Text hudActionsText;
    protected GameManager cam;

    public GameObject hud;
	public GameState gameState;
	public List<GameObject> lootCrates;


	public GameObject mainMenu;
	public GameObject pauseMenu;
	public GameObject helpAbout;
	public GameObject helpUI;
	public GameObject helpControls;
    public GameObject helpDifficulties;
	public GameObject difficultiesMenu;
	public GameObject inventory;

	protected virtual void Awake(){
		Time.timeScale = 1;

        helpUI = GameObject.FindGameObjectWithTag("HelpUI");
        helpAbout = GameObject.FindGameObjectWithTag("HelpAbout");
        pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        inventory = GameObject.FindGameObjectWithTag("inventory");
        helpControls = GameObject.FindGameObjectWithTag("HelpControls");
        helpDifficulties = GameObject.FindGameObjectWithTag("HelpDifficulties");
        difficultiesMenu = GameObject.FindGameObjectWithTag("Options");
		mainMenu = GameObject.FindGameObjectWithTag ("MainMenu");

		helpUI = helpUI.transform.Find ("Panel").gameObject;
		helpAbout = helpAbout.transform.Find ("Panel").gameObject;
		
		pauseMenu = pauseMenu.transform.Find ("Panel").gameObject;
		helpControls = helpControls.transform.Find ("Panel").gameObject;
		helpDifficulties = helpDifficulties.transform.Find ("Panel").gameObject;
        if (inventory != null)
        {
            inventory = inventory.transform.Find("Panel").gameObject;
        }
    }

	protected virtual void Start(){

    }


	protected virtual void PauseAll(){
		pauseMenu.SetActive (false);
		helpAbout.SetActive (false);
		helpControls.SetActive (false);
		helpUI.SetActive (false);
		helpDifficulties.SetActive (false);
        if (difficultiesMenu != null)
        {
            difficultiesMenu.SetActive(false);
        }
        if (inventory != null)
        {
            inventory.SetActive(false);
        }
	}

}
