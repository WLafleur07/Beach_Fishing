using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BF_ChallengeScreen : MonoBehaviour
{

    public GameObject challengePrefab;
    public BF_ChallengeSO challengeSO;

    private void LoadChallenges()
    {

        //TODO
        //To be called on start?
        //Instantiate prefabs from the challenge manager

    }

    public void GoBack(GameObject previousScreen)
    {

        previousScreen.SetActive(true);
        this.gameObject.SetActive(false);
        //TODO
        //Add method to unload challenges?

    }
}
