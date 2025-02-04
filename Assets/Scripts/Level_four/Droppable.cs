using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.EventSystems;

public class Droppable : MonoBehaviour, IPointerClickHandler
{
    private ControllerLevelFour controller;
    public VerticalLayoutGroup queue;


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
        Customer currentCustomer = controller.GetCurrentCustomer();
        if (currentCustomer != null)
        {
            PlanFile planFile = Instantiate(currentCustomer.GetPlanFile(), queue.transform);
            this.UpdateQueue();

            controller.SetCurrentCustomer(null);
        }
    }


    private void UpdateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(queue.GetComponent<RectTransform>());
    }
}
