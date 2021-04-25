using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransition : MonoBehaviour
{
    public static CameraTransition Instance { get; set; }

    public float mainGameDestination;
    public float mainMenuDestination;
    public float moveSpeed;
    public GameObject cam;
    public bool isStartClicked;
    public bool isHomeClicked;

    private void Start()
    {
        isStartClicked = false;
        isHomeClicked = false;
    }
    private void Update()
    {
        if (isStartClicked == true)
        {
            MainMenuToGame();
        }
        else if (isHomeClicked == true)
        {
            GameToMainMenu();
        }
    }

    public void MainMenuToGame()
    {
            if (cam.transform.position.x != mainGameDestination)
            {
                Vector3 camPos = new Vector3(mainGameDestination, transform.position.y, cam.transform.position.z);
                cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPos, Time.deltaTime * moveSpeed);
            }
    }

    public void GameToMainMenu()
    {
        if (cam.transform.position.x != mainMenuDestination)
        {
            Vector3 camPos = new Vector3(mainMenuDestination, transform.position.y, cam.transform.position.z);
            cam.transform.position = Vector3.MoveTowards(cam.transform.position, camPos, Time.deltaTime * moveSpeed);
        }
    }

    public void StartClicked()
    {
        isStartClicked = true;
        isHomeClicked = false;
    }

    public void HomeClicked()
    {
        isHomeClicked = true;
        isStartClicked = false;
    }
}
