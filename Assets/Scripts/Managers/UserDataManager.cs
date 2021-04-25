using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using UnityEngine.SceneManagement;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; set; }

    [HideInInspector]
    public readonly string[] sceneNames = { "MainScene", "BeachFishing", "GhostHuntMenu", "OrchardDrops", "UpperCanada", "GhostHuntGame" }; // Add multiple fields here if your minigame has multiple scenes
    public readonly string[] directories = { "/SaveData/", "/BeachFishing/", "/GhostHunt/", "/OrchardDrops/", "/UpperCanada/" };
    public readonly string[] fileNames = { "UserData.dat", "BeachFishingData_Version_", "GhostHuntData.dat", "OrchardDrops.dat", "UpperCanada.dat", "GhostHuntGameData.dat" };

    [HideInInspector]
    public BeachFishingData BF_data;

    [HideInInspector]
    public const int BF_VERSION = 3;

    public enum SCENES
    {
        MAIN_SCENE,
        BEACH_FISHING,
        GHOST_HUNT,
        ORCHARD_DROPS,
        UPPER_CANADA,
        GHOST_HUNT_GAME,

        NUM_SCENES
    } // Add additional fields to this enum if minigame has multiple scenes

    private void Awake()
    {
        // singleton
        if (Instance == null)
        {
            Instance = this;
            this.transform.parent = null;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Load();
        SceneManager.sceneLoaded += OnLoadCallback;
    }

    void OnLoadCallback(Scene scene, LoadSceneMode sceneMode)
    {
        Load();
    }

    // Function to save UserData in a JSON format to a text file.
    public void Save()
    {

        GetUserData(); // Function is called to assign value to data variable

        // Main scene data needs to be saved in all scenes to handle achievement data
        SerializeData((int)SCENES.MAIN_SCENE);

        if (SceneManager.GetActiveScene().name == sceneNames[(int)SCENES.BEACH_FISHING])
        {
            SerializeData((int)SCENES.BEACH_FISHING);
        }
    }

    void SerializeData(int index)
    {
        string dir = "";
        string json = "";
        string file = "";

        switch (index)
        {
            case (int)SCENES.BEACH_FISHING:
                {
                    dir = Application.persistentDataPath + directories[(int)SCENES.BEACH_FISHING]; // beach fishing directory
                    json = JsonUtility.ToJson(BF_data);
                    file = fileNames[index] + BF_VERSION.ToString() + ".dat";
                    break;
                }
        }

        if (!Directory.Exists(dir))
        {
            Directory.CreateDirectory(dir);
        }

        Debug.Log(json);

        File.WriteAllText(dir + file, json);
    }

    // Function to load UserData from a Text file in JSON format and de-serialize it
    public void Load()
    {
        string fullPath;
        if (SceneManager.GetActiveScene().name == sceneNames[(int)SCENES.BEACH_FISHING]) // Save to different location based on scene
        {
            fullPath = BF_data.BeachFishingDataPath();
            DeserializeData(fullPath, (int)SCENES.BEACH_FISHING);
        }
    }

    void DeserializeData(string path, int index)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);

            switch (index)
            {
                case (int)SCENES.BEACH_FISHING:
                    {
                        BeachFishingData BF_userData = JsonUtility.FromJson<BeachFishingData>(json);
                        BF_data.SetUserData(BF_userData); // Overloaded function to set data for BeachFishingData
                        break;
                    }
            }
        }
        else
        {
            switch (index)
            {
                case (int)SCENES.MAIN_SCENE:
                    {
                        break;
                    }
                case (int)SCENES.BEACH_FISHING:
                    {
                        BF_data.SetDefaultValues();
                        break;
                    }
            }
        }
    }

    public void GetUserData()
    {
        if (SceneManager.GetActiveScene().name == sceneNames[(int)SCENES.BEACH_FISHING])
        {
            BF_data.AssignBeachFishingData();
        }
    }

}
