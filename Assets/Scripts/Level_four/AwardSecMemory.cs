using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AwardSecMemory : MonoBehaviour
{
    public Sprite unlockedImage;
    private bool locked = true;

    void Start()
    {

    }

    void Update()
    {

    }

    public bool IsLocked()
    {
        return this.locked;
    }

    public void Unlock()
    {
        this.locked = false;
        if (gameObject.GetComponent<SpriteRenderer>() == null)
        {
            Debug.LogError("SpriteRenderer component not found on the GameObject.");
            return;
        }

        if (unlockedImage == null)
        {
            Debug.LogError("UnlockedImage is null.");
            return;
        }

        gameObject.GetComponent<SpriteRenderer>().sprite = unlockedImage;
    }
}
