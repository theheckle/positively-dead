﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

/// <summary>
/// Author: Nikolas Whiteside
/// Animates text on the hud like a typewriter
/// </summary>
public class Typewriter : MonoBehaviour
{
    //The text to be animated
    private Text animText;

    //Test to see if the string has changed
    private string testText;

    //The final string to display
    public string finalText;    

    //The typing delay
    public float typeDelay;

    public bool animComplete;

    // Use this for initialization
    void Awake()
    {
        animText = gameObject.GetComponent<Text>();
        StartCoroutine(Type());
    }

    // Update is called once per frame
    void Update()
    {
        //Check whether the string has changed
        if (finalText != testText)
        {
            //Reset the text and begin the animation again
            StopAllCoroutines();
            animText.text = "";
            StartCoroutine(Type());
        }

        //Set the test equal to the desired string
        testText = finalText;
    }

    /// <summary>
    /// Types out the text
    /// </summary>
    IEnumerator Type()
    {
        animComplete = false;
        for (int i = 0; i < finalText.Length + 1; i++)
        {
            AkSoundEngine.PostEvent("Type", gameObject);
            animText.text = finalText.Substring(0, i);
            yield return new WaitForSeconds(typeDelay);
        }
        animComplete = true;
    }
}