using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounceSound : MonoBehaviour {

    private AudioSource bounce_sound;

    public float min_volumne = 0.2f;
    public float max_volumne = 1f;
    public float threshold = 5f; // how fast the ball must be falling to reach full volume

    // Start is called before the first frame update
    void Start() {
        bounce_sound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (!collision.gameObject.CompareTag("Player")) {

            float impact_speed = collision.relativeVelocity.magnitude;

            // scale volume between mi and max based on impact speed
            float volume = Mathf.Clamp01(impact_speed / threshold); // 0 to 1
            float final_volume = Mathf.Lerp(min_volumne, max_volumne, volume);

            // Play the bounce sound
            if (bounce_sound != null) {
                bounce_sound.PlayOneShot(bounce_sound.clip, final_volume);
            }
        }
    }
}
