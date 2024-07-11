using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;


public class Dino : MonoBehaviour
{
    public float speed;
    private bool isMoving;
    private Vector3 finalPosition;
    private Animator animator;
    private Vector3 initialPosition;
    private Task currentTask;
    private RAMController controller;
    private Dest dest;

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
            controller.UpdateScore(this.currentTask.GetScore());
            ComeBack();
        }
    }

    private void MoveToDest()
    {
        this.dest = controller.dest[this.currentTask.GetIndexOfColor()];

        if (this.dest != null)
        {
            this.dest.SetBusy(true);
            StartCoroutine(Move(this.dest.transform.position));
        }
        else
        {
            Debug.LogError("dest é null em MoveToDest()");
        }
    }

    private void ComeBack()
    {
        this.dest.SetBusy(false);
        MoveToPosition(initialPosition);
    }

    public void DropTask(Task task)
    {
        int queueIndex = task.GetQueueIndex();
        this.currentTask = task;
        controller.RemoveChildOfQueue(queueIndex);
        MoveToDest();
    }
}
