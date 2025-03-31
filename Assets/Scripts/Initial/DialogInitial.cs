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
        process,
        RAM,
        ES,
        SecMemory
    }
    private InitialDialogType currentSceneType;

    private static string[] processTexts =
    {
        "Torre de controle: Aqui é onde acompanhamos e controlamos os voos que precisam ser executados ou que já estão em execução.", 
    };
    private LinkedList<string> processDialog = new LinkedList<string>(processTexts);

    private static string[] secMemoryTexts =
    {
        "Armazém e área administrativa: Aqui é a área administrativa do nosso aeroporto, onde os planos de voo são gerados e armazenados para que a torre de controle possa recuperá-los posteriormente.",
    };
    private LinkedList<string> secMemoryDialog = new LinkedList<string>(secMemoryTexts);

    private static string[] ESTexts =
    {
        "Pista de decolagem e pouso: Aqui é onde acompanhamos os pousos e decolagens dos aviões.",
    };
    private LinkedList<string> ESDialog = new LinkedList<string>(ESTexts);

    private static string[] ramTexts =
    {
        "Preparação dos voos: Aqui é onde realizamos as tarefas necessárias para permitir que os aviões decolem e cheguem ao destino corretamente e sem problemas.",
    };
    private LinkedList<string> ramDialog = new LinkedList<string>(ramTexts);


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
            case InitialDialogType.RAM:
                this.hidden();
                SceneManager.LoadScene("level_two");
                break;
            case InitialDialogType.ES:
                this.hidden();
                SceneManager.LoadScene("level_three");
                break;
            case InitialDialogType.SecMemory:
                this.hidden();
                SceneManager.LoadScene("level_four");
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
            case InitialDialogType.RAM:
                this.currentDialog = this.ramDialog; break;
            case InitialDialogType.ES:
                this.currentDialog = this.ESDialog; break;
            case InitialDialogType.SecMemory:
                this.currentDialog = this.secMemoryDialog; break;
        }

        if(this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
