using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallEntryTracker : MonoBehaviour
{
    public static HashSet<GameObject> valid_balls = new HashSet<GameObject>();
    public static HashSet<GameObject> invalid_balls = new HashSet<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    // check if a ball is valid for scoring
    public static void MarkAsValid(GameObject ball)
    {
        if (!valid_balls.Contains(ball))
        {
            valid_balls.Add(ball);
            // Debug.Log("Ball marked as valid: " + ball.name);
        }
    }

    // check if a ball is invalid for scoring
    public static void MarkAsInvalid(GameObject ball)
    {
        if (!invalid_balls.Contains(ball))
        {
            invalid_balls.Add(ball);
            // ("Ball marked as invalid: " + ball.name);
        }
    }
        
    // resets the ball's status by removing it from both valid and invalid lists
    public static void ResetBall(GameObject ball)
    {
        valid_balls.Remove(ball);
        invalid_balls.Remove(ball);
    }
}
