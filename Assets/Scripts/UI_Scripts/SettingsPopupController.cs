using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ToggleControl
{
    public Slider        slider;
    public RectTransform switchButtonRect;
    public Image         background;
    public float         travelDistance = 50f;

    [HideInInspector] public bool      isOn;
    [HideInInspector] public Vector2   offPosition;
    [HideInInspector] public Vector2   onPosition;
    [HideInInspector] public Coroutine routine;
}

public class SettingsPopupController : MonoBehaviour
{
    private const float ToggleAnimationDuration = 0.25f;

    [SerializeField] private Animator animator;

    [Header("Toggle Colors")]
    [SerializeField] public Color toggleOnColor  = new Color(0.541f, 0.835f, 0.059f, 1f);
    [SerializeField] public Color toggleOffColor = new Color(0.75f,  0.75f,  0.75f,  1f);

    [Header("Toggles")]
    [SerializeField] private ToggleControl sound;
    [SerializeField] private ToggleControl music;
    [SerializeField] private ToggleControl vibration;
    [SerializeField] private ToggleControl notification;

    private void Awake()
    {
        DisableSlider(sound.slider);
        DisableSlider(music.slider);
        DisableSlider(vibration.slider);
        DisableSlider(notification.slider);
    }

    private void Start()
    {
        StartCoroutine(CacheAllPositions());
    }

    private void DisableSlider(Slider slider)
    {
        if (slider == null) return;
        slider.interactable = false;
        Animator sliderAnimator = slider.GetComponent<Animator>();
        if (sliderAnimator != null)
            sliderAnimator.enabled = false;
    }

    private IEnumerator CacheAllPositions()
    {
        yield return null;
        CacheTogglePosition(sound);
        CacheTogglePosition(music);
        CacheTogglePosition(vibration);
        CacheTogglePosition(notification);
    }

    private void CacheTogglePosition(ToggleControl toggle)
    {
        toggle.offPosition      = toggle.switchButtonRect.anchoredPosition;
        toggle.onPosition       = toggle.offPosition + new Vector2(toggle.travelDistance, 0f);
        toggle.background.color = toggleOffColor;
    }

    public void OnCloseButtonClicked()
    {
        animator.SetTrigger("Close");
    }

    // Method called through animation event when the close animation is completed
    public void OnClosedAnimationCompleted()
    {
        gameObject.SetActive(false);
    }

    /// <summary>Wire to the Sound toggle Background Button's OnClick.</summary>
    public void OnSoundToggleClicked()        => ClickToggle(sound);

    /// <summary>Wire to the Music toggle Background Button's OnClick.</summary>
    public void OnMusicToggleClicked()        => ClickToggle(music);

    /// <summary>Wire to the Vibration toggle Background Button's OnClick.</summary>
    public void OnVibrationToggleClicked()    => ClickToggle(vibration);

    /// <summary>Wire to the Notification toggle Background Button's OnClick.</summary>
    public void OnNotificationToggleClicked() => ClickToggle(notification);

    private void ClickToggle(ToggleControl toggle)
    {
        toggle.isOn = !toggle.isOn;
        if (toggle.routine != null)
            StopCoroutine(toggle.routine);
        toggle.routine = StartCoroutine(AnimateToggle(toggle));
    }

    private IEnumerator AnimateToggle(ToggleControl toggle)
    {
        Vector2 startPos    = toggle.switchButtonRect.anchoredPosition;
        Vector2 targetPos   = toggle.isOn ? toggle.onPosition  : toggle.offPosition;
        Color   startColor  = toggle.background.color;
        Color   targetColor = toggle.isOn ? toggleOnColor : toggleOffColor;
        float   elapsed     = 0f;

        while (elapsed < ToggleAnimationDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / ToggleAnimationDuration);

            toggle.switchButtonRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            toggle.background.color                  = Color.Lerp(startColor, targetColor, t);

            yield return null;
        }

        toggle.switchButtonRect.anchoredPosition = targetPos;
        toggle.background.color                  = targetColor;
        toggle.routine                           = null;
    }
}
