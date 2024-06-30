using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;


public class DialogInitial : MonoBehaviour
{

    public enum InitialDialogType
    { 
        process
    }
    private InitialDialogType currentSceneType;

    private static string[] processTexts =
        {
            "Aqui é onde acompanhamos e controlamos os vôos que precisam ser executados ou que já estão em excecução.", 
    };
    private LinkedList<string> processDialog = new LinkedList<string>(processTexts);


    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    private static DialogInitial instance;
    public static DialogInitial Instance => instance;

    void Awake()
    {
   
    }

    void Start()
    {
 
    }

    void Update()
    {
        
    }

    public void nextText()
    {

        if (this.currentNode != null)
        {
            this.buildText();
            this.currentNode = this.currentNode.Next;
        }
        else
        {
            this.hidden();
        }
    }

    public void SetCurrentSceneType(InitialDialogType scene)
    {
        this.currentSceneType = scene;
    }

    public void LoadScene()
    {
        switch (this.currentSceneType)
        {
            case InitialDialogType.process:
                this.hidden();
                SceneManager.LoadScene("level_one");
                break;
        }
    }

    private void buildText()
    {
        if (this.currentNode != null && this.dialogText != null)
        {
            this.dialogText.text = this.currentNode.Value;
        }
    }

    private void show()
    {
        gameObject.SetActive(true);
    }

    public void hidden()
    {
        gameObject.SetActive(false);
    }

    public void showDialog(InitialDialogType type)
    {
        switch (type)
        {
            case InitialDialogType.process:
                this.currentDialog = this.processDialog; break;
            default:
                this.currentDialog = this.processDialog; break;
        }

        if(this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
