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
        if (this.isExe) return;
        processImage.raycastTarget = false;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (this.isExe) return;
        processImage.raycastTarget = true;

        Slot_Process_Exe slot = process_controller.slotProcessExe.GetComponent<Slot_Process_Exe>();

        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            process_controller.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out worldPosition);

        if (!RectTransformUtility.RectangleContainsScreenPoint(slot.GetComponent<RectTransform>(), worldPosition, null))
        {
            transform.position = startPosition;

            if (isExe)
            {
                AbortExe();
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (this.isExe) return;

        Slot_Process_Exe slot = process_controller.slotProcessExe.GetComponent<Slot_Process_Exe>();
        if (slot != null && slot.currentItemExe != null) return;

        // Converter a posição do mouse na tela para as coordenadas do mundo
        Vector3 worldPosition;
        RectTransformUtility.ScreenPointToWorldPointInRectangle(
            process_controller.GetComponent<RectTransform>(),
            eventData.position,
            eventData.pressEventCamera,
            out worldPosition);

        // Ajustar a posição do objeto para a célula do grid mais próxima
        transform.position = worldPosition;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
        processImage.raycastTarget = true;
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

    public void ResetTimeLeft()
    {
        this.timeLeft = this.timeToExecute;
    }

    public void UpdatePosition(Vector2 position)
    {
        transform.position = position;
        startPosition = position;
    }

    public void AbortExe()
    {
        process_controller.ClearSlotExe();
        this.isExe = false;
        process_controller.UpdateTimeText("");

        process_controller.queueExe.Add(this.ID);
        process_controller.ResetProgressBar();

        int index = process_controller.GetLastQueuePosition();

        process_controller.slots[index].SetSlot(this);
        processImage.raycastTarget = true;
    }
}
