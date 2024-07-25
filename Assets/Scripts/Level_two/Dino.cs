using System.Collections;
using System.Collections.Generic;
using UnityTask = System.Threading.Tasks.Task;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using System;

public class Dino : MonoBehaviour
{
    public float speed;
    private bool isMoving;
    private Vector3 finalPosition;
    private Animator animator;
    private Vector3 initialPosition;
    private AirportTask currentTask;
    private RAMController controller;
    private Dest dest;
    private Dest nextDest;
    private bool awaiting = false;
    public HorizontalLayoutGroup currentTasks;
    public int max;

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
            if (!nextDest.IsBusy())
            {
                this.awaiting = false;
                StartCoroutine(MoveToNextDest());
            }
        }
    }

    public Vector3 GetInitialPosition()
    {
        return this.initialPosition;
    }

    public bool IsAwaiting()
    {
        return this.awaiting;
    }

    public Dest GetDest()
    {
        return this.dest;
    }

    public Dest GetNextDest()
    {
        return this.nextDest;
    }

    public async UnityTask MoveToPosition(Vector3 targetPosition)
    {
        if (!isMoving)
        {
            await StartCoroutineAsTask(Move(targetPosition));
        }
    }

    public void SetCurrentTask(AirportTask task)
    {
        this.currentTask = task;
    }

    private IEnumerator Move(Vector3 targetPosition)
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
            if (this.currentTask != null)
            {
                int time = currentTask.GetTime();
                StartCoroutine(this.dest.InitProgressBar(time));
                yield return new WaitForSeconds(time);

                controller.UpdateScore(this.currentTask.GetScore());

                if (this.currentTask.GetNext() != null)
                {
                    this.currentTask = this.currentTask.GetNext();
                    Dest destOfNextTask = controller.dests[this.currentTask.GetIndexOfColor()];

                    if (destOfNextTask == null)
                    {
                        Debug.LogError("destOfNextTask is null in Move()");
                        yield break;
                    }

                    this.nextDest = destOfNextTask;
                    yield return StartCoroutine(MoveToNextDest());
                }
                else
                {
                    yield return StartCoroutine(ComeBack());
                }
            }
        }
    }

    private IEnumerator MoveToNextDest()
    {
        if (this.nextDest == this.dest || !this.nextDest.IsBusy())
        {
            this.dest.ClearProgressBar();
            this.dest.SetBusy(false);
            this.dest = this.nextDest;
            yield return StartCoroutine(Move(this.dest.transform.position));
        }
        else
        {
            this.awaiting = true;
        }
    }

    private void MoveToDest()
    {
        if (this.currentTask == null) return;

        this.dest = controller.dests[this.currentTask.GetIndexOfColor()];

        if (this.dest != null)
        {
            this.dest.SetBusy(true);
            StartCoroutine(Move(this.dest.transform.position));
        }
        else
        {
            Debug.LogError("dest is null in MoveToDest()");
        }
    }

    private IEnumerator ComeBack()
    {
        this.dest.ClearProgressBar();
        this.dest.SetBusy(false);
        ClearCurrentTasks();
        yield return StartCoroutine(MoveToPositionCoroutine(initialPosition));
    }

    private IEnumerator MoveToPositionCoroutine(Vector3 targetPosition)
    {
        yield return StartCoroutineAsTask(Move(targetPosition));
    }

    public void DropTask(AirportTask task)
    {
        int queueIndex = task.GetQueueIndex();
        this.currentTask = task;
        controller.RemoveChildOfQueue(queueIndex, task);
        UpdateCurrentTasks();
        MoveToDest();
    }

    private void UpdateCurrentTasks()
    {
        AirportTask current = this.currentTask;
        while (current != null)
        {
            AirportTask task = Instantiate(current, currentTasks.transform);
            task.UpdateStartPosition();
            task.Resize(0.3f);
            current = current.GetNext();
        }
        LayoutRebuilder.ForceRebuildLayoutImmediate(currentTasks.GetComponent<RectTransform>());
    }

    private void ClearCurrentTasks()
    {
        this.currentTask = null;

        foreach (Transform child in currentTasks.transform)
        {
            Destroy(child.gameObject);
        }
    }

    public void Reset()
    {
        this.dest = null;
        this.nextDest = null;
        this.awaiting = false;
        ClearCurrentTasks();
        StartCoroutine(ComeBack());
    }

    private UnityTask StartCoroutineAsTask(IEnumerator coroutine)
    {
        var tcs = new System.Threading.Tasks.TaskCompletionSource<bool>();
        StartCoroutine(RunCoroutine(coroutine, tcs));
        return tcs.Task;
    }

    private IEnumerator RunCoroutine(IEnumerator coroutine, System.Threading.Tasks.TaskCompletionSource<bool> tcs)
    {
        yield return coroutine;
        tcs.SetResult(true);
    }
}
