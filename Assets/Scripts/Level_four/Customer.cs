using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Customer : MonoBehaviour, IPointerClickHandler
{
    public enum Action { ler, escrever };

    private int fileId;
    private Action action;
    private ControllerLevelFour controller;
    public GameObject infoPanel;
    public TextMeshProUGUI processText;
    public TextMeshProUGUI actionText;

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

    public int GetFileId()
    {
        return this.fileId;
    }

    public Action GetAction()
    {
        return this.action;
    }   

    private int GetRandId()
    {
        return Random.Range(0,3);
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    public void Init()
    {
        if (this.controller == null)
        {
            this.controller = ControllerLevelFour.Instance;
        }

        if (this.controller == null)
        {
            Debug.LogError("ControllerLevelFour instance is still null in Init()");
            return; 
        }

        this.fileId = GetRandId();
        if (this.controller.FileIdExist(this.fileId))
        {
            this.action = Action.ler;
        }
        else {
            this.action = Action.escrever;
        }
        this.processText.text = "Plano de voo: " + this.fileId.ToString();
        this.actionText.text = "Opera��o: " + this.action.ToString();
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if(this == controller.GetFirstCustomerOfQueue())
        {
            this.Init();
            controller.SetCurrentFileId(this.fileId);
            this.infoPanel.SetActive(true);
        }
    }
}
