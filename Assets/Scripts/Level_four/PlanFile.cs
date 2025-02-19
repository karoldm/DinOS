using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

public class PlanFile : MonoBehaviour, IPointerClickHandler
{
    private bool hasPriority;
    public Sprite imageNoPriority;
    public Sprite imagePriority;
    private Image image;
    private ControllerLevelFour controller;
    private Vector3 scaleIncrement = new Vector3(0.1f, 0.1f, 0.1f);
    private float leftTime;
    private float timeToFetch;
    public ProgressBar progressBar;
    private bool fetching = false;

    void Update()
    {
        if (fetching && leftTime > 0)
        {
            leftTime -= Time.deltaTime;
            progressBar.UpdateProgressBar((leftTime / timeToFetch));
        }
    }

    void Awake()
    {
        controller = ControllerLevelFour.Instance;

        if (controller == null)
        {
            Debug.LogError("ControllerLevelFour instance not found in scene.");
        }
    }

    public float GetTimeToFetch()
    {
        return this.timeToFetch;
    }

    public void SetTimeToFetch(float time)
    {
        this.timeToFetch = time;
        this.leftTime = time;
    }

    public void Initialize(bool initialHasPriority)
    {
        this.hasPriority = initialHasPriority;

        this.image = GetComponent<Image>();

        if (this.image != null)
        {
            this.image.sprite = this.hasPriority ? this.imagePriority : this.imageNoPriority;
            gameObject.SetActive(true);
        }
        else
        {
            Debug.LogError("Nenhum componente Image encontrado no GameObject!");
        }
    }

    public bool GetHasPriority()
    {
        return this.hasPriority;
    }

    public void StartFetching()
    {
        this.fetching = true;
        this.progressBar.UpdateProgressBar(1);
        this.progressBar.Show();
    }

    public void Reset()
    {
        this.fetching = false;
        this.leftTime = this.timeToFetch;
        transform.localScale = new Vector3(0.4f, 0.5f, 1f);
    }

    public void OriginalSize()
    {
        transform.localScale = new Vector3(0.4f, 0.5f, 1f);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlanFile selectedPlanFile = controller.GetSelectedPlanFile();

        if (selectedPlanFile != null)
        {
            selectedPlanFile.OriginalSize();
        }

        if(controller.GetSelectedPlanFile() == this)
        {
            controller.SetSelectedPlanFile(null);
            this.OriginalSize();
        }
        else
        {
            controller.SetSelectedPlanFile(this);
            transform.localScale += scaleIncrement;
            controller.NextStepTutorial();
        }
    }
}
