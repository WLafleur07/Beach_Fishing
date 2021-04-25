using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Bait : MonoBehaviour
{
    public BaitSO bait;

    public float gameOverReduction=0;
    [SerializeField]
    private bool isUnlocked;
    public bool IsUnlocked
    {
        get
        {
            return isUnlocked;
        }
        set
        {
            isUnlocked = value;
            if (this.GetComponent<GrayScale>() != null && value == true)
            {
                this.GetComponent<Toggle>().interactable = true;
                this.GetComponent<GrayScale>().SetGrayScale(0);
                bait.isUnlocked = value;
            }
        }
    }

    private void Start()
    {
        SetBaitData(bait);
    }
    public void SetBaitData(BaitSO baitData)
    {
        bait = baitData;
        gameOverReduction = baitData.gameOverReduction;
        isUnlocked = baitData.isUnlocked;

        //ApplyModifiers();
    }

    private void ApplyModifiers()
    {
        BF_MinigameController.Instance.gameOverReduction = gameOverReduction;
    }

    public int GetBaitIndex()
    {
        return bait.Index;
    }
}
