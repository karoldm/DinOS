using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


public class Dino : MonoBehaviour, IDropHandler
{
    public float speed = 10f;
    private bool isMoving;
    private Vector3 finalPosition;
    private Animator animator;
    private Vector3 initialPosition;
    private Task currentTask;
    private RAMController controller;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        initialPosition = transform.position;

        controller = RAMController.Instance;

        if (controller == null)
        {
            Debug.LogError("RAMController instance not found in the scene.");
        }
    }

    public void MoveToPosition(Vector3 targetPosition)
    {
        if (!isMoving)
        {
            StartCoroutine(Move(targetPosition));
        }
    }

    public void SetCurrentTask(Task task)
    {
        this.currentTask = task;
    }

    IEnumerator Move(Vector3 targetPosition)
    {
        isMoving = true;

        animator.SetFloat("MoveX", Mathf.Sign(targetPosition.x - transform.position.x));
        animator.SetFloat("MoveY", Mathf.Sign(targetPosition.y - transform.position.y));
        animator.SetBool("IsMoving", true);

        while ((transform.position - targetPosition).sqrMagnitude > Mathf.Epsilon)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
            yield return null;
        }

        transform.position = targetPosition;

        animator.SetBool("IsMoving", false);

        isMoving = false;

        if (targetPosition != initialPosition)
        {
            yield return new WaitForSeconds(currentTask.GetTime());

            ComeBack();
        }
    }

    private void MoveToDest(Task task)
    {
        switch (task.GetColor())
        {
            case Task.TaskColor.red:
                Move(controller.dest[0].transform.position);
                break;
            case Task.TaskColor.green:
                Move(controller.dest[1].transform.position);
                break;
            case Task.TaskColor.blue:
                Move(controller.dest[2].transform.position);
                break;
        }
    }

    private void ComeBack()
    {
        MoveToPosition(initialPosition);
    }

    private void DropTask(Task task)
    {
        this.currentTask = task;
        Destroy(task);
        MoveToDest(task);
    }

    public void OnDrop(PointerEventData eventData)
    {
        Task draggedItem = eventData.pointerDrag.GetComponent<Task>();
        if (draggedItem != null)
        {
            DropTask(draggedItem);
        }
    }
}
