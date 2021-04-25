using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Fish : MonoBehaviour
{
    public FishSO fish;

    public Text FishName;
    public SpriteRenderer FishspriteRenderer;
    public Image miniGameFishIcon;
    public Image FishCaughtPopup;
    public Image fillGaugeFishIcon;
    public Text FishSize;
    public int FishIndex;
    public float difficultyMultiplier = 0;
    public Text fishCaughtAmount;
    public bool isUnlocked;
    public int unlockTreshold;

    public void SetFishData(FishSO fishData)
    {
        fish = fishData;
        FishName.text = fishData.fishName;
        FishspriteRenderer.sprite = fishData.FishSprite;
        FishCaughtPopup.sprite = fishData.FishSprite;
        FishSize.text = "Size: " + ReturnRandomWeight(fishData.minSize, fishData.maxSize) + " inches.";
        FishIndex = fishData.index;
        difficultyMultiplier = fishData.difficultyMultiplier;
        isUnlocked = fishData.isUnlocked;
        unlockTreshold = fishData.unlockTreshold;

        if (fishData.isGarbage)
        {
            miniGameFishIcon.sprite = fishData.QuestionMark;
            fillGaugeFishIcon.sprite = fishData.QuestionMark;
        }
        else if (fishData.caughtAmount < 1)
        {
            miniGameFishIcon.sprite = fishData.QuestionMark;
            fillGaugeFishIcon.sprite = fishData.QuestionMark;
        }
        else
        {
            miniGameFishIcon.sprite = fishData.FishSprite;
            fillGaugeFishIcon.sprite = fishData.FishSprite;
        }
    }

    public double ReturnRandomWeight(float minSize, float maxSize)
    {
        double fishSize = 0;
        float randVal = Random.Range(0.0f, 100.0f);

        if (minSize > 0)
        { 
            if (randVal < 1)
            {
                fishSize = System.Math.Round(Random.Range(minSize, maxSize), 2);
            }
            else if (randVal < 25)
            {
                fishSize = System.Math.Round(Random.Range(minSize, (maxSize / 1.2f)), 2);
            }
            else if (randVal < 75)
            {
                fishSize = System.Math.Round(Random.Range(minSize, (maxSize / 1.5f)), 2);
            }
            else if (randVal <= 100)
            {
                fishSize = System.Math.Round(Random.Range(minSize, (maxSize / 2)), 2);
            }
        }
        else
        {
            fishSize = maxSize;
        }

        return fishSize;
    }

    public void SetFishCaughtAmount()
    {
        fishCaughtAmount.text = "Caught: " + fish.caughtAmount;
    }
}
