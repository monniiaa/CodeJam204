using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsensorAdjuster: MonoBehaviour, ILerpable
{
    BrightnessManager brightnessManager;
    SensorManager SensorManager;

    float valueToLerp;
    bool isAnimating = false;
    float minLightLevel = 10;
    float lerpDuration = 10;
    float minBrightness = 0;
    float maxBrightness = 1;


    // Start is called before the first frame update
    void Start()
    {
        brightnessManager = BrightnessManager.Instance;
        SensorManager = SensorManager.Instance;
        SensorManager.OnLightLevel += AdjustBrightness;

    }

    /// <summary>
    /// Turns the brightness of the light up if the lightlevel from the lightsensor is below 10
    /// </summary>
    /// <param name="val">The lightlevel detected from the lightsensor</param>
    void AdjustBrightness(float val)
    {
            if(val < minLightLevel && !isAnimating)
            {
                float startvalue = brightnessManager.lightInScene.Amount;
                float endvalue = maxBrightness;
                StartCoroutine(LinearInterpolation(startvalue, endvalue, lerpDuration));
            }
            else if(!isAnimating && brightnessManager.lightInScene.Amount == maxBrightness)
            {
                float startvalue = brightnessManager.lightInScene.Amount;
                float endvalue = minBrightness;
                StartCoroutine(LinearInterpolation(startvalue, endvalue, lerpDuration));
            }
    }

    /// <summary>
    /// Sets the brightness level over a certain duration
    /// </summary>
    /// <param name="startvalue">The start value of the brightness</param>
    /// <param name="endvalue">The end value of the brightness</param>
    /// <param name="lerpDuration">The duartion of the lerp</param>
    /// <returns></returns>
    public IEnumerator LinearInterpolation(float startvalue, float endvalue, float lerpDuration)
    {
        isAnimating = true;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            valueToLerp = Mathf.Lerp(startvalue, endvalue, timeElapsed / lerpDuration);
            brightnessManager.SetBrightnessLevel(valueToLerp);
            timeElapsed += 1;

            yield return null;
        }
        valueToLerp = endvalue;
        brightnessManager.SetBrightnessLevel(valueToLerp);
        isAnimating = false;
    }
}
