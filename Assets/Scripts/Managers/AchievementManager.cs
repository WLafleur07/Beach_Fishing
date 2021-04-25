using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AchievementManager : MonoBehaviour
{
    public static AchievementManager Instance { get; set; }

    private List<Achievement> Achievements;
    private List<GameObject> AchievementItems;

    public Canvas canvas;
    public GameObject AchievementScreenGO;
    public GameObject AchievementPopup;

    public Achievement[] achievements;

    //vars for animating popup
    private const float AWARD_POPUP_OFFSCREEN_XPOS = -1000.0f;
    private const float BF_AWARD_POPUP_OFFSCREEN_XPOS = -1070.0f;
    private const float BF_AWARD_POPUP_ONSCREEN_XPOS = -750.0f;
    private const float AWARD_POPUP_ONSCREEN_XPOS = -640.0f;
    private const float AWARD_POPUP_ORIGINAL_WIDTH = 350.0f;
    private const float ORIGINAL_WIDTH = 640.0f;
    private const float ORIGINAL_HEIGHT = 960.0f;

    private float SCALED_OFFSCREEN_X = AWARD_POPUP_OFFSCREEN_XPOS;
    private float SCALED_ONSCREEN_X = AWARD_POPUP_ONSCREEN_XPOS;

    private GameObject popup = null;
    private float targetX = AWARD_POPUP_ONSCREEN_XPOS;
    private float slidespeed = 0f;
    private bool isWaiting = false;
    private float waitTimer = 0.0f;
    private bool isAnimating = false;

    private int legendary;
    private int gold;
    private int silver;
    private int bronze;

    public Camera mainCamera;

    private const int NUM_BADGES = 20;

    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        //Loading all achievements
        achievements = Resources.LoadAll<Achievement>("Achievements");     
    }

    // Start is called before the first frame update
    void Start()
    {
        float oldAspect = ORIGINAL_WIDTH / ORIGINAL_HEIGHT;
        float newAspect = canvas.pixelRect.width / canvas.pixelRect.height;

        float aspectDiff = 1 + (oldAspect - newAspect);

        if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.BEACH_FISHING])
        {
            SCALED_OFFSCREEN_X = (BF_AWARD_POPUP_OFFSCREEN_XPOS * aspectDiff);
            SCALED_ONSCREEN_X = (BF_AWARD_POPUP_OFFSCREEN_XPOS * aspectDiff) + (AWARD_POPUP_ORIGINAL_WIDTH * aspectDiff);

        }
        else
        {
            SCALED_OFFSCREEN_X = (AWARD_POPUP_OFFSCREEN_XPOS * aspectDiff);
            SCALED_ONSCREEN_X = (AWARD_POPUP_OFFSCREEN_XPOS * aspectDiff) + (AWARD_POPUP_ORIGINAL_WIDTH * aspectDiff);
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isAnimating && popup != null)
        {
            //move achievement popup
            if (isWaiting == false)
            {
                if ((popup.transform.localPosition.x <= targetX && targetX != SCALED_OFFSCREEN_X)
                    || (popup.transform.localPosition.x >= targetX && targetX == SCALED_OFFSCREEN_X))  //when popup isn't at its target
                {
                    popup.transform.localPosition = new Vector3(popup.transform.localPosition.x + slidespeed, popup.transform.localPosition.y, popup.transform.localPosition.z);
                }
                else // when popup has reached its target
                {
                    // reset animation variables and delete popup when animation is over
                    if (!isWaiting && targetX != SCALED_OFFSCREEN_X)
                    {
                        isWaiting = true;
                    }
                    else 
                    {
                        waitTimer = 0.0f;
                        Destroy(popup);
                        isAnimating = false;
                    }
                }
            }

            // pause when achievement popup is fully out
            if (isWaiting)
            {
                waitTimer += Time.deltaTime;

                if (waitTimer >= 2.0f) // after waiting, start moving popup off of the screen
                {
                    isWaiting = false;
                    targetX = SCALED_OFFSCREEN_X;

                    if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.BEACH_FISHING])
                    {
                        slidespeed = -4f;
                    }
                    else
                    {
                        slidespeed = -2f;
                    }
                }
            }
        }
    }

    public void IncreaseTwitterPosts()
    {
        UserDataManager.Instance.Save();

        AchievementScreenGO.GetComponent<AchievementScreenClass>().UpdateProgress();
        // Show popup for badge collected achievements
        StartCoroutine(ShowPopup());
    }

    public IEnumerator ShowPopup()
    {
        for (int i = 0; i < achievements.Length; i++)
        {
            float achProgress = GetProgress(achievements[i].type);
            float achGoal = achievements[i].TotalValues;
            yield return new WaitForSeconds(1f);
            // Check if achievement is completed, or just passed 50%
            /*if ((UserDataManager.Instance.User_data.Achievements_Completed[achievements[i].AchievementID] < 2 && achProgress == achGoal) ||
                (UserDataManager.Instance.User_data.Achievements_Completed[achievements[i].AchievementID] < 1 && achProgress >= (achGoal / 2) && (achProgress - 1) < achGoal / 2))
            {
                Vector3 location = new Vector3(0, 0, 0);
                // we need to wait here because if we get progress on two awards at the same time, 
                // both try to instantiate and we only keep a record of the most recent. let one go first 
                // then instantiate the other when the first one finishes
                while (popup != null)
                {
                    yield return new WaitForSeconds(1f);
                }
                //create achievement popup and set it as a child of the canvas
                if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.MAIN_SCENE])
                {
                    location = new Vector3(-10, -4, 0);
                }
                else if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.UPPER_CANADA])
                {
                    location = new Vector3(-20, -4, canvas.transform.position.z);
                }
                else if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.BEACH_FISHING])
                {
                    location = new Vector3(-970, -100, 0);
                }

                popup = Instantiate(AchievementPopup, location, Quaternion.identity);
                popup.transform.SetParent(canvas.transform);

                if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.BEACH_FISHING])
                {
                    popup.transform.localScale = new Vector3(1.137778f, 1.137778f, 1.137778f);
                    slidespeed = 4f;
                }
                else
                {
                    slidespeed = 2f;
                }

                    //set content of popup
                popup.GetComponent<AchievementDisplay>().achNameText.text = achievements[i].achName;
                popup.GetComponent<AchievementDisplay>().achImage.sprite = achievements[i].achImage;

                //update progress bar
                if (achProgress >= achGoal)
                {
                    popup.GetComponent<AchievementDisplay>().FillImage.color = new Color(0.0f, 1.0f, 0.0f);
                    popup.GetComponent<AchievementDisplay>().ProgressText.text = "COMPLETED";
                }
                else
                {
                    popup.GetComponent<AchievementDisplay>().FillImage.color = new Color(1.0f, 1.0f, 0.0f);
                    popup.GetComponent<AchievementDisplay>().ProgressText.text = achProgress.ToString() + "/" + achGoal.ToString();
                }

                popup.GetComponent<AchievementDisplay>().FillImage.fillAmount = achProgress/ achGoal;
                UserDataManager.Instance.Save();
                targetX = SCALED_ONSCREEN_X;
                isAnimating = true;
            }*/
        }
    }

    public int GetProgress(Achievement.AWARD_TYPES type)
    {
        int progress = 0;
        switch (type)
        {
/*-----------------------------Beach Fishing----------------------------------------*/
            case Achievement.AWARD_TYPES.BF_RAREST_FISH_CAUGHT:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.FishRarity[i] <= 0.01 && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] == 1)
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_LEGENDARY_FISH_CAUGHT:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.FishRarity[i] < 1 && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] == 1)
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_GOLD_FISH_CAUGHT:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.FishRarity[i] >= 1 && UserDataManager.Instance.BF_data.FishRarity[i] < 10 && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] == 1 && !UserDataManager.Instance.BF_data.isGarbage[i])
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_SILVER_FISH_CAUGHT:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.FishRarity[i] >= 10 && UserDataManager.Instance.BF_data.FishRarity[i] < 35 && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] == 1 && !UserDataManager.Instance.BF_data.isGarbage[i])
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_BRONZE_FISH_CAUGHT:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.FishRarity[i] >= 35 && UserDataManager.Instance.BF_data.FishRarity[i] < 60 && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] == 1 && !UserDataManager.Instance.BF_data.isGarbage[i])
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_FISH_CAUGHT:
                {
                    if (UserDataManager.Instance.BF_data.NumFishCaught == 1)
                    {
                        progress = 1;
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_UNLOCK_ROD:
                {
                    for (int i = 0; i < UserDataManager.Instance.BF_data.RodUnlocked.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.RodUnlocked[i] == true && i != 0)
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_UNLOCK_BAIT:
                {
                    
                    for (int i = 0; i < UserDataManager.Instance.BF_data.BaitUnlocked.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.BaitUnlocked[i] == true && i != 3)
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_UNLOCK_ALL_RODS:
                {
                    bool[] unlocked = new bool[UserDataManager.Instance.BF_data.RodUnlocked.Length];

                    for (int i = 0; i < UserDataManager.Instance.BF_data.RodUnlocked.Length; i++)
                    {
                        unlocked[i] = true;
                    }

                    if (unlocked != UserDataManager.Instance.BF_data.RodUnlocked)
                    {
                        for (int i = 0; i < UserDataManager.Instance.BF_data.RodUnlocked.Length; i++)
                        {
                            if (UserDataManager.Instance.BF_data.RodUnlocked[i] == true && i != 0)
                            {
                                progress++;
                            }
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_UNLOCK_ALL_BAITS:
                {
                    bool[] unlocked = new bool[UserDataManager.Instance.BF_data.BaitUnlocked.Length];

                    for (int i = 0; i < UserDataManager.Instance.BF_data.BaitUnlocked.Length; i++)
                    {
                        unlocked[i] = true;
                    }

                    if (unlocked != UserDataManager.Instance.BF_data.BaitUnlocked)
                    {
                        for (int i = 0; i < UserDataManager.Instance.BF_data.BaitUnlocked.Length; i++)
                        {
                            if (UserDataManager.Instance.BF_data.BaitUnlocked[i] == true && i != 3)
                            {
                                progress++;
                            }
                        }
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_RAIN:
                {
                    if (UserDataManager.Instance.BF_data.hasRained)
                    {
                        progress = 1;
                    }

                    break;
                }
            case Achievement.AWARD_TYPES.BF_CATCH_TRASH:
                {

                    for (int i = 0; i < UserDataManager.Instance.BF_data.FishRarity.Length; i++)
                    {
                        if (UserDataManager.Instance.BF_data.isGarbage[i] == true && UserDataManager.Instance.BF_data.numFishCaughtByIndex[i] > 0)
                        {
                            progress = 1;
                        }
                    }

                    break;
                }
        }
        return progress;
    }
}
