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
            "Ol�, seja bem-vindo � Torre de Controle! Por aqui n�s conseguimos acompanhar  quando um novo voo � agendado e chega para n�s por meio dos planos de voos.",
            "Os planos de voos possuem uma s�rie de instru��es que nos dizem o destino daquele avi�o, quanto tempo ele vai levar at� chegar ao destino e qual a prioridade dele. Como gerenciadores desse setor devemos nos atentar a qual voo deve ser iniciado primeiro  para podermos agradar a todos os clientes poss�veis!",
            "Olha s�, no momento temos 4 planos de voos esperando para iniciarem, que tal voc� tentar  selecionar um de cada vez e ver se consegue um resultado t�o bom quanto o meu :)",
            "Para iniciar um plano de voo voc� precisa arrast�-lo at� o painel de execu��o � sua direita.  No painel voc� consegue acompanhar quanto tempo falta at� o voo ser conclu�do. Lembre-se  de usar as informa��es que voc� possui para executar os planos da forma mais eficiente  poss�vel!",
            "Lembre-se que voc� pode interromper um voo a qualquer momento e devolver ele para o  final da fila! Por quest�es de seguran�a, voc� deve esperar certo tempo antes de  interromper um voo, isso garante que nenhuma instru��o dada ao piloto fique cortada ou pela  metade.",
            "Mas n�o se preocupe! Para interromper um voo voc� pode s� ativar o �ativar quantum� que ele ser� agendado para sempre ser interrompido no tempo correto. Lembre-se de ativar o quantum antes e iniciar a execu��o do primeiro voo! \r\n",
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] FCFSTexts =
        {
            "Emblema conquistado First Came First Served.",
            "Nada mal! Voc� sabia que executar os planos conforme eles chegam, apesar de ser f�cil e  intuitivo, n�o � uma das melhores formas? Se voc� se deparar com um voo muito urgente,  com um tempo curto, mas que est� em �ltimo na fila, isso pode ser um problema. Talvez seja  hora de considerar outras estrat�gias de escalonamento para lidar com situa��es mais  desafiadoras.",
            "O FCFS � utilizado em sistemas em lotes, ou seja, sistemas que n�o permitem intera��o com o usu�rio, uma vez que � uma abordagem simples e limitada, e n�o faz uso de um quantum.",
        };
    private LinkedList<string> FCFSDialog = new LinkedList<string>(FCFSTexts);

    private static string[] SJFTexts =
        {
            "Emblema conquistado Shortest Job First.",
            "Uau, muito bom! Optar por iniciar os voos mais curtos primeiro � uma �tima estrat�gia. No  entanto, lembre-se de que, ao priorizar os voos mais curtos, os voos com maior prioridade  podem acabar esperando mais do que o desejado.",
            "Assim como o FCFS, o SJF � um algoritmo n�o preemptivo que funciona em sistemas em lotes, desse modo ele pode n�o ser eficiente em sistemas din�micos e que precisam executar muitas coisas ao mesmo tempo!",
        };
    private LinkedList<string> SJFDialog = new LinkedList<string>(SJFTexts);

    private static string[] BPTexts =
        {
            "Emblema conquistado Por Prioridade.",
            "Boa! Iniciar os voos por prioridade � uma estrat�gia muito inteligente! No entanto, tome  cuidado com voos que, apesar de serem priorit�rios, s�o muito longos e podem acabar  atrasando os demais. Certifique-se de avaliar n�o apenas a prioridade, mas tamb�m a dura��o  dos voos ao planejar seu escalonamento.",
            "J� pensou se algum processo do seu Sistema Operacional passasse horas executando e n�o lhe deixasse fazer mais nada s� porque tem uma prioridade muito alta? Seria p�ssimo!",
        };
    private LinkedList<string> BPDialog = new LinkedList<string>(BPTexts);

    private static string[] RRTexts =
        {
            "Emblema conquistado Round Robin.",
            "Voc� � fera! Sabia que alternar entre os voos utilizando conex�es e paradas permite que voc�  distribua melhor o tempo de processamento? Essa abordagem garante que nenhum voo espere  muito tempo na fila, mantendo um equil�brio entre efici�ncia e justi�a. Continue assim e  explore outras t�cnicas de escalonamento para se tornar um mestre em gerenciamento de  processos! Ops! Coordenador de voos.",
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
