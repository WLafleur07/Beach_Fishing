using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Rod : MonoBehaviour
{
    public RodSO rod;

    public string RodName;
    public float catchModifier=0;
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
            if (this.GetComponent<GrayScale>() != null && value==true)
            {
                this.GetComponent<Toggle>().interactable = true;
                this.GetComponent<GrayScale>().SetGrayScale(0);
                rod.isUnlocked = value;
            }         
        }
    }
    public int Index;
    public int layerWeight;

    private void Start()
    {

        SetRodData(rod);
    }

    public void SetRodData(RodSO rodData)
    {
        rod = rodData;
        RodName = rodData.rodName;
        catchModifier = rodData.catchModifier;
        IsUnlocked = rodData.isUnlocked;
        Index = rodData.Index;

        //ApplyModifiers();
    }

    private void ApplyModifiers()
    {
        BF_MinigameController.Instance.catchModifier = catchModifier;
    }

    public int GetRodIndex()
    {
        return rod.Index;
    }
}
