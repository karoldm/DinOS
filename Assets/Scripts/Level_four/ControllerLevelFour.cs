using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ControllerLevelFour : UserController
{
    public enum ErrorType { TIMEOUT, READ_WHEN_MUST_WRITE, WRITE_WHEN_MUST_READ, DIFFERENT_PRIORITY }

    public VerticalLayoutGroup dinoQueue;
    public TextMeshProUGUI pointsText;
    public Customer modelDino;
    public DialogLevelFour dialog;
    public TextMeshProUGUI timeText;
    public GameObject timeContainer;
    public GameObject startButton;

    private float leftTime = 60f;
    public Award awardSecMemory;
    private int points = 0;
    private bool hasError = false;
    private bool gameOver = true;

    private Customer currentCustomer;
    private PlanFile selectedPlanFile;

    public GridLayoutGroup swapArea;
    public GridLayoutGroup secondMemoryArea;

    public GameObject cursorTutorial;
    private Dictionary<int, Vector3> tutorialSteps = new Dictionary<int, Vector3>();
    private int currentStep = 0;

    Queue<(Customer.Action Action, bool Priority)> operations = new Queue<(Customer.Action, bool)>(new List<(Customer.Action, bool)>
        {
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.WRITE, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
             (Customer.Action.READ, false),
             (Customer.Action.WRITE, true),
             (Customer.Action.READ, true),
        });

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

    private void SetStepsTutorial()
    {
        tutorialSteps.Add(1, new Vector3(900, -200, 0));
        tutorialSteps.Add(2, new Vector3(-900, +200, 0));
        tutorialSteps.Add(3, new Vector3(1500, -200, 0));
        tutorialSteps.Add(4, new Vector3(-1500, +200, 0));

        tutorialSteps.Add(5, new Vector3(900, -200, 0));
        tutorialSteps.Add(6, new Vector3(-900, +200, 0));
        tutorialSteps.Add(7, new Vector3(1780, -230, 0));
        tutorialSteps.Add(8, new Vector3(-1780, +230, 0));
    }

    void Start()
    {
        this.SetStepsTutorial();

        this.dialog.showDialog(DialogLevelFour.DialogType.intro);

        if (user == null) return;

        if (user.levelFour.awards.Contains("SECMEMORY"))
         {
             awardSecMemory.Unlock();
         }

         this.pointsText.text = user.levelFour.score.ToString();
    }

    void Update()
    {
        if (!gameOver)
        {
            if (leftTime > 0)
            {
                leftTime -= Time.deltaTime;
                timeText.text = Mathf.Round(leftTime).ToString();
            }
            else
            {
                FinishGame();
            }
        }
    }

    private void FinishGame()
    {
        gameOver = true;
        if(user != null)
        {
            user.levelFour.score = points;
            user.levelFour.firstTime = false;
        }
        this.leftTime = 60f;
        this.timeText.text = "";
        timeContainer.SetActive(false);
        startButton.SetActive(true);
        ClearQueue(1, dinoQueue);
        ClearQueue(0, swapArea);
        ClearQueue(0, secondMemoryArea);
        HiddenTutorial();

        if (!hasError && awardSecMemory.IsLocked())
        {
            dialog.showDialog(DialogLevelFour.DialogType.award);
            awardSecMemory.Unlock();
            if (user != null && !user.levelFour.awards.Contains("SECMEMORY"))
            {
                user.levelFour.awards.Add("SECMEMORY");
            }
        }
        UpdateUser();
    }

    private void ClearQueue(int initialIndex, LayoutGroup queue)
    {
        for (int i = initialIndex; i < queue.transform.childCount; i++)
        {
            Destroy(queue.transform.GetChild(i).gameObject);
        }
    }


    public void InitGame()
    {
        timeContainer.SetActive(true);
        startButton.SetActive(false);
        this.gameOver = false;
        this.pointsText.text = "0";
        this.points = 0;

        if(user != null && user.levelFour.firstTime)
        {
            this.AddDino(true, Customer.Action.WRITE);
            this.AddDino(false, Customer.Action.WRITE);
            this.AddDino(true, Customer.Action.READ);
            this.AddDino(false, Customer.Action.READ);
            this.ShowTutorial();
        }
        else
        {
            this.AddDino();
            this.AddDino();
            this.AddDino();
            this.AddDino();
        }
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Home");

    }

    private void AddDino(bool? initialHasPriority = null, Customer.Action? initialAction = null)
    {

        if (dinoQueue == null)
        {
            Debug.LogError("dinoQueue is null");
            return;
        }

        Customer dino = Instantiate(modelDino, dinoQueue.transform);
        var (action, priority) = operations.Dequeue();
        dino.Init(initialHasPriority ?? priority, initialAction ?? action);
        dino.SetActive();
        UpdateQueue();
    }

    private void UpdateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(dinoQueue.GetComponent<RectTransform>());
    }

    public Customer GetFirstCustomerOfQueue()
    {
        if (QueueSize() <= 0) return null;

        Transform firstChildTransform = dinoQueue.transform.GetChild(1);

        return firstChildTransform.GetComponent<Customer>();
    }

    private void RemoveFirstDino()
    {
        if (QueueSize() <= 0) return;

        Destroy(dinoQueue.transform.GetChild(1).gameObject);
        UpdateQueue();

        SetCurrentCustomer(null);
        SetSelectedPlanFile(null);

        if (!this.gameOver)
        {
            AddDino();
        }
    }

    public void SetCurrentCustomer(Customer customer)
    {
        this.currentCustomer = customer;
    }

    public Customer GetCurrentCustomer()
    {
        return this.currentCustomer;
    }

    public void SetSelectedPlanFile(PlanFile file)
    {
        this.selectedPlanFile = file;
    }

    public PlanFile GetSelectedPlanFile()
    {
        return this.selectedPlanFile;
    }

    public void ComputeError(ErrorType errorType)
    {
        this.points--;
        pointsText.text = points.ToString();
        this.RemoveFirstDino();
        dialog.ShowFeedbackDialog(errorType);
        hasError = true;
    }

    public void ComputeSuccess()
    {
        Debug.Log("ComputeSuccess");

        this.points++;
        pointsText.text = points.ToString();
        this.RemoveFirstDino();
    }

    private int QueueSize()
    {
        return dinoQueue.transform.childCount;
    }

    public void ShowTutorial()
    {
        if (user != null && user.levelFour.firstTime && tutorialSteps.Count > currentStep)
        {
            this.cursorTutorial.SetActive(true);
        }
    }

    public void HiddenTutorial()
    {
        this.cursorTutorial.SetActive(false);
    }

    public bool IsTutorial()
    {
        return this.cursorTutorial.activeSelf;
    }

    public void NextStepTutorial()
    {
        if (!this.IsTutorial()) return;

        currentStep++;

        if(tutorialSteps.Count < currentStep)
        {
            HiddenTutorial();
            return;
        }

        Vector3 pos = this.cursorTutorial.transform.position;
        Vector3 newPos = tutorialSteps[currentStep];
        this.cursorTutorial.transform.position = new Vector3(pos.x + newPos.x, pos.y + newPos.y, pos.z);
    }

    public int GetStep()
    {
        return this.currentStep;
    }
}
