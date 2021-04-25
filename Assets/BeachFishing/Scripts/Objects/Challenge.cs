using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Challenge : MonoBehaviour
{
    public BF_ChallengeSO challengeSO;
    public Text description;
    public Text unlockProgress;
    public bool isLocked;
    public bool isClaimed;
    public Image rewardImage; //Reward image
    public int challengeID;

    //Possible rewards
    public Rod rodReward;
    public Bait baitReward;

    public void SetData(BF_ChallengeSO tempChallengeSO)
    {
        challengeSO = tempChallengeSO;
        description.text = challengeSO.description;
        unlockProgress.text = challengeSO.unlockProgress.ToString() + "%";
        isLocked = challengeSO.isLocked;
        isClaimed = challengeSO.isClaimed;
        rewardImage.sprite = challengeSO.reward;
        challengeID = tempChallengeSO.challengeID;

        //If there is a reward, set it
        if(tempChallengeSO.rodReward != BF_UIManager.RODS.NONE)
        {
            GameObject temp = BF_UIManager.Instance.customScreenRods[(int)tempChallengeSO.rodReward].gameObject;
            rodReward = temp.GetComponent<Rod>();
        }
        else if (tempChallengeSO.baitReward != BF_UIManager.BAITS.NONE)
        {
            GameObject temp = BF_UIManager.Instance.customScreenBaits[(int)tempChallengeSO.baitReward].gameObject;
            baitReward = temp.GetComponent<Bait>();
        }
        
    }

    public void UpdateProgress(int increaseAmount)
    {
        unlockProgress.text = increaseAmount.ToString() + "%";
    }

    public void SetChallengeID()
    {
        //Setting challenge id
        BF_ChallengeManager.Instance.currentChallengeID = this.challengeID;
    }



}
