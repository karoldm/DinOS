using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class Shelf : MonoBehaviour, IPointerClickHandler
{
    private SpriteRenderer fileIcon;
    private TextMeshProUGUI fileName;
    private int? currentFileId = null;
    private ControllerLevelFour controller;
    public int shelfNumber;

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }
    }

    void Start()
    {
        fileIcon = GetComponentInChildren<SpriteRenderer>();
        fileName = GetComponentInChildren<TextMeshProUGUI>();

        fileIcon.enabled = false;
        fileName.text = "";
    }

    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        controller.SetCurrentShelf(this);
    }

    public void Write(int fileId)
    {
        this.currentFileId = fileId;
        this.fileName.text = fileId.ToString();
        this.fileIcon.enabled = true;
    }

    public int? Read()
    {
        int? toReturn = this.currentFileId;
        this.currentFileId = null;
        this.fileIcon.enabled = false;
        this.fileName.text = "";
        return toReturn;
    }

    public int? GetCurrentFileId()
    {
        return this.currentFileId;
    }

    public void Clear()
    {
        this.currentFileId = null;
        if(fileIcon == null || fileName == null) return;
        this.fileIcon.enabled = false;
        this.fileName.text = "";
    }   
}
