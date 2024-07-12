using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RAMController : MonoBehaviour
{

    private static RAMController instance;

    public List<Task> tasks;
    public List<HorizontalLayoutGroup> queues;
    public List<HorizontalLayoutGroup> queuesComplete;
    public List<Dino> dinos;
    public List<Dest> dest;
    public TextMeshProUGUI scoreText;
    private int score = 0;


    public static RAMController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<RAMController>();

                if (instance == null)
                {
                    Debug.LogError("No instance of RAMController found in the scene.");
                }

            }
            return instance;
        }
    }

    public void UpdateScore(int taskScore)
    {
        this.score += taskScore;
        this.scoreText.text = this.score.ToString();
    }

    private int GetRandInt(int min, int max)
    {
        int minRange = min;
        int maxRange = max;
        int randomNumberInRange = UnityEngine.Random.Range(minRange, maxRange + 1);

        return randomNumberInRange;
    }

    void Start()
    {
       for(int i = 0; i < queues.Count; i++)
        {
            AddElementToQueue(i, GetRandInt(0, 2));
            AddElementToQueue(i, GetRandInt(0, 2));
            AddElementToQueue(i, GetRandInt(0, 2));
        }
    }


    void Update()
    {

    }

    public void AddElementToQueue(int indexQueue, int indexTask)
    {
        if (indexQueue < 0 || indexQueue >= queues.Count)
        {
            Debug.LogError("Índice inválido para HorizontalLayoutGroup");
            Debug.LogError("indexQueue: " + indexQueue.ToString());
            return;
        }

        if (indexTask < 0 || indexTask >= tasks.Count)
        {
            Debug.LogError("Índice inválido para HorizontalLayoutGroup");
            Debug.LogError("indexTask: " + indexTask.ToString());
            return;
        }

        Task lastChild = GetLastTaskOfQueue(indexQueue);

        Task task = Instantiate(tasks[indexTask], queues[indexQueue].transform);
        
        bool willHadNext = GetRandInt(0, 1) == 0 ? false : true;

        if (willHadNext)
        {
            if (lastChild != null)
            {
                lastChild.SetNext(task);
            }
        }

        task.InstanciateTask(indexTask, indexQueue);

        ForceRebuildLayoutQueue(indexQueue);

        task.UpdateStartPosition();
    }

    public void IterateQueue(Transform queueTransform, Action<Task, int> action)
    {
        if (queueTransform == null || action == null)
        {
            Debug.LogError("Queue ou action é null.");
            return;
        }

        for (int i = 0; i < queueTransform.childCount; i++)
        {
            Task task = queueTransform.GetChild(i).GetComponent<Task>();
            if (task != null)
            {
                action(task, i);
            }
        }
    }

    public void IterateQueue(Transform queueTransform, Action<Transform, int> action)
    {
        if (queueTransform == null || action == null)
        {
            Debug.LogError("Queue ou action é null.");
            return;
        }

        for (int i = 0; i < queueTransform.childCount; i++)
        {
            Transform child = queueTransform.GetChild(i);
            if (child != null)
            {
                action(child, i);
            }
        }
    }


    public void ForceRebuildLayoutQueue(int indexQueue)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(queues[indexQueue].GetComponent<RectTransform>());
    }

    public void RemoveChildOfQueue(int indexQueue, Task task)
    {
        int taskToAdd = 0;

        Task currentTask = task;
        while (currentTask != null)
        {
            Transform child = GetFirstChildOfQueue(indexQueue);
            if (child != null)
            {
                child.SetParent(queuesComplete[indexQueue].transform, false);
                taskToAdd++;
            }
            currentTask = currentTask.GetNext();
        }

        ForceRebuildLayoutQueue(indexQueue);

        IterateQueue(queues[indexQueue].transform, (Task task, int index) => {
            task.UpdateStartPosition();
        });

       for (int i = 0; i < taskToAdd; i++)
        {
            AddElementToQueue(indexQueue, GetRandInt(0, 2));
        }

    }

    private int GetQueueCount(int index)
    {
        return queues [index].transform.childCount;
    }

    public Transform GetLastChildOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        return queues[queueIndex].transform.GetChild(GetQueueCount(queueIndex) - 1);
    }

    public Transform GetFirstChildOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        return queues[queueIndex].transform.GetChild(0);
    }

    public Task GetLastTaskOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(GetQueueCount(queueIndex) - 1);

        return lastChildTransform.GetComponent<Task>();
    }

    public Task GetFirstTaskOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(0);

        return lastChildTransform.GetComponent<Task>();
    }

    public Task GetTaskAt(int queueIndex, int index)
    {
        int queueCount = GetQueueCount(queueIndex);
        if (queueCount <= 0 || index >= queueCount || index <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(index);

        return lastChildTransform.GetComponent<Task>();
    }

    public bool DestIsBusy(Task task)
    {
        return dest[task.GetIndexOfColor()].IsBusy();
    }
}
