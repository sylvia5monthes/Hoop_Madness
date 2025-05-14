using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control the shot data of the ball
public class BallShotData : MonoBehaviour {

    public Vector2 shot_origin; 
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        
    }

    // record where the player shot the ball from
    // this is used to determine if the shot is a 2 or 3 pointer if it goes in
    public void RecordShotOrigin(Vector2 origin) {
        shot_origin = origin;
        // Debug.Log("Shot origin recorded: " + shot_origin);
    }
}
