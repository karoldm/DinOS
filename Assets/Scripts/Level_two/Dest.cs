using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dest : MonoBehaviour
{
    public ProgressBar progressBar;
    private bool isBusy = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    public void SetBusy(bool isBusy)
    {
        this.isBusy = isBusy;
    }

    public IEnumerator InitProgressBar(int busyFor)
    {
        float elapsedTime = 0f;

        while (elapsedTime < busyFor)
        {
            elapsedTime += Time.deltaTime;
            UpdateProgressBar(elapsedTime, busyFor);
            yield return null;
        }

        UpdateProgressBar(busyFor, busyFor);
    }

    public bool IsBusy()
    {
        return this.isBusy;
    }

    public void UpdateProgressBar(float currentTime, float fullTime)
    {
        float progress = currentTime / fullTime;
        progressBar.UpdateProgressBar(progress);
    }

    public void ClearProgressBar()
    {
        progressBar.UpdateProgressBar(0f);
    }
}
