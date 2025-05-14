using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control the powerups panel
public class AboutPowerupsController : MonoBehaviour {

    public GameObject about_powerups_panel;
    void Start() {
        about_powerups_panel.SetActive(false);
    }

    // Update is called once per frame
    void Update() {
        
    }

    public void ShowHowToPlayPanel() {
        about_powerups_panel.SetActive(true);
    }
    public void HideHowToPlayPanel() {
        about_powerups_panel.SetActive(false);
    }
}
