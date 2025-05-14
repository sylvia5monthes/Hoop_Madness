using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is attached to the ground hit box object and is responsible for detecting when a ball enters the zone
public class GroundZone : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Ball")) {
            BallEntryTracker.ResetBall(other.gameObject);
            // Debug.Log("Ball reset after touching ground.");
        }
    }
}
