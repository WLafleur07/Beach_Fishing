using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script contains the audio for all the fishing animations that the player performs
/// </summary>
public class Player_Audio : MonoBehaviour
{
    public void PlayReelInAudio()
    {

        int randClip = Random.Range((int)BF_AudioManager.SFX.REEL_IN_1, (int)BF_AudioManager.SFX.REEL_IN_2);
        if (BF_AudioManager.Instance.SFX_AudioSource.isPlaying == false)
        { 
            BF_AudioManager.Instance.PlaySFX(randClip);
        }
      
    }

    public void PlayRodCastAudio()
    {
        
        BF_AudioManager.Instance.PlaySFX((int)BF_AudioManager.SFX.ROD_WOOSH_1);

    }

    public void PlayBobberTouchWaterAudio()
    {

        int randClip = Random.Range((int)BF_AudioManager.SFX.TOUCH_WATER_1, (int)BF_AudioManager.SFX.TOUCH_WATER_2);
        BF_AudioManager.Instance.PlaySFX(randClip);

    }
}
