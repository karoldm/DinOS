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
          "Conquista desbloqueada: Segmenta��o Zero!",
          "� muito raro conseguir aproveitar 100% dos funcion�rios e garantir que n�o estamos  desperdi�ando tempo e recursos!",
          "Tal como nos sistemas computacionais, onde a mem�ria RAM � um recurso caro e precisa ser gerenciado com cuidado para um melhor aproveitamento, nossos funcion�rios tamb�m precisam ser alocados com cuidado, evitando �espa�os desperdi�ados� e bloqueio de tarefas por conta dos recursos dispon�veis. E voc� fez isso t�o bem quanto um gerenciador de mem�ria, parab�ns!",
        };
    private LinkedList<string> segmentationDialog = new LinkedList<string>(segmentationTexts);


    private static string[] introTexts =
        {
            "Bem-vindo aos voos em execu��o! Aqui realizamos todas as tarefas necess�rias para que um plano de voo iniciado pela Torre de Controle possa ser conduzido at� o final sem  falhas.",
            "Diversas tarefas como abastecer o avi�o, checar as travas de seguran�a, verificar a comunica��o com a torre, conduzir os passageiros, entre muitas outras s�o realizadas neste  local.",
            "Gra�as � nossa tecnologia avan�ada, podemos realizar essas tarefas a partir de terminais localizados aqui mesmo! Que tal tentar?",
            "Cada voo possui um conjunto de tarefas, e cada tarefa precisa ser realizada no seu pr�prio terminal (identificado pela cor e pelo �cone da tarefa).",
            "Nossos companheiros podem te ajudar a realizar essas tarefas, mas cada um deles s� pode realizar uma tarefa de cada, e enquanto estiver ocupado com uma tarefa n�o pode receber outras! Al�m disso, cada terminal s� pode ser usado por um Dino por vez.",
            "Como temos uma equipe diversificada, cada um dos nossos funcion�rios lida com diferentes dificuldades de tarefas. A capacidade de um Dino � indicada abaixo dele e mostra quantos pontos de dificuldade ele consegue suportar de uma vez!",
            "Muito cuidado com tarefas dependentes! Algumas de nossas tarefas dependem de outras de  modo que ser�o executadas imediatamente em seguida. Essas tarefas s�o marcadas com uma seta.",
            "Para iniciar, basta arrastar uma tarefa de um dos voos at� um Dino, e esperar ele concluir. Lembre-se que voc� consegue ver quanto tempo a tarefa vai levar e quantos pontos ela vai te  dar antes mesmo de execut�-la!",
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] dinnerProblemTexts =
        {
           "Ops! You are dead(lock)!",
           "Parece que dois ou mais dinossauros dependem um do terminal do outro para concluir suas  tarefas, e desse modo nenhum deles pode avan�ar! As tarefas precisam ser alocadas com  cuidado para n�o ocorrer situa��es como essas!", 
           "Conquista desbloqueada: Problema do jantar dos fil�sofos!"
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
