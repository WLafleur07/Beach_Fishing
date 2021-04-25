using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementScreenClass : MonoBehaviour
{
    public GameObject mainAchvScreen;
    public GameObject AwardPrefab;
    public GameObject Content;

    private List<GameObject> achievements = new List<GameObject>();

    private const float ORIGINAL_WIDTH = 640.0f;
    private const float ORIGINAL_HEIGHT = 960.0f;
    private const float PREFAB_HEIGHT = 300.0f;
    private const float PREFAB_WIDTH = 800.0f;
    private const float Y_OFFSET = -200.0f;

    float aspectDiff = 1.0f;
    void Start()
    {
        Canvas canvas = GetComponentInParent<Canvas>();

        float yCoord = mainAchvScreen.transform.position.y;


        float oldAspect = ORIGINAL_WIDTH / ORIGINAL_HEIGHT;
        float newAspect = canvas.pixelRect.width / canvas.pixelRect.height;

        aspectDiff = 1 + (oldAspect - newAspect);

        // THIS IS IMPORTANT ORDERING, WE MUST SCALE THE CONTENT FIRST THEN ADD THE AWARDS OTHERWISE ITS OFFSET
        ScaleContentToAwards();

        // NOW ADD ALL THE AWARDS
        AddAllAwardsToScreen();
    }

    private void ScaleContentToAwards()
    {
        RectTransform rect = Content.GetComponent<RectTransform>();
        // +3 here for extra scrolling space hidden by the bottom hud etc.
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, PREFAB_HEIGHT * aspectDiff * (AchievementManager.Instance.achievements.Length + 3));
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y);
    }

    private void AddAllAwardsToScreen()
    {
        for (int i = 0; i < AchievementManager.Instance.achievements.Length; i++)
        {
            GameObject Instance = GameObject.Instantiate(AwardPrefab, new Vector3(0, 0, 0), Quaternion.identity);
            Instance.transform.SetParent(Content.transform);


            Instance.transform.localScale = new Vector3(1f, aspectDiff, 1f);

            Instance.transform.localPosition = new Vector3(PREFAB_WIDTH, Y_OFFSET + (i * -PREFAB_HEIGHT * aspectDiff));

            Instance.GetComponent<AchievementDisplay>().SetAchievementData(AchievementManager.Instance.achievements[i]);

            achievements.Add(Instance);
        }
    }

    public void ScaleContentToAwards(int size)
    {
        RectTransform rect = Content.GetComponent<RectTransform>();
        // +3 here for extra scrolling space hidden by the bottom hud etc.
        rect.sizeDelta = new Vector2(rect.sizeDelta.x, PREFAB_HEIGHT * aspectDiff * (size + 3));
        rect.anchoredPosition = new Vector3(rect.anchoredPosition.x, rect.anchoredPosition.y);
    }

    public void UpdateProgress()
    {
        for (int i = 0; i < AchievementManager.Instance.achievements.Length; i++)
        {
            achievements[i].GetComponent<AchievementDisplay>().UpdateAwardProgress();
        }
    }
}
