using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[System.Serializable]
public class LevelData
{   

    public int score = 0;
    [SerializeField] public List<String> awards = new List<String>();

    public LevelData()
    {
       
    }

    public LevelData(List<String> awards, int score)
    {
        this.score = score;
        this.awards = awards;
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
