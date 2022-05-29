using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DigitalRuby.SimpleLUT;

public class BrightnessManager : SingletonPattern<BrightnessManager>
{
    public SimpleLUT lightInScene;

    /// <summary>
    /// Sets the mmount of the SimpleLUT in the scene
    /// </summary>
    /// <param name="val"> The value the amount of light in the scene should be set to</param>
    public void SetBrightnessLevel(float val)
    {
        lightInScene.Amount = val;
    }
}
