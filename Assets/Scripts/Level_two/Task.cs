using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class Task : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum TaskColor { red, green, blue }
    private int time;
    private int score;
    private TaskColor color;
    private RAMController controller;
    private Vector3 startPosition;
    private SpriteRenderer sprite;
    private int indexQueue;
    private Task next;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetNext(Task task)
    {
        this.next = task;

        this.sprite = GetComponentInChildren<SpriteRenderer>();

        if (this.sprite == null)
        {
            Debug.LogError("SpriteRenderer não encontrado em SetNext().");
            return;
        }

        Canvas canvas = sprite.GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas não encontrado no SpriteRenderer em SetNext().");
            return;
        }

        SpriteRenderer arrow = canvas.transform.Find("arrow")?.GetComponent<SpriteRenderer>();
        
        if(arrow == null)
        {
            Debug.LogError("Arrow não encontrado no canvas em SetNext().");
            return;
        }

        arrow.gameObject.SetActive(true);
    }

    public Task GetNext()
    {
        return this.next;
    }

    void Awake()
    {

        controller = RAMController.Instance;

        if (controller == null)
        {
            Debug.LogError("RAMController instance não encontrado na cena.");
        }
    }

    public void UpdateStartPosition()
    {
        this.startPosition = transform.position;
    }

    public int GetQueueIndex()
    {
        return this.indexQueue;
    }

    public int GetTime()
    {
        return this.time;
    }

    public int GetScore()
    {
        return this.score;
    }

    public TaskColor GetColor()
    {
        return this.color;
    }


    private int GetRandInt(int min, int max)
    {
        int minRange = min;
        int maxRange = max;
        int randomNumberInRange = Random.Range(minRange, maxRange + 1);

        return randomNumberInRange;
    }

    public TaskColor GetColorByIndex(int index)
    {
        switch(index)
        {
            case 0:
                return TaskColor.red;
            case 1:
                return TaskColor.green;
            case 2:
            default:
                return TaskColor.blue;
        }
    }

    public int GetIndexOfColor()
    {
        switch (this.color)
        {
            case TaskColor.red:
                return 0;
            case TaskColor.green:
                return 1;
            case TaskColor.blue:
                return 2;
            default:
                return 0;
        }  
    }

    public void InstanciateTask(int TaskColorindex, int indexQueue)
    {
        this.indexQueue = indexQueue;

        int score = GetRandInt(1, 5);
        int time = GetRandInt(1, 5);

        this.time = time;
        this.score = score;

        this.color = GetColorByIndex(TaskColorindex);

        this.sprite = GetComponentInChildren<SpriteRenderer>();

        if (this.sprite == null)
        {
            Debug.LogError("SpriteRenderer não encontrado em InstanciateTask().");
            return;
        }

        Canvas canvas = sprite.GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas não encontrado no SpriteRenderer em InstanciateTask().");
            return;
        }

        TextMeshProUGUI timeText = canvas.transform.Find("time")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = canvas.transform.Find("score")?.GetComponent<TextMeshProUGUI>();

        if (timeText == null || scoreText == null) return;

        timeText.text = time.ToString();
        scoreText.text = score.ToString();

        Show();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
    }

    private bool CanMove()
    {
        return IsLastInQueue() && !controller.DestIsBusy(this);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (!CanMove()) return;

        List<Dino> droppables = controller.dinos;
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        bool isInDino = false;

        if(droppables != null)
        {
            foreach (Dino dino in droppables)
            {
                if(dino == null)
                {
                    Debug.Log("Dino é null");
                    return;
                }

                Collider2D dinoCollider = dino.GetComponent<Collider2D>();

                if (dinoCollider == null)
                {
                    Debug.LogError("Dino não contém um Collider2D");
                }
                else if (dinoCollider.OverlapPoint(mousePosition))
                {
                    isInDino = true;
                    transform.position = dino.transform.position;
                    dino.DropTask(this);
                    return;
                }
            }
        }
        else
        {
            Debug.LogError("controller.dinos é null");
        }

        if (!isInDino)
        {
            transform.position = startPosition;
            MoveNextToStartPosition(this.next);
        }
        
    }

    private bool IsLastInQueue()
    {
        return (controller.GetFirstChildOfQueue(this.GetQueueIndex()) == transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!CanMove()) return;
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        worldPosition.z = 0; 
        transform.position = worldPosition;

        MoveNext(this.next, worldPosition, 1);
    }

    private void MoveNext(Task nextTask, Vector3 position, int i)
    {
        if(nextTask != null)
        {
            nextTask.transform.position = new Vector3(position.x - i*(250), position.y, position.z);
            i += 1;
            nextTask.MoveNext(nextTask.next, position, i);
        }
    }

    private void MoveNextToStartPosition(Task nextTask)
    {
        if (nextTask != null)
        {
            nextTask.transform.position = nextTask.startPosition;
            nextTask.MoveNextToStartPosition(nextTask.next);
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Resize(float scale)
    {
        RectTransform taskTransform = GetComponent<RectTransform>();
        if (taskTransform == null)
        {
            Debug.LogError("RectTransform da task é null em UpdateCurrentTasks");
            return;
        }
        taskTransform.localScale = new Vector3(0.3f, 0.3f, taskTransform.localScale.z);
    }
}