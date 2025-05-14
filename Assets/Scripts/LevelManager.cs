using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// this script is attached to the level manager object and is responsible for managing the current level
// that the player is playing
public class LevelManager : MonoBehaviour {

    // var for level data
    public List<LevelData> all_levels; // list of all levels
    public int current_level_index = 0; // index of the current level
    public Transform ball_spawn_point; 
    public GameObject player;
    public Transform player_spawn_point; // spawn point for the player
    public ScoreZone score_zone;
    public LevelSelectController level_select_controller;
    

    // var for current level
    private LevelData current_level;
    public GameManager game_manager;
    public ResultPanelController result_panel_controller;
    public PauseMenuController pause_menu_controller;
    public TMP_Text timer_text; 
    public TMP_Text win_condition_text; // "Score X points in Y seconds"

    private float timer;
    private bool is_level_active = false;
    public int current_score = 0;

    // audio 
    private AudioSource buzzer_audio;
    private AudioSource marching_band_audio;

    // powerups
    public BallAppearance active_ball;
    public float speed_boost_amount = 2f; // amount to boost the speed by
    public float speed_boost_duration = 5f; // duration of the speed boost
    public float growthFactor = 1.1f; // grows by 10% each time
    public float shrinkFactor = 0.9f; // shrinks by 10% each time
    private bool is_movement_inverted = false;
    private bool is_dribble_throw_inverted = false;
    public GameObject hoop_visuals;
    public float invisible_hoop_duration = 5f; // duration of the invisible hoop

    // var for bonus level
    private HashSet<BallPowerup.PowerupType> balls_scored = new HashSet<BallPowerup.PowerupType>(); // set of active powerups
    private AudioSource ice_cream_audio;
    private AudioSource win_haw_yea; 
    private AudioSource lose_haw_yea;
    private int curr_ball_config = 0; // index of the current ball config

    // Start is called before the first frame update
    void Start() {
        // get the buzzer audio source
        AudioSource[] all_audio = GetComponents<AudioSource>();
        if (all_audio != null && all_audio.Length > 4) {
            buzzer_audio = all_audio[0];
            marching_band_audio = all_audio[1];
            ice_cream_audio = all_audio[2];
            win_haw_yea = all_audio[3];
            lose_haw_yea = all_audio[4];
        } else {
            Debug.LogError("audio source not found.");
        }

        // freeze the game at the start
        Time.timeScale = 0f;
    }

    // Update is called once per frame
    void Update() {
        if (is_level_active) {
            timer -= Time.deltaTime;

            if (timer <= 0) {
                timer = 0;
                is_level_active = false;

                // play the buzzer sound
                if (buzzer_audio != null) {
                    buzzer_audio.Play();
                    StartCoroutine(WaitForBuzzerThenCheckResult());
                } else {
                    CheckWinCondition();
                }
                
            }

            // update the timer text
            if (timer_text != null) {
                timer_text.text = "Time: " + timer.ToString("F1");
            } 

            // if escape is pressed, pause the game
            if (Input.GetKeyDown(KeyCode.Escape)) {
                if (pause_menu_controller != null) {
                    pause_menu_controller.PauseGame();
                }
            }
        }
    }

    void StartLevel() {
        // unpause the game
        Time.timeScale = 1f;

        current_level = all_levels[current_level_index];

        // move player to spawn point
        if (player != null && player_spawn_point != null) {
            player.transform.position = player_spawn_point.position;

            // reset movement
            Rigidbody2D player_movement = player.GetComponent<Rigidbody2D>();
            if (player_movement != null) {
                player_movement.velocity = Vector2.zero;
                player_movement.angularVelocity = 0f;
            } 

            // reset player size
            ResetPlayerSize();

            // reset player controls
            player.GetComponent<PlayerMovement>().ResetPlayerControls();

            // reset player speed 
            player.GetComponent<PlayerMovement>().ModifySpeed(1f);

            // reset clothes / animation
            player.GetComponent<PlayerMovement>().ClearAllOutfits();

        } else {
            Debug.LogError("Player or player spawn point is not set.");
        }

        // update ball config and reset ball
        curr_ball_config = 0;
        if (active_ball != null) {
            ApplyNextBall();

            Rigidbody2D ball_movement = active_ball.GetComponent<Rigidbody2D>();
            if (ball_movement != null) {
                ball_movement.velocity = Vector2.zero;
                ball_movement.angularVelocity = 0f;
            }

        }

        // move ball to spawn point
        if (ball_spawn_point != null) {
            active_ball.transform.position = ball_spawn_point.position;
        }

        // clear balls scored
        balls_scored.Clear();
        

        timer = current_level.time_limit;
        is_level_active = true;
        current_score = 0;

        // if bonus level, play the ice cream sound from the start
        if (current_level_index == all_levels.Count - 1) {
            if (ice_cream_audio != null) {
                ice_cream_audio.Play();
            }
        }

        // show the win condition text
        if (current_level_index == all_levels.Count - 1) {
            if (win_condition_text != null) {
                win_condition_text.text = "Level " + current_level.level + ": Score all three ball types";
            }
        } else {
            // show the win condition text
            if (win_condition_text != null) {
                win_condition_text.text = "Level " + current_level.level + ": Score " + current_level.target_score;
            }
        }

        // update the timer text
        if (timer_text != null) {
            timer_text.text = "Time: " + timer.ToString("F1");
        } 

        // reset the score UI
        if (game_manager != null) {
            game_manager.UpdateScoreUI(current_score);
        } 
    }

