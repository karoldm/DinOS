using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class AwardTooltip : MonoBehaviour, IPointerClickHandler
{
    public Award award;
    public string descriptionText;
    public GameObject modalTooltip;
    public GameObject modalDescription;
    public TextMeshProUGUI descriptionTextObject;
    private bool modalTooltipOpen = false;

    void Start()
    {
        descriptionTextObject.text = this.descriptionText;
    }


    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    void OnMouseDown()
    {
        if (this.award.IsLocked())
        {
            modalTooltipOpen = !modalTooltipOpen;
            modalTooltip.SetActive(modalTooltipOpen);
            if (modalTooltipOpen)
            {
                SetTooltipPosition(modalTooltip, Input.mousePosition);
            }
        }
        else
        {
            modalDescription.SetActive(true);
        }
    }

    private void SetTooltipPosition(GameObject tooltip, Vector2 position)
    {
        RectTransform tooltipRect = tooltip.GetComponent<RectTransform>();

        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            tooltipRect.parent as RectTransform,
            position,
            null,
            out localPoint
        );

        tooltipRect.localPosition = localPoint;
    }

    public void CloseModalDescription()
    {
        this.modalDescription.SetActive(false);
    }
}
