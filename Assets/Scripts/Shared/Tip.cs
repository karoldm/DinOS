using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class Tip : MonoBehaviour, IPointerClickHandler
{
    public string text = "";
    public GameObject panel;
    private bool showing = false;

    void Start()
    {
        TextMeshProUGUI tipText = panel.GetComponentInChildren<TextMeshProUGUI>();
        tipText.text = this.text;

    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.showing = !this.showing;
        this.panel.SetActive(this.showing);
    }
}
