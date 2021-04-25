using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BF_GameplayManager : MonoBehaviour
{
    public RodSO[] RodSO;
    public List<FishSO> FishSO;
    public BaitSO[] BaitSO;
    public Fish fishObject;
    public ParticleSystem rainParticleSystem;
    public GameObject FillGauge;
    private BF_MinigameController minigameController;
    public GameObject sadFace;

    [HideInInspector]
    public bool hasRained = false;

    //The list index of the current fish.
    [HideInInspector]
    public int currentFishIndex;
    //Total sum of rarity we have between all fish
    private float totalRarity;

    public static BF_GameplayManager Instance { get; set; }

    private void Awake()
    {
        
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

        RodSO = Resources.LoadAll<RodSO>("Beach Fishing/Rods");
        FishSO = Resources.LoadAll<FishSO>("Beach Fishing/Fish").ToList();
        BaitSO = Resources.LoadAll<BaitSO>("Beach Fishing/Baits");

        SortFishList(FishSO);

        minigameController = catchFishMinigame.GetComponent<BF_MinigameController>();

        //Checks for repeated fish chance
        CheckRepeated();

        //Fixes the logic problem with having multiple fix catch chances
        UpdateTotalRarity();
    }


    public GameObject player;
    public GameObject fishRod;

    public GameObject progressBarUI;
    public GameObject catchFishMinigame;


    [Header("The amount of time we could be waiting for the fish")]
    [Range(0f, 25f)]
    public float minCastTime;
    [Range(0f, 25f)]
    public float maxCastTime;

    [Header("Every second of cast we have a X amount of chance of catching a fish")]
    [Range(0f, 100f)]
    public float catchChance;

    [Header("The amount of time the user has to catch the fish before the loses the opportunity")]
    public float timeToCatch;

    private bool stopCastingCounter = false;

    [HideInInspector]
    public FishSO chosenFish; //Public chosen fish so we can access the current fish on different scrips

    //This method is hooked up to the Cast button
    public void CastLine()
    {

        BF_Animations_Manager.Instance.animPlayer.ResetTrigger("Reset");


        //Choose the fish based on rarity
        float randVal = Random.Range(0.0f, totalRarity - 1);

        for (int i = 0; i < FishSO.Count; ++i)
        {
            if (FishSO[i].isUnlocked)
            {
                float rarity = FishSO[i].rarity;

                if (randVal < rarity)
                {
                    chosenFish = FishSO[i];
                    currentFishIndex = i;
                    break;
                }
                randVal -= rarity; // 'Remove' this fish from the sum, only consider the ones that can still be chosen     
            }
        }

        fishObject.SetFishData(chosenFish);
        minigameController.difficultyMultiplier = chosenFish.difficultyMultiplier;

        //Play cast animation
        BF_Animations_Manager.Instance.PlayCastAnimation();

        //Start after casting animation, try catching the fish every second
        float animationWaitTime = 2f;
        InvokeRepeating("TryHookFish", animationWaitTime, 1f);

        //Start counter, compensate for animation wait time
        StartCoroutine(StartCastingCounter(minCastTime + animationWaitTime, maxCastTime + animationWaitTime));

        stopCastingCounter = false;

    }

    public void ResetCast()
    {
        CancelInvoke("TryHookFish");
        stopCastingCounter = true;
        StopAllCoroutines();
        BF_UIManager.Instance.CastButton();
    }
    public void ResetCastComplete()
    {
        CancelInvoke("TryHookFish");
        stopCastingCounter = true;
        StopAllCoroutines();
        BF_Animations_Manager.Instance.PlayerResetAnimation();
        BF_AudioManager.Instance.StopPlayingSFX();
        catchFishMinigame.SetActive(false);
        BF_UIManager.Instance.CastButton();
        BF_UIManager.Instance.cancelFishingButton.gameObject.SetActive(false);
        BF_UIManager.Instance.OpenScreen((int)BF_UIManager.BF_UI_Screens.SETTINGS_PANEL);

        if (FillGauge.activeInHierarchy)
        {
            BF_FillGauge.Instance.CancelFillGauge();
        }

        BF_UIManager.Instance.OpenScreen((int)BF_UIManager.BF_UI_Screens.CHALLENGE_PANEL);
    }

    //Timer to limit our casting time (amount of time we can wait for the fish to be hooked)
    private IEnumerator StartCastingCounter(float minTime, float maxTime)
    {

        yield return new WaitForSeconds(Random.Range(minTime, maxTime));
        if (stopCastingCounter == false)
            ResetCastComplete(); //Time expired so reset cast                
        else
            stopCastingCounter = false; //Reset bool

    }

    //Being Called as an Invoke on the CastLine() method
    private void TryHookFish()
    {

        float chance = Random.Range(0f, 100f);

        //If fish is hooked we Prompt the user to catch
        if (chance <= catchChance)
        {
            CancelInvoke("TryHookFish"); //Cancel our own invoke (stop trying to hook fish, its already hooked)
            PromptUserToCatch(); //Display catch button
        }

    }

    public void TriggerMinigame()
    {
        catchFishMinigame.SetActive(true);
        StopAllCoroutines();

    }

    //This is hooked up to the Catch Button
    public void CatchFish()
    {

        BF_Animations_Manager.Instance.PlayReelInAnimation();
        TriggerMinigame(); //If fish is caught we trigger the minigame for reeling
        BF_UIManager.Instance.cancelFishingButton.gameObject.SetActive(false);

    }

    private void PromptUserToCatch()
    {
        FillGauge.SetActive(true);
        //Stop the miss catch counter since we're starting the fill gauge minigame
        // Otherwise the minigame will eventually close based on the cast time coroutine expire time
        stopCastingCounter = true; 
    }


    //If the fish is hooked but player misses the opportunity
    public void MissCatchOpportunity()
    {
        //TODO
        //Add more stuff here when player misses a catch?

        stopCastingCounter = true;
        BF_Animations_Manager.Instance.PlayFishLostAnimation();
        ResetCast();
        sadFace.SetActive(true);
        BF_ShakeSadFace.Instance.Begin();
    }

    /// <summary>
    /// Method to check for fish with the same catch chance
    /// </summary>
    private void CheckRepeated()
    {
        float[] chance = new float[FishSO.Count];

        for (int i = 0; i < FishSO.Count; i++)
        {
            chance[i] = FishSO[i].rarity;
        }

        for (int i = 0; i < FishSO.Count; i++)
        {
            for (int j = i + 1; j < FishSO.Count; j++)
            {
                if (chance[i] == FishSO[j].rarity)
                {
                    Debug.LogError("Duplicate fish rarity: " + FishSO[i].fishName + " and " + FishSO[j].fishName);
                }
            }
        }


    }

    public void CheckFishUnlock()
    {
        int totalFish = UserDataManager.Instance.BF_data.NumFishCaught;
        if (totalFish == 100)
        {
            UnlockFish(totalFish);
        }
        else if (totalFish == 200)
        {
            UnlockFish(totalFish);
        }
        else if (totalFish == 400)
        {
            UnlockFish(totalFish);
        }
    }

    private void UnlockFish(int unlockTreshold)
    {
        //Unlock fish
        foreach (FishSO fish in FishSO)
        {
            //If fish matches our unlock treshold, unlock it
            if (fish.unlockTreshold == unlockTreshold && fish.isUnlocked == false)
            {
                fish.isUnlocked = true;
            }
        }

        //Update fish list UI
        string ui_tag = "Locked_" + unlockTreshold.ToString();
        foreach (GameObject panel in BF_UIManager.Instance.locked_panels)
        {

            if (panel.CompareTag(ui_tag))
            {
                if (panel.gameObject.activeSelf)
                {
                    panel.SetActive(false);
                }
                //TODO
                //Save this info so next time we initialize the game the screen is inactive
            }
        }
    }

    //Method to sort the fish list in ascending order based on rarity
    private void SortFishList(List<FishSO> fishList)
    {
        bool isSorted = false;
        while (!isSorted)
        {
            isSorted = true;
            for (int i = 0; i < fishList.Count - 1; ++i)
            {
                if (fishList[i].rarity > fishList[i + 1].rarity)
                {
                    Swap(fishList, i, i + 1);
                    isSorted = false;
                }
            }
        }
    }

    //Helper method that swaps 2 indexes in a list
    private void Swap(List<FishSO> list, int indexA, int indexB)
    {
        FishSO temp = list[indexA];
        list[indexA] = list[indexB];
        list[indexB] = temp;
    }

    private void UpdateTotalRarity()
    {
        float currRarity = 0;
        for (int i = 0; i < FishSO.Count; i++)
        {
            if (FishSO[i].isUnlocked)
                currRarity += FishSO[i].rarity;
        }
        totalRarity = currRarity;
    }

}