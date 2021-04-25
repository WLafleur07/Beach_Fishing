using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardProgressBar : MonoBehaviour
{
    public GameObject Bar;
    public Text progressText;
    private Vector3 StartingLocation;
    private Vector3 StartingScale;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void SavePostion()
    {
        StartingLocation = Bar.transform.localPosition;
        StartingScale = Bar.transform.localScale;
    }

    public void SetData(Achievement Data)
    {
        int progress = AchievementManager.Instance.GetProgress(Data.type);
        progress = Mathf.Min(progress, Data.TotalValues);
        float percentage = (float)progress / (float)Data.TotalValues;

        Bar.GetComponent<Image>().fillAmount = percentage;
        if (progress >= Data.TotalValues)
        {
            Bar.GetComponent<Image>().color = new Color(0.0f, 1.0f, 0.0f);
            progressText.text = "COMPLETED";
        }
        else
        {
            Bar.GetComponent<Image>().color = new Color(1.0f, 1.0f, 0.0f);
            progressText.text = progress.ToString() + "/" + Data.TotalValues.ToString();
        }
    }

}
