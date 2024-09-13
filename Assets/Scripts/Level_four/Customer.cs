using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Customer : MonoBehaviour, IPointerClickHandler
{
    public enum Action { read, write };

    private int fileId;
    private Action action;
    private ControllerLevelFour controller;

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    
    private Action getRandomAction()
    {
        return (Action)Random.Range(0, 2);
    }

    public void Init(int id)
    {
        this.fileId = id;
    }

    public void OnPointerClick(PointerEventData eventData) 
    { 
        if(this == controller.GetFirstCustomerOfQueue())
        {

        }
    }
}
