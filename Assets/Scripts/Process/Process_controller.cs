using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Process_controller : MonoBehaviour
{
    private static Process_controller instance;

    public List<Process_item> itens;
    public List<Slot_Process> slots;
    public List<int> queueExe;

    public Slot_Process_Exe slotProcessExe;
    public TextMeshProUGUI timeText;

    public Star starRR;
    public Star starSJF;
    public Star starFCFS;
    public Star starPriority;

    private List<Process_item> itensExecuted = new List<Process_item>();
    

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
        for(int i = 0; i < itensExecuted.Count; i++)
        {
            Vector2 position = slots[i].GetPosition();

            itensExecuted[i].transform.position = position;
            itensExecuted[i].startPosition = position;

            itensExecuted[i].SetTimeLeft(itensExecuted[i].timeToExecute);
            slots[i].process = itensExecuted[i];

            itensExecuted[i].Show();
        }

        itensExecuted.Clear();
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
                if (itens[i].ID != itensExecuted[itens.Count - i - 1].ID)
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

                List<float> times = itensExecuted[i].pauseExe;

                for (int j = 1; j < times.Count; j++) {
                    if (j == times.Count - 1 && times[j] > quantum) {
                        isRR = false;
                        break;
                    }
                    if (times[j] - times[j-1] != quantum)
                    {
                        isRR = false;
                        break;
                    }
                }

                if (!isRR) break;
            }

            // int loopCount = 0;
            for(int i = 0; i < queueExe.Count; i++)
            {
                Debug.Log("RR id queueExe " + queueExe[i]);
                /* for(int j = itens.Count - 1; j >= 0; j--)
                {
                    if (i >= queueExe.Count) break;

                    Debug.Log("RR id itens " + itens[j].ID);
                         
                    if (((itens[j].timeToExecute / quantum) - loopCount > 0) && (queueExe[i] != itens[j].ID)) 
                    {
                        isRR = false;
                        break;
                    }
                    else
                    {
                        i++;
                    }
                }
                i--;
                loopCount++; */
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

    public void ClearSlotExe()
    {
        slotProcessExe.currentItemExe = null;
    }

    public int GetFirstSlot() 
    {
        return (slots.Count - (slots.Count - itensExecuted.Count));
    }

    public void UpdateQueue(Process_item itemExe)
    {
        int index = slots.FindIndex(slot => slot.process != null && slot.process.ID == itemExe.ID);


        for (int i = index; i > 0; i--)
        {
            if (slots[i-1].process != null)
            {
                slots[i].SetSlot(slots[i - 1].process);
            }
        }

        int firstSlot = GetFirstSlot();

        itemExe.startPosition = slots[firstSlot].GetPosition();
        slots[firstSlot].process = null;
    }

    public void AddItemExecuted(Process_item item)
    {
        itensExecuted.Add(item);
        HandleItemFinished();
    }
}
