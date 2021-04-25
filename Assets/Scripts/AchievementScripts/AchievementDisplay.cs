using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AchievementDisplay : MonoBehaviour
{
    public Achievement achievement;

    public Text achNameText;
    public Text achDescriptionText;
    public Text ProgressText;
    public GameObject progressBar;

    //Replaced gameObjects with images
    public Image achImage;
    public Image FillImage;

    public void SetAchievementData(Achievement data)
    {
        achievement = data;
        achNameText.text = achievement.achName;
        achDescriptionText.text = achievement.achDescription;

        achImage.sprite = achievement.achImage;

        progressBar.GetComponent<AwardProgressBar>().SavePostion();
        progressBar.GetComponent<AwardProgressBar>().SetData(data);
    }

    public void UpdateAwardProgress()
    {        
        progressBar.GetComponent<AwardProgressBar>().SetData(achievement);
    }
}
