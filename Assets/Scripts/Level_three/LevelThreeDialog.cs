using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelThreeDialog : MonoBehaviour
{
    public enum DialogType
    {
        award,
        intro,
        none,
    }

    private static string[] awardTexts =
    {
        "Conquista desbloqueada: Mestre em comunicações!",
        "Parabéns! Você está sempre garantindo que as coisas estejam no lugar correto e se comuniquem da melhor forma possível!",
        "Sabia que uma comunicação organizada é um dos pontos mais críticos para qualquer sistema ou organização dar certo? Imagine só se os drivers do seu Sistema Computacional não se comunicassem corretamente com os seus dispositivos externos? As coisas seriam uma bagunça! Por sorte os Sistemas Operacionais possuem o Software de Entrada e Saída para gerenciar isso, e para nossa sorte temos você por aqui!",
        "Parabéns por usar corretamente as chamadas de rádio (ou de sistema se preferir) e garantir uma comunicação precisa entre nossos pilotos e nossos funcionários.",
    };
    private LinkedList<string> awardDialog = new LinkedList<string>(awardTexts);

    private static string[] introTexts =
    {
        "Bem-vindo a uma das partes mais agitadas do nosso aeroporto! Aqui é onde nossos aviões decolam, pousam ou fazem paradas.",
        "Aqui temos diversos tipos de aviões (Assim como temos diversos tipos de periféricos no nosso computador). Cada um é indicado por uma cor, desse modo, cada pista possui uma bandeira da mesma cor para indicar ao piloto que é ali que o avião deve pousar.",
        "Para não haver problemas de comunicação com nossos funcionários, os pilotos devem vestir um uniforme da mesma cor que a bandeira, indicando o tipo de avião que ele está pilotando. Desse modo, um piloto com uniforme azul, só pode pousar ou decolar da pista indicada pela bandeira azul.",
        "Exatamente como ocorre quando você precisa conectar algum dispositivo externo no seu computador e para que o SO consiga se comunicar com ele, o sistema computacional deve possuir um driver e o dispositivo deve estar em conformidade com o protocolo de comunicação deste driver.",
        "Toda vez que um piloto precisa usar a pista, ele deve se comunicar com a torre de controle por meio de chamados de rádio (chamadas de sistema). Você deve analisar qual funcionário vai atender à solicitação e enviar um chamado para o mesmo na pista (como um tratamento de interrupção).",
        "Veja, temos vários aviões chegando no momento, que tal tentar gerenciar nossos funcionários e atender os chamados? Lembre-se que o avião deve estar na pista correta para ser atendido, você pode colocá-lo na pista correta trocando eles de lugar com um clique (clique em um avião e depois clique em outro para trocá-los de lugar). Aviões em processo de atendimento não podem ser trocados!",
        "Estando na posição correta você pode atribuir um chamado a um funcionário. Faça isso clicando no chamado e depois no Dino que você quer que seja responsável. Um Dino só pode atender um chamado por vez.",
        "Lembre-se de dar uma atenção especial para os chamados de alta prioridade e boa sorte!"
    };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] NoneTexts =
    {
        "As coisas parecem meio fora de ordem por aqui...",
    };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

    private static string[] feedbackTexts =
    {
        "Erro de coordenação! O avião está tentando pousar na pista errada!\r\nAssim como no aeroporto, onde cada avião precisa de uma pista específica para pousar com segurança, no mundo dos sistemas operacionais, cada periférico (avião) precisa do driver correto (piloto) para se comunicar com o sistema operacional (aeroporto). Se o avião tentar pousar na pista errada, o aeroporto não conseguirá atendê-lo, causando confusão e possíveis \"acidentes\" no sistema.",
        "No jogo, sua missão é garantir que cada avião (periférico) tenha o piloto (driver) certo e pouse na pista correta (interface do SO) para que o aeroporto (sistema operacional) possa gerenciar tudo sem problemas. Afinal, um pouso na pista errada pode levar a um caos generalizado"
    };
    private LinkedList<string> feedbackDialog = new LinkedList<string>(feedbackTexts);

    public Button button;
    public TextMeshProUGUI dialogText;

    private LinkedList<string> currentDialog;
    private LinkedListNode<string> currentNode;

    LevelThreeController controller;

    private static LevelThreeDialog instance;
    public static LevelThreeDialog Instance => instance;

    void Awake()
    {
        controller = LevelThreeController.Instance;

        if (controller == null)
        {
            Debug.LogError("LevelThreeController instance not found in the scene.");
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
            if(this.currentDialog != this.feedbackDialog)
            {
                controller.ShowTutorial();
            }
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
            case DialogType.award:
                this.currentDialog = this.awardDialog; break;
            case DialogType.intro:
                this.currentDialog = this.introDialog; break;
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

    public void ShowFeedbackDialog()
    {
        this.currentDialog = this.feedbackDialog;
        this.currentNode = this.currentDialog.First;
        this.nextText();
        this.show();
    }
}
