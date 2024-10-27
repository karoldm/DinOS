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
          "Parabéns! Você está sempre garantindo que as coisas estejam no lugar correto e se  comuniquem da melhor forma possível!",
          "Sabia que uma comunicação organizada é um dos pontos mais críticos para qualquer sistema ou organização dar certo? Imagine só se os drivers do seu Sistema Computacional não se comunicassem corretamente com os seus dispositivos externos? As coisas seriam uma bagunça… Por sorte os Sistemas Operacionais possuem os sistemas de Entrada e Saída para gerenciar isso, e para nossa sorte temos você por aqui!",
          "Parabéns por usar corretamente as chamadas de rádio (ou de sistema se preferir) e garantir uma comunicação precisa entre nossos pilotos e nossos funcionários.",
        };
    private LinkedList<string> awardDialog = new LinkedList<string>(awardTexts);


    private static string[] introTexts =
     {
        "Bem-vindo a uma das partes mais agitadas do nosso aeroporto! Aqui é onde nossos  aviões decolam, pousam ou fazem paradas. Tudo aqui é gerenciado pela torre de controle e  ela garante que os pilotos conduzam os aviões para a pista correta.",
        "Aqui temos diversos tipos de aviões: de passeios, de carga, particulares e comerciais. Cada  um é indicado por uma cor (sendo elas verde, azul, amarelo e vermelho, respectivamente), desse modo, cada  pista possui uma bandeira da mesma cor para indicar ao piloto que é ali que o avião deve  pousar.",
        "Para não haver problemas de comunicação com nossos funcionários, os pilotos devem vestir  um uniforme da mesma cor que a bandeira, indicando o tipo de avião que ele está pilotando. Desse modo, um piloto com uniforme azul, só pode pousar ou decolar da pista indicada pela  bandeira azul.",
        "Exatamente como ocorre quando você precisa conectar algum dispositivo externo no seu computador e para que o SO consiga se comunicar com ele, o sistema computacional deve possuir um driver e o dispositivo deve estar em conformidade com o protocolo de comunicação deste driver.",
        "Toda vez que um piloto precisa usar a pista, ele deve se comunicar com a torre de controle  por meio de chamados de rádio. A nossa torre irá analisar qual funcionário vai atender à solicitação e enviar um chamado para o mesmo na pista.",
        "Cada funcionário demora uma quantidade diferente de tempo para executar um chamado, e  esse tempo é indicado logo abaixo de cada um no nosso painel.",
        "Vale lembrar que os voos possuem prioridade por chamado, por exemplo, um chamado de  pouso de emergência com certeza tem uma prioridade maior do que um chamado de  permissão para decolar.",
        "Veja, temos vários aviões chegando no momento, que tal tentar gerenciar nossos funcionários  e atender os chamados? Lembre-se que o avião deve estar na pista correta para ser atendido,  você pode colocá-lo na pista correta trocando eles de lugar com um clique (clique em um avião e depois clique em outro para trocá-los de lugar). Aviões em processo de atendimento não  podem ser trocados!",
        "Estando na posição correta você pode atribuir um chamado a um funcionário. Faça isso  clicando no chamado e depois no Dino que você quer que seja responsável. Um Dino só pode  atender um chamado por vez. ",
        "Lembre-se de dar uma atenção especial para os chamados de alta prioridade e boa sorte!"
     };
    private LinkedList<string> introDialog = new LinkedList<string>(introTexts);

    private static string[] NoneTexts =
        {
            "As coisas parecem meio fora de ordem por aqui...",
        };
    private LinkedList<string> NoneDialog = new LinkedList<string>(NoneTexts);

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
}
