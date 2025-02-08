using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlanFile : MonoBehaviour, IPointerClickHandler
{
    private bool hasPriority;
    public Sprite imageNoPriority;
    public Sprite imagePriority;
    private Image image;
    private ControllerLevelFour controller;
    private Vector3 scaleIncrement = new Vector3(0.1f, 0.1f, 0.1f);

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }
    }

    private bool GenerateRandomHasPriority()
    {
        return Random.Range(0, 2) == 1;
    }

    public void Initialize()
    {
        this.hasPriority = GenerateRandomHasPriority();

        this.image = GetComponent<Image>();

        if (this.image != null)
        {
            this.image.sprite = this.hasPriority ? this.imagePriority : this.imageNoPriority;
            Debug.Log("Priority: " + this.hasPriority);
            Debug.Log("Priority: " + this.image.sprite);
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

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.SetSelectedPlanFile(this);
        transform.localScale += scaleIncrement;
    }
}
