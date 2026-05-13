using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class MenuFooterController : MonoBehaviour
{
    private const float Duration = .3f;
    [Header("Components")]
    [SerializeField] private GameObject indicator;
    [SerializeField] private ButtonFooterController startSelected;
    [SerializeField] private List<ButtonFooterController> footerButtons;

    private ButtonFooterController selectedButton;
    private GameObject currentSelectedObject;
    private void Start()
    {
        if (startSelected != null)
        {
            OnButtonClickedEvent(startSelected);
        }
        else
        {
            indicator.SetActive(false);
        }
    }
    private void OnEnable()
    {
        foreach (ButtonFooterController btn in footerButtons)
        {
            btn.OnButtonClickedEvent.AddListener(OnButtonClickedEvent);
        }
    }
    private void OnDisable()
    {
        foreach (ButtonFooterController btn in footerButtons)
        {
            btn.OnButtonClickedEvent.RemoveListener(OnButtonClickedEvent);
        }
    }
    private void OnButtonClickedEvent(ButtonFooterController buttonClicked)
    {
        if (footerButtons.Contains(buttonClicked))
        {
            if (selectedButton == buttonClicked)
            {
                selectedButton = null;
                currentSelectedObject = null;

                UpdateSelectionVisuals();

                indicator.SetActive(false);
                return;
            }

            selectedButton = buttonClicked;

            UpdateSelectionVisuals();

            MoveIndicator();
        }
    }

    private void UpdateSelectionVisuals()
    {
        foreach (ButtonFooterController btn in footerButtons)
        {
            btn.SetSelect(selectedButton == btn);
        }
    }
    private void MoveIndicator()
    {
        if (selectedButton == null) return;
        if (currentSelectedObject == selectedButton.gameObject) return;

        currentSelectedObject = selectedButton.gameObject;

        indicator.SetActive(true);
        indicator.transform.DOKill();

        indicator.transform
            .DOMoveX(currentSelectedObject.transform.position.x, Duration)
            .SetEase(Ease.OutCubic)
            .OnComplete(() =>
            {
                indicator.transform.position = new Vector3(
                    currentSelectedObject.transform.position.x,
                    indicator.transform.position.y,
                    indicator.transform.position.z);
            });
    }
}