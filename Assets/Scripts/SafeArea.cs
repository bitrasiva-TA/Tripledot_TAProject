using UnityEngine;

/// <summary>
/// Safe area implementation for notched mobile devices.
/// </summary>
[RequireComponent(typeof(RectTransform))]
public class SafeArea : MonoBehaviour
{
    #region Simulation

    /// <summary>
    /// Simulation device for editor testing.
    /// </summary>
    public enum SimDevice
    {
        None,
        Custom,
        iPhoneX,
        iPhoneXsMax,
        Pixel3XL_LSL,
        Pixel3XL_LSR
    }

    [Header("Simulation")]
    [SerializeField] private SimDevice simulationDevice = SimDevice.None;

    /// <summary>
    /// Default custom safe area profile (same as iPhone X).
    /// </summary>
    public static Rect[] CustomSafeArea =
    {
        new Rect(0f, 102f / 2436f, 1f, 2202f / 2436f),
        new Rect(132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)
    };

    private readonly Rect[] iPhoneXSafeArea =
    {
        new Rect(0f, 102f / 2436f, 1f, 2202f / 2436f),
        new Rect(132f / 2436f, 63f / 1125f, 2172f / 2436f, 1062f / 1125f)
    };

    private readonly Rect[] iPhoneXsMaxSafeArea =
    {
        new Rect(0f, 102f / 2688f, 1f, 2454f / 2688f),
        new Rect(132f / 2688f, 63f / 1242f, 2424f / 2688f, 1179f / 1242f)
    };

    private readonly Rect[] pixel3XLSafeAreaLeft =
    {
        new Rect(0f, 0f, 1f, 2789f / 2960f),
        new Rect(0f, 0f, 2789f / 2960f, 1f)
    };

    private readonly Rect[] pixel3XLSafeAreaRight =
    {
        new Rect(0f, 0f, 1f, 2789f / 2960f),
        new Rect(171f / 2960f, 0f, 2789f / 2960f, 1f)
    };

    #endregion

    #region Inspector

    [Header("Safe Area Settings")]
    [SerializeField]
    [Tooltip("Apply safe area on the horizontal axis.")]
    private bool conformX = true;

    [SerializeField]
    [Tooltip("Apply safe area on the vertical axis.")]
    private bool conformY = true;

    [SerializeField]
    [Tooltip("Enable debug logging.")]
    private bool enableLogging = false;

    #endregion

    #region Private Fields

    private RectTransform panel;
    private Rect lastSafeArea = new Rect(0, 0, 0, 0);
    private Vector2Int lastScreenSize = Vector2Int.zero;
    private ScreenOrientation lastOrientation = ScreenOrientation.AutoRotation;

    #endregion

    #region Unity Methods

    private void Awake()
    {
        panel = GetComponent<RectTransform>();
        Refresh();
    }

    private void Update()
    {
        // Polling is intentional because safe area and orientation
        // updates are inconsistent across some mobile devices.
        Refresh();
    }

    #endregion

    #region Core Logic

    private void Refresh()
    {
        Rect safeArea = GetSafeArea();

        bool hasScreenChanged =
            safeArea != lastSafeArea ||
            Screen.width != lastScreenSize.x ||
            Screen.height != lastScreenSize.y ||
            Screen.orientation != lastOrientation;

        if (!hasScreenChanged)
        {
            return;
        }

        lastScreenSize.x = Screen.width;
        lastScreenSize.y = Screen.height;
        lastOrientation = Screen.orientation;

        ApplySafeArea(safeArea);
    }

    private Rect GetSafeArea()
    {
        Rect safeArea = Screen.safeArea;

#if UNITY_EDITOR
        if (simulationDevice != SimDevice.None)
        {
            Rect simulatedSafeArea = GetSimulatedSafeArea(simulationDevice);

            safeArea = new Rect(
                Screen.width * simulatedSafeArea.x,
                Screen.height * simulatedSafeArea.y,
                Screen.width * simulatedSafeArea.width,
                Screen.height * simulatedSafeArea.height
            );
        }
#endif

        return safeArea;
    }

    private Rect GetSimulatedSafeArea(SimDevice device)
    {
        switch (device)
        {
            case SimDevice.Custom:
                return GetOrientationSafeArea(CustomSafeArea);

            case SimDevice.iPhoneX:
                return GetOrientationSafeArea(iPhoneXSafeArea);

            case SimDevice.iPhoneXsMax:
                return GetOrientationSafeArea(iPhoneXsMaxSafeArea);

            case SimDevice.Pixel3XL_LSL:
                return GetOrientationSafeArea(pixel3XLSafeAreaLeft);

            case SimDevice.Pixel3XL_LSR:
                return GetOrientationSafeArea(pixel3XLSafeAreaRight);

            default:
                return new Rect(0f, 0f, 1f, 1f);
        }
    }

    private Rect GetOrientationSafeArea(Rect[] safeAreas)
    {
        bool isPortrait = Screen.height > Screen.width;
        return isPortrait ? safeAreas[0] : safeAreas[1];
    }

    private void ApplySafeArea(Rect safeArea)
    {
        lastSafeArea = safeArea;

        if (!conformX)
        {
            safeArea.x = 0f;
            safeArea.width = Screen.width;
        }

        if (!conformY)
        {
            safeArea.y = 0f;
            safeArea.height = Screen.height;
        }

        if (Screen.width <= 0 || Screen.height <= 0)
        {
            return;
        }

        Vector2 anchorMin = safeArea.position;
        Vector2 anchorMax = safeArea.position + safeArea.size;

        anchorMin.x /= Screen.width;
        anchorMin.y /= Screen.height;

        anchorMax.x /= Screen.width;
        anchorMax.y /= Screen.height;

        // Samsung device safety check.
        if (anchorMin.x < 0 || anchorMin.y < 0 ||
            anchorMax.x < 0 || anchorMax.y < 0)
        {
            return;
        }

        panel.anchorMin = anchorMin;
        panel.anchorMax = anchorMax;

        if (enableLogging)
        {
            Debug.LogFormat(
                "Safe area applied to {0}: x={1}, y={2}, w={3}, h={4} | Screen: {5}x{6}",
                name,
                safeArea.x,
                safeArea.y,
                safeArea.width,
                safeArea.height,
                Screen.width,
                Screen.height
            );
        }
    }

    #endregion
}