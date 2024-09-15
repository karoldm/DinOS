using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Recepcao : MonoBehaviour, IPointerClickHandler
{
    public VirtualTable virtualTable;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        this.virtualTable.Open();
    }

    public void CloseVirtualTable()
    {
        this.virtualTable.Close();
    }
}
