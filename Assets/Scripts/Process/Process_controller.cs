using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Process_controller : MonoBehaviour
{
    private static Process_controller instance;
    private bool requestAbort = false;

    public List<Process_item> itens;
    /*
     * Simula uma fila 
     * Considere que:
     * Incio da fila: primeiro item inserido, posi��o 0
     * Final da fila: �ltimo item inserido, posi��o count - 1
     */
    public List<Slot_Process> slots; 
    public List<int> queueExe;

    public Slot_Process_Exe slotProcessExe;
    public TextMeshProUGUI timeText;

    public Star starRR;
    public Star starSJF;
    public Star starFCFS;
    public Star starPriority;


    private List<Process_item> itensExecuted = new List<Process_item>();
    
    public void Start()
    {

    }

    public void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            this.requestAbort = true;
        }
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

            itensExecuted[i].SetTimeLeft(itens[i].timeToExecute);
            slots[i].process = itens[i];
            
            itens[i].Show();
        }

        itensExecuted.Clear();
        ClearRequestAbort();
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
            float quantum = 5;
            for (int i = 1; i < itensExecuted.Count; i++)
            {
                if (itensExecuted[i].pauseExe.Count == 0)
                {
                    isRR = false;
                    break;
                }

                List<int> times = itensExecuted[i].pauseExe;

                for (int j = 1; j < times.Count; j++) {

                    int difference = times[j - 1] - times[j];
                    if (difference != quantum && difference != quantum + 1 && difference != quantum - 1)
                    {
                        isRR = false;
                        break;
                    }
                }

                if (!isRR) break;
            }

            int[] order = { 1, 2, 3, 4, 1, 3, 4, 1, 3, 1 };
            if(order.Length != queueExe.Count)
            {
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
            }

            else if (isSJF)
            {
               starSJF.Unlock();
            }

            else if (isFCFS)
            {
                starFCFS.Unlock();
            }

            else if (isByPriority)
            {
                starPriority.Unlock();
            }

            ResetGame();
        }
    }

    public void UpdateTimeText(string text)
    {
        if (timeText != null)
        {
            timeText.text = text;
        }
    }

    public void UpdateProgressBar(float currentTime, float fullTime)
    {
        float progress = currentTime / fullTime;
        Progress_bar.Instance.UpdateProgressBar(progress);
    }

    public void ResetProgressBar()
    {
        Progress_bar.Instance.UpdateProgressBar(1);
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

    public bool GetRequestAbort()
    {
        return this.requestAbort;
    }

    public void ClearRequestAbort()
    {
        this.requestAbort = false;
    }
}
