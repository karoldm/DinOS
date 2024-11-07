using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour
{   
    public enum Award
    {
        RR, 
        SJF,
        BP, 
        FCFS,
        SEGMENTATION,
        DEADLOCK,
        COMUNICATION,
        SECMEMORY
    }

    public int score;
    public HashSet<Award> awards;

    public LevelData()
    {
        this.score = 0;
        this.awards = new HashSet<Award>();
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
