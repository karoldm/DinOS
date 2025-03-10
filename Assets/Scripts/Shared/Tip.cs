using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Tip : MonoBehaviour, IPointerClickHandler
{
    public string text = "";
    public GameObject panel;
    private bool showing = false;

    private static List<Tip> allTips = new List<Tip>(); 

    void Start()
    {
        TextMeshProUGUI tipText = panel.GetComponentInChildren<TextMeshProUGUI>();
        tipText.text = this.text;
        allTips.Add(this);
    }

    void OnDestroy()
    {
        allTips.Remove(this);
    }

    void Update()
    {
        if (showing && Input.GetMouseButtonDown(0)) 
        {
            if (!IsPointerOverUIObject())
            {
                CloseAllTips();
            }
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        bool wasOpen = showing;
        CloseAllTips(); 

        if (!wasOpen)
        {
            showing = true;
            panel.SetActive(true);
        }
    }

    private static void CloseAllTips()
    {
        foreach (Tip tip in allTips)
        {
            tip.showing = false;
            tip.panel.SetActive(false);
        }
    }

    private bool IsPointerOverUIObject()
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);
        return results.Count > 0; 
    }
}
