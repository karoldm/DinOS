using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;


public class Slot_Process_Exe : MonoBehaviour, IDropHandler
{
    Process_controller processController;

    public Process_item currentItemExe;

    private bool isOverSlot;

    private static Slot_Process_Exe instance;
    public static Slot_Process_Exe Instance => instance;

    void Awake()
    {
        processController = Process_controller.Instance;

        if (processController == null)
        {
            Debug.LogError("Process_controller instance not found in the scene.");
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
    }

    void Start()
    {
        currentItemExe = null;
    }

    void Update()
    {

        if (currentItemExe != null && currentItemExe.isExe)
        {
            if (currentItemExe.GetTimeLeft() > 0)
            {
                currentItemExe.DecreaseTimeLeft(Time.deltaTime);
                processController.UpdateTimeText(currentItemExe.GetTimeLeft().ToString("F0"));
                processController.UpdateProgressBar(currentItemExe.GetTimeLeft(), currentItemExe.timeToExecute);

                // Request to abort exe of the current process
                if (processController.GetRequestAbort() &&
                    Math.Round(currentItemExe.GetTimeLeft()%5) == 0 && 
                    currentItemExe.GetTimeLeft() != currentItemExe.timeToExecute
                   ) 
                {
                    currentItemExe.AbortExe();
                }
            }
            else
            {
                processController.AddItemExecuted(currentItemExe);
                processController.queueExe.Add(currentItemExe.ID);
                currentItemExe.AddPauseTime();

                processController.UpdateTimeText("");
                currentItemExe.isExe = false;
                currentItemExe.Hide();
                currentItemExe = null;

                processController.HandleItemFinished();
            }
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        Process_item draggedItem = eventData.pointerDrag.GetComponent<Process_item>();

        if (currentItemExe == null && draggedItem != null)
        {
            currentItemExe = draggedItem;
            currentItemExe.transform.position = transform.position;
            currentItemExe.isExe = true;

            processController.UpdateTimeText(currentItemExe.GetTimeLeft().ToString("F0"));

            Slot_Process slotToClear = processController.slots.Find(slot => slot.process != null && slot.process.ID == currentItemExe.ID);
            if(slotToClear != null)
            {
                slotToClear.Clear();
            }

            processController.UpdateQueue(currentItemExe);
        }
    }
}
