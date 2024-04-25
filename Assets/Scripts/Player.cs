using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public float speed = 3f;

    private bool isMoving;

    private Vector2 input;

    private Animator animator;

    public List<Tilemap> tilemapObjects = new List<Tilemap>();

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (!isMoving)
        {
            input.x = Input.GetAxisRaw("Horizontal");
            input.y = Input.GetAxisRaw("Vertical");

            if (input.x != 0) input.y = 0;  

            input.Normalize();

            animator.SetFloat("MoveX", input.x);
            animator.SetFloat("MoveY", input.y);

            var targetPosition = transform.position;
            targetPosition.x += input.x;
            targetPosition.y += input.y;

            bool collision = false;

            for(int i = 0; i <  tilemapObjects.Count; i++)
            {
                Vector3Int obstacleMap = tilemapObjects[i].WorldToCell(targetPosition);

                if (tilemapObjects[i].GetTile(obstacleMap) != null)
                {
                    collision = true;
                    break;
                }
            }

            if (input != Vector2.zero && !collision)
            {
                StartCoroutine(Move(targetPosition));
            }
        }

        animator.SetBool("IsMoving", isMoving); 
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        while((targetPosition - transform.position).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }
        transform.position = targetPosition;

        isMoving = false;
    }
}
