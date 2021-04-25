using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all methods for the Achievement screen buttons
/// </summary>
public class BF_AchievementsScreen : MonoBehaviour
{
    [Tooltip("UI Element")]
    public GameObject AchievementScreen;

    public GameObject AchievementPrefab;
    public GameObject AchievementSO;

    //Is this for a button or should it be initialized when the screen is active?
    private void LoadAchievements()
    {

        //TODO
        //To be called on start?

    }

    public void GoBack(GameObject previousScreen)
    {

        previousScreen.SetActive(true);
        this.gameObject.SetActive(false);
        //TODO
        //Add method to unload Achievements?

    }

}
