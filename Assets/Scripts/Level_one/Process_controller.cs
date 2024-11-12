using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Process_controller : UserController
{
    private static Process_controller instance;
    public Toggle requestAbort;
    public List<Process_item> itens;
    /*
     * Simula uma fila 
     * Considere que:
     * Incio da fila: primeiro item inserido, posição 0
     * Final da fila: último item inserido, posição count - 1
     */
    public List<Slot_Process> slots; 
    public List<int> queueExe;

    public Slot_Process_Exe slotProcessExe;
    public Dialog dialog;
    public TextMeshProUGUI timeText;

    public Award starRR;
    public Award starSJF;
    public Award starFCFS;
    public Award starPriority;

    private List<Process_item> itensExecuted = new List<Process_item>();
    
    public void Start()
    {
        dialog.showDialog(Dialog.DialogType.intro);
    }

    public void Update()
    {

    }

    public static Process_controller Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<Process_controller>();

                if (instance == null)
                {
                    Debug.LogError("No instance of Process_controller found in the scene.");
                }

            }
            return instance;
        }
    }

    public bool CheckItensFinished()
    {
        return (itens.Count == itensExecuted.Count);
    }

    public void ResetGame()
    {
        for(int i = 0; i < itens.Count; i++)
        {
            Vector2 position = slots[i].GetPosition();

            itens[i].transform.position = position;
            itens[i].startPosition = position;
            itens[i].ResetTimeLeft();
            itens[i].Show();

            slots[i].process = itens[i];
        }

        itensExecuted.Clear();
        queueExe.Clear();
        ClearRequestAbort();
        timeText.text = string.Empty;
        this.requestAbort.interactable = true;
    }

    public void HandleItemFinished()
    {
        if(CheckItensFinished())
        {

            bool isFCFS = true;
            bool isSJF = true;
            bool isByPriority = true;
            bool isRR = true;

            // First Come First Serve
            for (int i = 0; i < itens.Count; i++)
            {
                if (itens[i].ID != itensExecuted[i].ID)
                {
                    isFCFS = false;
                    break;
                }
            }

            // Shortest Job First
            for (int i = 1; i < itensExecuted.Count; i++)
            {
                if (itensExecuted[i-1].timeToExecute > itensExecuted[i].timeToExecute)
                {
                    isSJF = false;
                    break;
                }
            }

            // Priority
            for (int i = 1; i < itensExecuted.Count; i++)
            {
                if (itensExecuted[i - 1].priority < itensExecuted[i].priority)
                {
                    isByPriority = false;
                    break;
                }
            }

            // Round Robin

            int[] order = { 1, 2, 3, 4, 1, 3, 4, 1, 3, 1 };

            if (order.Length != queueExe.Count)
            {
                Debug.Log("false");
                isRR = false;
            }
            else 
                for(int i = 0; i < queueExe.Count; i++) 
                {
                    if (queueExe[i] != order[i])
                    {
                        isRR = false;
                        break;
                    }
                }

            if (isRR)
            {
               starRR.Unlock();
               dialog.showDialog(Dialog.DialogType.RR);
               if (!user.levelOne.awards.Contains("RR")) {
                    user.levelOne.awards.Add("RR");
               }
            }

            else if (isSJF)
            {
               starSJF.Unlock();
               dialog.showDialog(Dialog.DialogType.SJF);
                if (!user.levelOne.awards.Contains("SJF"))
                {
                    user.levelOne.awards.Add("SJF");
                }
            }

            else if (isFCFS)
            {
               starFCFS.Unlock();
               dialog.showDialog(Dialog.DialogType.FCFS);
                if (!user.levelOne.awards.Contains("FCFS"))
                {
                    user.levelOne.awards.Add("FCFS");
                }
            }

            else if (isByPriority)
            {
               starPriority.Unlock();
               dialog.showDialog(Dialog.DialogType.BP);
                if (!user.levelOne.awards.Contains("BP"))
                {
                    user.levelOne.awards.Add("BP");
                }
            }
            else
            {
                dialog.showDialog(Dialog.DialogType.None);
            }
            ResetGame();
            UpdateUser();
        }
    }

    public void UpdateTimeText(string text)
    {
        if (timeText != null)
        {
            timeText.text = text;
        }
    }

    public void UpdateTimeColor(Color color)
    {
        timeText.color = color;
    }

    public void UpdateTimeSize(float size)
    {
        timeText.fontSize = size;
    }

    public void UpdateProgressBar(float currentTime, float fullTime)
    {
        float progress = currentTime / fullTime;
        Progress_bar.Instance.UpdateProgressBar(progress);
    }

    public void ResetProgressBar()
    {
        Progress_bar.Instance.UpdateProgressBar(0);
    }

    public void ClearSlotExe()
    {
        slotProcessExe.currentItemExe = null;
    }

    public int GetLastQueuePosition() 
    {
        return (slots.Count - itensExecuted.Count) - 1;
    }

    public Slot_Process GetFirstQueueSlot()
    {
        return slots[0];
    }

    public Process_item GetLastQueueProcess()
    {
        return slots[GetLastQueuePosition()].process;
    }

    public void UpdateQueue(Process_item itemExe)
    {
        int index = slots.FindIndex(slot => slot.process == null);

        for (int i = index; i < slots.Count; i++)
        {
            if (i + 1 < slots.Count && slots[i+1].process != null)
            {
                slots[i].SetSlot(slots[i + 1].process);
            }   
        }

        int lastSlot = GetLastQueuePosition();

        itemExe.startPosition = slots[lastSlot].GetPosition();
        slots[lastSlot].process = null;
    }

    public void AddItemExecuted(Process_item item)
    {
        itensExecuted.Add(item);
    }

    public int GetItensExecutedCount()
    {
        return this.itensExecuted.Count;
    }

    public bool GetRequestAbortValue()
    {
        if(this.requestAbort != null)
        {
            return this.requestAbort.isOn;
        }

        return false;
    }

    public void ClearRequestAbort()
    {
        this.requestAbort.isOn = false;
    }
}
