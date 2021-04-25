using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BF_ChallengeManager : MonoBehaviour
{
    public static BF_ChallengeManager Instance { get; set; }

    public BF_ChallengeSO[] challengesSO;
    public GameObject challengePrefab;
    [Header("Attach the challenge container object from the challenge screen here.")]
    public GameObject challengeContainer;

    private List<GameObject> currentLoadedChallenges = new List<GameObject>();

    public int currentChallengeID = 0;

    public GameObject challengePopUp;

    [Tooltip("How long does the challenge pop up stays before it pops out?")]
    public float popUpDuration = 3f;

    public enum CHALLENGE_ID
    {
        CATCH_5_FISH_ROW,
        CATCH_10_FISH_ROW,

        NUM_CHALLENGE
    }

    public List<Rod> rods;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

    }

    private void Start()
    {
        LoadChallenges();
    }

    private void LoadChallenges()
    {

        challengesSO = Resources.LoadAll<BF_ChallengeSO>("Beach Fishing/Challenges");

        foreach (BF_ChallengeSO challenge in challengesSO)
        {
            //Instantiate, set challenge data and set child.
            GameObject temp = Instantiate(challengePrefab);
            temp.GetComponent<Challenge>().SetData(challenge);
            temp.transform.parent = challengeContainer.transform;
            temp.transform.localScale = new Vector3(1.9f, 1, 1);
            temp.transform.GetChild(4).GetComponent<Button>().onClick.AddListener(ClaimReward); //Hook up claim button 


            //If challenge is already unlocked but unclaimed, enable claim button and disable percentage
            if (temp.GetComponent<Challenge>().isLocked == false && temp.GetComponent<Challenge>().isClaimed == false)
            {
                temp.transform.GetChild(4).gameObject.SetActive(true); //Claim button
                temp.GetComponent<Challenge>().unlockProgress.gameObject.SetActive(false); //Percentage
            }  //If challenge is already unlocked and claimed
            else if (temp.GetComponent<Challenge>().isLocked == false && temp.GetComponent<Challenge>().isClaimed == true)
            {
                Transform claimButtonObj = temp.transform.GetChild(4);
                Transform rewardChildObj = temp.transform.GetChild(3);
                claimButtonObj.gameObject.SetActive(true); //Enable button
                temp.GetComponent<Challenge>().unlockProgress.gameObject.SetActive(false); //Disable Percentage
                claimButtonObj.GetComponent<Button>().interactable = false; //Make claim button unclickable

                //Getting multiple indexes because the obj is nested
                rewardChildObj.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().color = Color.white;//Set reward icon color to colorful
                rewardChildObj.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.white;//Set reward background color to colorful
            }

            //Add challenge to a list so we can keep track of them
            currentLoadedChallenges.Add(temp);

        }

    }

    public void UnlockChallenge(CHALLENGE_ID id)
    {

        challengesSO[(int)id].isLocked = false;
        currentLoadedChallenges[(int)id].GetComponent<Challenge>().isLocked = false;

    }

    public void UpdateChallengeProgression(CHALLENGE_ID id, int increaseAmount)
    {

        currentChallengeID = (int)id;

        //Only update progression if the challenge is still locked
        if (currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().isLocked)
        {
            //Update scriptable object
            challengesSO[currentChallengeID].unlockProgress += increaseAmount;

            //Set the data of our object on the scene according to its scriptable object
            currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().UpdateProgress(challengesSO[currentChallengeID].unlockProgress);



            if (challengesSO[currentChallengeID].unlockProgress >= 100)
            {
                //Enable claim button
                currentLoadedChallenges[currentChallengeID].transform.GetChild(4).gameObject.SetActive(true); //4 = claim button index
                currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().unlockProgress.gameObject.SetActive(false); //Hide percentage

                //Display challenge unlock pop up and play sound
                BF_AudioManager.Instance.PlaySFX((int)BF_AudioManager.SFX.CHALLENGE_REWARD);
                challengePopUp.GetComponent<Animator>().SetTrigger("popIn");
                StartCoroutine(StartPopOutTimer(popUpDuration));

                UnlockChallenge(id);
            }
        }

    }

    public void ResetChallengeProgression(CHALLENGE_ID id)
    {
        currentChallengeID = (int)id;
        //Only update progression if the challenge is still locked
        if (currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().isLocked)
        {
            //Update scriptable object
            challengesSO[currentChallengeID].unlockProgress = 0;

            //Set the data of our object on the scene according to its scriptable object
            currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().UpdateProgress(challengesSO[currentChallengeID].unlockProgress);
        }

    }

    public void ClaimReward()
    {
        //TODO
        //Play particle system
        //Save this data
        Transform claimButtonObj = currentLoadedChallenges[currentChallengeID].transform.GetChild(4);
        Transform rewardChildObj = currentLoadedChallenges[currentChallengeID].transform.GetChild(3);
        claimButtonObj.GetComponent<Button>().interactable = false; //Disable claim button


        //Getting multiple indexes because the obj is nested
        rewardChildObj.transform.GetChild(1).transform.GetChild(1).GetComponent<Image>().color = Color.white;//Set reward icon color to colorful
        rewardChildObj.transform.GetChild(1).transform.GetChild(0).GetComponent<Image>().color = Color.white;//Set reward background color to colorful

        if (currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().rodReward != null)
        {
            currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().rodReward.IsUnlocked = true;
            UserDataManager.Instance.Save();

        }
        else if (currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().baitReward != null)
        {
            currentLoadedChallenges[currentChallengeID].GetComponent<Challenge>().baitReward.IsUnlocked = true;
            UserDataManager.Instance.Save();
        }

        StartCoroutine(AchievementManager.Instance.ShowPopup());

    }

    private IEnumerator StartPopOutTimer(float duration)
    {
        yield return new WaitForSeconds(duration);
        challengePopUp.GetComponent<Animator>().SetTrigger("popOut");
    }


}
