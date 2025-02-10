using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class Customer : MonoBehaviour, IPointerClickHandler
{
    public enum Action { READ, WRITE };

    private Action action;
    private ControllerLevelFour controller;
    private PlanFile planFile;
    public PlanFile planFileModel;
    public GameObject infoPanel;
    public TextMeshProUGUI infoText;
    private float leftTime; // Time for the customer
    private bool counting = false;
    private bool planFileBeingFetched = false;
    private float planFileFetchTime = 5f;
    private float customerWaitTime; // Store initial wait time for the customer

    private Vector3 originalScale;

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }

        originalScale = transform.localScale;
    }

    void Update()
    {
        if (counting)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                infoText.text = infoText.text + ": " + Mathf.Round(leftTime).ToString();
            }
            else
            {
                // If customer's waiting time finished, but the file isn't fetched yet
                if (!planFileBeingFetched)
                {
                    controller.ComputeError();
                    controller.SetCurrentCustomer(null);
                    controller.SetSelectedPlanFile(null);
                }
            }
        }
    }

    public PlanFile GetPlanFile()
    {
        return this.planFile;
    }

    public Action GetAction()
    {
        return this.action;
    }

    public void SetActive()
    {
        gameObject.SetActive(true);
    }

    private Action GetRandAction(bool hasPriority)
    {
        Action action = Random.Range(0, 2) == 1 ? Action.READ : Action.WRITE;
        if (action == Action.READ && hasPriority && !controller.FileWithPriorityExist())
        {
            action = Action.WRITE;
        }
        else if (action == Action.READ && !hasPriority && !controller.FileWithNoPriorityExist())
        {
            action = Action.WRITE;
        }
        return action;
    }

    public void Init()
    {
        Debug.Log("init customer");

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
        this.action = GetRandAction(this.planFile.GetHasPriority());
        if (this.action == Action.READ)
        {
            this.customerWaitTime = this.planFile.GetHasPriority() ? 2f : 4f;
        }

        this.leftTime = customerWaitTime; // Set the initial wait time for the customer
        this.infoText.text = "Olá, gostaria de " + (this.action == Action.READ ? "LER" : "ESCREVER")
            + " com prioridade: " + (this.planFile.GetHasPriority() ? "ALTA" : "BAIXA");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (controller.GetCurrentCustomer() == this)
        {
            controller.SetCurrentCustomer(null);
            this.infoPanel.SetActive(false);
            return;
        }

        if (this == controller.GetFirstCustomerOfQueue())
        {
            PlanFile selectedPlanFile = controller.GetSelectedPlanFile();
            if (selectedPlanFile != null)
            {
                if (controller.GetCurrentCustomer().GetAction() == Action.WRITE)
                {
                    controller.ComputeError();
                    return;
                }

                controller.SetSelectedPlanFile(null);
                this.counting = true;
            }
            else
            {
                controller.SetCurrentCustomer(this);
                this.infoPanel.SetActive(true);
                if(this.action == Action.READ)
                {
                    this.counting = true;
                }
            }
        }
    }

    private IEnumerator FetchPlanFile()
    {
        Debug.Log("Fetching Plan File...");

        // Wait for the recovery time of the plan file
        yield return new WaitForSeconds(planFileFetchTime);

        // Once the plan file is ready, check if the customer is still waiting
        if (leftTime > 0)
        {
            // Plan file is ready before the customer time finishes
            controller.ComputeSuccess();
        }
        else
        {
            // Customer's waiting time finished before the plan file is fetched
            controller.ComputeError();
        }

        // Clean up and reset all necessary data
        controller.SetCurrentCustomer(null);
        controller.SetSelectedPlanFile(null);

        // Reset customer scale and close the info panel
        transform.localScale = originalScale;
        this.infoPanel.SetActive(false);
    }
}
