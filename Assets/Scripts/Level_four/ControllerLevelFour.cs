using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ControllerLevelFour : MonoBehaviour
{
    public List<Bookcase> bookcases = new List<Bookcase>();
    private string currentFileName;
    public VerticalLayoutGroup dinoQueue;
    private int fileId = 0;

    private static ControllerLevelFour instance;


    public static ControllerLevelFour Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<ControllerLevelFour>();

                if (instance == null)
                {
                    Debug.LogError("No instance of ControllerLevelFour found in the scene.");
                }

            }
            return instance;
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    void addDino()
    {
        if (dinoQueue == null)
        {
            Debug.LogError("dinoQueue is null");
            return;
        }

        Customer dino = Instantiate(new Customer(), dinoQueue.transform);
        dino.Init(fileId);
        fileId++;
        updateQueue();
    }

    public void updateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(dinoQueue.GetComponent<RectTransform>());
    }

    public Customer GetFirstCustomerOfQueue()
    {
        if (QueueSize() <= 0) return null;

        Transform firstChildTransform = dinoQueue.transform.GetChild(0);

        return firstChildTransform.GetComponent<Customer>();
    }

    private int QueueSize()
    {
        return dinoQueue.transform.childCount;
    }

    public void OpenBookcaseA()
    {
        this.bookcases.Find(b => b.address == 'A').gameObject.SetActive(true);
    }

    public void OpenBookcaseB()
    {
        this.bookcases.Find(b => b.address == 'B').gameObject.SetActive(true);
    }

    public void OpenBookcaseC()
    {
        this.bookcases.Find(b => b.address == 'C').gameObject.SetActive(true);
    }

    public void OpenBookcaseD()
    {
        this.bookcases.Find(b => b.address == 'D').gameObject.SetActive(true);
    }

}
