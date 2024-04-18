using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Star : MonoBehaviour
{
    public Sprite unlockedImage;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void Unlock()
    {

        if (gameObject.GetComponent<Image>() == null)
        {
            Debug.LogError("Image component not found on the GameObject.");
            return;
        }

        if (unlockedImage == null)
        {
            Debug.LogError("UnlockedImage is null.");
            return;
        }

        gameObject.GetComponent<Image>().sprite = unlockedImage;
    }
}
