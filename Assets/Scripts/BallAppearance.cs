using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control the appearance of the ball by changing its sprite, color, and powerup type based on the configuration
public class BallAppearance : MonoBehaviour {
    public SpriteRenderer ball_sprite_renderer;
    public BallPowerup powerup;
    public Rigidbody2D ball_rigidbody;
    public CircleCollider2D ball_collider;

    // Start is called before the first frame update
    void Start(){
        
    }

    // Update is called once per frame
    void Update(){
        
    }

    public void ApplyConfig(BallConfig config) {
        if (ball_sprite_renderer != null && config.ball_sprite != null) {
            ball_sprite_renderer.sprite = config.ball_sprite; // set the sprite of the ball
            ball_sprite_renderer.color = config.ball_color;
        }

        if (powerup != null) {
            powerup.type = config.powerup_type; // set the powerup type of the ball
        }

        if (ball_rigidbody != null) {
            ball_rigidbody.mass = config.mass; // set the mass of the ball
        }
    }
}
