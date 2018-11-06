﻿using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Author: JaJuan Webster
/// Controls the Map Selection of the game
/// </summary>
public class MenuManager : MonoBehaviour
{
    // Attributes
    GameObject[] islandObjects;
    GameObject menuObject, startObject;
    GameObject menuIMG, worldIMG;
    public Button norseBtn, egyptBtn, diyuBtn;
    static bool norseClicked, egyptClicked, diyuClicked;

    // Use this for initialization
    void Start()
    {
        // Instantiate
        islandObjects = GameObject.FindGameObjectsWithTag("Island");
        menuObject = GameObject.FindGameObjectWithTag("Menu");
        startObject = GameObject.FindGameObjectWithTag("Start");
        menuIMG = GameObject.FindGameObjectWithTag("MenuIMG");
        worldIMG = GameObject.FindGameObjectWithTag("WSIMG");

        // Create buttons to add event listeners for the proper function
        Button btn1 = norseBtn.GetComponent<Button>();
        btn1.onClick.AddListener(NorseOnClick);
        Button btn2 = egyptBtn.GetComponent<Button>();
        btn2.onClick.AddListener(EgyptOnClick);
        Button btn3 = diyuBtn.GetComponent<Button>();
        btn3.onClick.AddListener(DiyuOnClick);
        HideIslands();
    }

    /// <summary>
    /// Shows the island options and the menu buttion
    /// </summary>
    public void ShowIslands()
    {
        if (norseClicked)
            norseBtn.interactable = false;
        if (egyptClicked)
            egyptBtn.interactable = false;
        if (diyuClicked)
            diyuBtn.interactable = false;

        foreach (GameObject g in islandObjects)
        {
            g.SetActive(true);
        }
        menuObject.SetActive(true);
        startObject.SetActive(false);
        menuIMG.SetActive(false);
        worldIMG.SetActive(true);
    }

    /// <summary>
    /// Hides the island options and the menu button
    /// </summary>
    public void HideIslands()
    {
        foreach (GameObject g in islandObjects)
        {
            g.SetActive(false);
        }
        menuObject.SetActive(false);
        startObject.SetActive(true);
        menuIMG.SetActive(true);
        worldIMG.SetActive(false);
    }

    /// <summary>
    /// Returns the game to the Menu
    /// </summary>
    public void MainMenu()
    {
        menuIMG.SetActive(true);
        worldIMG.SetActive(false);
        startObject.SetActive(true);
        menuObject.SetActive(false);
    }

    /// <summary>
    /// Goes to Norse Scene
    /// </summary>
    public void NorseOnClick()
    {
        norseClicked = true;
        SceneManager.LoadScene("Island 1 - NORSE", LoadSceneMode.Single);
    }

    /// <summary>
    /// Goes to Diyu Scene
    /// </summary>
    public void DiyuOnClick()
    {
        diyuClicked = true;
        SceneManager.LoadScene("Island 2 - DIYU", LoadSceneMode.Single);
    }

    /// <summary>
    /// Goes to Egypt Scene
    /// </summary>
    public void EgyptOnClick()
    {
        egyptClicked = true;
        SceneManager.LoadScene("Island 3 - EGYPT", LoadSceneMode.Single);
    }
}