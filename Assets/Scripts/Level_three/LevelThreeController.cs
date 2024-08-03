using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;


public class LevelThreeController : MonoBehaviour
{
    private static LevelThreeController instance;

    public List<VerticalLayoutGroup> queues;
    public List<Dino> dinos;
    private float interval = 5f; 
    private int maxOfQueue = 4;
    public List<AirplanePeriferic> listTypeOfAirplanes;
    public AirplanePeriferic firstSelected;
    public AirplanePeriferic secondSelected;

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
        InvokeRepeating("AddAirplane", 0f, interval);
    }

    void Update()
    {

    }

   public  void Swap()
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
            transform2.SetParent(queue1.transform);

            transform1.SetSiblingIndex(0);
            transform2.SetSiblingIndex(0);

            firstSelected = null;
            secondSelected = null;
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
            if (QueueSize(i) < maxOfQueue)
            {
                AddElementToQueue(i);
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

}
