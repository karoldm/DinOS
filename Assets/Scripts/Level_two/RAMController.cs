using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RAMController : MonoBehaviour
{

    private static RAMController instance;

    public List<Task> tasks;
    public List<HorizontalLayoutGroup> queues;
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
        int randomNumberInRange = Random.Range(minRange, maxRange + 1);

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

        Task task = Instantiate(tasks[indexTask], queues[indexQueue].transform);

        task.InstanciateTask(indexTask, indexQueue);

        ForceRebuildLayoutQueue(indexQueue);

        task.UpdateStartPosition();
    }

    public void ForceRebuildLayoutQueue(int indexQueue)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(queues[indexQueue].GetComponent<RectTransform>());

    }

    public void RemoveChildOfQueue(int queueIndex)
    {
        Transform child = GetFirstChildOfQueue(queueIndex);
        Destroy(child.gameObject);
        ForceRebuildLayoutQueue(queueIndex);

        AddElementToQueue(queueIndex, GetRandInt(0, 2));
    }

    private int GetQueueCount(int index)
    {
        return queues [index].transform.childCount;
    }

    public Transform GetLastChildOfQueue(int queueIndex)
    {
        return queues[queueIndex].transform.GetChild(GetQueueCount(queueIndex) - 1);
    }

    public Transform GetFirstChildOfQueue(int queueIndex)
    {
        return queues[queueIndex].transform.GetChild(0);
    }

    public bool DestIsBusy(Task task)
    {
        return dest[task.GetIndexOfColor()].IsBusy();
    }
}
