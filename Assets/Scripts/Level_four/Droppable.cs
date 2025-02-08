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
            PlanFile planFile = currentCustomer.GetPlanFile();
            planFile.transform.SetParent(transform);
            planFile.transform.localScale = new Vector3(0.4f, 0.5f, 1f);
            this.UpdateQueue();

            controller.SetCurrentCustomer(null);
        }

        PlanFile selectedPlanFile = controller.GetSelectedPlanFile();
        if(selectedPlanFile != null)
        {
            selectedPlanFile.transform.SetParent(transform);
            selectedPlanFile.transform.localScale = new Vector3(0.4f, 0.5f, 1f);
            this.UpdateQueue();

            controller.SetSelectedPlanFile(null);
        }
    }


    private void UpdateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
