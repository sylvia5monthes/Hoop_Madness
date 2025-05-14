using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FloatingScore : MonoBehaviour {

    public float display_time = 1.2f;
    public float timer = 0f;
    private TMP_Text text;

    // Start is called before the first frame update
    void Start() {
        text = GetComponent<TMP_Text>();
        if (text != null) {
            text.text = "";
        }
    }

    // Update is called once per frame
    void Update() {
        if (timer > 0f) {
            timer -= Time.deltaTime;
            if (timer <= 0f && text != null) {
                text.text = "";
            }
        }
    }

    public void SetPoints(int points) {
        if (text != null) {
            text.text = "+" + points.ToString();
            timer = display_time;
        } 
    }
}
