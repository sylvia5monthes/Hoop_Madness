using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// data type to store the level data
[System.Serializable]
public class LevelData {
    public int level; // level number
    public int target_score; // score to reach
    public int time_limit; // time limit in seconds

    public List<BallConfig> basketball_configs; // list of basketball prefabs that can potentially be spawned
}
