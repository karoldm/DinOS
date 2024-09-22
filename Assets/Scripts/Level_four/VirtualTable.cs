using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualTable : MonoBehaviour
{
    public List<Row> rows;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }

    public bool Find(string fileId, string shelf, string bookcase)
    {
        foreach(Row row in rows)
        {
            if(row.Compare(fileId, shelf, bookcase))
            {
                return true;
            }
        } 

        return false;
    }

    public void Remove(string fileId)
    {
        foreach (Row row in rows)
        {
            if (row.GetFileId() == fileId)
            {
                row.Clear();
            }
        }

    }

    public void Clear()
    {
        foreach (Row row in rows)
        {
            row.Clear();
        }
    }
}
