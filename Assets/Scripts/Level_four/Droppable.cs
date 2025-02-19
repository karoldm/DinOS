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
    public bool isSwapArea;

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

    private void PutPlanFile(PlanFile file)
    {
        file.transform.SetParent(transform);
        file.transform.localScale = new Vector3(0.4f, 0.5f, 1f);
        file.SetTimeToFetch(isSwapArea ? 1f : 3f);
        this.UpdateQueue();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (transform.childCount >= limit) return;

        Customer currentCustomer = controller.GetCurrentCustomer();
        if (currentCustomer != null)
        {
            if (currentCustomer.GetAction() == Customer.Action.READ) {
                controller.ComputeError();
                return;
            }
            PlanFile planFile = currentCustomer.GetPlanFile();
            this.PutPlanFile(planFile);

            controller.SetCurrentCustomer(null);
            controller.ComputeSuccess();
        }

        PlanFile selectedPlanFile = controller.GetSelectedPlanFile();

        if(selectedPlanFile != null)
        {
            this.PutPlanFile(selectedPlanFile);
            controller.SetSelectedPlanFile(null);
        }

        if (controller.GetStep() % 2 != 0)
        {
            controller.NextStepTutorial();
        }
    }


    private void UpdateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(GetComponent<RectTransform>());
    }
}
