using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VirtualTable : MonoBehaviour
{
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
}
