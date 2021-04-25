using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

[System.Serializable]
public class BeachFishingData
{
    private const int NUM_RODS = 3;
    private const int NUM_BAITS = 6;
    private const int NUM_FISH = 16;

    public int dataVersion;

    public bool[] BaitUnlocked;
    public bool[] BaitSelected;

    public bool[] RodUnlocked;
    public bool[] RodSelected;

    public float[] FishRarity;

    public bool hasRained;

    public bool[] isGarbage;

    //Each time we update this value, check if new fish can be unlocked
    [SerializeField]
    private int numFishCaught;
    public int NumFishCaught
    {
        get
        {
            return numFishCaught;
        }
        set
        {
            numFishCaught = value;

            if (SceneManager.GetActiveScene().name == UserDataManager.Instance.sceneNames[(int)UserDataManager.SCENES.BEACH_FISHING])
            {
                BF_GameplayManager.Instance.CheckFishUnlock();
            }
        }
    }

    public int[] numFishCaughtByIndex;


    /*--------------------UserDataManager Methods------------------------------*/
    public void InitializeBeachFishingData()
    {
        BaitUnlocked = new bool[NUM_BAITS];
        BaitSelected = new bool[NUM_BAITS];
        numFishCaughtByIndex = new int[NUM_FISH];
        FishRarity = new float[NUM_FISH];
        isGarbage = new bool[NUM_FISH];
        RodUnlocked = new bool[NUM_RODS];
        RodSelected = new bool[NUM_RODS];

        NumFishCaught = 0;
        hasRained = false;

        for (int i = 0; i < NUM_BAITS; i++)
        {
            if (i == 3)
            {
                BaitUnlocked[i] = true;
                BaitSelected[i] = true;
            }
            else
            {
                BaitUnlocked[i] = false;
                BaitSelected[i] = false;
            }
        }

        for (int i = 0; i < NUM_RODS; i++)
        {
            if (i == 0)
            {
                RodSelected[i] = true;
                RodUnlocked[i] = true;
            }
            else
            {
                RodSelected[i] = false;
                RodUnlocked[i] = false;
            }
        }

        for (int i = 0; i < NUM_FISH; i++)
        {
            numFishCaughtByIndex[i] = 0;
            FishRarity[i] = 0;
            isGarbage[i] = false;
        }
    }

    // Overloaded function to take in BeachFishingData
    public void SetUserData(BeachFishingData BF_Data)
    {
        for (int i = 0; i < BF_Data.BaitUnlocked.Length; i++)
        {
            BF_GameplayManager.Instance.BaitSO[BF_GameplayManager.Instance.BaitSO[i].Index].isUnlocked = BF_Data.BaitUnlocked[i];
            BF_UIManager.Instance.customScreenBaits[i].isOn = BF_Data.BaitSelected[i];
        }

        for (int i = 0; i < BF_Data.RodUnlocked.Length; i++)
        {
            BF_GameplayManager.Instance.RodSO[i].isUnlocked = BF_Data.RodUnlocked[i];
            BF_UIManager.Instance.customScreenRods[i].isOn = BF_Data.RodSelected[i];
        }

        NumFishCaught = BF_Data.NumFishCaught;
        BF_GameplayManager.Instance.hasRained = BF_Data.hasRained;

        for (int i = 0; i < BF_GameplayManager.Instance.FishSO.Count; i++)
        {
            BF_GameplayManager.Instance.FishSO[i].caughtAmount = 0;
        }

        for (int i = 0; i < BF_Data.numFishCaughtByIndex.Length; i++) // loop through the size of the array loaded in
        {
            if (BF_Data.dataVersion == UserDataManager.BF_VERSION)
            {
                BF_GameplayManager.Instance.FishSO[i].caughtAmount = BF_Data.numFishCaughtByIndex[BF_GameplayManager.Instance.FishSO[i].index];
                numFishCaughtByIndex[i] = BF_Data.numFishCaughtByIndex[BF_GameplayManager.Instance.FishSO[i].index];
                isGarbage[i] = BF_Data.isGarbage[BF_GameplayManager.Instance.FishSO[i].index];
            }
            else if (BF_Data.numFishCaughtByIndex.Length < BF_GameplayManager.Instance.FishSO.Count) // check to see if the loaded fish array is less than the amount of fish SO's
            {
                if (BF_GameplayManager.Instance.FishSO[i].index < BF_Data.numFishCaughtByIndex.Length) // only load the fish at an index less than 
                {                                                                                      // the total array to prevent going out of range
                    BF_GameplayManager.Instance.FishSO[i].caughtAmount = BF_Data.numFishCaughtByIndex[BF_GameplayManager.Instance.FishSO[i].index];
                }
                else
                {
                    BF_GameplayManager.Instance.FishSO[i].caughtAmount = 0; // set the caught amount to 0 because the player has never encounter this fish
                                                                            // as indicated by the save data
                }
            }
            else
            {
                if (BF_GameplayManager.Instance.FishSO.Count >= BF_Data.numFishCaughtByIndex.Length) // ensure that there aren't more loaded array indices than the amount of fish SO's
                {
                    BF_GameplayManager.Instance.FishSO[i].caughtAmount = BF_Data.numFishCaughtByIndex[BF_GameplayManager.Instance.FishSO[i].index]; // load things normally
                }
            }
        }
    }

