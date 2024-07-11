using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest : MonoBehaviour
{
    private bool isBusy = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBusy(bool isBusy)
    {
        this.isBusy = isBusy;
    }
    
    public bool IsBusy()
    {
        return this.isBusy;
    }
}
