using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookcase : MonoBehaviour
{

    public List<Slot> slots = new List<Slot>();
    public char address; 

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public int? ReadFrom(int shelf)
    {
        if(shelf < 0 || shelf >= this.slots.Count)
        {
            Debug.LogError("Invalid shelf number.");
            return -1;
        }

        return this.slots[shelf].Read();
    }

    public void WriteTo(int shelf, int processId)
    {
        if (shelf < 0 || shelf >= this.slots.Count)
        {
            Debug.LogError("Invalid shelf number.");
            return;
        }

        this.slots[shelf].Write(processId);  
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }
}
