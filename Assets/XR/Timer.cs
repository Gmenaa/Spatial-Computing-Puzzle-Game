using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

// Made by: Helen McKay | 2102556

// notes:
    // all variables are private, some use properties (C#'s getters and setters)
    // variables that need to be accessed in Unity have [SerializeField]
    // made a function for handling timer reaching 0 (if it's not needed, can remove the function)

// usage:
    // attach this script to an object
    // connect a Text Mesh Pro (TMP) object to the TMP_Text field timeText
    // initialTimeRemaining is set by the GameLoop, feel free to set it to be whatever
    // feel free to add [SerializeField] to more variables as desired

public class Timer: MonoBehaviour
{
    // variables

    [SerializeField]
    private float initialTimeRemaining;
    public float InitialTimeRemaining {get {return initialTimeRemaining;} set {initialTimeRemaining = value;}}  // the value the timer starts at

    private float rawTimeRemaining;                                                                             // the time remaining, as a float
    public float TimeRemaining {get { return rawTimeRemaining; }}                                                // the time remaining, as a property
    private bool isTimerRunning;
    public bool IsTimerRunning {get {return isTimerRunning;}}                                                   // boolean check of whether the timer is still running
    
    [SerializeField]
    private TMP_Text timeText;                                                                                  // Text Mesh Pro object that displays the time                   
    private bool didTimeRunOut;
    public bool DidTimeRunOut{get {return didTimeRunOut;}}                                                      // boolean check of whether timer reached zero (meaning, player did not solve puzzle in time)
    
    // getFormattedTime
        // formats the time in minutes and seconds
        // receives time as a float
        // returns formatted time as a string
    private string GetFormattedTime(float the_time)
    {
        float minutes = Mathf.FloorToInt(the_time / 60);
        float seconds = Mathf.FloorToInt(the_time % 60);

        return string.Format("{0:00}:{1:00}", minutes, seconds);
    }

    public void StopTheTimer()
    {
        isTimerRunning = false;
    }

    // handlers

    private void HandleTimeRunOut()
    {
        // FIXME: handle time run out
    }

    // before first frame
        // initialize variables
    void Start()
    {
        rawTimeRemaining = InitialTimeRemaining;
        timeText.text = GetFormattedTime(rawTimeRemaining);
        isTimerRunning = true;
        didTimeRunOut = false;
    }

    // every frame
    void Update()
    {
        if (IsTimerRunning)
        {
            if (rawTimeRemaining > 0)
            {
                rawTimeRemaining -= Time.deltaTime;
                timeText.text = GetFormattedTime(rawTimeRemaining);
            }
            else
            {
                rawTimeRemaining = 0;
                timeText.text = GetFormattedTime(0);
                isTimerRunning = false;
                didTimeRunOut = true;
            }
        }
        else if (DidTimeRunOut)
        {
            HandleTimeRunOut();
        }
    }
}