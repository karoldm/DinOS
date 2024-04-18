using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Process_item : MonoBehaviour, IDragHandler, IEndDragHandler, IBeginDragHandler
{
    Process_controller process_controller;

    public int ID;
    public int priority;
    public float timeToExecute;
    public Image processImage;
    public Vector2 startPosition;
    public bool isExe;
    public List<float> pauseExe = new List<float>();

    private float timeLeft;

    void Awake()
    {
        process_controller = Process_controller.Instance;

        if (process_controller == null)
        {
            Debug.LogError("Process_controller instance not found in the scene.");
        }
    }

    void Start()
    {
        processImage = GetComponent<Image>();
        startPosition = transform.position;
        isExe = false;
        timeLeft = timeToExecute;
    }

    void Update()
    {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isExe)
        {
            process_controller.ClearSlotExe();
            isExe = false;
            process_controller.UpdateTimeText("");
            pauseExe.Add(timeLeft);

            process_controller.queueExe.Add(ID);

            int index = process_controller.GetFirstSlot();

            process_controller.slots[index].SetSlot(this);
        }

        processImage.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        processImage.raycastTarget = true;

        if (transform.position != process_controller.slotProcessExe.transform.position)
        {
            transform.position = startPosition;
        }

    }

    public void UpdatePosition(Vector2 position)
    {
        transform.position = position;
        startPosition = position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void SetTimeLeft(float time)
    {
        this.timeLeft = time;
    }

    public void DecreaseTimeLeft(float time)
    {
        this.timeLeft -= time;
    }

    public float GetTimeLeft()
    {
        return this.timeLeft;
    }

}
