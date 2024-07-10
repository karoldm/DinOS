using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Progress_bar : MonoBehaviour
{
    public Image progressBar;
    private static Progress_bar _instance;
    public static Progress_bar Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }


    public void Start()
    {
        progressBar.type = Image.Type.Filled;
        progressBar.fillMethod = Image.FillMethod.Horizontal;
    }

    public void UpdateProgressBar(float progress)
    {
        float clampedProgress = Mathf.Clamp01(progress);
        progressBar.fillAmount = clampedProgress;
    }
}
