using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// control the rotation of the ball based on its velocity
public class BallRotation : MonoBehaviour {

    private Rigidbody2D rigid_body;

    // Start is called before the first frame update
    void Start() {
        rigid_body = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        Vector2 velocity = rigid_body.velocity;

        if (velocity.magnitude > 0) {
            // Rotate based on horizontal movement
            float rotationSpeed = velocity.x * 360f * Time.deltaTime;
            transform.Rotate(0, 0, -rotationSpeed); // Negative to match left/right roll
        }
    }
}
