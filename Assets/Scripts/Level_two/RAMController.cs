using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class RAMController : UserController
{

    private static RAMController instance;
    private int score = 0;

    public List<AirportTask> tasks;
    public List<HorizontalLayoutGroup> queues;
    public List<HorizontalLayoutGroup> queuesComplete;
    public List<Dino> dinos;
    public List<Dest> dests;
    public TextMeshProUGUI scoreText;
    public Award awardDeadlock;
    private bool hasSegmentation = false;
    public Award awardSegmentation;
    public DialogLevelTwo dialog;
    public int maxScore = 15;
    private float leftTime = 60f;
    public TextMeshProUGUI timeText;
    bool gamingRunning = false;
    public GameObject timeContainer;
    public GameObject startButton;

    public GameObject tutorial1;
    public GameObject tutorial2;
    public GameObject tutorial3;

    void Start()
    {
        dialog.showDialog(DialogLevelTwo.DialogType.intro);
        AddInitialTasks();

        if (user.levelTwo.awards.Contains("SEGMENTATION"))
        {
            awardSegmentation.Unlock();
        }

        if (user.levelTwo.awards.Contains("DEADLOCK"))
        {
            awardDeadlock.Unlock();
        }
        this.scoreText.text = user.levelTwo.score.ToString();
    }

    public bool GamingRunning()
    {
        return this.gamingRunning;
    }

    public void SetHasSegmentation()
    {
        this.hasSegmentation = true;
    }

    private void AddInitialTasks()
    {
   
        if (user.levelTwo.firstTime)
        {
            AddElementToQueue(0, 0, 10);
            AddElementToQueue(1, 1, 5);
            AddElementToQueue(2, 2, 15);
        }

       for (int i = 0; i < queues.Count; i++)
       {
            AddElementToQueue(i, GetRandInt(0, 2));
            AddElementToQueue(i, GetRandInt(0, 2));

            if(!user.levelTwo.firstTime)
            {
                AddElementToQueue(i, GetRandInt(0, 2));
            }
       }
    }

    void FinishGame()
    {
        if (awardSegmentation.IsLocked() && !this.hasSegmentation)
        {
            dialog.showDialog(DialogLevelTwo.DialogType.segmentation);
            awardSegmentation.Unlock();
            if (!user.levelTwo.awards.Contains("SEGMENTATION"))
            {
                user.levelTwo.awards.Add("SEGMENTATION");
            }
        }

        user.levelTwo.score = score;
        user.levelTwo.firstTime = false;
        UpdateUser();

        timeContainer.SetActive(false);
        startButton.SetActive(true);

        Reset();
    }


    void Update()
    {
        foreach (Dino dino1 in dinos)
        {
            foreach (Dino dino2 in dinos)
            {
                if (dino1 != dino2 && dino1.IsAwaiting() && dino2.IsAwaiting() &&
                    dino1.GetDest() == dino2.GetNextDest() && dino2.GetDest() == dino1.GetNextDest())
                {
                    Deadlock();
                }
            }
        }

        if (gamingRunning)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                timeText.text = Mathf.Round(leftTime).ToString();
            }
            else
            {
                FinishGame();
                gamingRunning = false;
            }
        }
    }

    public void InitGame()
    {
        gamingRunning = true;
        this.scoreText.text = "0";
        this.score = 0;

        timeContainer.SetActive(true);
        startButton.SetActive(false);

        this.ShowTutorial();
    }

    public void ShowTutorial()
    {
        if (user.levelTwo.firstTime)
        {
            tutorial1.SetActive(true);
            tutorial2.SetActive(true);
            tutorial3.SetActive(true);
        }
    }

    public void HiddenTutorial(int index)
    {
        if(index == 2)
        {
            tutorial1.SetActive(false);

        } 
        else if(index == 1)
        {
            tutorial2.SetActive(false);
            
        }
        else
        {
            tutorial3.SetActive(false);
        }
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Home");

    }

    private void Deadlock()
    {
        if (awardDeadlock.IsLocked())
        {
            dialog.showDialog(DialogLevelTwo.DialogType.dinnerProblem);
            awardDeadlock.Unlock();
            if (!user.levelTwo.awards.Contains("DEADLOCK"))
            {
                user.levelTwo.awards.Add("DEADLOCK");
            }
            UpdateUser();
        }
        Reset();
    }

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
        return UnityEngine.Random.Range(min, max + 1);
    }


    public void AddElementToQueue(int indexQueue, int indexTask, int? score = null)
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

        AirportTask firstChild = GetFirstTaskOfQueue(indexQueue);
        AirportTask lastChild = GetLastTaskOfQueue(indexQueue);

        bool willHadNext = GetRandInt(0, 1) == 0 ? false : true;

        int sum = 0;
        if (willHadNext)
        {

            if(lastChild != null)
            {
                if (firstChild != null && firstChild.GetNext() != null)
                {
                    sum = firstChild.SumOfScore();
                }
                else
                {
                    sum = lastChild.SumOfScore();
                }
            }
            else
            {
                willHadNext = false;
            }
        }

        AirportTask task = Instantiate(tasks[indexTask], queues[indexQueue].transform);
        task.InstanciateTask(indexTask, indexQueue, score);
        ForceRebuildLayoutQueue(indexQueue);
        task.UpdateStartPosition();

        if (willHadNext && (sum + task.GetScore()) <= maxScore)
        {
            lastChild.SetNext(task);
        }
    }

    public void IterateQueue(Transform queueTransform, Action<AirportTask, int> action)
    {
        if (queueTransform == null || action == null)
        {
            Debug.LogError("Queue ou action é null.");
            return;
        }

        for (int i = 0; i < queueTransform.childCount; i++)
        {
            AirportTask task = queueTransform.GetChild(i).GetComponent<AirportTask>();
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

    public void RemoveChildOfQueue(int indexQueue, AirportTask task)
    {
        int taskToAdd = 0;

        AirportTask currentTask = task;
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

        IterateQueue(queues[indexQueue].transform, (AirportTask task, int index) => {
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

    public AirportTask GetLastTaskOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(GetQueueCount(queueIndex) - 1);

        return lastChildTransform.GetComponent<AirportTask>();
    }

    public AirportTask GetFirstTaskOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(0);

        return lastChildTransform.GetComponent<AirportTask>();
    }

    public AirportTask GetTaskAt(int queueIndex, int index)
    {
        int queueCount = GetQueueCount(queueIndex);
        if (queueCount <= 0 || index >= queueCount || index <= 0) return null;

        Transform lastChildTransform = queues[queueIndex].transform.GetChild(index);

        return lastChildTransform.GetComponent<AirportTask>();
    }

    public bool DestIsBusy(AirportTask task)
    {
        return dests[task.GetIndexOfColor()].IsBusy();
    }

    private void Reset()
    {
        score = 0;
        leftTime = 60f;
        scoreText.text = score.ToString();

        foreach(Dino dino in dinos)
        {
            dino.Reset();
        }
        foreach(Dest dest in dests)
        {
            dest.SetBusy(false);
            dest.ClearProgressBar();
        }

        for(int i = 0; i < queues.Count; i++)
        {
            IterateQueue(queues[i].transform, (Transform child, int index) =>
            {
                Destroy(child.gameObject);
            });
            ForceRebuildLayoutQueue(i);
        }

        AddInitialTasks();
    }
}
