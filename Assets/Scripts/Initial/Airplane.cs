using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Airplane : MonoBehaviour
{
    public float speed = 300f;        
    public float endPointX = -220f;  

    private Vector3 startPosition;  
    private float direction = -1f; 

    void Start()
    {
        startPosition = transform.position; 
    }

    void Update()
    {
        transform.Translate(Vector3.right * direction * speed * Time.deltaTime);

        if (transform.position.x <= endPointX)
        {
            transform.position = startPosition;
        }
    }
}
