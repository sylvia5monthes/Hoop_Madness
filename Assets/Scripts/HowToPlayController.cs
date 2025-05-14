using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// manages the how to play panel
public class HowToPlayController : MonoBehaviour
{
    public GameObject how_to_play_panel;
    // Start is called before the first frame update
    void Start() {
        how_to_play_panel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShowHowToPlayPanel() {
        how_to_play_panel.SetActive(true);
    }
    public void HideHowToPlayPanel() {
        how_to_play_panel.SetActive(false);
    }
}
