using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// determines which levels are unlocked
public static class ProgressManager {

    public static int highest_unlocked_level = 0; // the highest level that has been unlocked
    
    public static bool IsLevelUnlocked(int level) {
        // check if the level is unlocked
        return level <= highest_unlocked_level;
    }

    public static void UnlockLevel(int level) {
        Debug.Log("Unlocking level: " + level);
        if (level > highest_unlocked_level) {
            highest_unlocked_level = level;
        }
    }
}
