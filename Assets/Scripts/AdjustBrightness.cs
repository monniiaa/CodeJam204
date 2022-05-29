using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class AdjustBrightness : MonoBehaviour
{
    [SerializeField]
    Slider slider;
    SensorManager sensorManager;
    [SerializeField]
    private ToggleButton toggleButton;
    private bool isOn = false;
    BrightnessManager brightnessManager;
    readonly float tresh = 0.5f;
    readonly float lightval = 0.007f;

    private void Start()
    {
        sensorManager = SensorManager.Instance;
        brightnessManager = BrightnessManager.Instance;
        sensorManager.OnAcceleration += AdjustSliderBrightness;
        toggleButton.Clicked += OnToggleButtonClicked;

    }

    private void Update()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    private void OnDisable()
    {
        sensorManager.OnAcceleration -= AdjustSliderBrightness;
        toggleButton.Clicked -= OnToggleButtonClicked;
    }
    
    /// <summary>
    /// If the button is clicked it sets isOn to true
    /// </summary>
    /// <param name="isOn">The variable of the toggle button</param>
    void OnToggleButtonClicked(bool isOn)
    {
        this.isOn = isOn;       
    }

    /// <summary>
    /// Adjusts the brightness of the screen depending on the slider value. If isOn is true
    /// brightness is dependent on the accelrometer
    /// </summary>
    /// <param name="vec">The acceleration of the device</param>
    void AdjustSliderBrightness(Vector3 vec)
    {
        if (isOn) {
            if (vec.x > tresh)
            {
                slider.value += lightval;
                brightnessManager.SetBrightnessLevel(slider.value);
            } 
            if (vec.x < -0.5)
            {
                slider.value -= lightval;
                brightnessManager.SetBrightnessLevel(slider.value);
            }
        } else {
            brightnessManager.SetBrightnessLevel(slider.value);
        }
    }
}
