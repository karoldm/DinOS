using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class ControllerLevelFour : UserController
{
    public VerticalLayoutGroup dinoQueue;
    private Customer currentCustomer;
    public Customer modelDino;
    private int points = 0;
    public TextMeshProUGUI pointsText;
    private bool gameOver = true;
    private float leftTime = 60f;
    public TextMeshProUGUI timeText;
    public GameObject timeContainer;
    public GameObject startButton;
    //private bool hasError = false;
    public Award awardSecMemory;
    public DialogLevelFour dialog;

    public VerticalLayoutGroup swapArea;
    public VerticalLayoutGroup secondMemoryArea;


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
        this.dialog.showDialog(DialogLevelFour.DialogType.intro);

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
        user.levelFour.score = points;

        gameOver = true;
        this.leftTime = 60f;
        this.timeText.text = "";
        timeContainer.SetActive(false);
        startButton.SetActive(true);
        ClearDinoQueue();

        /*if(!hasError && awardSecMemory.IsLocked())
        {
            dialog.showDialog(DialogLevelFour.DialogType.award);
            awardSecMemory.Unlock();
            if (!user.levelFour.awards.Contains("SECMEMORY"))
            {
                user.levelFour.awards.Add("SECMEMORY");
            }
        }*/
        UpdateUser();
    }

    private void ClearDinoQueue()
    {
        for (int i = 1; i < dinoQueue.transform.childCount; i++)
        {
            Destroy(dinoQueue.transform.GetChild(i).gameObject);
        }
    }

    public void InitGame()
    {
        timeContainer.SetActive(true);
        startButton.SetActive(false);
        this.gameOver = false;
        this.pointsText.text = "0";
        this.points = 0;
        this.AddDino();
        this.AddDino();
        this.AddDino();
        this.AddDino();
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Home");

    }

    private void AddDino()
    {
        if (dinoQueue == null)
        {
            Debug.LogError("dinoQueue is null");
            return;
        }

        Customer dino = Instantiate(modelDino, dinoQueue.transform);
        dino.Init();
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
    }

    public void SetCurrentCustomer(Customer customer)
    {
        this.currentCustomer = customer;
        if(currentCustomer == null)
        {
            this.RemoveFirstDino();
        }
    }

    public Customer GetCurrentCustomer()
    {
        return this.currentCustomer;
    }

    public void Write()
    {
        if (this.GetFirstCustomerOfQueue().GetAction() == Customer.Action.READ)
        {
            return;
        }
        /*if (this.currentFileID != null)
        {
            points++;
            pointsText.text = points.ToString();
            this.PlanFile = null;
            RemoveFirstDino();

            if (!this.gameOver)
            {
                AddDino();
            }
        }*/
    }


    public void Read()
    {
        if(this.GetFirstCustomerOfQueue().GetAction() == Customer.Action.WRITE)
        {
            return;
        }

        RemoveFirstDino();
        if (!this.gameOver)
        {
            AddDino();
        }
    }

    private int QueueSize()
    {
        return dinoQueue.transform.childCount;
    }

   
    public bool FileWithPriorityExist()
    {
        return this.swapArea.transform.childCount > 0;
    }
}
