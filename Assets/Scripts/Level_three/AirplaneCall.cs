using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class AirplaneCall : MonoBehaviour, IPointerClickHandler
{
    private LevelThreeController controller;
    private string color;
    public AirplanePeriferic airplane;
    public TextMeshProUGUI label;
    private Vector3 scaleIncrement = new Vector3(0.1f, 0.1f, 0.1f);


    void Start()
    {

    }

    void Update()
    {

    }

    void Awake()
    {

        controller = LevelThreeController.Instance;

        if (controller == null)
        {
            Debug.LogError("LevelThreeController instance não encontrado na cena.");
        }
    }

    public void SetAirplane(AirplanePeriferic airplane)
    {
        this.airplane = airplane;
        if(label != null)
        {
            label.text = Priority.GetPriorityLabel(airplane.GetPriority());
        }
        SetColor(airplane.color);
    }

    private void UpdateCircleColor()
    {
        SpriteRenderer circle = transform.Find("Circle").GetComponent<SpriteRenderer>();
        if (circle != null)
        {
            if (ColorUtility.TryParseHtmlString(this.color, out Color outColor))
            {
                circle.color = outColor;
            }
            else
            {
                Debug.LogError("Invalid hex color string");
            }
        }
        else
        {
            Debug.LogError("Circle is null");
        }
    }

    public void SetColor(string color)
    {
        this.color = color;
        UpdateCircleColor();
    }

    public string GetColor()
    {
        return this.color;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.GetSelectedCall() == this)
        {
            transform.localScale -= scaleIncrement;
            controller.SetSelectedCall(null);
        }
        else
        {
            transform.localScale += scaleIncrement;
            controller.SetSelectedCall(this);
        }
    }
}
