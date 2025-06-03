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
        "Conquista desbloqueada: Segmentação Zero!",
        "É muito raro conseguir aproveitar 100% dos funcionários e garantir que não estamos desperdiçando tempo e recursos!",
        "Tal como nos sistemas computacionais, onde a memória RAM é um recurso caro e precisa ser gerenciado com cuidado para um melhor aproveitamento, as tarefas atribuídas aos nossos funcionários também devem ser alocadas com cuidado para evitar que eles fiquem ociosos ou que acabem concorrendo pelo mesmo terminal. E você fez isso tão bem quanto um gerenciador de memória, parabéns! \r\n",
    };
    private LinkedList<string> segmentationDialog = new LinkedList<string>(segmentationTexts);

    private static string[] introTexts =
    {
        "Bem-vindo! Aqui alocamos todas as tarefas necessárias para que um plano de voo iniciado pela Torre de Controle possa ser conduzido até o final sem falhas. Diversas tarefas como abastecer o avião, checar as travas de segurança, verificar a comunicação com a torre, conduzir os passageiros, entre muitas outras são realizadas neste local.",
        "Cada voo possui um conjunto de tarefas, e cada tarefa precisa ser realizada no seu próprio terminal (identificado pela cor e pelo ícone da tarefa).",
        "Você pode associar os terminais aos núcleos da CPU, onde cada uma é responsável por executar processos diferentes.",
        "Nossos companheiros podem lhe ajudar a realizar essas tarefas, mas cada um deles só pode realizar uma tarefa de cada vez, e enquanto estiver ocupado com uma tarefa não pode receber outras! Além disso, cada terminal só pode ser usado por um Dino por vez.",
        "Assim como os espaços da memória RAM podem ser divididos em partições para diferentes processos, cada Dino representa uma partição com tamanho fixo. Se atente a essa capacidade quando for atribuir uma tarefa a um Dino.",
        "Muito cuidado com tarefas dependentes! Algumas de nossas tarefas dependem de outras de modo que serão executadas imediatamente em seguida. Essas tarefas são marcadas com uma seta.",
        "Para iniciar, basta arrastar uma tarefa de um dos voos até um Dino, e esperar ele concluir.",
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] dinnerProblemTexts =
    {
        "Ops! You are dead(lock)!",
        "Parece que dois ou mais dinossauros dependem um do terminal do outro para concluir suas tarefas, e desse modo nenhum deles pode avançar! As tarefas precisam ser alocadas com cuidado para não ocorrerem situações como essas, uma vez que os recursos do processador são limitados e atendem um processo por vez, se dois processos passarem a depender um do recurso do outro, o Sistema Operacional estará em um impasse!",
        "Conquista desbloqueada: Problema do jantar dos filósofos!",
    };
    private LinkedList<string> dinnerProblemDialog = new LinkedList<string>(dinnerProblemTexts);

    private static string[] NoneTexts =
    {
        "As coisas parecem meio fora de ordem por aqui...",
    };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

    private static string[] feedbackTexts =
    {
        "Ops! O Dino poderia lidar com tarefas mais complexas!\r\nNo aeroporto da memória RAM, cada Dino (partição da memória) tem uma capacidade específica para realizar tarefas (processos). Delegar uma tarefa pequena a um Dino com grande capacidade é como colocar um avião de carga para transportar uma mala de mão: ineficiente e um desperdício de recursos!",
        "No jogo, você precisa garantir que cada Dino receba tarefas adequadas ao seu tamanho. Tarefas grandes devem ser atribuídas a Dinos com maior capacidade, enquanto tarefas menores podem ser executadas por Dinos menores. Assim, o aeroporto (sistema) opera de forma eficiente, sem desperdiçar memória ou deixar processos importantes esperando!"
    };
    private LinkedList<string> feedbackDialog = new LinkedList<string>(feedbackTexts);

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

    public void ShowFeedback()
    {
        this.currentDialog = this.feedbackDialog;
        this.currentNode = this.currentDialog.First;
        this.nextText();
        this.show();
    }
}
