using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// in charge of updating the score UI
public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public TMP_Text score_text; 
    public TextPopEffect score_pop_effect; // reference to the text pop effect script
    public float three_point_threshold = 4f;

    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    // checks if the instance is null and if so assigns it to the current instance
    // Awake is called when the script instance is being loaded
    private void Awake() {
        if (instance == null) {
            instance = this;
        } else {
            Destroy(gameObject);
        }
    }

    public void UpdateScoreUI(int score) {
        Debug.Log("Updating score UI: " + score);
        if (score_text != null) {
            score_text.text = "Score: " + score;

            // trigger the pop effect
            if (score_pop_effect != null) {
                score_pop_effect.Pop();
            } 
        } 
    }




}
