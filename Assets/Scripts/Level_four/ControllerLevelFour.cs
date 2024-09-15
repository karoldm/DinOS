using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ControllerLevelFour : MonoBehaviour
{
    public List<Bookcase> bookcases = new List<Bookcase>();
    public VerticalLayoutGroup dinoQueue;
    private int? currentFileID;
    public Customer modelDino;
    private Bookcase openedBookcase;
    private Shelf currentShelf;

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
        this.addDino();
    }

    void Update()
    {
        
    }

    public void SetCurrentFileId(int fileId)
    {
        this.currentFileID = fileId;
    }

    public void LoadInitialScene()
    {
        SceneManager.LoadScene("Initial");

    }

    void addDino()
    {
        if (dinoQueue == null)
        {
            Debug.LogError("dinoQueue is null");
            return;
        }

        Customer dino = Instantiate(modelDino, dinoQueue.transform);
        dino.Init();
        updateQueue();
    }

    public void updateQueue()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(dinoQueue.GetComponent<RectTransform>());
    }

    public Customer GetFirstCustomerOfQueue()
    {
        if (QueueSize() <= 0) return null;

        Transform firstChildTransform = dinoQueue.transform.GetChild(1);

        return firstChildTransform.GetComponent<Customer>();
    }

    public void Write()
    {
        if(this.currentFileID != null)
        {
            this.currentShelf.Write((int)this.currentFileID);
            this.currentFileID = null;
        }
    }

    public void SetCurrentShelf(Shelf shelf)
    {
        this.currentShelf = shelf;
    }

    public void Read()
    {
        this.currentFileID = this.currentShelf.Read();
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
                if(shelf.GetCurrentFileId() == fileId)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
