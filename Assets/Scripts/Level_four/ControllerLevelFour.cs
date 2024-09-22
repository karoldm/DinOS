using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class ControllerLevelFour : MonoBehaviour
{
    public List<Bookcase> bookcases = new List<Bookcase>();
    public VerticalLayoutGroup dinoQueue;
    private int? currentFileID;
    public Customer modelDino;
    private Bookcase openedBookcase;
    private Shelf currentShelf;
    private int points = 0;
    public TextMeshProUGUI pointsText;
    public VirtualTable virtualTable;
    private bool gameOver = true;
    private float leftTime = 60f;
    public TextMeshProUGUI timeText;
    public GameObject timeContainer;
    public GameObject startButton;
    private bool hasError = false;
    public AwardSecMemory awardSecMemory;
    public DialogLevelFour dialog;


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

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        this.virtualTable.Close();
        this.dialog.showDialog(DialogLevelFour.DialogType.intro);
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
        this.leftTime = 60f;
        this.timeText.text = "";
        timeContainer.SetActive(true);
        startButton.SetActive(false);
        ClearDinoQueue();
        virtualTable.Clear();
        if(!hasError && awardSecMemory.IsLocked())
        {
            dialog.showDialog(DialogLevelFour.DialogType.award);
            awardSecMemory.Unlock();
        }
    }

    private void ClearDinoQueue()
    {
        for (int i = 1; i < dinoQueue.transform.childCount-1; i++)
        {
            Destroy(dinoQueue.transform.GetChild(i).gameObject);
        }
    }

    public void InitGame()
    {
        timeContainer.SetActive(true);
        startButton.SetActive(false);
        this.gameOver = false;
        this.AddDino();
        this.AddDino();
        this.AddDino();
        this.AddDino();
    }

    public void SetCurrentFileId(int fileId)
    {
        this.currentFileID = fileId;
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Initial");

    }

    void AddDino()
    {
        if (dinoQueue == null)
        {
            Debug.LogError("dinoQueue is null");
            return;
        }

        Customer dino = Instantiate(modelDino, dinoQueue.transform);
        dino.SetActive();
        UpdateQueue();
    }

    public void UpdateQueue()
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

    public void Write()
    {
        if (this.GetFirstCustomerOfQueue().GetAction() == Customer.Action.read)
        {
            return;
        }
        if (this.currentFileID != null)
        {
            this.currentShelf.Write((int)this.currentFileID);
            points++;
            pointsText.text = points.ToString();
            this.currentFileID = null;
            RemoveFirstDino();
            if (!this.gameOver)
            {
                AddDino();
            }
        }
        this.currentShelf = null;
    }

    public void SetCurrentShelf(Shelf shelf)
    {
        this.currentShelf = shelf;
    }

    public void SetOpenedBookcase(Bookcase bookcase)
    {
        this.openedBookcase = bookcase;
    }

    public void Read()
    {
        if(this.GetFirstCustomerOfQueue().GetAction() == Customer.Action.write)
        {
            return;
        }

        int? id = this.currentShelf.Read();

        if (id != null)
        {
            if(id != this.GetFirstCustomerOfQueue().GetFileId())
            {
                this.points -= 3;
                this.hasError = true;
                return;
            }
            bool isInVirtualTable = this.virtualTable.Find(
                id.ToString(),
                this.currentShelf.shelfNumber.ToString(),
                this.openedBookcase.address.ToString()
            );
            if (isInVirtualTable)
            {
                this.points += 2;
                virtualTable.Remove(id.ToString());
            }
            else
            {
                this.points -= 2;
            }   
            this.pointsText.text = this.points.ToString();
            RemoveFirstDino();
            if (!this.gameOver)
            {
                AddDino();
            }
        }
        this.currentShelf = null;
    }

    private int QueueSize()
    {
        return dinoQueue.transform.childCount;
    }

    public void OpenBookcaseA()
    {
        this.openedBookcase = this.bookcases.Find(b => b.address == 'A');
        this.openedBookcase.gameObject.SetActive(true);
    }

    public void OpenBookcaseB()
    {
        this.openedBookcase = this.bookcases.Find(b => b.address == 'B');
        this.openedBookcase.gameObject.SetActive(true);
    }

    public void OpenBookcaseC()
    {
        this.openedBookcase = this.bookcases.Find(b => b.address == 'C');
        this.openedBookcase.gameObject.SetActive(true);
    }

    public void OpenBookcaseD()
    {
        this.openedBookcase = this.bookcases.Find(b => b.address == 'D');
        this.openedBookcase.gameObject.SetActive(true);
    }

    public bool FileIdExist(int fileId)
    {
        foreach(Bookcase bookcase in bookcases)
        {
            foreach(Shelf shelf in bookcase.shelfs)
            {
                if (shelf.GetCurrentFileId() == fileId)
                {

                    return true;
                }
            }
        }
        return false;
    }
}
