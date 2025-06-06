using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class AirportTask : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    public enum TaskColor { red, green, blue }
    private int time;
    private int score;
    private TaskColor color;
    private RAMController controller;
    private Vector3 startPosition;
        private int indexQueue;
    private AirportTask next;

    public void SetNext(AirportTask task)
    {
        this.next = task;

        SpriteRenderer arrow = transform.Find("arrow")?.GetComponent<SpriteRenderer>();
        
        if(arrow == null)
        {
            Debug.LogError("Arrow não encontrado em SetNext().");
            return;
        }

        arrow.gameObject.SetActive(true);
    }

    public AirportTask GetNext()
    {
        return this.next;
    }

    void Awake()
    {
        startPosition = transform.position;

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

    private int GetRandScore()
    {
        int[] scores = {5, 10, 15};
        int index = GetRandInt(0, 2);
        return scores[index];
    }

    public void InstanciateTask(int TaskColorindex, int indexQueue, int? initialScore = null)
    {
        this.indexQueue = indexQueue;

        int score = initialScore ?? GetRandScore();
        int time = GetRandInt(1, 5);

        this.time = time;
        this.score = score;

        this.color = GetColorByIndex(TaskColorindex);

        TextMeshProUGUI timeText = transform.Find("time")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI scoreText = transform.Find("score")?.GetComponent<TextMeshProUGUI>();

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
        return IsLastInQueue() && !controller.DestIsBusy(this) && controller.GamingRunning();
    }

    public int SumOfScore()
    {
        int sum = this.score;
        AirportTask nextTask = this.next;
        while(nextTask != null)
        {
            sum += nextTask.score;
            nextTask = nextTask.next;
        }

        return sum;
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
                    Debug.Log("Dino is null");
                    continue;
                }

                Collider2D dinoCollider = dino.GetComponent<Collider2D>();

                if (dinoCollider == null)
                {
                    Debug.LogError("Dino não contém um Collider2D");
                    return;
                }
                if (dinoCollider.OverlapPoint(mousePosition))
                {
                    if (SumOfScore() > dino.max) continue;
                    if (dino.GetInitialPosition() != dino.transform.position) continue;

                    isInDino = true;
                    transform.position = dino.transform.position;
                    dino.DropTask(this);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("controller.dinos is null");
        }

        if (!isInDino)
        {
            transform.position = startPosition;
            MoveNextToStartPosition(this.next);
        }
        controller.ForceRebuildLayoutQueue(0);
        controller.ForceRebuildLayoutQueue(1);
        controller.ForceRebuildLayoutQueue(2);
        
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

    private void MoveNext(AirportTask nextTask, Vector3 position, int i)
    {
        if(nextTask != null)
        {
            nextTask.transform.position = new Vector3(position.x - i*(350), position.y, position.z);
            i += 1;
            nextTask.MoveNext(nextTask.next, position, i);
        }
    }

    private void MoveNextToStartPosition(AirportTask nextTask)
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
            Debug.LogError("RectTransform da task � null em UpdateCurrentTasks");
            return;
        }
        taskTransform.localScale = new Vector3(0.6f, 0.3f, taskTransform.localScale.z);
    }
}