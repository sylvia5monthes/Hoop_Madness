using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Ball/BallConfig")]
public class BallConfig : ScriptableObject
{
    public Sprite ball_sprite; // sprite for the ball
    public BallPowerup.PowerupType powerup_type; // type of powerup the ball has
    public float mass = 0.5f; // mass of the ball

    public Color ball_color = Color.white; // color of the ball
}
