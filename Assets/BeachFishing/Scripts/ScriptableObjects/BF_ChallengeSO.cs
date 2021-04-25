using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "New BF Challenge", menuName = "Beach Fishing/Challenges/Challenge")]
public class BF_ChallengeSO : ScriptableObject
{
    public string description;
    public int unlockProgress = 0;
    public bool isLocked;
    public bool isClaimed; 
    public Sprite reward;
    public int challengeID;

    //Possible rewards (enums to be used as an index of toogle groups)
    public BF_UIManager.RODS rodReward;
    public BF_UIManager.BAITS baitReward;
    
}