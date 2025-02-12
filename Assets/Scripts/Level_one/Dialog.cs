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
        SJF,
        None
    }

    private static string[] introTexts =
        {
            "Olá, seja bem-vindo à Torre de Controle! Por aqui nós conseguimos acompanhar  quando um novo voo é agendado e chega para nós por meio dos planos de voos.",
            "Os planos de voos possuem uma série de instruções que nos dizem o destino daquele avião, quanto tempo ele vai levar até chegar ao destino e qual a prioridade dele. Como gerenciadores desse setor devemos nos atentar a qual voo deve ser iniciado primeiro  para podermos agradar a todos os clientes possíveis!",
            "Olha só, no momento temos 4 planos de voos esperando para iniciarem, que tal você tentar  selecionar um de cada vez e ver se consegue um resultado tão bom quanto o meu :)",
            "Para iniciar um plano de voo você precisa arrastá-lo até o painel de execução à sua direita.  No painel você consegue acompanhar quanto tempo falta até o voo ser concluído. Lembre-se  de usar as informações que você possui para executar os planos da forma mais eficiente  possível!",
            "Lembre-se que você pode interromper um voo a qualquer momento e devolver ele para o  final da fila! Por questões de segurança, você deve esperar certo tempo antes de  interromper um voo, isso garante que nenhuma instrução dada ao piloto fique cortada ou pela  metade.",
            "Mas não se preocupe! Para interromper um voo você pode só ativar o “ativar quantum” que ele será agendado para sempre ser interrompido no tempo correto. Lembre-se de ativar o quantum antes e iniciar a execução do primeiro voo! \r\n",
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] FCFSTexts =
        {
            "Emblema conquistado First Came First Served.",
            "Nada mal! Você sabia que executar os planos conforme eles chegam, apesar de ser fácil e  intuitivo, não é uma das melhores formas? Se você se deparar com um voo muito urgente,  com um tempo curto, mas que está em último na fila, isso pode ser um problema. Talvez seja  hora de considerar outras estratégias de escalonamento para lidar com situações mais  desafiadoras.",
            "O FCFS é utilizado em sistemas em lotes, ou seja, sistemas que não permitem interação com o usuário, uma vez que é uma abordagem simples e limitada, e não faz uso de um quantum.",
        };
    private LinkedList<string> FCFSDialog = new LinkedList<string>(FCFSTexts);

    private static string[] SJFTexts =
        {
            "Emblema conquistado Shortest Job First.",
            "Uau, muito bom! Optar por iniciar os voos mais curtos primeiro é uma ótima estratégia. No  entanto, lembre-se de que, ao priorizar os voos mais curtos, os voos com maior prioridade  podem acabar esperando mais do que o desejado.",
            "Assim como o FCFS, o SJF é um algoritmo não preemptivo que funciona em sistemas em lotes, desse modo ele pode não ser eficiente em sistemas dinâmicos e que precisam executar muitas coisas ao mesmo tempo!",
        };
    private LinkedList<string> SJFDialog = new LinkedList<string>(SJFTexts);

    private static string[] BPTexts =
        {
            "Emblema conquistado Por Prioridade.",
            "Boa! Iniciar os voos por prioridade é uma estratégia muito inteligente! No entanto, tome  cuidado com voos que, apesar de serem prioritários, são muito longos e podem acabar  atrasando os demais. Certifique-se de avaliar não apenas a prioridade, mas também a duração  dos voos ao planejar seu escalonamento.",
            "Já pensou se algum processo do seu Sistema Operacional passasse horas executando e não lhe deixasse fazer mais nada só porque tem uma prioridade muito alta? Seria péssimo!",
        };
    private LinkedList<string> BPDialog = new LinkedList<string>(BPTexts);

    private static string[] RRTexts =
        {
            "Emblema conquistado Round Robin.",
            "Você é fera! Sabia que alternar entre os voos utilizando conexões e paradas permite que você  distribua melhor o tempo de processamento? Essa abordagem garante que nenhum voo espere  muito tempo na fila, mantendo um equilíbrio entre eficiência e justiça. Continue assim e  explore outras técnicas de escalonamento para se tornar um mestre em gerenciamento de  processos! Ops! Coordenador de voos.",
        };
    private LinkedList<string> RRDialog = new LinkedList<string>(RRTexts);

    private static string[] NoneTexts =
        {
            "As coisas parecem meio fora de ordem por aqui...",
        };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    private Process_controller processController;

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
            processController.InitTutotial();
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
            default:
                this.currentDialog = this.NoneDialog; break;
        }

        if(this.currentDialog != null)
        {
            this.currentNode = this.currentDialog.First;
            this.nextText();
            this.show();
        }
    }
}
