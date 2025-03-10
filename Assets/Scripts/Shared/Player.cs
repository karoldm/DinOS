using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float speed = 100f; 
    public float webSpeedMultiplier = 2f; 

    private bool isMoving;
    private Vector2 input;
    private Animator animator;

    public List<Tilemap> tilemapObjects = new List<Tilemap>();
    public Tilemap tilemapProcessPortal;
    public Tilemap tilemapInitialPortal;
    public Tilemap tilemapRAMPortal;
    public Tilemap tilemapSecMemoryPortal;
    public Tilemap tilemapESPortal;

    public DialogInitial initialDialog;

    private void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 120; // Increase FPS limit
    }


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

            if (input.x != 0) input.y = 0; // Prevent diagonal movement

            if (input != Vector2.zero)
            {
                animator.SetFloat("MoveX", input.x);
                animator.SetFloat("MoveY", input.y);

                var targetPosition = transform.position;
                targetPosition.x += input.x;
                targetPosition.y += input.y;

                bool collision = false;

                for (int i = 0; i < tilemapObjects.Count; i++)
                {
                    if (tilemapObjects[i] != null)
                    {
                        Vector3Int obstacleMap = tilemapObjects[i].WorldToCell(targetPosition);
                        if (tilemapObjects[i].GetTile(obstacleMap) != null)
                        {
                            collision = true;
                            break;
                        }
                    }
                }

                if (!collision)
                {
                    StartCoroutine(Move(targetPosition));
                }
                else
                {
                    CheckPortal(targetPosition);
                }
            }
        }

        animator.SetBool("IsMoving", isMoving);
    }


    private void CheckPortal(Vector3 targetPosition)
    {
        if (initialDialog != null && initialDialog.gameObject.activeInHierarchy) return;

        if (tilemapProcessPortal != null)
        {
            Vector3Int obstacleMap = tilemapProcessPortal.WorldToCell(targetPosition);

            if (tilemapProcessPortal.GetTile(obstacleMap) != null)
            {
                initialDialog.showDialog(DialogInitial.InitialDialogType.process);
                initialDialog.SetCurrentSceneType(DialogInitial.InitialDialogType.process);
            }
        }

        if (tilemapRAMPortal != null)
        {
            Vector3Int obstacleMap = tilemapRAMPortal.WorldToCell(targetPosition);

            if (tilemapRAMPortal.GetTile(obstacleMap) != null)
            {
                initialDialog.showDialog(DialogInitial.InitialDialogType.RAM);
                initialDialog.SetCurrentSceneType(DialogInitial.InitialDialogType.RAM);
            }
        }

        if (tilemapSecMemoryPortal != null)
        {
            Vector3Int obstacleMap = tilemapSecMemoryPortal.WorldToCell(targetPosition);

            if (tilemapSecMemoryPortal.GetTile(obstacleMap) != null)
            {
                initialDialog.showDialog(DialogInitial.InitialDialogType.SecMemory);
                initialDialog.SetCurrentSceneType(DialogInitial.InitialDialogType.SecMemory);
            }
        }

        if (tilemapESPortal != null)
        {
            Vector3Int obstacleMap = tilemapESPortal.WorldToCell(targetPosition);

            if (tilemapESPortal.GetTile(obstacleMap) != null)
            {
                initialDialog.showDialog(DialogInitial.InitialDialogType.ES);
                initialDialog.SetCurrentSceneType(DialogInitial.InitialDialogType.ES);
            }
        }

        if (tilemapInitialPortal != null)
        {
            Vector3Int obstacleMap = tilemapInitialPortal.WorldToCell(targetPosition);

            if (tilemapInitialPortal.GetTile(obstacleMap) != null)
            {
                SceneManager.LoadScene("Home");
            }
        }
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        float adjustedSpeed = speed;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            adjustedSpeed *= webSpeedMultiplier;
        }

        float stepTime = 1f / adjustedSpeed; // Time to complete the move
        yield return new WaitForSeconds(stepTime); // Ensures movement is frame-independent

        transform.position = targetPosition; // Instantly move to target position
        isMoving = false;
    }


}