    public void AddScore(int points) {
        Debug.Log("Adding score: " + points);

        if (!is_level_active) return;

        current_score += points;

        // update score ui via GameManager
        if (game_manager != null) {
            game_manager.UpdateScoreUI(current_score);
        } 

    }

    private void CheckWinCondition() {
        // pause the game
        Time.timeScale = 0f; 

        // for normal levels, the current score must be >= target
        // for bonus level, aka the last leve, the player must score all three balls
        if (current_level_index == all_levels.Count - 1) {
            // check if all three balls are scored
            Debug.Log("Checking win condition for bonus level");
            Debug.Log("Balls scored include: " + string.Join(", ", balls_scored));
            if (balls_scored.Contains(BallPowerup.PowerupType.ChocolateMint) &&
                balls_scored.Contains(BallPowerup.PowerupType.Strawberry) &&
                balls_scored.Contains(BallPowerup.PowerupType.CookiesAndCream)) {
                Win();
            } else {
                Lose();
            }
        } else {
            // check if the current score is greater than or equal to the target score
            if (current_score >= current_level.target_score) {
                Win();
            } else {
                Lose();
            }
        }
    }

    private void Win() {
        // show the result panel
        if (result_panel_controller != null) {
            result_panel_controller.ShowPanel("You win! Score: " + current_score, current_level_index < all_levels.Count - 1);
        }

        // play the marching band sound if not the bonus level
        if (current_level_index != all_levels.Count - 1) {
            if (marching_band_audio != null) {
                marching_band_audio.Play();
            }
        } else {
            // stop the ice cream sound if playing
            if (ice_cream_audio != null && ice_cream_audio.isPlaying) {
                ice_cream_audio.Stop();
            }

            // play the win sound
            if (win_haw_yea != null) {
                win_haw_yea.Play();
            }
        }

        ProgressManager.UnlockLevel(current_level_index + 1);
        level_select_controller.RefreshButtons();
    }

    private void Lose() {
        Debug.Log("You lose! Score: " + current_score);
        
        if (result_panel_controller != null) {
            result_panel_controller.ShowPanel("You lose! Score: " + current_score, false);
        }

        // stop the ice cream sound if playing
        if (ice_cream_audio != null && ice_cream_audio.isPlaying) {
            ice_cream_audio.Stop();
        }

        // play the lose sound if bonus level
        if (current_level_index == all_levels.Count - 1) {
            if (lose_haw_yea != null) {
                lose_haw_yea.Play();
            }
        }
    }

    public void RestartLevel() {
        if (result_panel_controller != null) {
            result_panel_controller.HidePanel();
        }

        StopAllAudio();

        StartLevel();
    }

    public void LoadNextLevel() {
        current_level_index++;

        if (current_level_index < all_levels.Count) {
            StartLevel();
        }

        if (result_panel_controller != null) {
            result_panel_controller.HidePanel();
        }

        // stop audio if still playing
        StopAllAudio();

    }

    public void LoadLevel(int level_index) {
        if (level_index >= 0 && level_index < all_levels.Count) {
            current_level_index = level_index;
            StartLevel();
        }
    }

    // stop all audio that is playing
    public void StopAllAudio() {
        if (marching_band_audio != null && marching_band_audio.isPlaying) {
            marching_band_audio.Stop();
        }

        if (buzzer_audio != null) {
            buzzer_audio.Stop();
        }

        if (ice_cream_audio != null) {
            ice_cream_audio.Stop();
        }

        if (win_haw_yea != null) {
            win_haw_yea.Stop();
        }

        if (lose_haw_yea != null) {
            lose_haw_yea.Stop();
        }

        score_zone.StopAllAudio();
    }

    // pause all potential audio that is playing
    public void PauseSong() {
        if (marching_band_audio != null && marching_band_audio.isPlaying) {
            marching_band_audio.Pause();
        }

        if (buzzer_audio != null && buzzer_audio.isPlaying) {
            buzzer_audio.Pause();
        }

        if (ice_cream_audio != null && ice_cream_audio.isPlaying) {
            ice_cream_audio.Pause();
        }

        if (win_haw_yea != null && win_haw_yea.isPlaying) {
            win_haw_yea.Pause();
        }

        if (lose_haw_yea != null && lose_haw_yea.isPlaying) {
            lose_haw_yea.Pause();
        }
    }

    public void ResumeSong() {
        if (marching_band_audio != null) {
            marching_band_audio.UnPause();
        }

        if (buzzer_audio != null) {
            buzzer_audio.UnPause();
        }

        if (ice_cream_audio != null) {
            ice_cream_audio.UnPause();
        }

        if (win_haw_yea != null) {
            win_haw_yea.UnPause();
        }

        if (lose_haw_yea != null) {
            lose_haw_yea.UnPause();
        }
    }

