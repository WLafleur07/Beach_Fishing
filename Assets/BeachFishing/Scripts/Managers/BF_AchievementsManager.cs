using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_AchievementsManager : MonoBehaviour
{
    public static BF_AchievementsManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    public void UnlockAchievement(int achievementID)
    {
        //TODO
    }
}