    public string BeachFishingDataPath()
    {
        string path = " ";

        if (Directory.Exists(Application.persistentDataPath + UserDataManager.Instance.directories[(int)UserDataManager.SCENES.BEACH_FISHING]))
        {
            string[] files = Directory.GetFiles(Application.persistentDataPath + UserDataManager.Instance.directories[(int)UserDataManager.SCENES.BEACH_FISHING]); // Checking all the files in the BF directory
            for (int i = files.Length - 1; i >= 0; i--)
            {
                if (files[i] == Application.persistentDataPath + UserDataManager.Instance.directories[(int)UserDataManager.SCENES.BEACH_FISHING] + UserDataManager.Instance.fileNames[(int)UserDataManager.SCENES.BEACH_FISHING] + UserDataManager.BF_VERSION.ToString() + ".dat")
                {
                    path = files[i];
                    break;
                }
                else
                {
                    path = files[files.Length - 1];
                }
            }
        }

        return path;
    }

    public void AssignBeachFishingData()
    {
        BaitUnlocked = new bool[BF_GameplayManager.Instance.BaitSO.Length];
        BaitSelected = new bool[BF_GameplayManager.Instance.BaitSO.Length];
        numFishCaughtByIndex = new int[BF_GameplayManager.Instance.FishSO.Count];
        FishRarity = new float[BF_GameplayManager.Instance.FishSO.Count];
        isGarbage = new bool[BF_GameplayManager.Instance.FishSO.Count];

        for (int i = 0; i < BF_GameplayManager.Instance.BaitSO.Length; i++)
        {
            BaitUnlocked[BF_GameplayManager.Instance.BaitSO[i].Index] = BF_GameplayManager.Instance.BaitSO[i].isUnlocked;
            BaitSelected[i] = BF_UIManager.Instance.customScreenBaits[i].isOn;
        }

        RodUnlocked = new bool[BF_GameplayManager.Instance.RodSO.Length];
        RodSelected = new bool[BF_GameplayManager.Instance.RodSO.Length];

        for (int i = 0; i < BF_GameplayManager.Instance.RodSO.Length; i++)
        {
            RodUnlocked[i] = BF_GameplayManager.Instance.RodSO[i].isUnlocked;
            RodSelected[i] = BF_UIManager.Instance.customScreenRods[i].isOn;
        }

        for (int i = 0; i < BF_GameplayManager.Instance.FishSO.Count; i++)
        {
            numFishCaughtByIndex[BF_GameplayManager.Instance.FishSO[i].index] = BF_GameplayManager.Instance.FishSO[i].caughtAmount;
            FishRarity[BF_GameplayManager.Instance.FishSO[i].index] = BF_GameplayManager.Instance.FishSO[i].rarity;
            isGarbage[BF_GameplayManager.Instance.FishSO[i].index] = BF_GameplayManager.Instance.FishSO[i].isGarbage;
        }

        dataVersion = UserDataManager.BF_VERSION;
    }

    // Called in GetUserData Method
    public void GetGameData(BeachFishingData BF_Data)
    {
        // setting data
        for (int i = 0; i < BF_Data.BaitUnlocked.Length; i++)
        {
            BaitUnlocked[i] = BF_Data.BaitUnlocked[i];
            BaitSelected[i] = BF_Data.BaitSelected[i];
        }

        for (int i = 0; i < BF_Data.RodUnlocked.Length; i++)
        {
            RodUnlocked[i] = BF_Data.RodUnlocked[i];
            RodSelected[i] = BF_Data.RodSelected[i];
        }

        for (int i = 0; i < BF_Data.numFishCaughtByIndex.Length; i++)
        {
            numFishCaughtByIndex[i] = BF_Data.numFishCaughtByIndex[i];
            FishRarity[i] = BF_Data.FishRarity[i];
            isGarbage[i] = BF_Data.isGarbage[i];
        }

        NumFishCaught = BF_Data.NumFishCaught;
        hasRained = BF_Data.hasRained;
    }

    public void GetMinigameData(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            BeachFishingData BF_userData = JsonUtility.FromJson<BeachFishingData>(json);
            GetGameData(BF_userData); // Overloaded function to set data for BeachFishingData
        }
    }

    public void SetDefaultValues()
    {
        for (int i = 0; i < BF_GameplayManager.Instance.FishSO.Count; i++)
        {
            BF_GameplayManager.Instance.FishSO[i].caughtAmount = 0;
        }

        for (int i = 0; i < BF_GameplayManager.Instance.BaitSO.Length; i++)
        {
            if (i == 3)
            {
                BF_GameplayManager.Instance.BaitSO[i].isUnlocked = true;
                BF_UIManager.Instance.customScreenBaits[i].isOn = true;
            }
            else
            {
                BF_GameplayManager.Instance.BaitSO[i].isUnlocked = false;
                BF_UIManager.Instance.customScreenBaits[i].isOn = false;
            }
        }

        for (int i = 0; i < BF_GameplayManager.Instance.RodSO.Length; i++)
        {
            if (i == 0)
            {
                BF_GameplayManager.Instance.RodSO[i].isUnlocked = true;
                BF_UIManager.Instance.customScreenRods[i].isOn = true;
            }
            else
            {
                BF_GameplayManager.Instance.RodSO[i].isUnlocked = false;
                BF_UIManager.Instance.customScreenRods[i].isOn = false;
            }
        }
    }
}
