using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bookcase : MonoBehaviour
{

    public List<Shelf> shelfs = new List<Shelf>();
    public char address;
    private ControllerLevelFour controller;

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
        
    }

    void Update()
    {
        
    }

    public void Open()
    {
        gameObject.SetActive(true);
        controller.SetOpenedBookcase(this);
    }

    public void Close()
    {
        gameObject.SetActive(false);
        controller.SetOpenedBookcase(null);
    }

    public void Clear()
    {
        foreach (Shelf shelf in this.shelfs)
        {
            shelf.Clear();
        }
    }
}
