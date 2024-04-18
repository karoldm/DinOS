using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slot_Process : MonoBehaviour
{
    public Process_item process;

    public void SetSlot(Process_item processItem)
    {
        processItem.UpdatePosition(transform.position);
        process = processItem;
    }

    public Vector2 GetPosition()
    {
        return transform.position;
    }
}
