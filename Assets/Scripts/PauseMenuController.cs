using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages the pause menu
public class PauseMenuController : MonoBehaviour {

    public GameObject pause_menu_panel; 
    public LevelManager level_manager;
    public GameObject level_select_panel;

    // Start is called before the first frame update
    void Start(){
        // initially, the pause menu is not active
        if (pause_menu_panel != null) {
            pause_menu_panel.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update(){
        // if (Input.GetKeyDown(KeyCode.Escape)){
        //     if (is_paused){
        //         ResumeGame();
        //     } else {
        //         PauseGame();
        //     }
        // }
    }

    public void PauseGame() {
        Time.timeScale = 0; // pause the game

        if (pause_menu_panel != null) {
            pause_menu_panel.SetActive(true);
        }

        level_manager.PauseSong();
    }

    public void ResumeGame() {
        Time.timeScale = 1; // resume the game

        if (pause_menu_panel != null) {
            pause_menu_panel.SetActive(false);
        }

        level_manager.ResumeSong();
    }

    public void RestartLevel() {
        if (level_manager != null) {
            level_manager.RestartLevel();
        }

        if (pause_menu_panel != null) {
            pause_menu_panel.SetActive(false);
        }
    }

    public void BackToMainMenu() {
        
        if (pause_menu_panel != null) {
            pause_menu_panel.SetActive(false);
        }

        if (level_select_panel != null) {
            level_select_panel.SetActive(true);
        }

        // stop audio if still playing
        level_manager.StopAllAudio();
    }
}
