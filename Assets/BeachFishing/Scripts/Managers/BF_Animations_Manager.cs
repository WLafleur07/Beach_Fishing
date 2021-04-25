using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_Animations_Manager : MonoBehaviour
{
    public GameObject player;
    public GameObject bobber;
    public GameObject fish;
    public Animator animPlayer;
    private Animator animBobber;
    private Animator animFish;

    public static BF_Animations_Manager Instance { get; set; }

    public enum ANIM_LAYER
    {
        DEFAULT,
        GOLD,
        MAGENTA,

        NUM_LAYERS
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this.gameObject);
    }

    void Start()
    {
        animPlayer = player.GetComponent<Animator>();
        animBobber = bobber.GetComponent<Animator>();
        animFish = fish.GetComponent<Animator>();

        animBobber.StopPlayback();
        animFish.StopPlayback();
    }

    void Update()
    {
        if (animPlayer.IsInTransition(0) && animPlayer.GetNextAnimatorStateInfo(0).IsName("Fisherman_Idle_Fishing") || 
                                            animPlayer.GetNextAnimatorStateInfo(0).IsName("Gold_Fisherman_Idle_Fishing"))
        {
            bobber.SetActive(true);
            animBobber.Play("Bobber_Idle");
        }

        if (animPlayer.IsInTransition(0) && animPlayer.GetNextAnimatorStateInfo(0).IsName("Fisherman_ReelIn") || 
                                            animPlayer.GetNextAnimatorStateInfo(0).IsName("Gold_Fisherman_ReelIn"))
        {
            bobber.transform.localPosition = new Vector2(0.7f, bobber.transform.localPosition.y);
        }

        if (animPlayer.IsInTransition(0) && animPlayer.GetNextAnimatorStateInfo(0).IsName("Fisherman_Idle") || 
                                            animPlayer.GetNextAnimatorStateInfo(0).IsName("Gold_Fisherman_Idle"))
        {
            bobber.transform.localPosition = new Vector2(0.6f, bobber.transform.localPosition.y);
            bobber.SetActive(false);
        }

        if (animPlayer.IsInTransition(0) && animPlayer.GetNextAnimatorStateInfo(0).IsName("Fisherman_FishCaught") ||
                                            animPlayer.GetNextAnimatorStateInfo(0).IsName("Gold_Fisherman_FishCaught"))
        {
            bobber.transform.localPosition = new Vector2(0.6f, bobber.transform.localPosition.y);
            bobber.SetActive(false);
        }
    }

    // Being called from the CAST Button
    public void PlayCastAnimation()
    {
        animPlayer.SetTrigger("Cast");
        bobber.SetActive(false);
        //Reset cast timer
        BF_UIManager.Instance.CancelButton();
        BF_UIManager.Instance.castButton.gameObject.SetActive(false);
    }

    public void PlayReelInAnimation()
    {
        animPlayer.SetBool("FishHooked", true);
    }

    public void PlayFishLostAnimation()
    {
        animPlayer.SetTrigger("Reset");
    }

    public void PlayFishCaughtAnimation()
    {
        animPlayer.SetBool("FishHooked", false);

        animPlayer.SetBool("FishCaught", true);
        animBobber.SetBool("FishCaught", true);
        fish.SetActive(true);
        animFish.Play("FishCaught");
    }

    // TODO: Call when User has viewed their Caught Fish
    public void PlayerResetAnimation()
    {

        animPlayer.SetTrigger("Reset");
        animPlayer.SetBool("FishCaught", false);
        animPlayer.SetBool("FishHooked", false);
        animBobber.SetBool("FishCaught", false);
        fish.SetActive(false);
        bobber.SetActive(false);
        BF_UIManager.Instance.caughtFishPopup.SetActive(false);
        BF_UIManager.Instance.CastButton();
        BF_UIManager.Instance.OpenScreen(7); // Home panel
        BF_UIManager.Instance.OpenScreen(8); // Challenge panel
    }


    public void FishHookedWarningAnimation(bool isHooked)
    {
        animPlayer.SetBool("FishHooked", isHooked);
    }

    public void SetRodLayerWeight(int index)
    {
        switch(index)
        {
            case (int)ANIM_LAYER.DEFAULT:
                animPlayer.SetLayerWeight((int)ANIM_LAYER.DEFAULT, 1);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.GOLD, 0);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.MAGENTA, 0);
                break;
            case (int)ANIM_LAYER.GOLD:
                animPlayer.SetLayerWeight((int)ANIM_LAYER.DEFAULT, 0);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.GOLD, 1);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.MAGENTA, 0);
                break;
            case (int)ANIM_LAYER.MAGENTA:
                animPlayer.SetLayerWeight((int)ANIM_LAYER.DEFAULT, 0);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.GOLD, 0);
                animPlayer.SetLayerWeight((int)ANIM_LAYER.MAGENTA, 1);
                break;
        }
    }
}
