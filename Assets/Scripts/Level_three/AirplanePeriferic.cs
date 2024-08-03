using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class AirplanePeriferic : MonoBehaviour, IPointerClickHandler    
{
    public enum Priority {High, Medium, Low}

    private LevelThreeController controller;

    private int timeToExecute;
    private int score;
    private Priority priority;
    private VerticalLayoutGroup queue;

    void Awake()
    {

        controller = LevelThreeController.Instance;

        if (controller == null)
        {
            Debug.LogError("LevelThreeController instance não encontrado na cena.");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        

    }

    public VerticalLayoutGroup GetQueue()
    {
        return this.queue;

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        int index = GetIndexOfChild();

        Debug.Log(index.ToString());

        if (index == -1 || index != 0) return;

        Debug.Log("swap called");

        if (controller.firstSelected != null && controller.firstSelected != this)
        {
            controller.secondSelected = this;
            controller.Swap();
        }
        else
        {
            controller.firstSelected = this;
        }
    }

    int GetIndexOfChild()
    {
        Transform parentTransform = this.queue.transform;
        for (int i = 0; i < parentTransform.childCount; i++)
        {
            if (parentTransform.GetChild(i).gameObject == gameObject)
            {
                return i;
            }
        }
        return -1; 
    }

    public void Instanciate(int airplaneIndex, VerticalLayoutGroup queue)
    {
        this.queue = queue;
        Show();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }


    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
