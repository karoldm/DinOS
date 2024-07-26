using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogLevelTwo : MonoBehaviour
{
    public enum DialogType
    {
        dinnerProblem,
        none,
        intro,
        segmentation,
    }

    private static string[] segmentationTexts =
        {
          "Conquista desbloqueada: Zero Segmentaçã!",
          "É muito raro conseguir aproveitar 100% dos funcionários e garantir que não estamos desperdiçando tempo e recursos!"
        };
    private LinkedList<string> segmentationDialog = new LinkedList<string>(segmentationTexts);


    private static string[] introTexts =
        {
           "Bem vindo a sala de gerenciamento de tarefas! Aqui realizamos todas as tarefas necessárias para que um plano de vôo iniciado possa ser conduzido até o final sem falhar.",
           "Diversas tarefas como abastecer o avião, checar as travas de segurança, verificar a comunicação com a torre, conduzir os passageiros, entre muitas outras são realizadas desde o preparo da decolagem até o termino do vôo!",
           "Graças a nossa tecnologia avançada, podemos realizar essas tarefas apartir de terminais localizados aqui mesmo! Que tal tentar?",
           "Cada vôo possui um conjunto de tarefas, e cada tarefa precisa ser realizada no seu próprio terminal (identificado pela cor e pelo ícone da tarefa).",
           "Nossos companheiros podem te ajudar a realizar essas tarefas, mas cada um deles só pode realizar uma tarefa de cada, e enquanto estiver ocupando com uma tarefa não pode receber outras! Além disso, cada terminal só pode ser usado por um Dino por vez.",
           "Muito cuidado com tarefas dependentes! Algumas de nossas tarefas dependem de outras de modo que serão executadas imediatamente em seguida. Essas tarefas são marcadas com uma seta.",
           "Para iniciar, basta arrastar uma tarefa de um dos vôos até um dino, e esperar ele concluir. Lembre-se que você consegue ver quanto tempo a tarefa vai levar e quantos pontos ela vai te dar antes mesmo de executá-la!",
        };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] dinnerProblemTexts =
        {
           "Ops! You are dead(lock)!",
           "Parece que dois ou mais dinossauros dependem um do terminal do outro para concluir suas tarefas, e desse modo nenhum deles pode avançar! As tarefas precisam ser alocadas com cuidado para não ocorrer situações como essas!", 
           "Conquista desbloqueada: Problema do jantar dos filósofos!"
        };
    private LinkedList<string> dinnerProblemDialog = new LinkedList<string>(dinnerProblemTexts);

    private static string[] NoneTexts =
        {
            "As coisas parecem meio fora de ordem por aqui...",
        };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    RAMController ramController;

    private static DialogLevelTwo instance;
    public static DialogLevelTwo Instance => instance;

    void Awake()
    {
        ramController = RAMController.Instance;

        if (ramController == null)
        {
            Debug.LogError("RAMController instance not found in the scene.");
        }

        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
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

    private void hidden()
    {
        gameObject.SetActive(false);
    }

    public void showDialog(DialogType type)
    {
        switch (type)
        {
            case DialogType.dinnerProblem:
                this.currentDialog = this.dinnerProblemDialog; break;
            case DialogType.intro:
                this.currentDialog = this.introDialog; break;
            case DialogType.segmentation:
                this.currentDialog = this.segmentationDialog; break;
            default:
                this.currentDialog = this.NoneDialog; break;
        }

        if (this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
