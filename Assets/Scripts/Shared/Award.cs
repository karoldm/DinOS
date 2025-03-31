using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Award : MonoBehaviour, IPointerClickHandler
{
    public Sprite unlockedImage; 
    private bool locked = true;
    public DialogAwardInfo dialog;
    public string title;
    public string description;

    public Image imageComponent;

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

        if (imageComponent == null)
        {
            Debug.LogError("Image component not found on the GameObject.");
            return;
        }

        if (unlockedImage == null)
        {
            Debug.LogError("UnlockedImage is null.");
            return;
        }

        imageComponent.sprite = unlockedImage;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if(this.locked)
        {
            dialog.SetText(title + ": Você ainda não desbloqueou essa conquista...");
               
        } else
        {
            dialog.SetText(description);
        }
        dialog.Show();
    }
}
