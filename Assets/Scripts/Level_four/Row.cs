using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Row : MonoBehaviour
{
    public TMP_InputField bookcase;
    public TMP_InputField shelf;
    public TMP_InputField fileId;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public bool Compare(string fileId, string shelf, string bookcase)
    {
        return (this.fileId.text == fileId && this.shelf.text == shelf && this.bookcase.text == bookcase);
    }

    public string GetFileId()
    {
        return fileId.text;
    }

    public void Clear()
    {
        this.fileId.text = "";
        this.bookcase.text = "";
        this.shelf.text = "";
    }
}
