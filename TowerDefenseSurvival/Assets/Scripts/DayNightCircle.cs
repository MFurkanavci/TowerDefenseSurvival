using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCircle : MonoBehaviour
{
    //make a day night cycle with dusk and dawn in between 

    public float dayLength = 120f;
    public float nightLength = 120f;
    public float duskLength = 10f;
    public float dawnLength = 10f;

    public float currentTime = 0f;
    public float timeMultiplier = 1f;

    enum TimeOfDay { Day, Night, Dusk, Dawn };
    TimeOfDay tod = TimeOfDay.Day;

    public Light sun;
    
    public Color dayColor;
    public Color nightColor;
    public Color duskColor;
    public Color dawnColor;


    // Start is called before the first frame update
    void Start()
    {
        currentTime = 0f;
        sun.color = dayColor;
    }

    // Update is called once per frame
    void Update()
    {
        currentTime += Time.deltaTime * timeMultiplier;

        if (tod == TimeOfDay.Day)
        {
            if (currentTime >= dayLength)
            {
                currentTime = 0f;
                tod = TimeOfDay.Dusk;
            }
        }
        else if (tod == TimeOfDay.Dusk)
        {
            if (currentTime >= duskLength)
            {
                currentTime = 0f;
                tod = TimeOfDay.Night;
            }
        }
        else if (tod == TimeOfDay.Night)
        {
            if (currentTime >= nightLength)
            {
                currentTime = 0f;
                tod = TimeOfDay.Dawn;
            }
        }
        else if (tod == TimeOfDay.Dawn)
        {
            if (currentTime >= dawnLength)
            {
                currentTime = 0f;
                tod = TimeOfDay.Day;
            }
        }

        if (tod == TimeOfDay.Day)
        {
            sun.color = Color.Lerp(dayColor, duskColor, currentTime / dayLength);
        }
        else if (tod == TimeOfDay.Dusk)
        {
            sun.color = Color.Lerp(duskColor, nightColor, currentTime / duskLength);
        }
        else if (tod == TimeOfDay.Night)
        {
            sun.color = Color.Lerp(nightColor, dawnColor, currentTime / nightLength);
        }
        else if (tod == TimeOfDay.Dawn)
        {
            sun.color = Color.Lerp(dawnColor, dayColor, currentTime / dawnLength);
        }
    }

    public bool IsNight()
    {
        return tod == TimeOfDay.Night;
    }

    public bool IsDay()
    {
        return tod == TimeOfDay.Day;
    }

    public bool IsDusk()
    {
        return tod == TimeOfDay.Dusk;
    }

    public bool IsDawn()
    {
        return tod == TimeOfDay.Dawn;
    }

}
