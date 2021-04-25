using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//You need to use the UnityEngine.UI namespace in order to manipulate the UI.
using UnityEngine.UI;

public class BF_FillGauge : MonoBehaviour
{

    [Header("Image to act as a gauge filling up")]
    public Image clickMeter;

    [Header("Max number gauge needs to reach to be full")]
    public float curMeter, maxMeter;

    [Header("Amount meter goes up by each click")]
    public float incMeter;

    [Header("Amount meter goes down by")]
    public float decMeter;

    //float to determine the time between clicks.
    private float meterReduceTimer;

    [Header("How many frames need to transpire before the bar starts going back down")]
    public float timeBetweenClicks;

    [Header("Value to set at what level the player needs to fill the gauge before they can lose")]
    public float loseConditionVariable;

    // User to see if the player has made it past the point of no return
    private bool canLose = false;
    Coroutine timerRoutine = null;
    private bool coroutineRunning = false;


    public static BF_FillGauge Instance { get; set; }

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

    }

    void Update()
    {
        // Stop actions when meter is full, then process to minigame
        if (curMeter != maxMeter)
        {
            ImageChange();
            MaxMinValue();
            ReduceMeter();
            CheckFillProgress();
        }
        else //This is being triggered when the fill gauge goes over the loseConditionVariable and then back to 0
        {
            BF_AudioManager.Instance.StopPlayingSFX();
            BF_GameplayManager.Instance.CatchFish();
            gameObject.SetActive(false);
            curMeter = 0;
            canLose = false;
        }
    }

    // Called in FillGauge button OnClick
    public void ClickToFill()
    {
        if (BF_AudioManager.Instance.SFX_AudioSource.isPlaying == false)
            BF_AudioManager.Instance.PlaySFX((int)BF_AudioManager.SFX.REEL_IN_2);

        // increases the meter by the specified value assigned in the inspector
        curMeter += incMeter;
        // Whenever button is clicked, set the timer back to 0
        meterReduceTimer = 0;

        if (coroutineRunning)
        {
            StopCoroutine(timerRoutine);
            coroutineRunning = false;
        }
    }

    void ReduceMeter()
    {

        // Increase the reduce timer
        meterReduceTimer += 1;

        // If too much time elapses between clicks
        if (meterReduceTimer > timeBetweenClicks)
        {
            // Start reducing the image fill
            curMeter -= decMeter;
        }
    }

    void ImageChange()
    {
        //The image fill amount is CurrentMeter divided by MaxMeter
        clickMeter.fillAmount = curMeter / maxMeter;
    }

    void MaxMinValue()
    {

        //if current meter is less than 0 then current meter equals 0.
        //if current meter is more than max meter then current meter equals max meter

        if (curMeter < 0)
        {
            curMeter = 0;

            if (canLose)
            {
                BF_AudioManager.Instance.StopPlayingSFX();
                gameObject.SetActive(false);
                BF_GameplayManager.Instance.MissCatchOpportunity();
                canLose = false;
            }
        }
        else if (curMeter > maxMeter)
        {
            curMeter = maxMeter;
        }
    }

    public void CheckFillProgress()
    {
        if (curMeter > loseConditionVariable && canLose == false)
        {
            canLose = true;
        }
    }

    public void CancelFillGauge()
    {
        gameObject.SetActive(false);
        curMeter = 0;
        canLose = false;
    }

    void OnEnable()
    {
        timerRoutine = StartCoroutine(Timer());
    }

    private IEnumerator Timer()
    {
        coroutineRunning = true;
        yield return new WaitForSeconds(5);
        yield return canLose = true;
        yield return coroutineRunning = false;
    }

}