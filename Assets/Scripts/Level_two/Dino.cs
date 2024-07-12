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
    private Dest nextDest;
    private bool awaiting = false;

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

    void Update()
    {
        if (this.awaiting)
        {
            MoveToNextDest();
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
            int time = currentTask.GetTime();
            StartCoroutine(this.dest.InitProgressBar(time));
            yield return new WaitForSeconds(time);
            controller.UpdateScore(this.currentTask.GetScore());

            if(this.currentTask.GetNext() != null)
            {
                this.currentTask = this.currentTask.GetNext();
                Dest destOfNextTask = controller.dest[this.currentTask.GetIndexOfColor()];

                if(destOfNextTask == null)
                {
                    Debug.LogError("destOfNextTask é null em Move()");
                    yield return null;
                }

                this.nextDest = destOfNextTask;
                MoveToNextDest();
            }
            else
            {
                ComeBack();
            }
        }
    }

    private void MoveToNextDest() {
        if (this.nextDest == this.dest || !this.nextDest.IsBusy())
        {
            this.dest.SetBusy(false);
            this.dest.ClearProgressBar();
            this.dest = this.nextDest;
            MoveToDest();
        }
        else
        {
            this.awaiting = true;
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
        this.dest.ClearProgressBar();
        MoveToPosition(initialPosition);
        this.dest.SetBusy(false);
    }

    public void DropTask(Task task)
    {
        int queueIndex = task.GetQueueIndex();
        this.currentTask = task;
        controller.RemoveChildOfQueue(queueIndex, task);
        MoveToDest();
    }
}
