using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is attached to the exit zone object and is responsible for detecting when a ball exits the zone
public class ExitZone : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        // check if the object that entered the trigger is a ball
        if (other.CompareTag("Ball")) {
            Rigidbody2D rigid_body = other.GetComponent<Rigidbody2D>();

            if (rigid_body != null && rigid_body.velocity.y > 0){
                // Debug.Log("Ball exited the zone!");
                BallEntryTracker.MarkAsInvalid(other.gameObject);
            }
        }
    }
}
