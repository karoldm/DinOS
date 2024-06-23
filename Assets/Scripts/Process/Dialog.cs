using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dialog : MonoBehaviour
{
    public enum DialogType
    {
        intro,
        FCFS,
        BP,
        RR,
        SJF
    }

    private static string[] introTexts =
        {
            "Olá, seja bem vindo a sala de controle! Por aqui nós conseguimos acompanhar quando um novo vôo é agendado e chega para nós por meio dos planos de vôos.",
            "Os planos de vôos nos dizem o destino daquele avião, quanto tempo ele vai levar até chegar ao destino e qual a priodade dele. Como gerenciadores desse setor devemos nos atentar a qual vôo deve ser iniciado primeiro para podermos agradar a todos os clientes possíveis!",
            "Olha só, no momento temos 4 planos de vôos esperando para iniciarem, que tal você tentar selecionar um de cada vez e ver se consegue um resultado tão bom quanto o meu :)",
            "Para iniciar um plano de vôo você precisa arrastar ele até o painel de excecução a sua direita. No painel você consegue acompanhar quanto tempo falta até o vôo ser concluído. Lembre-se de usar as informações que você possuí para executar os planos da forma mais eficiente possível!",
            "Lembre-se que você pode interromper um vôo a qualquer momento e devolver ele para o final da fila! Por questões de segurança, você deve esperar um certo tempo antes de interromeper um vôo, isso garante que nenhuma instrução dada ao piloto fique cortada ou pela metade.",
            "Mas não se preocupe! para interromper um vôo você pode só ativar o “cancelamento automatico” que ele será agendado para sempre ser interrompido no tempo correto."
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] FCFSTexts =
        {
            "Emblema conquistado First Came First Served.",
            "Nada mal! Você sabia que executar os planos conforme eles chegam, apesar de ser fácil e intuitivo, não é uma das melhores formas? Se você se deparar com um vôo muito urgente, com um tempo curto, mas que está em último na fila, isso pode ser um problema. Talvez seja hora de considerar outras estratégias de escalonamento para lidar com situações mais desafiadoras."
        };
    private LinkedList<string> FCFSDialog = new LinkedList<string>(FCFSTexts);

    private static string[] SJFTexts =
        {
            "Emblema conquistado Shortest Job Firs.",
            "Uau, muito bom! Optar por iniciar os vôos mais curtos primeiro é uma ótima estratégia. No entanto, lembre-se de que, ao priorizar os vôos mais curtos, os vôos com maior prioridade podem acabar esperando mais do que o desejado. Certifique-se de equilibrar eficiência com prioridades para garantir um bom desempenho em diferentes situações."
        };
    private LinkedList<string> SJFDialog = new LinkedList<string>(SJFTexts);

    private static string[] BPTexts =
        {
            "Emblema conquistado Por Prioridade.",
            "Boa! Iniciar os vôos por prioridade é uma estratégia muito inteligente! No entanto, tome cuidado com vôos que, apesar de serem prioritários, são muito longos e podem acabar atrasando os demais. Certifique-se de avaliar não apenas a prioridade, mas também a duração dos vôos ao planejar seu escalonamento."
        };
    private LinkedList<string> BPDialog = new LinkedList<string>(BPTexts);

    private static string[] RRTexts =
        {
            "Emblema conquistado Round Robin.",
            "Você é fera! Sabia que alternar entre os vôos utilizando conexões e paradas permite que você distribua melhor o tempo de processamento? Essa abordagem garante que nenhum vôo espere muito tempo na fila, mantendo um equilíbrio entre eficiência e justiça. Continue assim e explore outras técnicas de escalonamento para se tornar um mestre em gerenciamento de processos! Ops! Coordenador de vôos."
        };
    private LinkedList<string> RRDialog = new LinkedList<string>(RRTexts);

    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    Process_controller processController;

    private static Dialog instance;
    public static Dialog Instance => instance;

    void Awake()
    {
        processController = Process_controller.Instance;

        if (processController == null)
        {
            Debug.LogError("Process_controller instance not found in the scene.");
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
            case DialogType.intro:
                this.currentDialog = this.introDialog; break;
            case DialogType.SJF:
                this.currentDialog = this.SJFDialog; break;
            case DialogType.BP:
                this.currentDialog = this.BPDialog; break;
            case DialogType.FCFS:
                this.currentDialog = this.FCFSDialog; break;
            case DialogType.RR:
                this.currentDialog = this.RRDialog; break;
        }

        if(this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
