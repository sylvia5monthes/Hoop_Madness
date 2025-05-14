using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// lists all the powerups available in the game
public class BallPowerup : MonoBehaviour {
    
    public enum PowerupType {
        None,
        SpeedBoost,
        SlowMotion,
        PlayerGrowth,
        PlayerShrink,
        ReverseMovement,
        ReverseDribbleThrow,
        InvisibleHoop,
        ChocolateMint,
        Strawberry,
        CookiesAndCream
    }

    public PowerupType type = PowerupType.None;
    
    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update(){
        
    }
}
