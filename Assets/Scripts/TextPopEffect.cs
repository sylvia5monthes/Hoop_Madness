using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopEffect : MonoBehaviour {

    public float pop_scale = 1.2f; // scale to which the text will pop
    public float pop_duration = 0.5f; // duration of the pop effect

    private Vector3 original_scale; 
    private float timer = 0f; 
    private bool is_popping = false;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    private void Update() {
        if (is_popping) {
            timer -= Time.deltaTime;
            if (timer <= 0f) {
                is_popping = false;
                transform.localScale = original_scale;
            }
        }
    }

    private void Awake() {
        original_scale = transform.localScale;
    }

    public void Pop() {
        transform.localScale = original_scale * pop_scale;
        timer = pop_duration;
        is_popping = true;
    }
}
