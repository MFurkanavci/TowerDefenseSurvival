using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    public float dayLength = 120f;
    public float nightLength = 120f;
    public float duskLength = 10f;
    public float dawnLength = 10f;

    public Light sun;

    public Color dayColor;
    public Color nightColor;
    public Color duskColor;
    public Color dawnColor;

    private Color targetColor;
    private float targetTime;
    private IEnumerator dayNightCoroutine;

    public static event Action OnNight;
    public static event Action OnDay;

    public static DayNightCycle Instance { get; private set; }

    private void Start()
    {  
        Instance = this;
        targetColor = dayColor;
        targetTime = dayLength;
        StartDayNightCycle();
    }

    private void StartDayNightCycle()
    {
        dayNightCoroutine = DayNightCycleCoroutine();
        StartCoroutine(dayNightCoroutine);
    }

    private IEnumerator DayNightCycleCoroutine()
    {
        while (true)
        {
            yield return LerpColor(sun.color, targetColor, targetTime);
            SwitchToNextTimeOfDay();
        }
    }

    private IEnumerator LerpColor(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;
        while (elapsedTime < duration)
        {
            sun.color = Color.Lerp(startColor, endColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        sun.color = endColor;
    }

    private void SwitchToNextTimeOfDay()
    {
        if (targetColor == dayColor)
        {
            targetColor = duskColor;
            targetTime = duskLength;
            OnNight?.Invoke();
        }
        else if (targetColor == duskColor)
        {
            targetColor = nightColor;
            targetTime = nightLength;
        }
        else if (targetColor == nightColor)
        {
            targetColor = dawnColor;
            targetTime = dawnLength;
            OnDay?.Invoke();
        }
        else if (targetColor == dawnColor)
        {
            targetColor = dayColor;
            targetTime = dayLength;
        }
    }


        /*day: #87CEEB (Sky Blue) .

        Night: #000033 (Midnight Blue) -

        Dusk: #FFA500 (Orange)

        Dawn: #FFC0CB  */
        
        
    public bool IsNight()
    {
        return targetColor == nightColor;
    }

    public bool IsDay()
    {
        return targetColor == dayColor;
    }

    public bool IsDusk()
    {
        return targetColor == duskColor;
    }

    public bool IsDawn()
    {
        return targetColor == dawnColor;
    }
}
