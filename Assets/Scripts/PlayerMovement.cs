using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour {
    public float speed = 5f; 
    private float base_speed; // base speed of the player
    private Vector3 base_scale; // base scale of the player
    public float jump_vertical_distance = 7f; 
    private Rigidbody2D rigid_body; 
    private bool on_ground = false;
    private bool is_movement_inverted = false;
    private bool is_dribble_throw_inverted = false;

    // variables for picking up/dribbling the ball
    private GameObject ball = null; 
    public Transform throw_hold_point; // position to hold the ball when trying to throw it
    public float hold_point_x = 1f;
    private bool is_aiming = false; 
    private bool is_throwing = false;
    private float throw_timer = 0f;
    public float throw_duration = 0.8f; // duration of the throw effect
    public Vector2 box_size = new Vector2(3f, 2f);
    public Vector2 base_box_size;
    public Vector2 box_offset = new Vector2(0f, -0.25f);
    public Vector2 base_box_offset;
    private bool is_dribbling = false;
    private float dribble_timer = 0f;
    public float dribble_duration = 0.2f; // duration of the dribble effect

    // variables for aiming visualization
    private float aim_angle = 45f;
    private float aim_speed = 60f; 
    private float min_aim_angle = -45f;
    private float max_aim_angle = 90f;
    private bool aim_increasing = true; 
    
    public LineRenderer aim_line;
    public float aim_line_length = 2.5f;

    // animations 
    Animator anim;


    // Start is called before the first frame update
    void Start() {
        rigid_body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();

        // set base stats
        base_speed = speed;
        base_scale = transform.localScale;
        base_box_size = box_size;
        base_box_offset = box_offset;
    }

    // Update is called once per frame
    void Update() {

        KeyCode dribble_key = KeyCode.Q;
        KeyCode throw_key = KeyCode.T;

        // invert the controls if needed
        if (is_dribble_throw_inverted) {
            (dribble_key, throw_key) = (throw_key, dribble_key);
        }

        if (!is_aiming) {
            float moveX = Input.GetAxis("Horizontal");

            // invert the movement if needed
            if (is_movement_inverted) {
                moveX = -moveX;
            }

            anim.SetBool("isRunning", moveX != 0);

            if (moveX != 0 && is_dribbling == false) {
                // flip the player sprite based on direction
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (moveX > 0 ? 1 : -1);
                transform.localScale = scale;
            }


            rigid_body.velocity = new Vector2(moveX * speed, rigid_body.velocity.y);

            if (Input.GetKeyDown(KeyCode.UpArrow) && on_ground) {
                rigid_body.velocity = new Vector2(rigid_body.velocity.x, jump_vertical_distance);
                on_ground = false;
                anim.SetBool("isJumping", !on_ground);
                is_dribbling = false;
                anim.SetBool("isDribbling", is_dribbling);
            } else if (Input.GetKey(dribble_key)) {
                Dribble();

            } else if (Input.GetKey(throw_key)) {
                PickupBall();
                is_dribbling = false;
                anim.SetBool("isDribbling", false); 
            } else {
                
            }
        } else {
            UpdateAimDirection(); 
            UpdateAimVisuals();
            anim.SetBool("isRunning", false);
            anim.SetBool("isDribbling", false);
        }

        if (is_aiming && Input.GetKeyUp(throw_key)) {
            ThrowBall();
        }

        // if the player is dribbling, reduce the timer
        // and stop dribbling when the timer reaches zero
        if (is_dribbling) {
            dribble_timer -= Time.deltaTime;
            if (dribble_timer <= 0f) {
                is_dribbling = false;
                anim.SetBool("isDribbling", is_dribbling);
            }
        }

        // if the player is throwing, reduce the timer
        // and stop throwing when the timer reaches zero
        if (is_throwing) {
            throw_timer -= Time.deltaTime;
            if (throw_timer <= 0f) {
                is_throwing = false;
                anim.SetBool("isThrowing", is_throwing);
                throw_timer = throw_duration; // reset the timer
            }
        }

        
         
    }

    // applies force to the ball in the direction of the aim angle 
    // and detaches it from the player
    void ThrowBall() {
        if (ball == null) {
            return;
        }

        // record ball's position for distance calculation if ball scores
        ball.GetComponent<BallShotData>().RecordShotOrigin(ball.transform.position);

        // angle to direction 
        float angle_rad = aim_angle * Mathf.Deg2Rad;
        Vector2 throw_direction = new Vector2(Mathf.Cos(angle_rad), Mathf.Sin(angle_rad)).normalized;

        // detach ball
        Rigidbody2D rigid_body = ball.GetComponent<Rigidbody2D>();
        rigid_body.isKinematic = false;
        rigid_body.velocity = Vector2.zero;
        rigid_body.AddForce(throw_direction * 8f, ForceMode2D.Impulse);

        // re-enable ball physics
        ball.GetComponent<Collider2D>().enabled = true;
        ball.transform.SetParent(null);

        // reset aiming state
        is_aiming = false;
        anim.SetBool("isAiming", is_aiming);
        aim_line.enabled = false;
        ball = null;

        is_throwing = true;
        throw_timer = throw_duration; // reset the timer
        anim.SetBool("isThrowing", is_throwing);
    }

    // update the aim direction of the ball in between the min and max angles
    // Reference: ChatGPT
    void UpdateAimDirection() {
        float delta = aim_speed * Time.deltaTime;

        if (aim_increasing) {
            aim_angle += delta;
            if (aim_angle >= max_aim_angle) {
                aim_angle = max_aim_angle;
                aim_increasing = false;
            }
        }
        else {
            aim_angle -= delta;
            if (aim_angle <= min_aim_angle) {
                aim_angle = min_aim_angle;
                aim_increasing = true;
            }
        }
    }

    // method to update the aim line visuals when aiming the ball
    void UpdateAimVisuals() {
        if (aim_line == null || throw_hold_point == null) {
            return;
        }

        Vector3 start_position = throw_hold_point.position;

        float angle_rad = aim_angle * Mathf.Deg2Rad;
        Vector2 aim_direction = new Vector2(Mathf.Cos(angle_rad), Mathf.Sin(angle_rad)).normalized;
        Vector3 end_position = start_position + (Vector3)aim_direction * aim_line_length;

        aim_line.material.mainTextureScale = new Vector2(5f, 1f);

        aim_line.SetPosition(0, start_position);
        aim_line.SetPosition(1, end_position);
    }

    // pick up the ball if it is nearby
    void PickupBall() {
        //Debug.Log("Trying to pick up ball");

        Vector2 box_center = (Vector2)transform.position + new Vector2(0f, -0.25f);
        Collider2D hit = Physics2D.OverlapBox(box_center, box_size, 0f, LayerMask.GetMask("Ball"));

        if (hit != null) {
            //Debug.Log("Ball found nearby!");
            Rigidbody2D rigid_body = hit.GetComponent<Rigidbody2D>();
            if (rigid_body != null)
            {
                ball = hit.gameObject;

                float direction = Mathf.Sign(transform.localScale.x);
                Debug.Log("Player direction: " + direction);
                // determine the side of the player to hold the ball
                if (direction > 0) {
                    // player is to the left of the ball
                    aim_angle = 45f;
                    min_aim_angle = -45f;
                    max_aim_angle = 90f;

                   
                } else {
                    Debug.Log("Player is to the right of the ball");
                    // player is to the right of the ball
                    aim_angle = 135f;
                    min_aim_angle = 90f;
                    max_aim_angle = 225f;
                }   

                throw_hold_point.localPosition = new Vector3(hold_point_x, throw_hold_point.localPosition.y,throw_hold_point.localPosition.z);

                is_aiming = true;
                anim.SetBool("isAiming", is_aiming);

                rigid_body.velocity = Vector2.zero;
                rigid_body.isKinematic = true; // stop physics
                rigid_body.GetComponent<Collider2D>().enabled = false;

                ball.transform.SetParent(throw_hold_point);
                ball.transform.localPosition = Vector3.zero;

                // reset the ball's status 
                BallEntryTracker.ResetBall(ball);
                //Debug.Log("Ball reset after pickup.");

                // Debug.Log("Ball picked up, now aiming...");
                aim_line.enabled = true;
            }
        }
    }

    // dribble the ball if it is nearby by applying a force to it
    void Dribble(){
        // Debug.Log("Dribble key pressed — applying force");

        Vector2 box_center = (Vector2)transform.position + box_offset;
        Collider2D hit = Physics2D.OverlapBox(box_center, box_size, 0f, LayerMask.GetMask("Ball"));

        if (hit != null)
        {
            Rigidbody2D rigid_body = hit.GetComponent<Rigidbody2D>();

            if (rigid_body != null)
            {
                Rigidbody2D player_rigid_body = GetComponent<Rigidbody2D>();
                float speed_x = player_rigid_body.velocity.x;

                float horizontal_force = 0f;
                if (Mathf.Abs(speed_x) > 0.05f)
                {
                    horizontal_force = Mathf.Clamp(speed_x, -1f, 1f) * 0.3f;
                    float facing = speed_x > 0 ? 1f : -1f;
                    horizontal_force += facing * 0.1f;
                }

                Vector2 dribble_force = new Vector2(horizontal_force, -1f).normalized;

                rigid_body.velocity = Vector2.zero;
                rigid_body.AddForce(dribble_force * 6f, ForceMode2D.Impulse);

                // update animation
                dribble_timer = dribble_duration;
                
                if (!is_dribbling) {
                    is_dribbling = true;
                    anim.SetBool("isDribbling", is_dribbling);
                }

                // Flip player based on ball side
                float ballX = rigid_body.transform.position.x;
                float playerX = transform.position.x;
                bool dribble_on_left = ballX < playerX;

                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) * (dribble_on_left ? -1 : 1);
                transform.localScale = scale;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision){
        if (collision.gameObject.CompareTag("Ground")) {
            on_ground = true;
            anim.SetBool("isJumping", !on_ground);
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Ground")) {
            on_ground = false;
            anim.SetBool("isJumping", !on_ground);
        }
    }

    // method to set the speed of the player
    public void ModifySpeed(float multiplier) {
        speed = base_speed * multiplier;
    }

    // method to scale the hitbox for the basketball
    public void ScaleBoxSize(float multiplier) {
        float old_height = box_size.y;
        box_size *= multiplier;

        float new_height = box_size.y;
        float offset = (new_height - old_height) / 2;
        box_offset -= new Vector2(0f, offset);
    }
    
    // reset the player's scale and hitbox size
    public void ResetPlayerSize() {
        box_size = base_box_size;
        box_offset = base_box_offset;
        transform.localScale = base_scale;
    }

    // reset the player's controls to default in case they are inverted
    public void ResetPlayerControls() {
        is_dribble_throw_inverted = false;
        is_movement_inverted = false;
    }

    // sets the status of whether to invert the player's movement
    public void ApplyInverseMovement(bool state) {
        is_movement_inverted = state;
    }

    // sets the status of whether to invert the player's dribble and throw controls
    public void ApplyInverseDribbleThrow(bool state) {
        is_dribble_throw_inverted = state;
    }

    public void AddBow(bool state) {
        anim.SetBool("hasBow", state);
    }

    public void AddDress(bool state) {
        anim.SetBool("hasDress", state);
    }

    public void AddSocks(bool state) {
        anim.SetBool("hasSocks", state);
    }

    // reset all animations to the default ones (not the bonus level ones)
    public void ClearAllOutfits() {
        anim.SetBool("hasBow", false);
        anim.SetBool("hasDress", false);
        anim.SetBool("hasSocks", false);
    }

    // debugging hitbox for dribbling
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Vector2 box_center = (Vector2)transform.position + box_offset;
        Gizmos.DrawWireCube(box_center, box_size);

        if (throw_hold_point != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(throw_hold_point.position, 0.2f);
        }

    }
    
}
