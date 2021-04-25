using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contains all methods for the Menu screen buttons
/// </summary>
public class BF_MenuScreen : MonoBehaviour
{

    public void OpenAchievements(GameObject AchievementScreen)
    {

        AchievementScreen.SetActive(true);
        this.gameObject.SetActive(false);

    }

    public void OpenCustomization(GameObject customizationScreen)
    {

        customizationScreen.SetActive(true);
        this.gameObject.SetActive(false);

    }

    public void OpenSettings(GameObject settingsScreen)
    {

        settingsScreen.SetActive(true);
        this.gameObject.SetActive(false);

    }

    public void OpenChallenges(GameObject challengesScreen)
    {

        challengesScreen.SetActive(true);
        this.gameObject.SetActive(false);

    }

    public void Quit()
    {
        //TODO
    }
}
