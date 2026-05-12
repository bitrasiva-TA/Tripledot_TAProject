using System.Collections;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class ToggleControl
{
    public Slider slider;
    public RectTransform switchButtonRect;
    public Image background;
    public float travelDistance = 50f;

    [HideInInspector] public bool isOn;
    [HideInInspector] public Vector2 offPosition;
    [HideInInspector] public Vector2 onPosition;
    [HideInInspector] public Coroutine routine;
}

public class SettingsPopupController : MonoBehaviour
{
    private const float TOGGLE_ANIMATION_DURATION = 0.25f;

    private const string SOUND_PREF_KEY = "SoundEnabled";
    private const string MUSIC_PREF_KEY = "MusicEnabled";
    private const string VIBRATION_PREF_KEY = "VibrationEnabled";
    private const string NOTIFICATION_PREF_KEY = "NotificationEnabled";

    [SerializeField] private Animator animator;

    [Header("Toggle Colors")]
    [SerializeField] private Color toggleOnColor = new Color(0.541f, 0.835f, 0.059f, 1f);
    [SerializeField] private Color toggleOffColor = new Color(0.75f, 0.75f, 0.75f, 1f);
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
        StartCoroutine(InitializeToggles());
    }
    private void DisableSlider(Slider slider)
    {
        if (slider == null) return;
        slider.interactable = false;
        Animator sliderAnimator = slider.GetComponent<Animator>();
        if (sliderAnimator != null)
        {
            sliderAnimator.enabled = false;
        }
    }
    private IEnumerator InitializeToggles()
    {
        yield return null;
        CacheTogglePosition(sound);
        CacheTogglePosition(music);
        CacheTogglePosition(vibration);
        CacheTogglePosition(notification);
        LoadToggleStates();
    }

    private void CacheTogglePosition(ToggleControl toggle)
    {
        toggle.offPosition = toggle.switchButtonRect.anchoredPosition;
        toggle.onPosition = toggle.offPosition + new Vector2(toggle.travelDistance, 0f);
    }
    private void LoadToggleStates()
    {
        SetToggleState(sound,PlayerPrefs.GetInt(SOUND_PREF_KEY, 1) == 1);
        SetToggleState(music,PlayerPrefs.GetInt(MUSIC_PREF_KEY, 1) == 1);
        SetToggleState(vibration,PlayerPrefs.GetInt(VIBRATION_PREF_KEY, 1) == 1);
        SetToggleState(notification,PlayerPrefs.GetInt(NOTIFICATION_PREF_KEY, 1) == 1);
    }
    private void SetToggleState(ToggleControl toggle, bool isOn)
    {
        toggle.isOn = isOn;
        toggle.switchButtonRect.anchoredPosition = isOn ? toggle.onPosition : toggle.offPosition;
        toggle.background.color = isOn ? toggleOnColor : toggleOffColor;
    }
    public void OnCloseButtonClicked()
    {
        animator.SetTrigger("Close");
    }
    public void OnClosedAnimationCompleted()
    {
        gameObject.SetActive(false);
    }
    public void OnSoundToggleClicked()
    {
        ClickToggle(sound, SOUND_PREF_KEY);
    }
    public void OnMusicToggleClicked()
    {
        ClickToggle(music, MUSIC_PREF_KEY);
    }
    public void OnVibrationToggleClicked()
    {
        ClickToggle(vibration, VIBRATION_PREF_KEY);
    }
    public void OnNotificationToggleClicked()
    {
        ClickToggle(notification, NOTIFICATION_PREF_KEY);
    }
    private void ClickToggle(ToggleControl toggle, string prefKey)
    {
        toggle.isOn = !toggle.isOn; PlayerPrefs.SetInt(prefKey, toggle.isOn ? 1 : 0); PlayerPrefs.Save();
        if (toggle.routine != null)
        {
            StopCoroutine(toggle.routine);
        }
        toggle.routine = StartCoroutine(AnimateToggle(toggle));
    }
    private IEnumerator AnimateToggle(ToggleControl toggle)
    {
        Vector2 startPos = toggle.switchButtonRect.anchoredPosition;
        Vector2 targetPos = toggle.isOn ? toggle.onPosition : toggle.offPosition;
        Color startColor = toggle.background.color;
        Color targetColor = toggle.isOn ? toggleOnColor : toggleOffColor;
        float elapsed = 0f;

        while (elapsed < TOGGLE_ANIMATION_DURATION)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.SmoothStep(0f, 1f, elapsed / TOGGLE_ANIMATION_DURATION);
            toggle.switchButtonRect.anchoredPosition = Vector2.Lerp(startPos, targetPos, t);
            toggle.background.color = Color.Lerp(startColor, targetColor, t);
            yield return null;
        }

        toggle.switchButtonRect.anchoredPosition = targetPos;
        toggle.background.color = targetColor;
        toggle.routine = null;
    }
}