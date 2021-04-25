using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Bait", menuName = "Beach Fishing/Baits/Bait")]
public class BaitSO : ScriptableObject
{
    public string baitName;
    public bool isUnlocked;
    public int Index;

    [Header("Modifiers")]
    [Tooltip("Takes longer to lose the minigame")]
    public float gameOverReduction=0;
}
