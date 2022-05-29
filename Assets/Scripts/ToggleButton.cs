using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public class ToggleButton : MonoBehaviour, ILerpable
{
    public delegate void IsClickedEvent(bool isOn);
    public event IsClickedEvent Clicked;

    private bool isOn = false;

    [SerializeField]
    private Color offColor;
    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color circleColor;
    [SerializeField]
    public float _animationDuration;

    private bool isAnimating = false;

    private int mode = -1;

    private Button _button;
    private Image _buttonImage;
    private GameObject _buttonCircle;
    private Image _buttonCircleImage;

    void Start()
    {
        _button = GetComponent<Button>();
        _buttonImage = GetComponent<Image>();
        _buttonCircle = transform.GetChild(0).gameObject;

        _buttonCircleImage = _buttonCircle.GetComponent<Image>();

        _buttonImage.color = offColor;
        _buttonCircleImage.color = circleColor;

        _button.onClick.AddListener(StartAnimation);
    }
    /// <summary>
    /// Starts animating the circle of the button
    /// </summary>
    private void StartAnimation()
    {
        if (isAnimating) return;

        isOn = (mode == -1) ? true : false;
        
        Vector3 pos = _buttonCircle.transform.localPosition;

        StartCoroutine(LinearInterpolation(pos.x, -pos.x, _animationDuration));

        _buttonImage.color = (isOn) ? onColor : offColor;
        mode *= -1;

        if (Clicked != null)
        {
            Clicked(isOn);
        }
    }

    /// <summary>
    /// Starts the transition of the circle in the button
    /// </summary>
    /// <param name="startPosition"> The start position of the button</param>
    /// <param name="endPosition">The end position of the button</param>
    /// <param name="duration">The duration it takes to go from the start to end position</param>
    /// <returns></returns>

    public IEnumerator LinearInterpolation(float startvalue, float endvalue, float lerpDuration)
    {
        // https://gamedevbeginner.com/the-right-way-to-lerp-in-unity-with-examples/
        isAnimating = true;
        float timeElapsed = 0;
        while (timeElapsed < lerpDuration)
        {
            //changes the position of the circle over time
            _buttonCircle.transform.localPosition = new Vector3(Mathf.Lerp(startvalue, endvalue, timeElapsed / lerpDuration), 0, 0);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        // snaps the circle position to the end position
        _buttonCircle.transform.localPosition = new Vector2(endvalue, 0);
        isAnimating = false;
    }
}
