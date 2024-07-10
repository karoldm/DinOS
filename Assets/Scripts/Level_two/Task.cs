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

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void Awake()
    {

        controller = RAMController.Instance;

        if (controller == null)
        {
            Debug.LogError("RAMController instance not found in the scene.");
        }

        this.startPosition = transform.position;
    }

    public int GetTime()
    {
        return this.time;
    }

    public int GetScores()
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

    private TaskColor GetColorByIndex(int index)
    {
        switch(index)
        {
            case 0:
                return TaskColor.blue;
            case 1:
                return TaskColor.red;
            case 2:
            default:
                return TaskColor.green;
        }
    }

    public void InstanciateTask(int index)
    {
        int score = GetRandInt(1, 5);
        int time = GetRandInt(1, 5);

        this.time = time;
        this.score = score;

        this.color = GetColorByIndex(index);

        SpriteRenderer sprite = GetComponentInChildren<SpriteRenderer>();

        if (sprite == null)
        {
            Debug.LogError("SpriteRenderer não encontrado.");
            return;
        }

        Canvas canvas = sprite.GetComponentInChildren<Canvas>();

        if (canvas == null)
        {
            Debug.LogError("Canvas não encontrado no SpriteRenderer.");
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
     //   gameObject.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {

      //  gameObject.raycastTarget = true;

        List<Dino> droppable = controller.dinos;

        foreach (Dino dino in droppable)
        {
            if (RectTransformUtility.RectangleContainsScreenPoint(dino.GetComponent<RectTransform>(), Input.mousePosition, null))
            {
                return;
            }
        }
        
        transform.position = startPosition;
        
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}