using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This script will notify the minigame when we overlap the fishIcon, meaning the progress bar should go up
/// This script should be attached to the player_Bar object
/// </summary>
public class HandleCollisions : MonoBehaviour
{

    public BF_MinigameController minigameScript;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("FishIcon"))
        {
            minigameScript.isOverlapping = true;
            minigameScript.hasTouchedFishIcon = true; //On enter player hasTouched it the first time
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FishIcon"))
        {
            minigameScript.isOverlapping = false;
        }
    }

}
