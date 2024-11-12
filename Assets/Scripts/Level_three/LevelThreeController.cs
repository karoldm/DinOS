using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class LevelThreeController : UserController
{
    private static LevelThreeController instance;

    public List<VerticalLayoutGroup> queues;
    public List<AirplaneCall> calls;
    public List<AirplaneDino> dinos;
    private float interval = 5f; 
    private int maxOfQueue = 4;
    public List<AirplanePeriferic> listTypeOfAirplanes;
    public AirplanePeriferic firstSelected;
    public AirplanePeriferic secondSelected;
    private AirplaneCall selectedCall;
    public TextMeshProUGUI scoreText;
    private int score = 0;
    private bool wrongFlag = false; 
    public Award award;
    private int totalAirplanes = 0;
    private int maxAirplanes = 32;
    public LevelThreeDialog dialog;



    public static LevelThreeController Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<LevelThreeController>();

                if (instance == null)
                {
                    Debug.LogError("No instance of LevelThreeController found in the scene.");
                }

            }
            return instance;
        }
    }

    void Start()
    {
        dialog.showDialog(LevelThreeDialog.DialogType.intro);
        InvokeRepeating("AddAirplane", 0f, interval);
    }

    void Update()
    {

    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Initial");

    }

    public void SetSelectedCall(AirplaneCall call)
    {
        this.selectedCall = call;
    }

    public AirplaneCall GetSelectedCall()
    {
        return this.selectedCall;
    }

    private int GetQueueCount(int index)
    {
        return queues[index].transform.childCount;
    }

    public AirplanePeriferic GetFirstAirplaneOfQueue(int queueIndex)
    {
        if (GetQueueCount(queueIndex) <= 0) return null;

        Transform firstChildTransform = queues[queueIndex].transform.GetChild(0);

        return firstChildTransform.GetComponent<AirplanePeriferic>();
    }

    public void UpdateCalls()
    {
        for(int i = 0; i < queues.Count; i++)
        {
            AirplanePeriferic airplane = GetFirstAirplaneOfQueue(i);

            if (airplane != null)
            {
                AirplaneCall call = calls[i];
                call.SetAirplane(airplane);
            }
        }
    }

    public void Swap()
    {
        if (firstSelected != null && secondSelected != null)
        {
            VerticalLayoutGroup queue1 = firstSelected.GetQueue();
            VerticalLayoutGroup queue2 = secondSelected.GetQueue();

            Transform transform1 = firstSelected.transform;
            Transform transform2 = secondSelected.transform;

            transform1.SetParent(null);
            transform2.SetParent(null);

            transform1.SetParent(queue2.transform);
            firstSelected.SetQueue(queue2);

            transform2.SetParent(queue1.transform);
            secondSelected.SetQueue(queue1);

            transform1.SetSiblingIndex(0);
            transform2.SetSiblingIndex(0);

            firstSelected.UpdateCorrectQueue();
            secondSelected.UpdateCorrectQueue();

            firstSelected = null;
            secondSelected = null;

            UpdateCalls();

            LayoutRebuilder.ForceRebuildLayoutImmediate(queue1.GetComponent<RectTransform>());
            LayoutRebuilder.ForceRebuildLayoutImmediate(queue2.GetComponent<RectTransform>());
        }
        else
        {
            Debug.LogWarning("Um dos elementos referenciados é nulo.");
        }
    }

    void OnDestroy()
    {
        CancelInvoke("AddAirplane");
    }

    private int QueueSize(int index)
    {
        return queues[index].transform.childCount;
    }


    void AddAirplane()
    {
        for(int i = 0; i < queues.Count; i++)
        {
            if (QueueSize(i) < maxOfQueue && totalAirplanes < maxAirplanes)
            {
                AddElementToQueue(i);
                UpdateCalls();
            }
        } 
    }

    private int GetRandInt(int min, int max)
    {
       return UnityEngine.Random.Range(min, max + 1);
    }

    public void ForceRebuildLayoutQueue(int indexQueue)
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(queues[indexQueue].GetComponent<RectTransform>());
    }


    public void AddElementToQueue(int indexQueue)
    {
        if (indexQueue < 0 || indexQueue >= queues.Count)
        {
            Debug.LogError("Índice inválido para VerticalLayoutGroup");
            Debug.LogError("indexQueue: " + indexQueue.ToString());
            return;
        }

        int index = GetRandInt(0, 3);


        if (index < 0 || index >= listTypeOfAirplanes.Count)
        {
            Debug.LogError("Índice inválido para VerticalLayoutGroup");
            Debug.LogError("index: " + index.ToString());
            return;
        }

        AirplanePeriferic airplane = Instantiate(listTypeOfAirplanes[index], queues[indexQueue].transform);
        airplane.Instanciate(index, queues[indexQueue]);
        ForceRebuildLayoutQueue(indexQueue);
    }

    public void EndService(AirplanePeriferic airplane)
    {
        if (!airplane.GetCorrectQueue())
        {
            this.wrongFlag = true;
        }
        this.score += (airplane.GetCorrectQueue() ? 1 : -1) * airplane.GetScore();
        if (this.score < 0) this.score = 0;
        this.scoreText.text = score.ToString();

        RemoveChildOfQueue(airplane.GetQueue());

        totalAirplanes++;

        if(totalAirplanes >= maxAirplanes)
        {
            EndGame();
        }
    }

    public void RemoveChildOfQueue(VerticalLayoutGroup queue)
    {
       
        Transform child = queue.transform.GetChild(0);

        if (child != null)
        {
            child.SetParent(null, false);
        }

        LayoutRebuilder.ForceRebuildLayoutImmediate(queue.GetComponent<RectTransform>());
        UpdateCalls();

        child.gameObject.SetActive(false);
    }

    private void EndGame()
    {
        if (!this.wrongFlag)
        {
            award.Unlock();
            dialog.showDialog(LevelThreeDialog.DialogType.award);
            user.levelThree.awards.Add(LevelData.Award.COMUNICATION);
        }

        user.levelThree.score = score;
        UpdateUser();

        foreach (VerticalLayoutGroup queue in queues)
        {
            HideAllElements(queue);
        }
    }

    public void HideAllElements(VerticalLayoutGroup queue)
    {
        foreach (Transform child in queue.transform)
        {
            child.gameObject.SetActive(false);
        }
    }
}
