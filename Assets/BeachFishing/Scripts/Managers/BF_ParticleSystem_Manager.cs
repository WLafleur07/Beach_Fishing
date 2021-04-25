using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_ParticleSystem_Manager : MonoBehaviour
{
    public static BF_ParticleSystem_Manager Instance { get; set; }

    public ParticleSystem[] particleSystems;

    #region Rain Settings
    [Space(15)]
    [Header("Rain Settings")]

    public GameObject rainDimmer;

    [Header("Rain duration (random between 2 values)")]
    [Min(45f)]
    public float minRainDuration;
    public float maxRainDuration;

    [Header("How long until the rain start playing again (random between 2 values)")]
    public float minRainStartTime;
    public float maxRainStartTime;
    [HideInInspector]
    public float rainTimer = 300f;

    [Header("A variable to fade out the rain music and rain particle system")]
    public float rainFadeTime = 3f;
    #endregion

    private void Awake()
    {

        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);

    }
  

    public enum EFFECTS
    {
        RAIN_EFFECT,
        FISH_CAUGHT_COMMOM_EFFECT,
        FISH_CAUGHT_MERDIUM_EFFECT,
        FISH_CAUGHT_RARE_EFFECT,
        FISH_CAUGHT_ULTRA_RARE_EFFECT,

        NUM_EFFECTS
    }

    void Start()
    {

        StartCoroutine(ActivateRainOnTimer());

    }



    public void PlayEffect(int effectID)
    {

        particleSystems[effectID].Play();

    }

    public void StopEffect(int effectID)
    {

        particleSystems[effectID].Stop();

    }

    public bool IsEffectPlaying(int effectID)
    {

        return particleSystems[effectID].isPlaying ? true : false;  

    }

    public IEnumerator ActivateRainOnTimer()
    {
        int rainID = (int) EFFECTS.RAIN_EFFECT;
        float rainTimer = Random.Range(minRainStartTime, maxRainStartTime);

        while (true)
        {
            yield return new WaitForSeconds(rainTimer);

            if (IsEffectPlaying(rainID))
            {

                StopEffect(rainID);

                yield return new WaitForSeconds(1); // Wait 1 second to allow rain to stop falling


               BF_AudioManager.Instance.FadeOut(BF_AudioManager.Instance.Rain_AudioSource, rainFadeTime); //Fade out time = to the dimmer fade out time

                //Disable dimmer
                LeanTween.alpha(gameObject: rainDimmer, to: 0f, time: rainFadeTime); // Increase time: to increase transition duration

                // Random time until rain starts again
                rainTimer = Random.Range(minRainStartTime, minRainStartTime);

            }
            else
            {

                //Enable dimmer
                LeanTween.alpha(gameObject: rainDimmer, to: 1f, time: 3f); // Increase time: to increase transition duration

                // Random time for how long the rain lasts
                rainTimer = Random.Range(minRainDuration, maxRainDuration);

                yield return new WaitForSeconds(3); // Wait 3 seconds after the rainDimmer transition starts before raining
                BF_AudioManager.Instance.PlayAtmosphere_SFX((int)BF_AudioManager.ATMOSPHERE_SFX.RAIN);
                BF_AudioManager.Instance.FadeIn(BF_AudioManager.Instance.Rain_AudioSource,2f);
                PlayEffect(rainID);

                if (!BF_GameplayManager.Instance.hasRained)
                {
                    BF_GameplayManager.Instance.hasRained = true;
                    StartCoroutine(AchievementManager.Instance.ShowPopup());
                }
            }

        }

    }

}
