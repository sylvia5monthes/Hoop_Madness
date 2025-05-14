using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

// controls the result panel
public class ResultPanelController : MonoBehaviour {

    public GameObject result_panel;
    public TMP_Text result_text;
    public Button next_level_button;
    public LevelManager level_manager;
    public GameObject levelSelectPanel;


    // Start is called before the first frame update
    void Start(){
        if (result_panel != null) {
            result_panel.SetActive(false);
        } 
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void ShowPanel(string message, bool show_next_level_button = false) {
        result_panel.SetActive(true);

        if (result_text != null) {
            result_text.text = message;
        }

        if (next_level_button != null) {
            next_level_button.gameObject.SetActive(show_next_level_button);
        }

        // freeze the game
        Time.timeScale = 0f; // pause the game
    }

    public void HidePanel() {
        result_panel.SetActive(false);
    }

    public void RestartLevel() {

        if (level_manager != null) {
            level_manager.RestartLevel();
        }
    }

    public void LoadNextLevel() {
  
        if (level_manager != null) {
            level_manager.LoadNextLevel();
        }
    }

    public void BackToMainMenu() {
        
        if (result_panel != null) {
            result_panel.SetActive(false);
        }

        if (levelSelectPanel != null) {
            levelSelectPanel.SetActive(true);
        }

        // stop audio if still playing
        level_manager.StopAllAudio();
    }
}
