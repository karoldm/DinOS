using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Slot : MonoBehaviour
{
    private SpriteRenderer fileIcon;
    private TextMeshProUGUI fileName;
    private int? currentFileId = null;

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

    public void Write(int fileId)
    {
        this.currentFileId = fileId;
        this.fileIcon.enabled = true;
    }

    public int? Read()
    {
        int? toReturn = this.currentFileId;
        this.currentFileId = null;
        this.fileIcon.enabled = false;
        return toReturn;
    }
}
