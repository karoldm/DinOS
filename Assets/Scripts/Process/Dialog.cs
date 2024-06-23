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
            "Ol�, seja bem vindo a sala de controle! Por aqui n�s conseguimos acompanhar quando um novo v�o � agendado e chega para n�s por meio dos planos de v�os.",
            "Os planos de v�os nos dizem o destino daquele avi�o, quanto tempo ele vai levar at� chegar ao destino e qual a priodade dele. Como gerenciadores desse setor devemos nos atentar a qual v�o deve ser iniciado primeiro para podermos agradar a todos os clientes poss�veis!",
            "Olha s�, no momento temos 4 planos de v�os esperando para iniciarem, que tal voc� tentar selecionar um de cada vez e ver se consegue um resultado t�o bom quanto o meu :)",
            "Para iniciar um plano de v�o voc� precisa arrastar ele at� o painel de excecu��o a sua direita. No painel voc� consegue acompanhar quanto tempo falta at� o v�o ser conclu�do. Lembre-se de usar as informa��es que voc� possu� para executar os planos da forma mais eficiente poss�vel!",
            "Lembre-se que voc� pode interromper um v�o a qualquer momento e devolver ele para o final da fila! Por quest�es de seguran�a, voc� deve esperar um certo tempo antes de interromeper um v�o, isso garante que nenhuma instru��o dada ao piloto fique cortada ou pela metade.",
            "Mas n�o se preocupe! para interromper um v�o voc� pode s� ativar o �cancelamento automatico� que ele ser� agendado para sempre ser interrompido no tempo correto."
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] FCFSTexts =
        {
            "Emblema conquistado First Came First Served.",
            "Nada mal! Voc� sabia que executar os planos conforme eles chegam, apesar de ser f�cil e intuitivo, n�o � uma das melhores formas? Se voc� se deparar com um v�o muito urgente, com um tempo curto, mas que est� em �ltimo na fila, isso pode ser um problema. Talvez seja hora de considerar outras estrat�gias de escalonamento para lidar com situa��es mais desafiadoras."
        };
    private LinkedList<string> FCFSDialog = new LinkedList<string>(FCFSTexts);

    private static string[] SJFTexts =
        {
            "Emblema conquistado Shortest Job Firs.",
            "Uau, muito bom! Optar por iniciar os v�os mais curtos primeiro � uma �tima estrat�gia. No entanto, lembre-se de que, ao priorizar os v�os mais curtos, os v�os com maior prioridade podem acabar esperando mais do que o desejado. Certifique-se de equilibrar efici�ncia com prioridades para garantir um bom desempenho em diferentes situa��es."
        };
    private LinkedList<string> SJFDialog = new LinkedList<string>(SJFTexts);

    private static string[] BPTexts =
        {
            "Emblema conquistado Por Prioridade.",
            "Boa! Iniciar os v�os por prioridade � uma estrat�gia muito inteligente! No entanto, tome cuidado com v�os que, apesar de serem priorit�rios, s�o muito longos e podem acabar atrasando os demais. Certifique-se de avaliar n�o apenas a prioridade, mas tamb�m a dura��o dos v�os ao planejar seu escalonamento."
        };
    private LinkedList<string> BPDialog = new LinkedList<string>(BPTexts);

    private static string[] RRTexts =
        {
            "Emblema conquistado Round Robin.",
            "Voc� � fera! Sabia que alternar entre os v�os utilizando conex�es e paradas permite que voc� distribua melhor o tempo de processamento? Essa abordagem garante que nenhum v�o espere muito tempo na fila, mantendo um equil�brio entre efici�ncia e justi�a. Continue assim e explore outras t�cnicas de escalonamento para se tornar um mestre em gerenciamento de processos! Ops! Coordenador de v�os."
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
