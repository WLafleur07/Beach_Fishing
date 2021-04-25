using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_AudioManager : MonoBehaviour
{
    public static BF_AudioManager Instance { get; set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }


    public AudioSource SFX_AudioSource;

    public AudioSource Music_AudioSource;

    /********NOTE*******/
    //When adding new atmosphere audio sources remember to include it in all the following methods:
    // PlayAtmosphere_SFX
    // StopPlayingAtmosphere_SFX
    // SetSFXVolume
    [Header("Atmosphere audio sources")]
    public AudioSource Rain_AudioSource;
    public AudioSource Waves_AudioSource;

    public AudioClip[] musicClips;
    public AudioClip[] SFXClips;
    public AudioClip[] AtmosphereClips;

    private bool sfxOn = true;
    private bool musicOn = true;
    private bool atmosphereSFXOn = true;

    //Use Start and Update for Background Music
    int previousClip = 0;
    int randClip = 0;

    private void Start()
    {

        randClip = Random.Range((int)MUSIC.BACKGROUND_1, (int)MUSIC.BACKGROUND_5);
        previousClip = randClip;

        Music_AudioSource.clip = musicClips[randClip];
        Music_AudioSource.Play();

    }

    private void Update()
    {

        //When song is over play new song
        if (Music_AudioSource.isPlaying == false)
        {
            //Make sure we don't repeat the same song
            while (randClip == previousClip)
            {
                randClip = Random.Range((int)MUSIC.BACKGROUND_1, (int)MUSIC.BACKGROUND_5);
            }

            Music_AudioSource.clip = musicClips[randClip];
            Music_AudioSource.Play();

        }

    }

    public enum SFX
    {
        FISH_HOOKED_1,
        FISH_HOOKED_2,
        REEL_IN_1,
        REEL_IN_2,
        ROD_WOOSH_1,
        TOUCH_WATER_1,
        TOUCH_WATER_2,
        CATCH_FISH_1,
        CATCH_FISH_2,
        ACHIEVEMENT_UNLOCK_1,
        BUTTON_CLICK_1,
        FISH_LOST_1,
        CHALLENGE_REWARD,

        NUM_SFX
    }

    public enum ATMOSPHERE_SFX
    {
        RAIN,
        WAVES,

        NUM_ATMOSPHERE_SFX
    }


    public enum MUSIC
    {
        BACKGROUND_1,
        BACKGROUND_2,
        BACKGROUND_3,
        BACKGROUND_4,
        BACKGROUND_5,

        NUM_MUSIC
    }

    //TODO - we can either have 1 method for each UI sound we wanna play, such as achieements click, choose rod click or choose bait click
    //Or we can have different audio sources for those,assign the clip and just hook up the button
    public void PlayButtonClickSound()
    {
        if (sfxOn)
            SFX_AudioSource.PlayOneShot(SFXClips[(int)SFX.BUTTON_CLICK_1]);
    }

    public void PlaySFX(int clipID)
    {

        if (sfxOn)
        {
            SFX_AudioSource.PlayOneShot(SFXClips[(int)clipID]);
        }

    }

    public void PlayMusic(int clipID)
    {

        if (musicOn)
        {
            Music_AudioSource.clip = musicClips[(int)clipID];
            Music_AudioSource.Play();
        }

    }

    public void PlayAtmosphere_SFX(int clipID)
    {

        if (atmosphereSFXOn)
        {
            switch (clipID)
            {
                case (int)ATMOSPHERE_SFX.RAIN:
                    Rain_AudioSource.clip = AtmosphereClips[clipID];
                    Rain_AudioSource.Play();
                    break;
                case (int)ATMOSPHERE_SFX.WAVES:
                    Waves_AudioSource.clip = AtmosphereClips[clipID];
                    Waves_AudioSource.Play();
                    break;
            }
        }

    }

    public void StopPlayingMusic()
    {
        if (Music_AudioSource.isPlaying)
            Music_AudioSource.Stop();
    }

    public void StopPlayingSFX()
    {
        if (SFX_AudioSource.isPlaying)
            SFX_AudioSource.Stop();
    }

    public void StopPlayingAtmosphere_SFX(ATMOSPHERE_SFX sfx)
    {

        switch (sfx)
        {
            case ATMOSPHERE_SFX.RAIN:
                if (Rain_AudioSource.isPlaying)
                    Rain_AudioSource.Stop();
                break;
        }

    }

    //Maybe get rid of this in the future
    public void TurnMusicOn()
    {
        musicOn = true;
    }

    public void TurnMusicOff()
    {
        musicOn = false;
    }

    public void TurnSFXOn()
    {
        sfxOn = true;
    }

    public void TurnSFXOff()
    {
        sfxOn = false;
    }

    public void TurnAtmosphere_SFX_On()
    {
        atmosphereSFXOn = true;
    }

    public void TurnAtmosphere_SFX_Off()
    {
        atmosphereSFXOn = false;
    }

    public bool MusicIsOn()
    {
        return musicOn;
    }

    public bool SFXIsOn()
    {
        return sfxOn;
    }

    public void SetSFXVolume(float vol)
    {
        SFX_AudioSource.volume = vol;
        Rain_AudioSource.volume = vol;
        Waves_AudioSource.volume = vol;
    }

    public void SetMusicVolume(float vol)
    {
        Music_AudioSource.volume = vol;
    }

    public float GetMusicVolume()
    {
        return Music_AudioSource.volume;
    }

    public float GetSFXVolume()
    {
        return SFX_AudioSource.volume;
    }

    public void MuteSFX()
    {
        SFX_AudioSource.volume = 0;
        TurnAtmosphere_SFX_Off();
    }

    public void MuteMusic()
    {
        Music_AudioSource.volume = 0;
    }

    public void UnmuteSFX()
    {
        SFX_AudioSource.volume = 0.144f;
        TurnAtmosphere_SFX_On();
    }

    public void UnmuteMusic()
    {
        Music_AudioSource.volume = 0.02f;
    }

    public void FadeOut(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeOutCoroutine(audioSource, fadeTime));
    }

    public void FadeIn(AudioSource audioSource, float fadeTime)
    {
        StartCoroutine(FadeInCoroutine(audioSource, fadeTime));
    }

    //Method that allows for fading out an audio source over time
    private IEnumerator FadeOutCoroutine(AudioSource audioSource, float fadeTime)
    {
        if (audioSource.isPlaying)
        {
            float startVolume = audioSource.volume;

            while (audioSource.volume > 0)
            {
                audioSource.volume -= startVolume * Time.deltaTime / fadeTime;

                yield return null;
            }

            audioSource.Stop();
            audioSource.volume = startVolume;
        }

    }

    //Method that allows for fading in an audio source over time
    private IEnumerator FadeInCoroutine(AudioSource audioSource, float fadeTime)
    {
        if (audioSource.isPlaying)
        {
            float endVolume = audioSource.volume;
            audioSource.volume = 0;

            while (audioSource.volume <= endVolume)
            {
                audioSource.volume += endVolume * Time.deltaTime / fadeTime;

                yield return null;
            }
        }

    }

}