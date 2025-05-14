using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// in charge of the level select screen
public class LevelSelectController : MonoBehaviour {
    
    public Button[] level_buttons; // array of level buttons
    public LevelManager level_manager; 
    public GameObject level_select_panel; // panel that contains the level buttons

    // Start is called before the first frame update
    void Start() {
        level_select_panel.SetActive(true); // show the level select panel
        RefreshButtons();
    }

    // Update is called once per frame
    void Update() {
        
    }

    // refresh buttons to see whether they are unlocked or not
    public void RefreshButtons() {
        for (int i = 0; i < level_buttons.Length; i++) {
            int level = i;
            // Debug.Log("Refreshing button for level: " + level);
            Button button = level_buttons[level]; 
            //bool unlocked = ProgressManager.IsLevelUnlocked(level); // check if the level is unlocked
            bool unlocked = true;
            level_buttons[level].interactable = unlocked; // set the button interactable based on the level's unlocked status

            TMP_Text button_text = button.GetComponentInChildren<TMP_Text>();
            if (button_text != null && !unlocked) {
                button_text.color = Color.gray;
            } else {
                button_text.color = Color.white;
            }

            button.onClick.RemoveAllListeners(); // remove all listeners from the button
            button.onClick.AddListener(() => {
                SelectLevel(level); // add a listener to the button to select the level
            });

        }
    }

    public void SelectLevel(int level_index) {
        level_select_panel.SetActive(false); // hide the level select panel
        level_manager.LoadLevel(level_index); // load the selected level
    }


}
