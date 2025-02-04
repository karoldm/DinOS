using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlanFile : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    private bool hasPriority;
    public Sprite imageNoPriority;
    public Sprite imagePriority;

    private bool GenerateRandomHasPriority()
    {
        return Random.Range(0, 2) == 1;
    }

    public void Initialize()
    {
        this.hasPriority = GenerateRandomHasPriority();

        Image imageComponent = GetComponent<Image>();

        if (imageComponent != null)
        {
            imageComponent.sprite = this.hasPriority ? this.imagePriority : this.imageNoPriority;
            Debug.Log("Priority: " + this.hasPriority);
            Debug.Log("Priority: " + imageComponent.sprite);
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Nenhum componente Image encontrado no GameObject!");
        }
    }

    public bool GetHasPriority()
    {
        return this.hasPriority;
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
    }

    public void OnDrag(PointerEventData eventData)
    {
    }
}
