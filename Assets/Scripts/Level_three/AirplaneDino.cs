using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class AirplaneDino : MonoBehaviour, IPointerClickHandler
{
    private bool isBusy;
    public double velocity;
    public SpriteRenderer circle;
    private AirplaneCall call;

    private LevelThreeController controller;

    void Awake()
    {
        controller = LevelThreeController.Instance;

        if (controller == null)
        {
            Debug.LogError("LevelThreeController instance não encontrado na cena.");
        }
    }

    void Start()
    {

    }

    void Update()
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (isBusy) return;

        this.call = controller.GetSelectedCall();

        if (call == null) return;

        UpdateCircleColor(call.GetColor());
        call.airplane.SetBusy();
        controller.SetSelectedCall(null);
        StartCoroutine(StartService());
    }

    private IEnumerator StartService()
    {
        isBusy = true;

        /*StartCoroutine(this.dest.InitProgressBar(time));*/
        yield return new WaitForSeconds((float)velocity);
        EndService();
    }

    private void EndService()
    {
        isBusy = false;
        UpdateCircleColor("#EEEEEE");
        controller.EndService(call.airplane);
        this.call = null;
    }

    private void UpdateCircleColor(string color)
    {
        SpriteRenderer circle = transform.Find("Circle").GetComponent<SpriteRenderer>();
        if (circle != null)
        {
            if (ColorUtility.TryParseHtmlString(color, out Color outColor))
            {
                circle.color = outColor;
            }
            else
            {
                Debug.LogError("Invalid hex color string");
            }
        }
        else
        {
            Debug.LogError("Circle is null");
        }
    }
}
