using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RAMController : MonoBehaviour
{

    private static RAMController instance;

    public List<Task> tasks;
    public List<HorizontalLayoutGroup> horizontalLayoutGroup;
    public List<Dino> dinos;
    public List<GameObject> dest;


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

    void Start()
    {
        AddElementToQueue(1, 0);
        AddElementToQueue(1, 1);
    }


    void Update()
    {

    }

    public void AddElementToQueue(int indexQueue, int indexTask)
    {
        if (horizontalLayoutGroup[indexQueue] == null || tasks[indexTask] == null) return;

        Task task = Instantiate(tasks[indexTask], horizontalLayoutGroup[indexQueue].transform);

        task.InstanciateTask(indexTask);

        LayoutRebuilder.ForceRebuildLayoutImmediate(horizontalLayoutGroup[indexQueue].GetComponent<RectTransform>());
    }
}