    public void ActivatePowerup(BallPowerup.PowerupType powerup_type) {
        // activate the powerup
        Debug.Log("Activating powerup: " + powerup_type);
        switch (powerup_type) {
            case BallPowerup.PowerupType.SpeedBoost:
                player.GetComponent<PlayerMovement>().ModifySpeed(speed_boost_amount);
                break;
            case BallPowerup.PowerupType.SlowMotion:
                player.GetComponent<PlayerMovement>().ModifySpeed(1f/speed_boost_amount);
                break;
            case BallPowerup.PowerupType.PlayerGrowth:
                ApplyPlayerScale(growthFactor);
                break;
            case BallPowerup.PowerupType.PlayerShrink:
                ApplyPlayerScale(shrinkFactor);
                break;
            case BallPowerup.PowerupType.ReverseMovement:
                ApplyInverseMovement();
                break;
            case BallPowerup.PowerupType.ReverseDribbleThrow:
                ApplyInverseDribbleThrow();
                break;
            case BallPowerup.PowerupType.InvisibleHoop:
                StartCoroutine(ApplyInvisibleHoop());
                break;
            case BallPowerup.PowerupType.ChocolateMint:
                player.GetComponent<PlayerMovement>().AddBow(true);
                break;
            case BallPowerup.PowerupType.Strawberry:
                player.GetComponent<PlayerMovement>().AddDress(true);
                break;
            case BallPowerup.PowerupType.CookiesAndCream:
                player.GetComponent<PlayerMovement>().AddSocks(true);
                break;
        }
    }

    // picks a random ball config from the list of ball configs in the level data
    // and applies it to the active ball
    public void ApplyRandomBall() {
        var level = all_levels[current_level_index];

        if (level.basketball_configs.Count == 0) return;

        BallConfig config = level.basketball_configs[Random.Range(0, level.basketball_configs.Count)];
        // Debug.Log("Applying ball config: " + config.name);
        active_ball.ApplyConfig(config);
    }

    // apply the next ball in the order that is defined in the level data
    public void ApplyNextBallInOrder() {
        var level = all_levels[current_level_index];

        if (level.basketball_configs.Count == 0) return;

        BallConfig config = level.basketball_configs[curr_ball_config];
        // Debug.Log("Applying ball config: " + config.name);
        active_ball.ApplyConfig(config);

        curr_ball_config++;
        if (curr_ball_config >= level.basketball_configs.Count) {
            curr_ball_config = 0;
        }
    }

    // apply the next ball config depending on whether it is a bonus level or not
    public void ApplyNextBall() {
        // if bonus level, apply deterministic ball config
        if (current_level_index == all_levels.Count - 1) {
            ApplyNextBallInOrder();
        } else {
            ApplyRandomBall();
        }
    }

    private void ApplyPlayerScale(float scale) {
        Transform player_transform = player.transform;

        // grow the player by the growth factor
        // and adjust the player collider to stay the same distance from the ground
        BoxCollider2D player_collider = player.GetComponent<BoxCollider2D>();
        float previous_y = player_collider.size.y;
        player_transform.localScale *= scale;
        float new_y = player_collider.size.y;
        float offset = (new_y - previous_y)/2;
        player_collider.offset += new Vector2(0f, offset);

        // adjust player hitbox
        PlayerMovement player_movement = player.GetComponent<PlayerMovement>();
        player_movement.ScaleBoxSize(scale);

        Debug.Log("Player scale: " + player_transform.localScale);

    }

    private void ResetPlayerSize() {
        player.GetComponent<PlayerMovement>().ResetPlayerSize();
    }

    private void ApplyInverseMovement() {
        // apply the inverse movement to the player
        is_movement_inverted = !is_movement_inverted;
        player.GetComponent<PlayerMovement>().ApplyInverseMovement(is_movement_inverted);
    }

    private void ApplyInverseDribbleThrow() {
        // apply the inverse dribble throw to the player
        is_dribble_throw_inverted = !is_dribble_throw_inverted;
        player.GetComponent<PlayerMovement>().ApplyInverseDribbleThrow(is_dribble_throw_inverted);
    }

    public void OnBallScored(BallPowerup.PowerupType ball_type) {
        // add the ball type to the set of balls scored
        if (ball_type != BallPowerup.PowerupType.None) {
            balls_scored.Add(ball_type);
        }
    }


    // Coroutines 

    private IEnumerator WaitForBuzzerThenCheckResult() {
        yield return new WaitForSeconds(buzzer_audio.clip.length);
        CheckWinCondition();
    }

    private IEnumerator ApplyInvisibleHoop() {
        // make the hoop invisible
        if (hoop_visuals != null) {
            hoop_visuals.SetActive(false);
        }

        yield return new WaitForSeconds(invisible_hoop_duration);

        // make the hoop visible again
        if (hoop_visuals != null) {
            hoop_visuals.SetActive(true);
        }
    }
}
