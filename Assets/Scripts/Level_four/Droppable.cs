using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Droppable : MonoBehaviour, IPointerClickHandler
{
    private ControllerLevelFour controller;
    public int limit;


    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount >= limit) return;

        Customer currentCustomer = controller.GetCurrentCustomer();
        if (currentCustomer != null && currentCustomer.GetAction() == Customer.Action.WRITE)
        {
            PlanFile planFile = Instantiate(currentCustomer.GetPlanFile(), transform);
            this.UpdateQueue();

            controller.SetCurrentCustomer(null);
        }
    }


    private void UpdateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
