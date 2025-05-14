using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// this script is attached to the score zone object and is responsible for detecting when a ball enters the zone
public class ScoreZone : MonoBehaviour {

    public Transform hoop_position; // position of the hoop
    public FloatingScore floating_score;
    public AudioSource score_sound;
    public LevelManager level_manager;

    private void OnTriggerEnter2D(Collider2D other) {

        // check if the object that entered the trigger is a ball
        if (other.CompareTag("Ball")) {
            Rigidbody2D rigid_body = other.GetComponent<Rigidbody2D>();

            if (rigid_body != null && BallEntryTracker.valid_balls.Contains(other.gameObject) && !BallEntryTracker.invalid_balls.Contains(other.gameObject)) {
                Debug.Log("Ball scored!");
                
                BallShotData data = other.GetComponent<BallShotData>();
                if (data != null) {
                    float distance = Mathf.Abs(data.shot_origin.x - hoop_position.position.x);
                    Debug.Log("Distance from hoop: " + distance);

                    // check whether it's a 2 or 3 point shot
                    int points = (distance < GameManager.instance.three_point_threshold) ? 2 : 3;

                    // add the score to the level manager
                    level_manager.AddScore(points);

                    if (floating_score != null) {
                        floating_score.SetPoints(points);
                    }
                }
                BallEntryTracker.ResetBall(other.gameObject);

                // play the score sound
                if (score_sound != null) {
                    score_sound.PlayOneShot(score_sound.clip);
                }

                // activate powerup
                BallPowerup powerup = other.GetComponent<BallPowerup>();
                if (powerup != null) {
                    level_manager.ActivatePowerup(powerup.type);
                    level_manager.OnBallScored(powerup.type);
                }

                // apply new ball config
                level_manager.ApplyNextBall();

            } else {
                Debug.Log("Ball not scored because wrong direction.");
            }
        }
    }

    public void StopAllAudio() {
        if (score_sound != null) {
            score_sound.Stop();
        }
    }
}

