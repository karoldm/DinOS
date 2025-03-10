using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogAwardInfo : MonoBehaviour
{
    public TextMeshProUGUI text;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetText(string text)
    {
        this.text.text = text;
    }

    public void Hidden()
    {
        this.gameObject.SetActive(false);
    }


    public void Show()
    {
        this.gameObject.SetActive(true);
    }


}
