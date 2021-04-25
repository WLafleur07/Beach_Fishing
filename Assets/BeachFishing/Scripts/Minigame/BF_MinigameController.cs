using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BF_MinigameController : MonoBehaviour
{
    
    #region FishIcon Settings

    [Space(15)]
    [Header("FishIcon Settings")]

    public GameObject fishIcon;
    private Vector3 newPosition;

    [Space(5)]
    [Header("Should the fishIcon chance velocity each time it reaches destination?")]
    public bool randomizeSpeed;

    public float minVel;
    public float maxVel;

    [Space(5)]
    [Header("Constant move speed it we choose not to randomize")]
    public float moveSpeed;

    private float randomSpeed;

    [Space(10)]
    #endregion

    #region Player Bar Settings

    [Space(15)]
    [Header("Player Bar Settings")]
    [Space(15)]
    public GameObject playerBar;

    [Space(5)]
    [Header("The bigger this value the faster the bar falls")]
    public float gravityModifier;
    public float bumpPower;

    [HideInInspector]
    public bool isOverlapping = false; // Are we overlapping the fishIcon?

    private Vector3 playerBarInitialPos;

    #endregion

    #region Progress Bar Settings

    [Space(15)]
    [Header("Progress Bar Settings")]
    [Space(15)]
    public Image progressBar;

    [Tooltip("The speed in which the bar is filled when we are overlapping the fishIcon (does not account for modifiers)")]
    public float fillSpeed;
    [Tooltip("The speed in which the bar is unfilled when we are overlapping the fishIcon (does not account for modifiers)")]
    public float unFillSpeed;
    [Tooltip("The amount of time it takes for the player to lose the minigame after touching the fishIcon (does not account for modifiers)")]
    public float gameOverTime;

    [HideInInspector]
    public bool hasTouchedFishIcon = false;
    private bool gameOver = false;

    #endregion

    #region Modifier Variables

    public float gameOverReduction=0; 
    public float catchModifier=0; //Makes progress bar fill faster
    public float difficultyMultiplier=0;//Changes fish speed

    #endregion

    #region Other

    [Header("Default minigame scale, so the object can be made sure to initialize in the right scale (due to scale changes on gameOver animation)")]
    public Vector3 minigameScale;  //Default scale is x = 0.809375, Y = 2.8125, z = 1

    [Header("The top and bottom minigame boundaries values. Check position in scene and tweak it accordingly if necessary.")]
    public float minigameTopPosition=42; 
    public float minigameBottomPosition=-42;

    //For challenges:


    #endregion

    public static BF_MinigameController Instance { get; set; }

    private void Awake()
    {
     
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

    }

    void Start()
    {


        playerBarInitialPos = playerBar.transform.localPosition;

        //Sets random dest position to start with
        newPosition = new Vector3(0, Random.Range(minigameBottomPosition, minigameTopPosition), 0);
        if (randomizeSpeed)
            RandomizeSpeed(minVel, maxVel);

        //Tweak player bar mass and gravity scaler to compensate for different resolutions
        float heightScaler = Screen.height / 960.0f;
        float inverseHeightScaler = 960.0f / Screen.height;

        gravityModifier *= heightScaler;
        playerBar.GetComponent<Rigidbody2D>().gravityScale = gravityModifier;
        playerBar.GetComponent<Rigidbody2D>().mass *= inverseHeightScaler;

        //Initializing progress bar to 0
        progressBar.fillAmount = 0;

        BF_UIManager.Instance.castButton.gameObject.SetActive(false); // Disable cast button

    }

    //On Disable reset anything we have
    private void OnDisable()
    {
        //Disable vsync
        QualitySettings.vSyncCount = 0;

        isOverlapping = false;
        hasTouchedFishIcon = false;
        gameOver = false;

        //Unfreeze player bar so its ready for the next run
        UnfreezePlayerBar();

        //Reset player bar position
        playerBar.transform.localPosition = playerBarInitialPos; 

        //Next time fish appears will be on the top, this prevents the fish from spawning immediately on top of the player bar
        fishIcon.transform.localPosition = new Vector3(0, minigameTopPosition, 0); 

        StopAllCoroutines();
    }

    private void OnEnable()
    {
        //Enable vsync to ensure minigame runs smoothly
        QualitySettings.vSyncCount = 1;

        //Reset our scale (because its changed after the gameOver animation)
        this.gameObject.GetComponent<RectTransform>().localScale = minigameScale;
        //Apply modifiers
        ApplyModifiers();
    }

   
    void FixedUpdate()
    {
        
        if (gameOver == false)
        {

            MovefishIcon();

            if (isOverlapping)
            {
                IncreaseProgressBar(fillSpeed+catchModifier);
            }
            else
            {
                DecreaseProgressBar(unFillSpeed);
            }
            if (progressBar.fillAmount >= 0.99f)
            {

                WinGame();

            }

            //Check for gameOver conditions
            if (hasTouchedFishIcon)
            {

                if (progressBar.fillAmount <= 0)
                {
                    if (this.gameObject.activeInHierarchy)
                        StartCoroutine(StartGameOverTimer(gameOverTime-gameOverReduction));
                }
                else // If has touched once and still touching don't let the coroutine continue            
                    StopAllCoroutines();

            }

        }    
           
    }

    private void MovefishIcon()
    {

        //Setting speed
        if (randomizeSpeed == false)
        {
            fishIcon.transform.localPosition = Vector3.MoveTowards(fishIcon.transform.localPosition, newPosition, Time.deltaTime * moveSpeed);
        }
        else
        {
            float applyAmount = (difficultyMultiplier / 100) * randomSpeed;
            float tempRandSpeed = randomSpeed + applyAmount;
            fishIcon.transform.localPosition = Vector3.MoveTowards(fishIcon.transform.localPosition, newPosition, Time.deltaTime * tempRandSpeed);
        }


        if (newPosition.y > 0) // If y > 0
        {
            if  (fishIcon.transform.localPosition.y <= newPosition.y && fishIcon.transform.localPosition.y >= newPosition.y) //Eg: y = 42, if fishIcon is between 42 and 41, consider done
            {
                newPosition = new Vector3(0, Random.Range(-42, 42), 0);

                if (randomizeSpeed)
                    RandomizeSpeed(minVel, maxVel);
            }
        }
        else if (newPosition.y == 0) // If y = 0
        {
            if (fishIcon.transform.localPosition.y <= 0.5f && fishIcon.transform.localPosition.y >= -0.5f) // If y = 0, if fishIcon is between -0.5f and 0.5f, consider done
            {
                newPosition = new Vector3(0, Random.Range(-42, 42), 0);

                if (randomizeSpeed)
                    RandomizeSpeed(minVel, maxVel);
            }
        }
        else // If y < 0
        {
            if (fishIcon.transform.localPosition.y >= newPosition.y && fishIcon.transform.localPosition.y <= newPosition.y) //Eg: y = -42, if fishIcon is between -42 and -41, consider done
            {
                newPosition = new Vector3(0, Random.Range(-42, 42), 0);

                if (randomizeSpeed)
                    RandomizeSpeed(minVel, maxVel);
            }
        }

    }

    private void RandomizeSpeed(float minVel, float maxVel)
    {
        randomSpeed = Random.Range(minVel, maxVel);
    }


    //Makes player bar go up (is hooked to playerBar button)
    public void BumpPlayerBar()
    {
        Vector2 force = new Vector2(0f, 5f);

        playerBar.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bumpPower, ForceMode2D.Impulse);

    }
    
    public void IncreaseProgressBar(float fillSpeed)
    {
        progressBar.fillAmount += Time.deltaTime * fillSpeed;
    }

    public void DecreaseProgressBar(float unFillSpeed)
    {
        progressBar.fillAmount -= Time.deltaTime * unFillSpeed;
    }

    private IEnumerator StartGameOverTimer(float timeLimit)
    {

        yield return new WaitForSeconds(timeLimit); //Make sure we reset this method every time the player starts touching the fishIcon again      
        LoseGame();
       
    }

    private void LoseGame()
    {
        if(gameOver == false)
        {
            gameOver = true;
            FreezePlayerBar();
            //Stop player bar movement and give it back on disable;
            BF_Animations_Manager.Instance.PlayerResetAnimation();
            BF_AudioManager.Instance.StopPlayingSFX(); //Stop reeling sound
            this.gameObject.GetComponent<Animator>().enabled = true;
            this.gameObject.GetComponent<Animator>().SetTrigger("gameOver");
            #region Challenge Stuff
            //If we miss a fish reset counter
            if (BF_GameplayManager.Instance.chosenFish.isGarbage == false)
            {
                BF_ChallengeManager.Instance.ResetChallengeProgression(BF_ChallengeManager.CHALLENGE_ID.CATCH_5_FISH_ROW); //Lost, so reset back to 0
                BF_ChallengeManager.Instance.ResetChallengeProgression(BF_ChallengeManager.CHALLENGE_ID.CATCH_10_FISH_ROW); //Lost, so reset back to 0
            }

            #endregion Challenge Stuff
            BF_GameplayManager.Instance.sadFace.SetActive(true);
            BF_ShakeSadFace.Instance.Begin();
        }
       
    }

    private void WinGame()
    {
        #region Challenge Stuff

        //If we catch a fish increase
        if(BF_GameplayManager.Instance.chosenFish.isGarbage == false)
        {
            BF_ChallengeManager.Instance.UpdateChallengeProgression(BF_ChallengeManager.CHALLENGE_ID.CATCH_5_FISH_ROW, 20); //5 fish so increase progression by 20%
            BF_ChallengeManager.Instance.UpdateChallengeProgression(BF_ChallengeManager.CHALLENGE_ID.CATCH_10_FISH_ROW, 10); //10 fish so increase progression by 10%
        }
            

        #endregion Challenge Stuff

        BF_AudioManager.Instance.StopPlayingSFX(); //Stop reeling sound
        BF_Animations_Manager.Instance.PlayFishCaughtAnimation();
        BF_UIManager.Instance.caughtFishPopup.SetActive(true);

        if(BF_GameplayManager.Instance.chosenFish.isGarbage == false)
            UserDataManager.Instance.BF_data.NumFishCaught++;

        BF_GameplayManager.Instance.FishSO[BF_GameplayManager.Instance.currentFishIndex].caughtAmount++;
        UserDataManager.Instance.Save();
        //Set particle effect color and play fish caught particle effect
        Color particleCol = BF_GameplayManager.Instance.chosenFish.fishCaughtParticleColor;
        BF_ParticleSystem_Manager.Instance.particleSystems[(int)BF_ParticleSystem_Manager.EFFECTS.FISH_CAUGHT_COMMOM_EFFECT].GetComponent<ParticleSystemRenderer>().material.color = particleCol;
        BF_ParticleSystem_Manager.Instance.PlayEffect((int)BF_ParticleSystem_Manager.EFFECTS.FISH_CAUGHT_COMMOM_EFFECT);

        BF_AudioManager.Instance.PlaySFX((int)BF_AudioManager.SFX.CATCH_FISH_1); // Play catch fish reward sound

        StartCoroutine(AchievementManager.Instance.ShowPopup());

        //End game
        progressBar.fillAmount = 0;
        this.gameObject.SetActive(false);

    }

    //Applies any modifiers we currently have
    private void ApplyModifiers()
    {
        //Formula = modifier /100 * currentAmount
        if (gameOverReduction != 0)
        {
            float applyAmount = (gameOverReduction / 100) * gameOverTime;
            gameOverTime += applyAmount;
        }
        if (catchModifier != 0)
        {
            float applyAmount = (catchModifier / 100) * fillSpeed;
            fillSpeed += applyAmount;
        }
        if (difficultyMultiplier != 0 && !randomizeSpeed) //Only pre-apply when speed is constant
        {
            float applyAmount = (difficultyMultiplier / 100) * moveSpeed;
            moveSpeed += applyAmount;
        }

    }

    //To be used as an animation event for game over
    private void DeactivateSelf()
    {
        this.gameObject.GetComponent<Animator>().enabled = false;
        this.gameObject.SetActive(false);
    }

    private void FreezePlayerBar()
    {
        playerBar.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezeAll;
    }

    private void UnfreezePlayerBar()
    {
        playerBar.GetComponent<Rigidbody2D>().constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
    }

}
