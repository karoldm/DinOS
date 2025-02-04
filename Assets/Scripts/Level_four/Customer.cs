using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Customer : MonoBehaviour, IPointerClickHandler
{
    public enum Action { READ, WRITE};

    private Action action;
    private ControllerLevelFour controller;
    private PlanFile planFile;
    public PlanFile planFileModel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;

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

    public PlanFile GetPlanFile()
    {
        return this.planFile ;
    }

    public Action GetAction()
    {
        return this.action;
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

        this.planFile = Instantiate(
            this.planFileModel,
            this.planFileModel.transform.position,
            this.planFileModel.transform.rotation
        );

        this.planFile.Initialize();

        if (this.planFile.GetHasPriority() && this.controller.FileWithPriorityExist())
        {
            this.action = Action.READ;
        }
        else {
            this.action = Action.WRITE;
        }

        this.infoText.text = "Olá, gostaria de " + (this.action == Action.READ ? "LER" : "ESCREVER")
            +" com prioridade: " + (this.planFile.GetHasPriority() ? "ALTA" : "BAIXA");
    }

    public void OnPointerClick(PointerEventData eventData) 
    {
        if(this == controller.GetFirstCustomerOfQueue())
        {
            controller.SetCurrentCustomer(this);
            this.infoPanel.SetActive(true);
        }
    }
}
