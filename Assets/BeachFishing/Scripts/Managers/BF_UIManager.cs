using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BF_UIManager : MonoBehaviour
{
    public Button castButton;
    public Button cancelFishingButton;
    public Button settingsButton;
    public Button playerStatsButton;
    public GameObject uiIconsPanel; // Panel that contains: Home button, Settings button, FishList Button and PlayerStats Button
    public GameObject caughtFishPopup;
    public GameObject homeAndInGameSettingsPanel;
    public GameObject challengesAndCustomizationPanel;
    public Toggle[] customScreenRods; // fishing rod Toggle buttons from customization screen
    public Toggle[] customScreenBaits; // bait rod Toggle buttons from customization screen

    public bool isSettingsOpenInMenu;
    public bool isSettingsOpenInGame;

    public static BF_UIManager Instance { get; set; }

    /// <summary>
    /// All menu screens, remember to update each time we add a new screen
    /// </summary>
    public enum BF_UI_Screens
    {
        SETTINGS_SCREEN, 
        CUSTOMIZATION_SCREEN,
        CHALLENGE_SCREEN,
        ACHIEVEMENTS_SCREEN,
        MENU_SCREEN,
        FISH_SCREEN,
        PLAYER_STATS,
        SETTINGS_PANEL,
        CHALLENGE_PANEL,
        INGAME_SCREEN,
        HOWTOPLAY_SCREEN,

        NUM_SCREENS
    }

    public enum RODS
    {
        NONE = -1,
        DEFAULT_ROD,
        GOLD_ROD,
        MAGENTA_ROD,

        NUM_RODS
    }

    public enum BAITS
    {
        CHATTERBAIT,
        CRANKBAIT,
        JIG,
        NONE,
        SPOON,
        TEXASRIG,

        NUM_BAIT

    }



    public List<GameObject> UI_Screens;
    [Header("Place the fish locked panels from the fish list screen here.")]
    public List<GameObject> locked_panels;
    [Header("Place all of the stats gameObjects here.")]
    public List<GameObject> playerStats;

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        // instantiating a material already assigned. This allows each material to be independent 
        for (int i = 0; i < customScreenRods.Length; i++)
        {
            customScreenRods[i].GetComponent<Image>().material = Instantiate(customScreenRods[i].GetComponent<Image>().material);
        }

        // instantiating a material already assigned. This allows each material to be independent 
        for (int i = 0; i < customScreenBaits.Length; i++)
        {
            customScreenBaits[i].GetComponent<Image>().material = Instantiate(customScreenBaits[i].GetComponent<Image>().material);
        }

        // Changes the grayscale value based on isUnlocked
        BaitGrayScale();
        RodGrayScale();

        isSettingsOpenInMenu = true;
        isSettingsOpenInGame = false;
    }

    //Method to check if a certain menu screen is open
    public bool IsOpen(BF_UI_Screens screen)
    {
        return UI_Screens[(int)screen].activeInHierarchy;
    }

    //Method to open a menu screen
    public void OpenScreen(int ID)
    {
        if (UI_Screens[(int)ScreenID(ID)].activeInHierarchy == false)
            UI_Screens[(int)ScreenID(ID)].SetActive(true);
    }

    public void CloseScreen(int ID)
    {
        if (UI_Screens[(int)ScreenID(ID)].activeInHierarchy == true)
            UI_Screens[(int)ScreenID(ID)].SetActive(false);
    }

    //Overloaded method to open a menu screen and close another one at the same time
    public void OpenScreen (int menuScreenOpen, int menuScreenClose)
    {
        if (UI_Screens[(int)ScreenID(menuScreenOpen)].activeInHierarchy == false)
        {
            UI_Screens[(int)ScreenID(menuScreenOpen)].SetActive(true);
        }
            
        if (UI_Screens[(int)ScreenID(menuScreenClose)].activeInHierarchy == true)
        {
            UI_Screens[(int)ScreenID(menuScreenClose)].SetActive(false);
        }

    }

    //Closes all screens
    public void CloseAll()
    {

        foreach(GameObject s in UI_Screens)
        {
            if (s.activeInHierarchy)
                s.SetActive(false);
        }

    }

    public void ReturnToMainApp()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void CastButton()
    {
        castButton.gameObject.SetActive(true);
    }

    public void CancelButton()
    {
        cancelFishingButton.gameObject.SetActive(true);
    }

    public void StartButton()
    {
        settingsButton.transform.localPosition = new Vector3(0, -85, 0);
        isSettingsOpenInMenu = false;
        isSettingsOpenInGame = true;
        OpenScreen((int)BF_UIManager.BF_UI_Screens.INGAME_SCREEN);
        CloseScreen((int)BF_UIManager.BF_UI_Screens.MENU_SCREEN);
        CastButton();

        //Load player stats each time we click the player stats screen
        playerStatsButton.onClick.AddListener(delegate () { UpdatePlayerStats(); });

    }

    public void HomeButton()
    {
        settingsButton.transform.localPosition = new Vector3(0, -15, 0);
        isSettingsOpenInMenu = true;
        isSettingsOpenInGame = false;
        CloseScreen((int)BF_UIManager.BF_UI_Screens.INGAME_SCREEN);
        OpenScreen((int)BF_UIManager.BF_UI_Screens.MENU_SCREEN);
    }

    public void CloseSettingsScreen()
    {
        if (isSettingsOpenInMenu == true)
        {
            OpenScreen((int)BF_UIManager.BF_UI_Screens.MENU_SCREEN);
            OpenScreen((int)BF_UIManager.BF_UI_Screens.SETTINGS_PANEL);
        }

        if (isSettingsOpenInGame == true)
        {
           OpenScreen((int)BF_UIManager.BF_UI_Screens.INGAME_SCREEN);
           OpenScreen((int)BF_UIManager.BF_UI_Screens.SETTINGS_PANEL);
           OpenScreen((int)BF_UIManager.BF_UI_Screens.CHALLENGE_PANEL);
           CastButton();

        }
    }

    public void BaitGrayScale()
    {
        for (int i = 0; i < BF_GameplayManager.Instance.BaitSO.Length; i++)
        {
            if (!BF_GameplayManager.Instance.BaitSO[i].isUnlocked)
            {
                customScreenBaits[i].GetComponent<GrayScale>().SetGrayScale(); // sets Grayscale on start
                customScreenBaits[i].interactable = false; // if obj is grayed out, prevent user from interacting with it
            }
        }
    }

    public void RodGrayScale()
    {
        for (int i = 1; i < BF_GameplayManager.Instance.RodSO.Length; i++)
        {
            if (!BF_GameplayManager.Instance.RodSO[i].isUnlocked)
            {
                if (i != 0)
                {   
                    customScreenRods[i].GetComponent<GrayScale>().SetGrayScale(); // sets Grayscale on start
                    customScreenRods[i].interactable = false; // if obj is grayed out, prevent user from interacting with it
                }
            }
        }
    }


    public BF_UI_Screens ScreenID(int ID)
    {
        switch(ID)
        {
            case (int)BF_UI_Screens.SETTINGS_SCREEN:
                break;
            case (int)BF_UI_Screens.CUSTOMIZATION_SCREEN:
                break;
            case (int)BF_UI_Screens.CHALLENGE_SCREEN:
                break;
            case (int)BF_UI_Screens.ACHIEVEMENTS_SCREEN:
                break;
            case (int)BF_UI_Screens.MENU_SCREEN:
                break;
            case (int)BF_UI_Screens.FISH_SCREEN:
                break;
            case (int)BF_UI_Screens.PLAYER_STATS:
                break;
            default:
                break;
        }

        return (BF_UI_Screens)(ID);
    }

    //Make sure to call it after updating the caught amount
    bool isSet = false;
    public void UpdatePlayerStats()
    {
        
        List<FishSO> fishList = BF_GameplayManager.Instance.FishSO;
        int commomCaught = 0,bronzeCaught=0,silverCaught=0,goldCaught=0,legendaryCaught=0, garbageCaught = 0;

        foreach(FishSO fish in fishList)
        {
            if (!fish.isGarbage)
            {
                if (fish.rarity < 1)
                    legendaryCaught += fish.caughtAmount;
                else if (fish.rarity >= 1 && fish.rarity < 10)
                    goldCaught += fish.caughtAmount;
                else if (fish.rarity >= 10 && fish.rarity < 35)
                    silverCaught += fish.caughtAmount;
                else if (fish.rarity >= 35 && fish.rarity < 60)
                    bronzeCaught += fish.caughtAmount;
                else if (fish.rarity >= 60 && fish.rarity <= 100)
                    commomCaught += fish.caughtAmount;
            }
            else
            {
                garbageCaught += fish.caughtAmount;
            }
        }
        
        foreach(GameObject stat in playerStats)
        {
            if(stat.name == "TotalNumFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = UserDataManager.Instance.BF_data.NumFishCaught.ToString();
            }
            else if (stat.name == "TotalNumCommonFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = commomCaught.ToString();
            }
            else if (stat.name == "TotalNumBronzeFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = bronzeCaught.ToString();
            }
            else if (stat.name == "TotalNumSilverFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = silverCaught.ToString();
            }
            else if (stat.name == "TotalNumGoldFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = goldCaught.ToString();
            }
            else if (stat.name == "TotalNumLegendaryFishCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = legendaryCaught.ToString();
            }
            else if (stat.name == "TotalNumGarbageCaught")
            {
                Transform text = stat.transform.GetChild(1); //Get text child object (index 1)
                text.GetComponent<Text>().text = garbageCaught.ToString();
            }
        }
    }
}
