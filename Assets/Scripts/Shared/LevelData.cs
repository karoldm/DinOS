using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LevelData
{   

    public int score = 0;
    public bool firstTime = true;
    [SerializeField] public List<String> awards = new List<String>();

    public LevelData()
    {
    }

    public LevelData(List<String> awards, int score, bool firstTime)
    {
        this.score = score;
        this.awards = awards;
        this.firstTime = firstTime;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
